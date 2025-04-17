using System.Text.Json.Serialization;
using VisualStudioCodeWorkspaceExtension.WorkspacesHelper;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(VSCodeStorageEntries))]
[JsonSerializable(typeof(VSCodeWorkspaceEntry))]
public partial class VSCodeJsonSerializerContext : JsonSerializerContext
{

}