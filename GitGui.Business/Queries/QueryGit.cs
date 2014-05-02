using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitGui.Business.Queries
{
    public class QueryGit
    {
        private readonly ProcessStartInfo gitProcessInfo;
        private readonly Process gitProcess;

        private string _path;
        private string _workingDirectory;
        private bool _gitSetup;

        public QueryGit()
        {
            gitProcessInfo = new ProcessStartInfo();
            gitProcess     = new Process();
        }

        public void SetRepo(string path, string workingDirectory)
        {
            _path = path;
            _workingDirectory = workingDirectory;
            Configure();
            _gitSetup = true;
        }

        public string GetCurrentBranch()
        {
            //Configure("branch");

            if (!_gitSetup) 
                return "An error has occurred";

            StreamReader outputStream;
            StreamReader errorStream;

            Run("branch", out outputStream, out errorStream);

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

        public IEnumerable<string> GetAllBranches()
        {
            StreamReader outputStream;
            StreamReader errorStream;

            Run("branch", out outputStream, out errorStream);

            while (!outputStream.EndOfStream)
                yield return outputStream.ReadLine();
        }

        public IEnumerable<string> GetModifiedFiles()
        {
            StreamReader outputStream;
            StreamReader errorStream;

            Run("diff --name-only", out outputStream, out errorStream);

            while (!outputStream.EndOfStream)
                yield return outputStream.ReadLine();

            //TODO: error logging
            //if (errorStream.Peek() > 0)
            //    errorStream.ReadToEnd().Dump("ERROR!!!:");
        }

        private void Configure()
        {
            gitProcessInfo.CreateNoWindow         = true;
            gitProcessInfo.FileName               = _path;
            gitProcessInfo.RedirectStandardError  = true;
            gitProcessInfo.RedirectStandardOutput = true;
            gitProcessInfo.UseShellExecute        = false;
            gitProcessInfo.WorkingDirectory       = _workingDirectory;
            gitProcess.StartInfo                  = gitProcessInfo;
        }

        private void Run(string commamd, out StreamReader outputStream, out StreamReader errorStream)
        {
            gitProcessInfo.Arguments = commamd;

            gitProcess.Start();

            outputStream = gitProcess.StandardOutput;
            errorStream  = gitProcess.StandardError;
        }
    }
}
