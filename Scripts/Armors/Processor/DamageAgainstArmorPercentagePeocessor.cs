using AALUND13Cards.Extensions;
using JARL.Armor;
using JARL.Armor.Bases;
using JARL.Armor.Processors;

namespace AALUND13Cards.Armors.Processor {
    internal class DamageAgainstArmorPercentagePeocessor : ArmorProcessor {
        public override float AfterArmorProcess(float remaindingDamage, float originalDamage, float takenArmorDamage) {
            if(Armor.ArmorTags.Contains("CanArmorPierce")) {
                if(DamagingPlayer == null || DamagingPlayer.data.GetAdditionalData().DamageAgainstArmorPercentage == 1f || takenArmorDamage <= 0)
                    return remaindingDamage;

                // Log all values to see what nulls
                UnityEngine.Debug.Log($"DamageAgainstArmorPercentagePeocessor: remaindingDamage={remaindingDamage}, originalDamage={originalDamage}, takenArmorDamage={takenArmorDamage}, DamagingPlayer={DamagingPlayer?.name}, Armor={Armor?.ToString()}");

                DamageArmorInfo damageArmorInfo = Armor.OnDamage(takenArmorDamage * (DamagingPlayer.data.GetAdditionalData().DamageAgainstArmorPercentage - 1), DamagingPlayer, ArmorDamagePatchType);
                Armor.CurrentArmorValue = damageArmorInfo.Armor;
            }
            return remaindingDamage;
        }
    }
}
