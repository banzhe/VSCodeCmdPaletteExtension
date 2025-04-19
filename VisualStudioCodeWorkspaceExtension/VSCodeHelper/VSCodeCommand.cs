using System.Diagnostics;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using VisualStudioCodeWorkspaceExtension.WorkspacesHelper;

namespace VisualStudioCodeWorkspaceExtension.VSCodeHelper;

internal sealed partial class VsCodeCommand(VSCodeWorkspace workspace) : InvokableCommand
{
    public VSCodeWorkspace Workspace { get; } = workspace;

    public override ICommandResult Invoke()
    {
        ShellHelpers.OpenCommandInShell(
            path: Workspace.VSCodeInstance.ExecutablePath,
            pattern: null,
            arguments: Workspace.WorkspaceType == WorkspaceType.ProjectFolder ? $"--folder-uri {Workspace.Path}" : $"--file-uri {Workspace.Path}",
            runWithHiddenWindow: true
            );
        return CommandResult.Hide();
    }
}
