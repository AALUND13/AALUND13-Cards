using BepInEx.Configuration;
using TMPro;
using UnboundLib;
using UnboundLib.Utils.UI;
using UnityEngine;

namespace AALUND13Card.Handlers {
    internal class ConfigHandler {
        public static ConfigEntry<bool> DetailsMode;
        public static ConfigEntry<bool> DebugMode;

        public static void RegesterMenu(ConfigFile config) {
            Unbound.RegisterMenu(AALUND13_Cards.ModName, () => { }, NewGui, null, false);
            DebugMode = config.Bind(AALUND13_Cards.ModName, "DebugMode", false, "Enabled or disabled Debug Mode");
        }

        public static void addBlank(GameObject menu) {
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 30);
        }

        public static void NewGui(GameObject menu) {
            MenuHandler.CreateToggle(DebugMode.Value, "<#c41010> Debug Mode", menu, DebugModeChanged, 30);
            void DebugModeChanged(bool val) {
                DebugMode.Value = val;
            }
        }
    }
}
