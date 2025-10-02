using AALUND13Cards.Armors.Cards;
using AALUND13Cards.Core.Extensions;
using JARL.Armor.Processors;

namespace AALUND13Cards.Armors.Armors.Processors {
    public class ArmorDamageReductionProcessor : ArmorProcessor {
        public override float BeforeArmorProcess(float remaindingDamage, float originalDamage) {
            if(!Armor.HasArmorTag("NoDamageReduction")) {
                if(HurtPlayer.data.GetCustomStatsRegistry().GetOrCreate<ArmorStats>().ArmorDamageReduction == 0f)
                    return remaindingDamage;

                return remaindingDamage * (1f - HurtPlayer.data.GetCustomStatsRegistry().GetOrCreate<ArmorStats>().ArmorDamageReduction);
            }
            return remaindingDamage;
        }
    }
}
