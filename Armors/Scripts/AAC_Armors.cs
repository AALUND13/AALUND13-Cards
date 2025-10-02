using AALUND13Cards.Armors.Armors;
using AALUND13Cards.Armors.Armors.Processors;
using AALUND13Cards.Armors.Utils;
using AALUND13Cards.Core;
using AALUND13Cards.Core.Cards;
using BepInEx;
using HarmonyLib;
using JARL.Armor;
using System;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Armors {
    [BepInDependency("AALUND13.Cards.Core")]

    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    internal class AAC_Armors : BaseUnityPlugin {
        internal const string ModId = "AALUND13.Cards.Armors";
        internal const string ModName = "AALUND13 Armors Cards";
        internal const string Version = "1.0.0";

        private static AssetBundle assets;

        private void Awake() {
            assets = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("aac_armors_assets", typeof(AAC_Armors).Assembly);

            if(assets != null) {
                new Harmony(ModId).PatchAll();
            }
        }

        private void Start() {
            if(assets == null) {
                Unbound.BuildModal("AALUND13 Cards Error", $"The mod \"{ModName}\" assets failled to load, All the cards will be disable in this mod");
                throw new NullReferenceException($"Failled to load \"{ModName}\" assets");
            }

            ArmorTypeGetterUtils.RegiterArmorType<TitaniumArmor>("Titanium");
            ArmorTypeGetterUtils.RegiterArmorType<BattleforgedArmor>("Battleforged");

            ArmorFramework.RegisterArmorProcessor<DamageAgainstArmorPercentageProcessor>();
            ArmorFramework.RegisterArmorProcessor<ArmorDamageReductionProcessor>();

            assets.LoadAsset<GameObject>("ArmorsModCards").GetComponent<CardResgester>().RegisterCards();

            if(AAC_Core.Plugins.Exists(plugin => plugin.Info.Metadata.GUID == "com.willuwontu.rounds.tabinfo"))
                TabinfoInterface.Setup();
        }
    }
}