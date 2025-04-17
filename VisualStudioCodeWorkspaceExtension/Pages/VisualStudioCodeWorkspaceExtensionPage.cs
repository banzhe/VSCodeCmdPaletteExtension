// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using VisualStudioCodeWorkspaceExtension.VSCodeHelper;
using VisualStudioCodeWorkspaceExtension.WorkspacesHelper;

namespace VisualStudioCodeWorkspaceExtension;

internal sealed partial class VisualStudioCodeWorkspaceExtensionPage : ListPage
{
    private readonly VSCodeWorkspacesApi _workspacesApi = new VSCodeWorkspacesApi();
    public VisualStudioCodeWorkspaceExtensionPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        Title = "Visual Studio Code Workspace";
        Name = "Open";
    }

    public override IListItem[] GetItems()
    {

        return [
            .. _workspacesApi.Workspaces
                .Select(x => new ListItem(new NoOpCommand()) { Title = x.FolderName })
        ];
    }
}
