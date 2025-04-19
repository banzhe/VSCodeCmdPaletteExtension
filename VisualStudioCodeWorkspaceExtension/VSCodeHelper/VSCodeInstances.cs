using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CommandPalette.Extensions.Toolkit;
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

        private static void DoLoadVsCodeInstance(string path)
        {
            var files = Directory.GetFiles(path)
                .Select(x => Path.GetFileName(x))
                .Where(x => (x.Contains("code", StringComparison.OrdinalIgnoreCase) || x.Contains("codium", StringComparison.OrdinalIgnoreCase))
                    && !x.EndsWith(".cmd", StringComparison.OrdinalIgnoreCase)).ToArray();

            // Remove the trailing backslash to always get the correct path
            var iconPath = Path.GetDirectoryName(path.TrimEnd('\\'));

            if (files.Length == 0)
            {
                return;
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
                return;
            }

            var portableData = Path.Join(iconPath, "data");
            instance.AppData = Directory.Exists(portableData) ? Path.Join(portableData, "user-data") : Path.Combine(_userAppDataPath, version);
            var vsCodeIconPath = Path.Join(iconPath, $"{version}.exe");
            if (!File.Exists(vsCodeIconPath))
            {
                return;
            }

            instance.WorkspaceIconInfo = IconHelpers.FromRelativePath(vsCodeIconPath);

            Instances.Add(instance);
        }

        private static void DoLoadCursorInstance(string path)
        {
            var files = Directory.GetFiles(path)
                .Select(x => Path.GetFileName(x))
                .Where(x => x.Contains("cursor", StringComparison.OrdinalIgnoreCase)
                    && !x.EndsWith(".cmd", StringComparison.OrdinalIgnoreCase)).ToArray();

            // Remove the trailing backslash to always get the correct path
            var iconPath = Path.GetDirectoryName(path.TrimEnd('\\'));

            if (files.Length == 0)
            {
                return;
            }

            var file = files[0];
            var version = string.Empty;

            var instance = new VSCodeInstance
            {
                ExecutablePath = file,
            };

            if (file.EndsWith("cursor", StringComparison.OrdinalIgnoreCase))
            {
                version = "Cursor";
                instance.VSCodeVersion = VSCodeVersion.Cursor;
            }

            if (string.IsNullOrEmpty(version))
            {
                return;
            }

            var portableData = Path.Join(iconPath, "data");
            instance.AppData = Directory.Exists(portableData) ? Path.Join(portableData, "user-data") : Path.Combine(_userAppDataPath, version);
            var vsCodeIconPath = Path.Join(iconPath, "..", "..", $"{version}.exe");
            if (!File.Exists(vsCodeIconPath))
            {
                return;
            }

            instance.WorkspaceIconInfo = IconHelpers.FromRelativePath(vsCodeIconPath);

            Instances.Add(instance);
        }


        // Gets the executablePath and AppData foreach instance of VSCode
        public static void LoadVSCodeInstances()
        {
            var environmentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User) ?? string.Empty;
            environmentPath += (environmentPath.Length > 0 && environmentPath.EndsWith(';') ? string.Empty : ";") + Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
            Debug.WriteLine(environmentPath);
            var paths = environmentPath
                .Split(';')
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Where(x => x.Contains("VS Code", StringComparison.OrdinalIgnoreCase)
                    || x.Contains("VSCodium", StringComparison.OrdinalIgnoreCase)
                    || x.Contains("vscode", StringComparison.OrdinalIgnoreCase)
                    || x.Contains("cursor", StringComparison.OrdinalIgnoreCase)
                    ).ToArray();


            foreach (var path in paths)
            {
                Debug.WriteLine(path);
                if (!Directory.Exists(path))
                {
                    continue;
                }

                if (path.Contains("cursor", StringComparison.OrdinalIgnoreCase))
                {
                    DoLoadCursorInstance(path);
                }
                else
                {
                    DoLoadVsCodeInstance(path);
                }
            }

            Console.WriteLine($"Loaded {Instances.Count} VSCode instances");
        }
    }
}
