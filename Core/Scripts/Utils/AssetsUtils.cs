using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AALUND13Cards.Core.Utils {
    public class AssetsUtils {
        public static AssetBundle LoadAssetBundle(string bundleName, Assembly assembly) {
            string asmName = assembly.FullName;
            LoggerUtils.LogInfo($"Loading embedded asset bundle. Using assembly: {asmName}", true);
            LoggerUtils.LogInfo($"bundleName requested: '{bundleName}'", true);

            string[] resourceNames = assembly.GetManifestResourceNames();
            LoggerUtils.LogInfo($"Found {resourceNames.Length} embedded resource(s) in assembly:", true);
            foreach(string rn in resourceNames) {
                LoggerUtils.LogInfo($"  '{rn}'", true);
            }

            string matched = null;
            try {
                matched = resourceNames.Single(str => str.EndsWith(bundleName));
                LoggerUtils.LogInfo($"Matched resource name: '{matched}'", true);
            } catch(Exception ex) {
                LoggerUtils.LogError($"Exception while matching resource name: {ex.GetType().Name} — {ex.Message}");
            }

            if(matched == null) {
                LoggerUtils.LogError($"No resource name ending with '{bundleName}' was found.");
                return null;
            }

            try {
                using(Stream stream = assembly.GetManifestResourceStream(matched)) {
                    if(stream == null) {
                        LoggerUtils.LogError($"GetManifestResourceStream returned null for resource '{matched}'");
                        return null;
                    }
                    AssetBundle bundle = AssetBundle.LoadFromStream(stream);
                    if(bundle == null) {
                        LoggerUtils.LogError($"AssetBundle.LoadFromStream returned null for resource '{matched}'");
                    } else {
                        LoggerUtils.LogInfo($"Successfully loaded AssetBundle from resource '{matched}'", true);
                    }
                    return bundle;
                }
            } catch(Exception ex) {
                LoggerUtils.LogError($"Exception loading from stream: {ex.GetType().Name} — {ex.Message}");
                return null;
            }
        }
    }
}
