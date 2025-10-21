using AALUND13Cards.Core;
using AALUND13Cards.Core.Cards;
using AALUND13Cards.Core.Utils;
using AALUND13Cards.Devil.Handlers;
using BepInEx;
using HarmonyLib;
using System;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Devil {
    [BepInDependency("AALUND13.Cards.Core")]

    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    internal class AAC_Devil : BaseUnityPlugin {
        internal const string ModId = "AALUND13.Cards.Devil";
        internal const string ModName = "AALUND13 Devil Cards";
        internal const string Version = "1.0.0";

        private static AssetBundle assets;

        private void Awake() {
            assets = AssetsUtils.LoadAssetBundle("aac_devil_assets", typeof(AAC_Devil).Assembly);
            if(assets != null) {
                new Harmony(ModId).PatchAll();
            }

            gameObject.AddComponent<DevilCardsHandler>();
        }

        private void Start() {
            if(assets == null) {
                Unbound.BuildModal("AALUND13 Cards Error", $"The mod \"{ModName}\" assets failled to load, All the cards will be disable in this mod");
                throw new NullReferenceException($"Failled to load \"{ModName}\" assets");
            }

            if(AAC_Core.Plugins.Exists(plugin => plugin.Info.Metadata.GUID == "com.willuwontu.rounds.tabinfo"))
                TabinfoInterface.Setup();

            assets.LoadAsset<GameObject>("DevilModCards").GetComponent<CardResgester>().RegisterCards();
        }
    }
}