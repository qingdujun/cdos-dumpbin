using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cdos_dumpbin
{
    class Program
    {
        private static string dumpbin = "";// = @"C:\Program Files\Microsoft Visual Studio 10.0\VC\bin\dumpbin.exe";
        private static string folder = "";// @"C:\Program Files\Microsoft Office";
        private static string endsWith = @".dll";
        private static string type = "";//"dll";
        private static string config = "./config.ini";

        public static void start(string[] args) {
            for (int i = 0; i < args.Length; ++i) {
                switch (args[i]){
                    case "-folder": case "-f":
                        folder = args[++i];
                        break;
                    case "-type": case "-t":
                        type = args[++i];
                        break;
                    case "-end": case "-e":
                        endsWith = args[++i];
                        break;
                    default: break;
                }
            }
            if (folder == "" || type == "") {
                Console.WriteLine(@"Usage: cdos-dumpbin.exe -folder [C:\Windows\System32] -type [dll|function] [-end [.dll|.exe]]");
                return;
            }
            if ((dumpbin = ToolUtils.readLine(config)).Equals("")) {
                Console.WriteLine(@"Please configure the dumpbin.exe path in ./config.ini");
                return;
            }
            ToolUtils.initFilesAbsolutePath(folder, endsWith);
            switch (type) {
                case "d": case "dll":
                    new DLLs().outputDependents(dumpbin, ToolUtils.getFileAbsPath());
                    break;
                case "f": case "function":
                    break;
                default: break;
            }
        }

        static void Main(string[] args){
            start(args);
        }
    }
}
