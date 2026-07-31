using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ToggleCardsCategories;
using UnityEngine;

namespace AALUND13Cards.Core.Utils {
    internal static class ReadmeGenerator {
        public static string CreateReadme(string modName, string version, List<CardInfo> cardInfos) {
            string[] allowParentCategories = new string[] { "Classes", "Extra Cards" };

            List<string> insertedClassCategories = new List<string>();
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"# {modName} [v{version}]");
            stringBuilder.AppendLine($"{modName} introduces <b>{cardInfos.Count}</b> cards developed by <b>AALUND13</b>.  ");
            stringBuilder.AppendLine($"If you encounter any bugs, please report them in the [issues](https://github.com/AALUND13/AALUND13-Cards/issues) tab.");
            stringBuilder.AppendLine($"<h3>Cards:</h3>");

            var cardsCategories = GetCardWithCategories(cardInfos);
            int longestName = cardInfos.Max(c => c.cardName.Length);
            foreach(var cardCategory in cardsCategories) {
                Stack<string> categoriesPath = new Stack<string>(cardCategory.Key.Split('/'));
                var categoryName = categoriesPath.Pop();
                while(categoriesPath.Count > 0) {
                    string categoryParent = categoriesPath.Pop();
                    if(allowParentCategories.Contains(categoryParent)) categoryName = $"{categoryParent} - {categoryName}";
                }

                stringBuilder.AppendLine($"<b>{categoryName}:</b>\n");
                stringBuilder.AppendLine(CreateCardsTable(cardCategory.Value));
            }

            return stringBuilder.ToString();
        }

        private static string CreateCardsTable(List<CardInfo> cards) {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("| Name | Rarity | Description | Stats |");
            stringBuilder.AppendLine("|---|---|---|---|");
            foreach(CardInfo card in cards) {
                stringBuilder.AppendLine(StripTags(
                    $"| {NormalizeLineBreaks(card.cardName)} | " +
                    $"{NormalizeLineBreaks(card.rarity.ToString())} | " +
                    $"{NormalizeLineBreaks(card.cardDestription)} | " +
                    $"{string.Join("<br>", card.cardStats.Select(s => NormalizeLineBreaks($"{s.amount} {s.stat}")))}"
                ));
            }
            return stringBuilder.ToString();
        }

        private static string NormalizeLineBreaks(string input) {
            return input
                .Replace("\r\n", "<br>")
                .Replace("\n", "<br>")
                .Replace("\r", "<br>");
        }

        private static string StripTags(string input) {
            return Regex.Replace(
                input,
                "<(?!/?(br|b)\\b)[^>]+>",
                string.Empty,
                RegexOptions.IgnoreCase
            );
        }

        private static Dictionary<string, List<CardInfo>> GetCardWithCategories(List<CardInfo> cardInfos) {
            Dictionary<string, string> categoryMaps = new Dictionary<string, string>() {
                { "Devil/Outcomes", "Devil" }
            };

            var cardWithCategories = new Dictionary<string, List<CardInfo>>(StringComparer.OrdinalIgnoreCase);

            foreach(var card in cardInfos) {
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
