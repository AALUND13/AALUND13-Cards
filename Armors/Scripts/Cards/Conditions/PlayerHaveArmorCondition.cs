using AALUND13Cards.Core.Cards.Conditions;
using JARL.Armor;
using System.Collections.Generic;
using System.Linq;

namespace AALUND13Cards.Armors.Cards.Conditions {
    public class PlayerHaveArmorCondition : CardCondition {
        public List<string> BlacklistedArmorTag = new List<string>();

        public override bool IsPlayerAllowedCard(Player player) {
            return ArmorFramework.ArmorHandlers[player]
                .ActiveArmors
                .Any(armor => !BlacklistedArmorTag.Any(tag => armor.ArmorTags.Contains(tag)));
        }
    }
}
