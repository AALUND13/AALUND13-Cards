﻿using AALUND13Card.Cards;
using System.Linq;
using TMPro;
using UnboundLib;

namespace AALUND13Card.RandomStatGenerators {
    public class BuildRandomStatCard : AACustomCard {
        public string CardName;

        public override void OnSetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block) {
            this.ExecuteAfterFrames(1, () => {
                TextMeshProUGUI[] allChildren = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
                if(allChildren.Length > 0) {
                    allChildren.Where(obj => obj.gameObject.name == "Text_Name").FirstOrDefault().GetComponent<TextMeshProUGUI>().text = CardName;
                }
            });
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
        }
    }
}
