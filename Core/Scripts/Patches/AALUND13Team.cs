using HarmonyLib;
using System.Reflection;
using UnboundLib;
using UnboundLib.Utils;
using UnityEngine;

namespace AALUND13Cards.Core.Patches {
    [HarmonyPatch(typeof(ExtraPlayerSkins))]
    public class AALUND13Team {
        [HarmonyPatch("GetTeamColorName")]
        [HarmonyPostfix]
        public static void PatchName(int teamID, ref string __result) {
            bool flag = teamID == TeamID;
            if(flag) {
                __result = "<u><b>AALUND13</b></u>";
            }
        }

        [HarmonyPatch("GetPlayerSkinColors")]
        [HarmonyPrefix]
        public static bool PatchGetSkin(int colorID, ref PlayerSkin __result) {
            bool isTeam = colorID != TeamID;
            if(isTeam) {
                return true;
            } else {
                if(SkinWhite == null) {
                    SkinWhite = CreateSkins(colorID, White);
                }
                __result = SkinWhite;
                return false;
            }
        }

        private static PlayerSkin CreateSkins(int colorID, PlayerSkin skinPrefab) {
            PropertyInfo property = typeof(PlayerSkinBank).GetProperty("Instance", BindingFlags.Static | BindingFlags.NonPublic);
            PlayerSkinBank playerSkinBank = (PlayerSkinBank)((property != null) ? property.GetValue(null, null) : null);

            PlayerSkin playerSkin = ((playerSkinBank != null) ? playerSkinBank.skins[colorID % 4].currentPlayerSkin : null);
            PlayerSkin component = Object.Instantiate<PlayerSkin>(playerSkin).gameObject.GetComponent<PlayerSkin>();

            Object.DontDestroyOnLoad(component);

            PlayerSkin skin = skinPrefab;
            component.color = skin.color;
            component.backgroundColor = skin.backgroundColor;
            component.winText = skin.winText;
            component.particleEffect = skin.particleEffect;

            PlayerSkinParticle componentInChildren = component.GetComponentInChildren<PlayerSkinParticle>();
            ParticleSystem component2 = componentInChildren.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = component2.main;
            ParticleSystem.MinMaxGradient startColor = main.startColor;

            startColor.colorMin = skin.backgroundColor;
            startColor.colorMax = skin.color;
            main.startColor = startColor;
            componentInChildren.SetFieldValue("startColor1", skin.backgroundColor);
            componentInChildren.SetFieldValue("startColor2", skin.color);

            return component;
        }

        private static PlayerSkin SkinWhite;

        private static readonly PlayerSkin White = new PlayerSkin {
            color = new Color(0.8f, 0.8f, 0.8f, 1.0f),
            backgroundColor = new Color(0.8f, 0.8f, 0.8f, 1f),
            winText = new Color(0.8f, 0.8f, 0.8f, 1.0f),
            particleEffect = new Color(0.8f, 0.8f, 0.8f, 1.0f)
        };

        public const int TeamID = 131;
    }
}
