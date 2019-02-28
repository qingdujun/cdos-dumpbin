using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cdos_dumpbin{
    class DLLs{
        private int count_ = 50;
        private string output_ = @"dependents.csv";

        public DLLs() {

        }

        private string dependents(List<string> fileAbsPath, int begin, int end){
            string args = "/DEPENDENTS";
            for (int i = begin; i < end; ++i) {
                args += (" \""+fileAbsPath[i]+"\"");
            }
            //args += string.Join(" \"", fileAbsPath.Take(count).Skip(skip).ToArray());
            return args;
        }

        private string exports(int skip, int count){
            string args = @"/EXPORTS ";
            //args += string.Join(" ", absPath_.Take(count).Skip(skip).ToArray());
            return args;
        }

        private bool hasOperationPermission(string folder){
            var currentUserIdentity = Path.Combine(Environment.UserDomainName, Environment.UserName);
            DirectorySecurity fileAcl = Directory.GetAccessControl(folder);
            var userAccessRules = fileAcl.GetAccessRules(true, true,
                typeof(System.Security.Principal.NTAccount))
                .OfType<FileSystemAccessRule>()
                .Where(i => i.IdentityReference.Value == currentUserIdentity).ToList();
            return userAccessRules.Any(i => i.AccessControlType == AccessControlType.Deny);
        }

        private bool isNotHiddenFile(string folder){
            return ((new FileInfo(folder).Attributes & FileAttributes.Hidden) != FileAttributes.Hidden);
        }

        private bool dumpbin(string exePath, string args){
            try{
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo(exePath, args);
                process.StartInfo = startInfo;
                process.StartInfo.UseShellExecute = false;
                startInfo.RedirectStandardInput = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.CreateNoWindow = true;
                process.Start();
                //Console.WriteLine(process.StandardOutput.ReadToEnd());
                saveToCsv(process);
                process.WaitForExit();
                process.Close();
            }
            catch (Exception e){
                Console.WriteLine(e.ToString());
                return false;
            }
            return true;
        }
        /**
         * Dump of file E:\test\iscsied.dll
         *
         * File Type: DLL
         *   
         *   Image has the following dependencies:
         *
         *   msvcrt.dll
         *   ISCSIUM.dll
         *   ISCSIEXE.dll
         *   USERENV.dll
         *   api-ms-win-core-registry-l1-1-0.dll
         *   api-ms-win-core-errorhandling-l1-1-0.dll
         * →
         * **/
        private void saveToCsv(Process process){
            using (StreamWriter sw = new StreamWriter(output_, true)){
                string line = "";
                int num = 0;
                for (; !process.StandardOutput.EndOfStream; ){
                    string res = process.StandardOutput.ReadLine().Trim();
                    if (res.StartsWith("Dump")){
                        if (line.Equals("")){
                            line += Environment.NewLine;
                        }
                        else{
                            line.Trim();
                            line += ("," + num + Environment.NewLine);
                        }
                        string name = res.Substring(res.LastIndexOf('\\') + 1);
                        line += (name + ",");
                        num = 0;
                    }
                    else if (res.EndsWith(".dll")){
                        line += (res + "  ");
                        ++num;
                    }
                }
                line.Trim();
                line += ("," + num);
                sw.Write(line);
            }
        }

        public void outputDependents(string exePath, List<string> fileAbsPath){
            using (StreamWriter sw = new StreamWriter(output_, false)){
                string title = @"Current DLL,Directly dependent DLL, Number";
                sw.WriteLine(title);
            }
            Console.WriteLine("We are analyzing...\n");
            for (int begin = 0, end = 0; begin < fileAbsPath.Count; begin = end) {
                end = begin + Math.Min(count_, fileAbsPath.Count - begin);
                string args = dependents(fileAbsPath, begin, end);
                Console.WriteLine("     Analysing........" + end + "/" + fileAbsPath.Count);
                dumpbin(exePath, args);
            }
            Console.WriteLine("\nHas been completed!\nOutput: " + System.Environment.CurrentDirectory+"\\"+output_ + "\n");
        }

        private void outputExport(string folder, string endsWith){

        }

       
    }
}
