using AALUND13Cards.Core;
using AALUND13Cards.Core.Cards;
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
        public static readonly List<CardInfo> GeneratedCurseCards = new List<CardInfo>();

        private static NormalDrawableRandomCard drawableRandomDebuffCard;


        public static void CreateRandomDebuffCard(GameObject cardArt, CardResgester cardResgester) {
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
                ApplyCurseCategory(cardInfo.CardInfo);
                GeneratedCurseCards.Add(cardInfo.CardInfo);
            };
            
            CreateRandomDebuffToggleCard(randomCardsGenerator, cardArt, cardResgester);
        }

        private static DrawableRandomCard CreateRandomDebuffToggleCard(RandomCardsGenerator generator, GameObject cardArt, CardResgester cardResgester) {
            drawableRandomDebuffCard = new NormalDrawableRandomCard(
                generator
            );

            drawableRandomDebuffCard.ToggleCard.toggleCardInfo.gameObject.AddComponent<AddToToggleCardCategory>().SetCategoryFromPath("Curses");

            ApplyCurseCategory(drawableRandomDebuffCard.ToggleCard.toggleCardInfo);
            ApplyCurseCategory(drawableRandomDebuffCard.CardInfo);

            drawableRandomDebuffCard.ToggleCard.toggleCardInfo.cardArt = cardArt;
            cardResgester.Cards.Add(drawableRandomDebuffCard.ToggleCard.toggleCardInfo.gameObject);
            CardResgester.AllModCards.Add(drawableRandomDebuffCard.ToggleCard.toggleCardInfo);

            return drawableRandomDebuffCard;
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
