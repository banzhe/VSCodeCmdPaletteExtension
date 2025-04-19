// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace VisualStudioCodeWorkspaceExtension;

public partial class VisualStudioCodeWorkspaceExtensionCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;
    public VisualStudioCodeWorkspaceExtensionCommandsProvider()
    {
        DisplayName = "Visual Studio Code Workspace";
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        _commands = [
            new CommandItem(new VisualStudioCodeWorkspaceExtensionPage()) { Title = DisplayName },
        ];
    }

    public override ICommandItem[] TopLevelCommands()
    {
        return _commands;
    }

}
