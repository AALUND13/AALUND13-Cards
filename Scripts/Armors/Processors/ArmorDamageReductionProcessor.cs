using AALUND13Cards.Extensions;
using JARL.Armor;
using JARL.Armor.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AALUND13Cards.Armors.Processors {
    public class ArmorDamageReductionProcessor : ArmorProcessor {
        public override float BeforeArmorProcess(float remaindingDamage, float originalDamage) {
            if(!Armor.HasArmorTag("NoDamageReduction")) {
                if(HurtPlayer.data.GetAdditionalData().ArmorDamageReduction == 0f)
                    return remaindingDamage;

                return remaindingDamage * (1f - HurtPlayer.data.GetAdditionalData().ArmorDamageReduction);
            }
            return remaindingDamage;
        }
    }
}
