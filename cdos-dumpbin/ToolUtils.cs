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
        private static List<string> fileAbsPath_ = new List<string>();

        public static void initFilesAbsolutePath(string folder, string endsWith){
            try {
                DirectoryInfo dir = new DirectoryInfo(folder);
                FileSystemInfo[] fsinfos = dir.GetFileSystemInfos();
                foreach (FileSystemInfo fsinfo in fsinfos){
                    if (fsinfo is DirectoryInfo) {
                        initFilesAbsolutePath(fsinfo.FullName, endsWith);
                    } else if (fsinfo.FullName.EndsWith(endsWith, StringComparison.OrdinalIgnoreCase)){
                        fileAbsPath_.Add(fsinfo.FullName);
                    }
                }
            } catch (Exception e) {
                //Console.WriteLine(e.ToString());
            }
        }

        public static List<string> getFileAbsPath() {
            return fileAbsPath_;
        }

        public static string readLine(string path) {
            string line = "";
            try {
                using (StreamReader sr = new StreamReader(path, Encoding.Default)) {
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
    }
}
