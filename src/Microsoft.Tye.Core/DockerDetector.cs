// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

namespace Microsoft.Tye
{
    public class DockerDetector
    {
        public static DockerDetector Instance { get; } = new DockerDetector();

        private DockerDetector()
        {
            Engine = "docker";
            try
            {
                int exitCode;
                try
                {
                    exitCode = RunVersionCommand("docker");
                }
                catch
                {
                    exitCode = RunVersionCommand("podman");
                    Engine = "podman";
                }
                IsInstalled = true;
                IsConnected = exitCode == 0;
            }
            catch
            { }
        }

        private int RunVersionCommand(string filename)
        {
            var result = ProcessUtil.RunAsync(filename, "version", throwOnError: false).GetAwaiter().GetResult();
            return result.ExitCode;
        }

        public bool IsInstalled { get; }

        public bool IsConnected { get; }

        public string Engine { get; set; }
    }
}
