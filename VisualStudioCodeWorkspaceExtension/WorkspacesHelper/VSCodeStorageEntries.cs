// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace VisualStudioCodeWorkspaceExtension.WorkspacesHelper
{
    // v1.64 uses AppData\Roaming\Code\User\globalStorage\state.vscdb - history.recentlyOpenedPathsList
    [JsonSerializable(typeof(VSCodeStorageEntries))]
    public class VSCodeStorageEntries
    {
        [JsonPropertyName("entries")]
        public List<VSCodeWorkspaceEntry> Entries { get; set; }
    }
}
