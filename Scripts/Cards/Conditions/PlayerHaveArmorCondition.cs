using JARL.Armor;
using JARL.Armor.Bases;
using JARL.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AALUND13Cards.Cards.Conditions {
    public class PlayerHaveArmorCondition : CardCondition {
        public List<string> BlacklistedArmorTag = new List<string>();

        public override bool IsPlayerAllowedCard(Player player) {
            return ArmorFramework.ArmorHandlers[player]
                .ActiveArmors
                .Any(armor => !BlacklistedArmorTag.Any(tag => armor.ArmorTags.Contains(tag)));
        }
    }
}
