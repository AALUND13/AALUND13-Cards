using AALUND13Cards.Core.Cards;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using ToggleCardsCategories;
using UnboundLib;
using UnboundLib.Utils.UI;
using UnityEngine;

namespace AALUND13Cards.Core.Handlers {
    internal class ConfigHandler {
        public static ConfigEntry<bool> DetailsMode;
        public static ConfigEntry<bool> DebugMode;

        public static void RegesterMenu(ConfigFile config) {
            Unbound.RegisterMenu(AAC_Core.ModName, () => { }, NewGui, null, false);
            DebugMode = config.Bind(AAC_Core.ModName, "DebugMode", false, "Enabled or disabled Debug Mode");
        }

        public static void addBlank(GameObject menu) {
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 30);
        }

        public static void NewGui(GameObject menu) {
            MenuHandler.CreateToggle(DebugMode.Value, "<#c41010> Debug Mode", menu, DebugModeChanged, 30);
            void DebugModeChanged(bool val) {
                DebugMode.Value = val;
            }

            MenuHandler.CreateButton("Create Readme", menu, CreateModFullReadme);
            void CreateModFullReadme() {
                string readme = CreateReadme();
                GUIUtility.systemCopyBuffer = readme;
            }
        }

        // Helper methods that help in craeting the `README.md`

        private static string CreateReadme() {
            string[] allowParentCategories = new string[] { "Classes", "Extra Cards" };

            List<string> insertedClassCategories = new List<string>();
            var stringBuilder = new StringBuilder();
            var firstCategory = true;

            stringBuilder.AppendLine($"# AALUND13 Cards [v{AAC_Core.FullVersion}{(AAC_Core.IsBeta ? " Beta" : "")}]");
            stringBuilder.AppendLine($"AALUND13 Cards introduces <b>{CardResgester.AllModCards.Count}</b> unique cards developed by <b>AALUND13</b>.  ");
            stringBuilder.AppendLine($"If you encounter any bugs, please report them in the [issues](https://github.com/AALUND13/AALUND13-Cards/issues) tab.");
            stringBuilder.AppendLine($"<h3>Cards:</h3>");

            var cardsCategories = GetCardWithCategories();
            int longestName = CardResgester.AllModCards.Max(c => c.cardName.Length);
            foreach(var cardCategory in cardsCategories) {
                Stack<string> categoriesPath = new Stack<string>(cardCategory.Key.Split('/'));
                var categoryName = categoriesPath.Pop();
                while(categoriesPath.Count > 0) {
                    string categoryParent = categoriesPath.Pop();
                    if(allowParentCategories.Contains(categoryParent)) categoryName = $"{categoryParent} - {categoryName}";
                }

                if(!firstCategory) {
                    stringBuilder.AppendLine($"<br>");
                }

                stringBuilder.AppendLine($"<b>{categoryName}:</b>");
                stringBuilder.AppendLine($"<pre>");

                int cardsPerRow = 4;
                int columnWidth = longestName + 2;

                for(int i = 0; i < cardCategory.Value.Count; i++) {
                    string cardName = cardCategory.Value[i].cardName;
                    stringBuilder.Append("- " + cardName.PadRight(columnWidth));

                    if((i + 1) % cardsPerRow == 0 || i == cardCategory.Value.Count - 1) {
                        stringBuilder.AppendLine();
                    }
                }

                stringBuilder.AppendLine($"</pre>");
                firstCategory = false;
            }

            return stringBuilder.ToString();
        }

        private static Dictionary<string, List<CardInfo>> GetCardWithCategories() {
            Dictionary<string, string> categoryMaps = new Dictionary<string, string>() {
                { "Devil/Outcomes", "Devil" }
            };

            var cardWithCategories = new Dictionary<string, List<CardInfo>>(StringComparer.OrdinalIgnoreCase);

            foreach(var card in CardResgester.AllModCards) {
                IToggleCardCategory cardCategory = card.GetComponent<IToggleCardCategory>();
                string cardCategoryText = cardCategory != null
                    ? cardCategory.GetCardCategoryInfo().Name
                    : "Other";

                if(categoryMaps.TryGetValue(cardCategoryText, out var mappedCategory)) {
                    cardCategoryText = mappedCategory;
                }

                if(!cardWithCategories.TryGetValue(cardCategoryText, out var list)) {
                    list = new List<CardInfo>();
                    cardWithCategories[cardCategoryText] = list;
                }

                list.Add(card.GetComponent<CardInfo>());
            }

            var sorted = cardWithCategories
                .OrderBy(kvp => kvp.Key, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return sorted;
        }

    }
}
