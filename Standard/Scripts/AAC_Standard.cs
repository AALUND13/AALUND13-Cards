using AALUND13Cards.Core;
using AALUND13Cards.Core.Cards;
using AALUND13Cards.Core.Utils;
using BepInEx;
using HarmonyLib;
using System;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Standard {
    [BepInDependency("AALUND13.Cards.Core")]

    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    internal class AAC_Standard : BaseUnityPlugin {
        internal const string ModId = "AALUND13.Cards.Standard";
        internal const string ModName = "AALUND13 Standard Cards";
        internal const string Version = "1.0.0";


        private static AssetBundle assets;

        private void Awake() {
            assets = AssetsUtils.LoadAssetBundle("aac_standard_assets", typeof(AAC_Standard).Assembly);

            if(assets != null) {
                new Harmony(ModId).PatchAll();
            }
        }

        private void Start() {
            if(assets == null) {
                Unbound.BuildModal("AALUND13 Cards Error", $"The mod \"{ModName}\" assets failled to load, All the cards will be disable in this mod");
                throw new NullReferenceException($"Failled to load \"{ModName}\" assets");
            }

            if(AAC_Core.Plugins.Exists(plugin => plugin.Info.Metadata.GUID == "com.willuwontu.rounds.tabinfo"))
                TabinfoInterface.Setup();

            CardResgester cardResgester = assets.LoadAsset<GameObject>("StandardModCards").GetComponent<CardResgester>();
            cardResgester.RegisterCards();
            AACMenu.OnMenuRegister += () => AACMenu.CreateModuleMenuWithReadmeGenerator(ModName, Version, cardResgester);
        }
    }
}