﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using VisualStudioCodeWorkspaceExtension.VSCodeHelper;

namespace VisualStudioCodeWorkspaceExtension.WorkspacesHelper
{
    public class VSCodeWorkspace
    {
        public string Path { get; set; }

        public string RelativePath { get; set; }

        public string FolderName { get; set; }

        public string ExtraInfo { get; set; }

        public WorkspaceEnvironment WorkspaceEnvironment { get; set; }

        public WorkspaceType WorkspaceType { get; set; }

        public VSCodeInstance VSCodeInstance { get; set; }

        public string WorkspaceEnvironmentToString()
        {
            switch (WorkspaceEnvironment)
            {
                case WorkspaceEnvironment.Local: return "Local";
                case WorkspaceEnvironment.Codespaces: return "Codespaces";
                case WorkspaceEnvironment.RemoteSSH: return "SSH";
                case WorkspaceEnvironment.RemoteWSL: return "WSL";
                case WorkspaceEnvironment.DevContainer: return "DevContainer";
                case WorkspaceEnvironment.RemoteTunnel: return "Tunnel";
            }

            return string.Empty;
        }
    }

    public enum WorkspaceEnvironment
    {
        Local = 1,
        Codespaces = 2,
        RemoteWSL = 3,
        RemoteSSH = 4,
        DevContainer = 5,
        RemoteTunnel = 6,
    }

    public enum WorkspaceType
    {
        ProjectFolder = 1,
        WorkspaceFile = 2,
    }
}
