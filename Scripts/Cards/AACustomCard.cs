using AALUND13Cards.Cards.Effects;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using JARL.Bases;
using System.Collections.Generic;
using System.Linq;
using WillsWackyManagers.Utils;

namespace AALUND13Cards.Cards {
    public class AACustomCard : CustomUnityCard {
        public bool IsCursed = false;

        public override void OnRegister(CardInfo cardInfo) {
            if(IsCursed) {
                CurseManager.instance.RegisterCurse(cardInfo);
                List<CardCategory> cardCategoriesList = new List<CardCategory>(cardInfo.categories) {
                    CustomCardCategories.instance.CardCategory("Curse")
                };
                cardInfo.categories = cardCategoriesList.ToArray();
            }
        }

        public override void OnReassignCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            AAStatModifers statModifers = GetComponent<AAStatModifers>();
            if(statModifers != null) {
                statModifers.OnReassign(player);
            }
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            OnAddedEffect[] onAddedEffects = GetComponents<OnAddedEffect>();
            foreach(OnAddedEffect onAddedEffect in onAddedEffects) {
                onAddedEffect.OnAdded(player, gun, gunAmmo, data, health, gravity, block, characterStats);
            }
        }

        public override string GetModName() {
            if(IsCursed) {
                return AALUND13_Cards.CurseInitials;
            } else {
                return AALUND13_Cards.ModInitials;
            }
        }
    }
}