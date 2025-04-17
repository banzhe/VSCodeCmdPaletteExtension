using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage;

namespace VisualStudioCodeWorkspaceExtension.VSCodeHelper
{
    public static class VSCodeInstances
    {
        private static readonly string _userAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public static List<VSCodeInstance> Instances { get; set; } = new List<VSCodeInstance>();

        private static async Task<BitmapImage> LoadBitmapImageFromFile(string filePath)
        {
            var file = await StorageFile.GetFileFromPathAsync(filePath);
            using var stream = await file.OpenStreamForReadAsync();
            var bitmapImage = new BitmapImage();
            await bitmapImage.SetSourceAsync(stream.AsRandomAccessStream());
            return bitmapImage;
        }

        private static async Task<BitmapImage> OverlayImages(BitmapImage baseImage, BitmapImage overlayImage)
        {
            // For now, we'll just return the base image as the overlay functionality
            // would require more complex pixel manipulation that's better handled
            // by a dedicated image processing library
            return baseImage;
        }

        // Gets the executablePath and AppData foreach instance of VSCode
        public static async Task LoadVSCodeInstances()
        {
            var environmentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User) ?? string.Empty;
            environmentPath += (environmentPath.Length > 0 && environmentPath.EndsWith(';') ? string.Empty : ";") + Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
            var paths = environmentPath
                .Split(';')
                .Distinct()
                .Where(x => x.Contains("VS Code", StringComparison.OrdinalIgnoreCase)
                    || x.Contains("VSCodium", StringComparison.OrdinalIgnoreCase)
                    || x.Contains("vscode", StringComparison.OrdinalIgnoreCase)).ToArray();

            foreach (var path in paths)
            {
                if (!Directory.Exists(path))
                {
                    continue;
                }

                var files = Directory.GetFiles(path)
                    .Where(x => (x.Contains("code", StringComparison.OrdinalIgnoreCase) || x.Contains("codium", StringComparison.OrdinalIgnoreCase))
                        && !x.EndsWith(".cmd", StringComparison.OrdinalIgnoreCase)).ToArray();

                // Remove the trailing backslash to always get the correct path
                var iconPath = Path.GetDirectoryName(path.TrimEnd('\\'));

                if (files.Length == 0)
                {
                    continue;
                }

                var file = files[0];
                var version = string.Empty;

                var instance = new VSCodeInstance
                {
                    ExecutablePath = file,
                };

                if (file.EndsWith("code", StringComparison.OrdinalIgnoreCase))
                {
                    version = "Code";
                    instance.VSCodeVersion = VSCodeVersion.Stable;
                }
                else if (file.EndsWith("code-insiders", StringComparison.OrdinalIgnoreCase))
                {
                    version = "Code - Insiders";
                    instance.VSCodeVersion = VSCodeVersion.Insiders;
                }
                else if (file.EndsWith("code-exploration", StringComparison.OrdinalIgnoreCase))
                {
                    version = "Code - Exploration";
                    instance.VSCodeVersion = VSCodeVersion.Exploration;
                }
                else if (file.EndsWith("codium", StringComparison.OrdinalIgnoreCase))
                {
                    version = "VSCodium";
                    instance.VSCodeVersion = VSCodeVersion.Stable;
                }
                else if (file.EndsWith("codium-insiders", StringComparison.OrdinalIgnoreCase))
                {
                    version = "VSCodium - Insiders";
                    instance.VSCodeVersion = VSCodeVersion.Insiders;
                }

                if (string.IsNullOrEmpty(version))
                {
                    continue;
                }

                var portableData = Path.Join(iconPath, "data");
                instance.AppData = Directory.Exists(portableData) ? Path.Join(portableData, "user-data") : Path.Combine(_userAppDataPath, version);
                var vsCodeIconPath = Path.Join(iconPath, $"{version}.exe");
                if (!File.Exists(vsCodeIconPath))
                {
                    continue;
                }

                /* var vsCodeIcon = await LoadBitmapImageFromFile(vsCodeIconPath);
                if (vsCodeIcon == null)
                {
                    continue;
                }

                // Workspace
                var folderIconPath = Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Images//folder.png");
                var folderIcon = await LoadBitmapImageFromFile(folderIconPath);
                instance.WorkspaceIconBitMap = await OverlayImages(folderIcon, vsCodeIcon);

                // Remote
                var monitorIconPath = Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Images//monitor.png");
                var monitorIcon = await LoadBitmapImageFromFile(monitorIconPath);
                instance.RemoteIconBitMap = await OverlayImages(monitorIcon, vsCodeIcon); */

                Instances.Add(instance);
            }

            Console.WriteLine($"Loaded {Instances.Count} VSCode instances");
        }
    }
}
