using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitGui.Business.Queries
{
    public class QueryGit
    {
        ProcessStartInfo gitProcessInfo;
        Process gitProcess;

        public QueryGit()
        {
            gitProcessInfo = new ProcessStartInfo();
            gitProcess     = new Process();
        }

        public string GetCurrentBranch(string path, string workingDirectory)
        {
            Configure(path, workingDirectory, "branch");

            gitProcess.Start();

            var outputStream = gitProcess.StandardOutput;
            var errorStream = gitProcess.StandardError;

            while (!outputStream.EndOfStream)
            {
                var line = outputStream.ReadLine();
                if (line.Contains("*"))
                    return line.Replace("*", string.Empty);
            }

            return string.Empty;

            //TODO: error logging
            //if (errorStream.Peek() > 0)
            //    errorStream.ReadToEnd().Dump("ERROR!!!:");
        }

        public IEnumerable<string> GetModifiedFiles(string path, string workingDirectory)
        {
            Configure(path, workingDirectory, "diff --name-only");

            var outputStream = gitProcess.StandardOutput;
            var errorStream = gitProcess.StandardError;

            while (!outputStream.EndOfStream)
                yield return outputStream.ReadLine();

            //TODO: error logging
            //if (errorStream.Peek() > 0)
            //    errorStream.ReadToEnd().Dump("ERROR!!!:");
        }

        private void Configure(string path, string workingDirectory, string command)
        {
            gitProcessInfo.CreateNoWindow         = true;
            gitProcessInfo.FileName               = path;
            gitProcessInfo.RedirectStandardError  = true;
            gitProcessInfo.RedirectStandardOutput = true;
            gitProcessInfo.UseShellExecute        = false;
            gitProcessInfo.Arguments              = command;
            gitProcessInfo.WorkingDirectory       = workingDirectory;
            gitProcess.StartInfo                  = gitProcessInfo;
        }	
    }
}
