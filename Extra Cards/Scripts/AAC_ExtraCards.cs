using AALUND13Cards.Core;
using AALUND13Cards.Core.Cards;
using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.Utils;
using AALUND13Cards.ExtraCards.Cards;
using AALUND13Cards.ExtraCards.Handlers;
using BepInEx;
using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections;
using System.Linq;
using UnboundLib;
using UnboundLib.GameModes;
using UnityEngine;

namespace AALUND13Cards.ExtraCards {
    [BepInDependency("AALUND13.Cards.Core")]

    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    internal class AAC_ExtraCards : BaseUnityPlugin {
        internal const string ModId = "AALUND13.Cards.Extra_Cards";
        internal const string ModName = "AALUND13 Extra Picks Cards";
        internal const string Version = "1.0.0";

        private static AssetBundle assets;

        private void Awake() {
            assets = AssetsUtils.LoadAssetBundle("aac_extra_cards_assets", typeof(AAC_ExtraCards).Assembly);
            if(assets != null) {
                new Harmony(ModId).PatchAll();
                CardsGenerators.RegisterGenerators();
            }
        }

        private void Start() {
            if(assets == null) {
                Unbound.BuildModal("AALUND13 Cards Error", $"The mod \"{ModName}\" assets failled to load, All the cards will be disable in this mod");
                throw new NullReferenceException($"Failled to load \"{ModName}\" assets");
            }

            GameModeManager.AddHook(GameModeHooks.HookPlayerPickEnd, (gm) => ExtraCardPickHandler.HandleExtraPicks(ExtraPickPhaseTrigger.TriggerInPlayerPickEnd));
            GameModeManager.AddHook(GameModeHooks.HookPickEnd, (gm) => ExtraCardPickHandler.HandleExtraPicks(ExtraPickPhaseTrigger.TriggerInPickEnd));

            GameModeManager.AddHook(GameModeHooks.HookPickStart, OnPickStart);

            if(AAC_Core.Plugins.Exists(plugin => plugin.Info.Metadata.GUID == "com.willuwontu.rounds.tabinfo"))
                TabinfoInterface.Setup();

            assets.LoadAsset<GameObject>("ExtraPicksModCards").GetComponent<CardResgester>().RegisterCards();
        }

        IEnumerator OnPickStart(IGameModeHandler gameModeHandler) {
            foreach(Player player in PlayerManager.instance.players) {
                if(PhotonNetwork.IsMasterClient || PhotonNetwork.OfflineMode) {
                    bool isWinner = gameModeHandler.GetRoundWinners().Contains(player.teamID);
                    if(player.data.GetAdditionalData().CustomStatsRegistry.GetOrCreate<ExtraCardsStats>().ExtraCardPicksPerPickPhase > 0 && !isWinner)
                        ExtraCardPickHandler.AddExtraPick<ExtraPickHandler>(player, player.data.GetAdditionalData().CustomStatsRegistry.GetOrCreate<ExtraCardsStats>().ExtraCardPicksPerPickPhase);
                }
            }

            yield break;
        }

    }
}