using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using IronPython.Modules;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MirrorOfErised.models.Repos
{
    public class PythonRunner
    {
        private readonly IConfiguration _configuration;
        public PythonRunner(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private ProcessStartInfo GetDefaultStartInfo()
        {
            var startInfo =  new ProcessStartInfo { FileName = _configuration["UploadConfig:PythonPath"] };
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;

            return startInfo;
        }

        private async Task<RunnerResult> StartJob(ProcessStartInfo startInfo)
        {
            string errors = null;
            string results = null;
            using (Process process = Process.Start(startInfo))
            {
                errors = await process.StandardError.ReadToEndAsync();
                results = await process.StandardOutput.ReadToEndAsync();
            }

            return new RunnerResult() {Errors = errors, Output = results};
        }
        
        public async Task<bool> ValidateImage(ImageEntry item)
        {
            var start = GetDefaultStartInfo();

            string scriptSource = _configuration["UploadConfig:ValidationScriptPath"];
            start.Arguments = $"\"{scriptSource}\" \"{item.ImagePath}\"";

            RunnerResult result = await StartJob(start);

            if (!string.IsNullOrEmpty(result.Errors) || result.Output.Contains("NOK"))
                return false;
            return true;
        }

        public async Task<RunnerResult> StartTraining()
        {
            var startInfo = GetDefaultStartInfo();
            string scriptSource = _configuration["TrainConfig:TrainScriptPath"];
            startInfo.Arguments = $"\"{scriptSource}";

            RunnerResult result = await StartJob(startInfo);
            return result;
        }
    }
}
