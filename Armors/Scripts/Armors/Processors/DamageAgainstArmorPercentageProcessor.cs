using AALUND13Cards.Armors.Cards;
using AALUND13Cards.Core.Extensions;
using JARL.Armor;
using JARL.Armor.Processors;

namespace AALUND13Cards.Armors.Armors.Processors {
    internal class DamageAgainstArmorPercentageProcessor : ArmorProcessor {
        public override float AfterArmorProcess(float remaindingDamage, float originalDamage, float takenArmorDamage) {
            if(Armor.HasArmorTag("CanArmorPierce")) {
                if(DamagingPlayer == null || DamagingPlayer.data.GetCustomStatsRegistry().GetOrCreate<ArmorStats>().DamageAgainstArmorPercentage == 1f || takenArmorDamage <= 0)
                    return remaindingDamage;

                DamageArmorInfo damageArmorInfo = Armor.OnDamage(takenArmorDamage * (DamagingPlayer.data.GetCustomStatsRegistry().GetOrCreate<ArmorStats>().DamageAgainstArmorPercentage - 1), DamagingPlayer, ArmorDamagePatchType);
                Armor.CurrentArmorValue = damageArmorInfo.Armor;
            }
            return remaindingDamage;
        }
    }
}
