using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cdos_dumpbin {
    class ToolUtils {
        /// <summary>
        /// 生成folder路径下，所有后缀为endsWith文件绝对路径
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="endsWith"></param>
        private static void generateFilesAbsolutePath(string folder, List<string> endsWith, ref List<string> res){
            try {
                DirectoryInfo dir = new DirectoryInfo(folder);
                FileSystemInfo[] fsinfos = dir.GetFileSystemInfos();
                foreach (FileSystemInfo fsinfo in fsinfos){
                    if (fsinfo is DirectoryInfo) {
                        generateFilesAbsolutePath(fsinfo.FullName, endsWith, ref res);
                    } else if (strIsEndsWith(fsinfo.FullName, endsWith)) {
                        res.Add(fsinfo.FullName);
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }
        /// <summary>
        /// 生成folder路径下，所有后缀为endsWith文件相对路径
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="endsWith"></param>
        private static void generateFilesPath(string folder, List<string> endsWith, ref List<string> res) {
            try {
                DirectoryInfo dir = new DirectoryInfo(folder);
                FileSystemInfo[] fsinfos = dir.GetFileSystemInfos();
                foreach (FileSystemInfo fsinfo in fsinfos) {
                    if (fsinfo is DirectoryInfo) {
                        generateFilesPath(fsinfo.FullName, endsWith, ref res);
                    } else if (strIsEndsWith(fsinfo.Name, endsWith)) {
                        res.Add(fsinfo.Name);
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }
        /// <summary>
        /// 获取folder路径下，所有后缀为endsWith文件绝对路径
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="endsWith"></param>
        /// <returns></returns>
        public static List<string> getFilesAbsolutePath(string folder, List<string> endsWith) {
            List<string> res = new List<string>();
            generateFilesAbsolutePath(folder, endsWith, ref res);
            return res;
        }
        /// <summary>
        /// 获取folder路径下，所有后缀为endsWith文件相对路径
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="endsWith"></param>
        /// <returns></returns>
        public static List<string> getFilesPath(string folder, List<string> endsWith) {
            List<string> res = new List<string>();
            generateFilesPath(folder, endsWith, ref res);
            return res;
        }
        /// <summary>
        /// 字符串str中是否包含endsWith后缀
        /// </summary>
        /// <param name="str"></param>
        /// <param name="endsWith"></param>
        /// <returns></returns>
        public static bool strIsEndsWith(string str, List<string> endsWith) {
            foreach (string s in endsWith) {
                if (str.EndsWith(s, StringComparison.OrdinalIgnoreCase)) {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取dumpbin.exe软件路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string getDumpbinPath() {
            string line = "";
            try {
                using (StreamReader sr = new StreamReader("./config.ini", Encoding.Default)) {
                    line = sr.ReadToEnd();
                }
            } catch (Exception e) {
                //Console.WriteLine(e.ToString());
                line = "";
            }
            return line;
            /*while ((line = sr.ReadLine()) != null) {
                Console.WriteLine(line.ToString());
            }*/
        }
        /// <summary>
        /// 获取某目录下后缀为endsWith的文件，它的所有依赖
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="endsWith"></param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> getDLLDependencies(string folder, List<string> endsWith) {
            Dictionary<string, List<string>> res = new Dictionary<string, List<string>>();
            //List去重复
            List<string> files = new List<string>(new HashSet<string>(getFilesAbsolutePath(folder, endsWith)));
            for (int i = 0; i < files.Count; ) {
                string args = "/DEPENDENTS";
                int end = i + Math.Min(files.Count-i, 50); //每次最多分析50个模块
                for ( ; i < end; ++i) {
                    if (!res.Keys.Contains(files[i].Substring(files[i].LastIndexOf('\\') + 1))) {
                        args += (" \"" + files[i] + "\"");
                    }
                }
                //合并两个Dictionary
                res = res.Concat(dumpbin(args)).ToDictionary(k => k.Key, v => v.Value);
            }
            return res;
        }
        /// <summary>
        /// 调用Microsoft工具dumpbin.exe获取dll相关依赖
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static Dictionary<string, List<string>> dumpbin(string args) {
            Dictionary<string, List<string>> res = new Dictionary<string, List<string>>();
            try {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo(getDumpbinPath(), args);
                process.StartInfo = startInfo;
                process.StartInfo.UseShellExecute = false;
                startInfo.RedirectStandardInput = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.CreateNoWindow = true;
                process.Start();
                res = getRecognizedDLL(process);
                process.WaitForExit();
                process.Close();
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
            return res;
        }
        
        /// <summary>
        /// 识别数据流中的dll依赖，并放于Dictionary中
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        private static Dictionary<string, List<string>> getRecognizedDLL(Process process) {
            Dictionary<string, List<string>> res = new Dictionary<string,List<string>>();
            for (string name = ""; !process.StandardOutput.EndOfStream; ) {
                string line = process.StandardOutput.ReadLine().Trim();
                if (line.StartsWith("Dump")) {
                    //获取被分析文件名称
                    name = line.Substring(line.LastIndexOf('\\') + 1);
                    if (!res.ContainsKey(name)) {
                        //Console.WriteLine(name);
                        res.Add(name, new List<string>());
                    }
                } else if (line.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)) {
                    //Console.WriteLine(line);
                    res[name].Add(line);
                }
            }
            return res;
        }

        /// <summary>
        /// 获取某目录下后缀为endsWith的文件，它的所有依赖，当contain为false则不包含当前路径下自带dll
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="endsWith"></param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> getDLLDependenciesNoSelf(string folder, List<string> endsWith) {
            Dictionary<string, List<string>> res = new Dictionary<string, List<string>>();
            List<string> self = new List<string>(new HashSet<string>(getFilesPath(folder, new List<string>{".dll"})));
            res = getDLLDependencies(folder, endsWith);
            foreach (var item in res) {//移除重复内容
                for (var i = item.Value.Count - 1; i >= 0; --i) {
                    //Console.WriteLine(item.Value[i]);
                    if (self.Contains(item.Value[i])) {
                        //Console.WriteLine("kill");
                        item.Value.Remove(item.Value[i]);
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// 返回字典Values的交集
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static List<string> getValuesListUnion(Dictionary<string, List<string>> dict) {
            List<string> res = new List<string>();
            foreach (var kv in dict) {
                res = res.Union(kv.Value).ToList<string>();
            }
            return res;
        }
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        public static void writeToFile(string path, string data) {
            using (StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8)) {
                sw.Write(data);
            }
        }

    }
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