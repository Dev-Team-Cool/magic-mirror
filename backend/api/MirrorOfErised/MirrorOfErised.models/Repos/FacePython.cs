using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MirrorOfErised.models.Repos
{
    public class FacePython
    {
        private ScriptEngine _engine;

        public FacePython()
        {
            _engine = Python.CreateEngine();
        }

        /*        public TResult RunFromString<TResult>(string item, string variableName)
                {
                    // for easier debugging write it out to a file and call: _engine.CreateScriptSourceFromFile(filePath);
                    ScriptSource source = _engine.CreateScriptSourceFromFile(@"F:\School\magic-mirror\backend\api\MirrorOfErised\MirrorOfErised\Controllers\test.py", scope);
                    scope.SetVariable
                    var argv = new List<string>();
                    argv.Add("");
                    argv.Add(item);

                    _engine.GetSysModule().SetVariable("argv", argv);

                    CompiledCode cc = source.Compile();

                    ScriptScope scope = _engine.CreateScope();
                    cc.Execute(scope);

                    return scope.GetVariable<TResult>(variableName);
                }*/

        public string validateImage(string item)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"F:\SideProjects\AI\venv\Scripts\python.exe";

            
            var script = @"F:\School\magic-mirror\backend\api\MirrorOfErised\MirrorOfErised\Controllers\test.py";


            start.Arguments = $"\"{script}\" \"{item}\"";

            start.UseShellExecute = false;
            start.CreateNoWindow = true;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;

            var errors = "";
            var results = "";
            using (Process process = Process.Start(start))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
                /*using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }*/
            }

            if (errors == "")
            {
                return results;
            }
            else
            {
                return errors;
            }

        }
    }
}
