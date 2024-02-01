using AALUND13Card.MonoBehaviours;
using JARL.ArmorFramework.Abstract;
using JARL.ArmorFramework.Classes;
using UnityEngine;

namespace AALUND13Card.Armors
{
    public class SoulArmor : ArmorBase
    {
        
        public override string GetArmorType()
        {
            return "Soul";
        }

        public override BarColor GetBarColor()
        {
            return new BarColor(Color.magenta * 0.6f, Color.magenta * 0.45f);
        }

        public override void OnRespawn()
        {
            SoulstreakMono soulstreak = armorHandler.GetComponentInChildren<SoulstreakMono>();
            if (soulstreak != null)
            {
                maxArmorValue = 0;
                currentArmorValue = 0;
                soulstreak.abilityCooldown = 0;
                soulstreak.abilityActive = false;
            }
        }

        public override void SetupArmor()
        {
            //deactivateText = "Depleted";
            //reactivateArmorType = ArmorReactivateType.Percent;
            //reactivateArmorValue = 0.3f;
            armorRegenCooldownSeconds = 5;
        }
    }
}
