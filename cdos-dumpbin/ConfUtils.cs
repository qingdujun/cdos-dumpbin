using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace cdos_dumpbin {
    class ConfUtils {

        public static bool dependents(string folder) {
            List<string> baseDlls = ToolUtils.getValuesListUnion(ToolUtils.getDLLDependenciesNoSelf(folder, new List<string> { ".exe", ".dll" }));
            Console.WriteLine("\nDLLs dependents: folder=["+folder+"]\n\n");
            Console.WriteLine(string.Join(" ", baseDlls.ToArray()));
            Console.WriteLine("\nTotal: "+baseDlls.Count);
            Console.WriteLine("\n\nAnalysis completed\n\n");
            return true;
        }

        class Cmp1vsN {
            public Cmp1vsN(string[] args) {
                if (args.Length < 3) {
                    return;
                }
                this.name = args[0].Trim();
                this.version = args[1].Trim();
                this.path = args[2].Trim();
            }
            public string name;
            public string version;
            public string path;
        }

        public static bool cmp1VsN(string folder) {//"./conf_cmp_1vsN.ini"
            List<Cmp1vsN> cmpList = new List<Cmp1vsN>();
            using (StreamReader sr = new StreamReader(folder, Encoding.UTF8)) {
                for (string line; (line = sr.ReadLine()) != null; ) {
                    string[] res = line.Split(',');
                    Cmp1vsN cmp = new Cmp1vsN(res);
                    cmpList.Add(cmp);
                }
            }
            if (cmpList.Count < 2) {
                Console.WriteLine("./conf_cmp_1vsN.ini error.");
                return false;
            }
            string title = string.Format(@"软件名称,版本号,依赖的Windows系统DLL清单（非自带DLL）,统计（左）,与{0}相同的DLL,统计（左）,与{1}不同的DLL,统计（左）", cmpList[0].name, cmpList[0].name);
            string writePath = "./res_cmp_1vsN.csv";
            ToolUtils.writeToFile(writePath, title + Environment.NewLine, false);
            List<string> baseDlls = ToolUtils.getValuesListUnion(ToolUtils.getDLLDependenciesNoSelf(cmpList[0].path, new List<string> { ".exe", ".dll" }));
            string baseSoft = cmpList[0].name+","+cmpList[0].version+"," + string.Join(" ", baseDlls.ToArray()) + "," + baseDlls.Count;
            ToolUtils.writeToFile(writePath, baseSoft + Environment.NewLine);
            Console.WriteLine("Staring....\n");
            int count = 0;
            foreach (var soft in cmpList.Skip(1)) {
                Console.WriteLine("       Analyzing " + soft.name +"(v"+soft.version+")");
                Console.WriteLine("       Location: " + soft.path);
                string data = "";
                data += (soft.name + ","); //软件名称
                data += (soft.version+ ","); //版本号
                List<string> cmpDlls = ToolUtils.getValuesListUnion(ToolUtils.getDLLDependenciesNoSelf(soft.path, new List<string> { ".exe", ".dll" }));
                data += (string.Join(" ", cmpDlls.ToArray()) + ",");//依赖的Windows系统DLL清单（非自带DLL）
                data += (cmpDlls.Count + ",");
                List<string> intersect = DLLsHelper.getIntersectDependency(cmpList[0].path, soft.path);
                data += (string.Join(" ", intersect.ToArray()) + ",");
                data += (intersect.Count + ",");
                List<string> except = DLLsHelper.getExceptDependency(soft.path, cmpList[0].path);
                data += (string.Join(" ", except.ToArray()) + ",");
                data += (except.Count);
                ToolUtils.writeToFile(writePath, data + Environment.NewLine);
                Console.WriteLine("       has analyzed " + (++count) + "\n");
            }
            Console.WriteLine("\n\nAnalysis completed\n\n");

            return true;
        }
    }
}
