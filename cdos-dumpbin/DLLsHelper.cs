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
    class DLLsHelper{

        /// <summary>
        /// 求两目录依赖的交集
        /// </summary>
        /// <param name="baseFolder"></param>
        /// <param name="cmpFolder"></param>
        /// <param name="contain"></param>
        /// <returns></returns>
        public static List<string> getIntersectDependency(string baseFolder, string cmpFolder, bool contain = false) {
            var baseUnion = ToolUtils.getValuesListUnion(ToolUtils.getDLLDependenciesNoSelf(baseFolder, new List<string>{".exe", ".dll"}));
            var cmpUnion = ToolUtils.getValuesListUnion(ToolUtils.getDLLDependenciesNoSelf(cmpFolder, new List<string> { ".exe", ".dll" }));
            return baseUnion.Intersect(cmpUnion).ToList<string>();
        }
        /// <summary>
        /// 差集
        /// </summary>
        /// <param name="baseFolder"></param>
        /// <param name="cmpFolder"></param>
        /// <param name="contain"></param>
        /// <returns></returns>
        public static List<string> getExceptDependency(string baseFolder, string cmpFolder, bool contain = false) {
            var baseUnion = ToolUtils.getValuesListUnion(ToolUtils.getDLLDependenciesNoSelf(baseFolder, new List<string> { ".exe", ".dll" }));
            var cmpUnion = ToolUtils.getValuesListUnion(ToolUtils.getDLLDependenciesNoSelf(cmpFolder, new List<string> { ".exe", ".dll" }));
            return baseUnion.Except(cmpUnion).ToList<string>();
        }
        /// <summary>
        /// 并集
        /// </summary>
        /// <param name="baseFolder"></param>
        /// <param name="cmpFolder"></param>
        /// <param name="contain"></param>
        /// <returns></returns>
        public static List<string> getUnionDependency(string baseFolder, string cmpFolder, bool contain = false) {
            var baseUnion = ToolUtils.getValuesListUnion(ToolUtils.getDLLDependenciesNoSelf(baseFolder, new List<string> { ".exe", ".dll" }));
            var cmpUnion = ToolUtils.getValuesListUnion(ToolUtils.getDLLDependenciesNoSelf(cmpFolder, new List<string> { ".exe", ".dll" }));
            return baseUnion.Union(cmpUnion).ToList<string>();
        }
       
    }
}
