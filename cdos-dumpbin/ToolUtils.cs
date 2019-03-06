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
        private static void generateFilesAbsolutePath(string folder, HashSet<string> suffix, ref List<string> res) {
            try {
                DirectoryInfo dir = new DirectoryInfo(folder);
                FileSystemInfo[] fsinfos = dir.GetFileSystemInfos();
                foreach (FileSystemInfo fsinfo in fsinfos){
                    if (fsinfo is DirectoryInfo) {
                        generateFilesAbsolutePath(fsinfo.FullName.ToLower(), suffix, ref res);
                    } else if (isEndsWith(fsinfo.FullName.ToLower(), suffix)) {
                        res.Add(fsinfo.FullName.ToLower());
                    }
                }
            } catch (Exception e) {
                //Console.WriteLine(e.ToString());
            }
        }
        /// <summary>
        /// 生成绝对路径，返回HashSet
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="suffix"></param>
        /// <param name="res"></param>
        private static void generateFilesAbsolutePath(string folder, HashSet<string> suffix, ref HashSet<string> res) {
            try {
                DirectoryInfo dir = new DirectoryInfo(folder);
                FileSystemInfo[] fsinfos = dir.GetFileSystemInfos();
                foreach (FileSystemInfo fsinfo in fsinfos) {
                    if (fsinfo is DirectoryInfo) {
                        generateFilesAbsolutePath(fsinfo.FullName.ToLower(), suffix, ref res);
                    } else if (isEndsWith(fsinfo.FullName.ToLower(), suffix)) {
                        res.Add(fsinfo.FullName.ToLower());
                    }
                }
            } catch (Exception e) {
                //Console.WriteLine(e.ToString());
            }
        }
        /// <summary>
        /// 生成绝对路径，返回map
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="endsWith"></param>
        /// <param name="res"></param>
        private static void generateFilesAbsolutePath(string folder, HashSet<string> suffix, ref Dictionary<string, string> res) {
            try {
                DirectoryInfo dir = new DirectoryInfo(folder);
                FileSystemInfo[] fsinfos = dir.GetFileSystemInfos();
                foreach (FileSystemInfo fsinfo in fsinfos) {
                    if (fsinfo is DirectoryInfo) {
                        generateFilesAbsolutePath(fsinfo.FullName.ToLower(), suffix, ref res);
                    } else if (isEndsWith(fsinfo.FullName.ToLower(), suffix)) {
                        res[fsinfo.Name.ToLower()] = fsinfo.FullName.ToLower();
                    }
                }
            } catch (Exception e) {
                //Console.WriteLine(e.ToString());
            }
        }
        /// <summary>
        /// 生成folder路径下，所有后缀为endsWith文件名
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="endsWith"></param>
        private static void generateFilesName(string folder, HashSet<string> suffix, ref List<string> res) {
            try {
                DirectoryInfo dir = new DirectoryInfo(folder);
                FileSystemInfo[] fsinfos = dir.GetFileSystemInfos();
                foreach (FileSystemInfo fsinfo in fsinfos) {
                    if (fsinfo is DirectoryInfo) {
                        generateFilesName(fsinfo.FullName.ToLower(), suffix, ref res);
                    } else if (isEndsWith(fsinfo.Name.ToLower(), suffix)) {
                        res.Add(fsinfo.Name.ToLower());
                    }
                }
            } catch (Exception e) {
                //Console.WriteLine(e.ToString());
            }
        }
        /// <summary>
        /// 生成folder路径下，所有后缀为endsWith文件名,返回HashSet
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="endsWith"></param>
        private static void generateFilesName(string folder, HashSet<string> suffix, ref HashSet<string> res) {
            try {
                DirectoryInfo dir = new DirectoryInfo(folder);
                FileSystemInfo[] fsinfos = dir.GetFileSystemInfos();
                foreach (FileSystemInfo fsinfo in fsinfos) {
                    if (fsinfo is DirectoryInfo) {
                        generateFilesName(fsinfo.FullName.ToLower(), suffix, ref res);
                    } else if (isEndsWith(fsinfo.Name.ToLower(), suffix)) {
                        res.Add(fsinfo.Name.ToLower());
                    }
                }
            } catch (Exception e) {
                //Console.WriteLine(e.ToString());
            }
        }
        /// <summary>
        /// 获取folder路径下，所有后缀为endsWith文件绝对路径
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="endsWith"></param>
        /// <returns></returns>
        public static List<string> getFilesAbsolutePath(string folder, HashSet<string> suffix) {
            List<string> res = new List<string>();
            generateFilesAbsolutePath(folder, suffix, ref res);
            return res;
        }
        /// <summary>
        /// 获取去重的绝对路径
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static HashSet<string> getFilterAbsolutePath(string folder, HashSet<string> suffix) {
            HashSet<string> res = new HashSet<string>();
            generateFilesAbsolutePath(folder, suffix, ref res);
            return res;
        }
        /// <summary>
        /// 生成<k,v>格式路径
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static Dictionary<string, string> getMapAbsolutePath(string folder, HashSet<string> suffix) {
            Dictionary<string, string> res = new Dictionary<string, string>();
            generateFilesAbsolutePath(folder, suffix, ref res);
            return res;
        }
        /// <summary>
        /// 获取folder路径下，所有后缀为endsWith文件名
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="endsWith"></param>
        /// <returns></returns>
        public static List<string> getFilesName(string folder, HashSet<string> suffix) {
            List<string> res = new List<string>();
            generateFilesName(folder, suffix, ref res);
            return res;
        }
        /// <summary>
        /// 返回去重文件名
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static HashSet<string> getFilterName(string folder, HashSet<string> suffix) {
            HashSet<string> res = new HashSet<string>();
            generateFilesName(folder, suffix, ref res);
            return res;
        }
        /// <summary>
        /// 字符串str中是否包含endsWith后缀
        /// </summary>
        /// <param name="str"></param>
        /// <param name="endsWith"></param>
        /// <returns></returns>
        public static bool isEndsWith(string str, HashSet<string> suffix) {
            foreach (string s in suffix) {
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
        public static string getToolsPath() {
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
        public static Dictionary<string, HashSet<string>> getDependencies(string folder, HashSet<string> suffix) {
            Dictionary<string, HashSet<string>> res = new Dictionary<string, HashSet<string>>();
            List<string> files = new List<string>(new HashSet<string>(getFilesAbsolutePath(folder, suffix)));
            for (int i = 0; i < files.Count; ) {
                string args = "/DEPENDENTS";
                int end = i + Math.Min(files.Count-i, 50); //每次最多分析50个模块
                for ( ; i < end; ++i) {
                    string file = files[i].ToLower();
                    if (!res.Keys.Contains(file.Substring(file.LastIndexOf('\\') + 1))) {
                        args += (" \"" + file + "\"");
                    }
                }
                res = res.Concat(dumpbin(args)).ToDictionary(k => k.Key, v => v.Value);
            }
            return res;
        }
        /// <summary>
        /// 分析DLL的依赖
        /// </summary>
        /// <param name="dlls"></param>
        /// <returns></returns>
        public static Dictionary<string, HashSet<string>> getDependencies(List<string> dllsPath) {
            Dictionary<string, HashSet<string>> res = new Dictionary<string, HashSet<string>>();
            for (int i = 0; i < dllsPath.Count; ) {
                string args = "/DEPENDENTS";
                int end = i + Math.Min(dllsPath.Count - i, 50); //每次最多分析50个模块
                for (; i < end; ++i) {
                    string dllPath = dllsPath[i].ToLower();
                    if (!res.Keys.Contains(dllPath.Substring(dllPath.LastIndexOf('\\') + 1))) {
                        args += (" \"" + dllPath + "\"");
                    }
                }
                res = res.Concat(dumpbin(args)).ToDictionary(k => k.Key, v => v.Value);
            }
            return res;
        }
        /// <summary>
        /// 分析DLL依赖
        /// </summary>
        /// <param name="mp"></param>
        /// <param name="dlls"></param>
        /// <returns></returns>
        public static Dictionary<string, HashSet<string>> getDependencies(Dictionary<string, string> mp, List<string> dlls) {
            Dictionary<string, HashSet<string>> res = new Dictionary<string, HashSet<string>>();
            res.Add("404", new HashSet<string>());
            for (int i = 0; i < dlls.Count; ) {
                string args = "/DEPENDENTS";
                int end = i + Math.Min(dlls.Count - i, 50); //每次最多分析50个模块
                for (; i < end; ++i) {
                    string dll = dlls[i].ToLower();
                    if (dll.EndsWith("(?)")){
                        continue;
                    }
                    if (mp.Keys.Contains(dll)) {
                        if (!res.Keys.Contains(dll)){
                            args += (" \"" + mp[dll] + "\"");
                        }
                    } else {
                        res["404"].Add(dll+"(?)");
                    }
                }
                res = res.Concat(dumpbin(args)).ToDictionary(k => k.Key, v => v.Value);
            }
            if (res["404"].Count <= 0) {
                res.Remove("404");
            }
            return res;
        }
        /// <summary>
        /// 调用Microsoft工具dumpbin.exe获取dll相关依赖
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static Dictionary<string, HashSet<string>> dumpbin(string args) {
            Dictionary<string, HashSet<string>> res = new Dictionary<string, HashSet<string>>();
            try {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo(getToolsPath(), args);
                process.StartInfo = startInfo;
                process.StartInfo.UseShellExecute = false;
                startInfo.RedirectStandardInput = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.CreateNoWindow = true;
                process.Start();
                res = getRecognized(process);
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
        private static Dictionary<string, HashSet<string>> getRecognized(Process process) {
            Dictionary<string, HashSet<string>> res = new Dictionary<string, HashSet<string>>();
            for (string name = ""; !process.StandardOutput.EndOfStream; ) {
                string line = process.StandardOutput.ReadLine().Trim().ToLower();
                if (line.StartsWith("dump")) {
                    //获取被分析文件名称
                    name = line.Substring(line.LastIndexOf('\\') + 1);
                    if (!res.ContainsKey(name)) {
                        res.Add(name, new HashSet<string>());
                    }
                } else if (line.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)) {
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
        public static Dictionary<string, HashSet<string>> getDependenciesNoSelf(string folder, HashSet<string> suffix) {
            Dictionary<string, HashSet<string>> res = new Dictionary<string, HashSet<string>>();
            HashSet<string> self = getFilterName(folder, new HashSet<string> { ".dll" });
            res = getDependencies(folder, suffix);
            foreach (var item in res) {//移除重复内容
                item.Value.ExceptWith(self);
            }
            return res;
        }
        /// <summary>
        /// 返回字典Values的交集
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static HashSet<string> getValuesUnion(Dictionary<string, HashSet<string>> dict) {
            HashSet<string> res = new HashSet<string>();
            foreach (var kv in dict) {
                res.UnionWith(kv.Value);
            }
            return res;
        }
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        public static void writeToFile(string path, string data, bool erase = true) {
            using (StreamWriter sw = new StreamWriter(path, erase, Encoding.UTF8)) {
                sw.Write(data);
            }
        }
        /// <summary>
        /// 默认以map形式，递归路径
        /// </summary>
        /// <param name="endsWith"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static Dictionary<string, string> getRecMapAbsolutePath(HashSet<string> folder, HashSet<string> suffix) {
            Dictionary<string, string> res = new Dictionary<string, string>();
            foreach (var s in folder) {
                Dictionary<string, string> mp = getMapAbsolutePath(s, suffix);
                foreach (var item in mp) {
                    if (!res.ContainsKey(item.Key)) {
                        res.Add(item.Key, item.Value);
                    }
                }
            }
            return res;
        }
        /// <summary>
        /// 查询递归依赖
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static HashSet<string> getRecDependenciesNoSelf(string folder, HashSet<string> suffix) {
            HashSet<string> res = getValuesUnion(getDependenciesNoSelf(folder, suffix));
            HashSet<string> folders = new HashSet<string>();
            folders.Add(@"C:\Windows\System32");
            folders.Add(@"C:\Windows\winsxs");
            folders.Add(folder);
            Dictionary<string, string> mp = getRecMapAbsolutePath(folders, new HashSet<string>(new string[] { ".dll" }));
            //Console.WriteLine(mp.Count);
            //Console.WriteLine(string.Join(" ", mp.ToArray()));
            List<string> dlls = new List<string>(res);
            res.Clear();
            for (; ; ) {
                HashSet<string> layer = new HashSet<string>(res);
                res.UnionWith(getValuesUnion(getDependencies(mp, dlls)));
                if (!res.IsSubsetOf(layer)) {
                    break;
                }
                dlls = new List<string>(res.Except<string>(layer));
            }

            HashSet<string> self = getFilterName(folder, new HashSet<string> { ".dll" });
            foreach (var hs in self) {
                //Console.WriteLine(hs);
                res.Remove(hs + "(?)");
            }

            return res;
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