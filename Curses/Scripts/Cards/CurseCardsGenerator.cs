using AALUND13Cards.Core;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using CardThemeLib;
using RandomCardsGenerators;
using RandomCardsGenerators.Cards;
using RandomCardsGenerators.StatsGroup;
using RarityLib.Utils;
using System.Collections.Generic;
using System.Xml.Linq;
using ToggleCardsCategories.Utils;
using UnityEngine;
using WillsWackyManagers.Utils;

namespace AALUND13Cards.Curses.Cards {
    public class CurseCardsGenerator {
        public static void CreateRandomDebuffCard(GameObject cardArt) {
            var randomCardOption = new RandomCardOption(
                "Random Debuff",
                AAC_Core.ModInitials,
                "This is a random debuff card, it can give you a random negative stat.",
                "Rd",
                1, 3,
                RarityUtils.GetRarity("Trinket"),
                CardThemeLib.CardThemeLib.instance.CreateOrGetType("CorruptedRed", new CardThemeColor { targetColor = Color.black, bgColor = Color.black })
            );

            RandomCardsGenerator randomCardsGenerator = new RandomCardsGenerator("RandomDebuffCardGenerators", randomCardOption, new List<RandomStatGenerator> {
              new DamageStatGenerator(-0.25f, 0),
              new ReloadTimeStatGenerator(0, 0.25f),
              new AttackSpeedStatGenerator(0, 0.25f),
              new MovementSpeedStatGenerator(-0.25f, 0f),
              new HealthStatGenerator(-0.25f, 0),
              new BlockCooldownStatGenerator(0, 0.25f),
              new BulletSpeedStatGenerator(-0.25f, 0)
            });
            randomCardsGenerator.GeneratorActions.OnCardGenerated += (GeneratedCardInfo cardInfo) => {
                cardInfo.CardInfo.cardArt = cardArt;
            };

            CreateRandomDebuffToggleCard(randomCardsGenerator, cardArt);
        }

        private static DrawableRandomCard CreateRandomDebuffToggleCard(RandomCardsGenerator generator, GameObject cardArt) {
            NormalDrawableRandomCard drawableRandomCard = new NormalDrawableRandomCard(
                generator
            );

            drawableRandomCard.ToggleCard.toggleCardInfo.gameObject.AddComponent<AddToToggleCardCategory>().SetCategoryFromPath("Curses");

            ApplyCurseCategory(drawableRandomCard.ToggleCard.toggleCardInfo);
            ApplyCurseCategory(drawableRandomCard.CardInfo);

            drawableRandomCard.ToggleCard.toggleCardInfo.cardArt = cardArt;

            return drawableRandomCard;
        }

        private static void ApplyCurseCategory(CardInfo cardInfo) {
            CurseManager.instance.RegisterCurse(cardInfo);
            List<CardCategory> cardCategoriesList = new List<CardCategory>() {
                CustomCardCategories.instance.CardCategory("Curse")
            };
            cardInfo.categories = cardCategoriesList.ToArray();
        }
    }
}
