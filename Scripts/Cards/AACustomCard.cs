using AALUND13Cards.Cards.Effects;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using JARL.Bases;
using System.Collections.Generic;
using WillsWackyManagers.Utils;
using ToggleCardsCategories;
using UnboundLib.Cards;
using UnityEngine;

namespace AALUND13Cards.Cards {
    public enum CardListingCategory {
        Standard,
        ExtraCards,
        Curses,
        Armor,
        ClassesSoulstreak,
        ClassesReaper,
        ClassesExoArmor
    }



    public class AACustomCard : CustomUnityCard, IToggleCardCategory {
        public bool IsCursed = false;
        public CardListingCategory Category = CardListingCategory.Standard;

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
            CustomStatModifers[] statModifers = GetComponents<CustomStatModifers>();
            foreach(CustomStatModifers statModifer in statModifers) {
                statModifer.OnReassign(player);
            }
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            OnAddedEffect[] onAddedEffects = GetComponents<OnAddedEffect>();
            foreach(OnAddedEffect onAddedEffect in onAddedEffects) {
                onAddedEffect.OnAdded(player, gun, gunAmmo, data, health, gravity, block, characterStats);
            }
        }

        public override string GetModName() {
            return AALUND13_Cards.ModInitials;
        }

        public ToggleCardCategoryInfo GetCardCategoryInfo() {
            string category;
            switch (Category) {
                default:
                    category = "Standard";
                    break;
                case CardListingCategory.ExtraCards:
                    category = "Extra Cards";
                    break;
                case CardListingCategory.Armor:
                    category = "Armors";
                    break;
                case CardListingCategory.Curses:
                    category = "Curses";
                    break;
                case CardListingCategory.ClassesReaper:
                    category = "Classes/Reaper";
                    break;
                case CardListingCategory.ClassesSoulstreak:
                    category = "Classes/Soulstreak";
                    break;
                case CardListingCategory.ClassesExoArmor:
                    category = "Classes/Exo Armor";
                    break;
            }

            return new ToggleCardCategoryInfo(category);
        }
    }
}