using AALUND13Cards.Armors.Utils;
using AALUND13Cards.Core;
using AALUND13Cards.Core.Cards;
using AALUND13Cards.Core.Extensions;
using BepInEx;
using JARL.Armor;
using JARL.Armor.Bases;
using System.Linq;
using UnityEngine;

namespace AALUND13Cards.Armors.Cards.StatModifers {
    public class ArmorStatModifers : CustomStatModifers {
        [Header("Armors Stats")]
        public float ArmorDamageReduction = 0f;

        [Space(10)]
        public string ArmorTypeId = "";
        public float ArmorHealth = 0f;
        public float ArmorHealthMultiplier = 1f;
        public float ArmorRegenRate = 0f;
        public float ArmorRegenCooldown = 0f;
        public bool RemoveArmorPirceTag = false;

        [Space(10)]
        public bool OverrideArmorDamagePatchType = false;
        public bool PatchDoDamage = false;
        public bool PatchTakeDamage = false;
        public bool PatchTakeDamageOverTime = false;

        [Space(10)]
        public ArmorReactivateType ArmorReactivateType = ArmorReactivateType.Percent;
        public float ArmorReactivateValue = 0f;

        [Header("Armor Pierce Stats")]
        public float ArmorPiercePercent = 0f;
        public float DamageAgainstArmorPercentage = 1f;

        public override void Apply(Player player) {
            var additionalData = player.data.GetCustomStatsRegistry().GetOrCreate<ArmorStats>();
            var jarlAdditionalData = JARL.Extensions.CharacterDataExtensions.GetAdditionalData(player.data);

            // Apply Armor Stats
            additionalData.ArmorDamageReduction = Mathf.Min(additionalData.ArmorDamageReduction + ArmorDamageReduction, 0.80f);

            if(!ArmorTypeId.IsNullOrWhiteSpace()) {
                ArmorBase armorInstance = ArmorFramework.ArmorHandlers[player].Armors.First(x => x.GetType() == ArmorTypeGetterUtils.GetArmorType(ArmorTypeId));
                if(armorInstance != null) {
                    armorInstance.MaxArmorValue += ArmorHealth;
                    if(AAC_Core.Plugins.Exists(plugin => plugin.Info.Metadata.GUID == "Systems.R00t.AdditiveStats")) {
                        armorInstance.MaxArmorValue += 100 * (ArmorHealthMultiplier - 1);
                    } else {
                        armorInstance.MaxArmorValue *= ArmorHealthMultiplier;
                    }

                    armorInstance.ArmorRegenerationRate += ArmorRegenRate;
                    armorInstance.ArmorRegenCooldownSeconds += ArmorRegenCooldown;
                    if(ArmorReactivateValue > 0f) {
                        armorInstance.reactivateArmorType = ArmorReactivateType;
                        armorInstance.reactivateArmorValue = ArmorReactivateValue;
                    }
                    if(RemoveArmorPirceTag && armorInstance.HasArmorTag("CanArmorPierce")) {
                        armorInstance.ArmorTags.Remove("CanArmorPierce");
                    }
                    if(OverrideArmorDamagePatchType) {
                        armorInstance.ArmorDamagePatch = 0; // Remove all flags
                        if(PatchDoDamage) armorInstance.ArmorDamagePatch |= ArmorDamagePatchType.DoDamage;
                        if(PatchTakeDamage) armorInstance.ArmorDamagePatch |= ArmorDamagePatchType.TakeDamage;
                        if(PatchTakeDamageOverTime) armorInstance.ArmorDamagePatch |= ArmorDamagePatchType.TakeDamageOverTime;
                    }
                }
            }

            jarlAdditionalData.ArmorPiercePercent = Mathf.Clamp(jarlAdditionalData.ArmorPiercePercent + ArmorPiercePercent, 0f, 1f);
            additionalData.DamageAgainstArmorPercentage += DamageAgainstArmorPercentage - 1f;
        }
    }
}
