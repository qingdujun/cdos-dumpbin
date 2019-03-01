using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cdos_dumpbin{
    class Start{
        private static string visio = @"C:\Program Files\Microsoft Office";
        private static string edrawmax = @"C:\Program Files\edrawmax-cn 9.3";
        private static string dia = @"C:\Program Files\Dia";
        private static string openoffice4 = @"C:\Program Files\OpenOffice 4";
        private static string inkscape = @"C:\Program Files\Inkscape";
        private static string graphviz = @"C:\Program Files\Graphviz2.37";
        private static string qq = @"C:\Program Files\Tencent\QQ";
        private static string wechat = @"C:\Program Files\Tencent\WeChat";
        private static string thunder = @"C:\Program Files\Thunder Network\Thunder";
        private static string foxmail = @"C:\Foxmail 7.2";
        private static string sourceinsight = @"C:\Program Files\Source Insight 4.0";
        private static string youdao = @"C:\Program Files\Youdao";
        private static string cajviewer = @"C:\Program Files\TTKN";
        private static string meituxiuxiu = @"C:\Program Files\Meitu";
        private static string mathtype = @"C:\Program Files\MathType";
        private static string mailmaster = @"C:\Program Files\Netease\MailMaster";
        private static string photoshop = @"C:\Program Files\Adobe\Adobe Photoshop CC 2018 (32 Bit)";
        private static string foobar = @"C:\Program Files\foobar2000";
        private static string autocad = @"C:\Program Files\AutoCAD 2008";
        private static string zwcad = @"C:\Program Files\ZWCAD+ 2014";
        private static string iqiyi = @"C:\Program Files\IQIYI Video";
        private static string youku = @"C:\Program Files\YouKu";
        private static string baofeng = @"C:\Program Files\Baofeng";
        private static string qqplayer = @"C:\Program Files\Tencent\QQPlayer";
        private static string thunderplayer = @"C:\Program Files\Thunder Network\XMP";

        private static string cmp = qq;

        private static string writePath = @"res.csv";

        private static Dictionary<string, string> table = new Dictionary<string,string>{
                {@"亿图图示", edrawmax}, 
                {@"DIA Diagram Editor", dia},
                {@"Open Office Draw",openoffice4},
                {@"Inkscape",inkscape},
                {@"Graphviz", graphviz},
                {@"QQ", qq},
                {@"微信PC版", wechat},
                {@"迅雷", thunder},
                {@"Foxmail 7.2", foxmail},
                {@"Source Insight", sourceinsight},
                {@"有道云笔记", youdao},
                {@"知网阅读器（CAJViewer）", cajviewer},
                {@"美图秀秀", meituxiuxiu},
                {@"MathType", mathtype},
                {@"网易邮箱大师", mailmaster},
                {@"PhotoShop", photoshop},
                {@"Foobar", foobar},
                {@"AutoCAD", autocad},
                {@"中望CAD", zwcad},
                {@"爱奇艺客户端", iqiyi},
                {@"优酷客户端", youku},
                {@"暴风影音播放器", baofeng},
                {@"QQ影音", qqplayer},
                {@"迅雷影音", thunderplayer}
        };

        public static void start(string[] args) {
            string title = @"软件名称,版本号,依赖的Windows系统DLL清单（非自带DLL）,统计（左）,与Visio相同的DLL,统计（左）,与Visio不同的DLL,统计（左）";
            ToolUtils.writeToFile(writePath, title + Environment.NewLine);
            List<string> visioDlls = ToolUtils.getValuesListUnion(ToolUtils.getDLLDependenciesNoSelf(visio, new List<string>{".exe", ".dll"}));
            string visio2010 = @"Microsoft Visio, 2010,"+string.Join(" ", visioDlls.ToArray())+","+visioDlls.Count;
            ToolUtils.writeToFile(writePath, visio2010 + Environment.NewLine);
            Console.WriteLine("Staring....\n");
            int count = 0;
            foreach (var tb in table) {
                Console.WriteLine("       Analyzing " + tb.Key);
                Console.WriteLine("       Location: " + tb.Value);
                string data = "";
                data += (tb.Key+","); //软件名称
                data += ("0.0.0.0,"); //版本号
                List<string> cmpDlls = ToolUtils.getValuesListUnion(ToolUtils.getDLLDependenciesNoSelf(tb.Value, new List<string> { ".exe", ".dll" }));
                data += (string.Join(" ", cmpDlls.ToArray())+",");//依赖的Windows系统DLL清单（非自带DLL）
                data += (cmpDlls.Count+",");
                List<string> intersect = DLLsHelper.getIntersectDependency(visio, tb.Value);
                data += (string.Join(" ", intersect.ToArray()) + ",");
                data += (intersect.Count + ",");
                List<string> except = DLLsHelper.getExceptDependency(tb.Value, visio);
                data += (string.Join(" ", except.ToArray()) + ",");
                data += (except.Count);
                ToolUtils.writeToFile(writePath, data + Environment.NewLine);
                Console.WriteLine("       has analyzed " + (++count)+"\n");
            }
            Console.WriteLine("\n\nAnalysis completed\n\n");
        }

        static void Main(string[] args){
            start(args);
        }
    }
}


/*
 
 
 public static void start(string[] args) {

            List<string> visioDlls = ToolUtils.getDLLDependencies(visio, ".exe");
            ToolUtils.writeToFile(writePath, "----------visioDlls----------" + Environment.NewLine);
            ToolUtils.writeToFile(writePath, string.Join(" ", visioDlls.ToArray()) + Environment.NewLine + visioDlls.Count + Environment.NewLine);

            List<string> cmpDlls = ToolUtils.getDLLDependencies(cmp, ".exe");
            ToolUtils.writeToFile(writePath, Environment.NewLine + "cmpDlls:"+cmp + Environment.NewLine);
            ToolUtils.writeToFile(writePath, string.Join(" ", cmpDlls.ToArray()) + Environment.NewLine + cmpDlls.Count + Environment.NewLine);

            List<string> union = DLLsHelper.getUnionDependency(visio, cmp);
            List<string> except = DLLsHelper.getExceptDependency(cmp, visio);
            List<string> intersect = DLLsHelper.getIntersectDependency(visio, cmp);
            ToolUtils.writeToFile(writePath, Environment.NewLine + "intersect" + Environment.NewLine);
            ToolUtils.writeToFile(writePath, string.Join(" ", intersect.ToArray()) + Environment.NewLine + intersect.Count + Environment.NewLine);
            ToolUtils.writeToFile(writePath, Environment.NewLine + "union" + Environment.NewLine);
            ToolUtils.writeToFile(writePath, string.Join(" ", union.ToArray()) + Environment.NewLine + union.Count + Environment.NewLine);
            ToolUtils.writeToFile(writePath, Environment.NewLine + "except" + Environment.NewLine);
            ToolUtils.writeToFile(writePath, string.Join(" ", except.ToArray()) + Environment.NewLine + except.Count + Environment.NewLine);
        }
 
 
 */