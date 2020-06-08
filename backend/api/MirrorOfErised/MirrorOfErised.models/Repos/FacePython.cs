using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MirrorOfErised.models.Repos
{
    public class FacePython
    {
        private readonly IConfiguration _configuration;
        public FacePython(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool ValidateImage(ref ImageEntry item)
        {
            ProcessStartInfo start = new ProcessStartInfo { FileName = _configuration["UploadConfig:PythonPath"] };
            string scriptSource = _configuration["UploadConfig:ValidationScriptPath"];


            start.Arguments = $"\"{scriptSource}\" \"{item.ImagePath}\"";

            start.UseShellExecute = false;
            start.CreateNoWindow = true;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;

            string errors = null;
            string results = null;
            using (Process process = Process.Start(start))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }

            if (!string.IsNullOrEmpty(errors) || results.Contains("NOK"))
                return false;
            return true;
        }
    }
}
