using AALUND13Cards.Core.Cards;
using AALUND13Cards.Core.Utils;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnboundLib;
using UnboundLib.Utils.UI;
using UnityEngine;

namespace AALUND13Cards.Core {
    public static class AACMenu {
        public static Action OnMenuRegister;
        
        internal static ConfigEntry<bool> DebugMode;
        
        private static GameObject ModulesMenu;

        internal static void RegesterMenu(ConfigFile config) {
            Unbound.RegisterMenu("AALUND13 Cards", () => { }, NewGui, null, false);
            DebugMode = config.Bind(AAC_Core.ModName, "DebugMode", false, "Enabled or disabled Debug Mode");
        }

        public static GameObject CreateModuleMenu(string moduleName) {
            LoggerUtils.LogInfo($"Is ModulesMenu null?: {ModulesMenu == null}");
            var menu = MenuHandler.CreateMenu($"{moduleName}", () => { }, ModulesMenu, parentForMenu: ModulesMenu.transform.parent.gameObject);
            MenuHandler.CreateText($"<b>{moduleName}</b>", menu, out TextMeshProUGUI _, 70);
            return menu;
        }

        public static GameObject CreateModuleMenuWithReadmeGenerator(string moduleName, string version, CardResgester cardResgester) {
            GameObject menu = CreateModuleMenu(moduleName);
#if DEBUG
            AAC_Core.Instance.ExecuteAfterFrames(2, () => {
                List<CardInfo> cardInfos = cardResgester.Cards.Select(obj => obj.GetComponent<CardInfo>()).ToList();
                MenuHandler.CreateButton($"Generate Readme: {moduleName}", menu, () => {
                    string readme = ReadmeGenerator.CreateReadme(moduleName, version, cardInfos);
                    GUIUtility.systemCopyBuffer = readme;
                });
            });
#endif
            return menu;
        }

        public static void AddBlank(GameObject menu, int space = 30) {
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, space);
        }

        private static void NewGui(GameObject menu) {
            ModulesMenu = MenuHandler.CreateMenu("Modules", () => { }, menu, parentForMenu: menu.transform.parent.gameObject);
            MenuHandler.CreateToggle(DebugMode.Value, "<#c41010> Debug Mode", menu, (val) => DebugMode.Value = val, 30);

#if DEBUG
            GameObject ModuleMenu = CreateModuleMenu(AAC_Core.ModName);
            MenuHandler.CreateButton($"Generate Full Readme", ModuleMenu, () => {
                string readme = ReadmeGenerator.CreateReadme("AALUND13 Cards", AAC_Core.FullVersion, CardResgester.AllModCards);
                GUIUtility.systemCopyBuffer = readme;
            });
#endif

            OnMenuRegister?.Invoke();
        }
    }
}
