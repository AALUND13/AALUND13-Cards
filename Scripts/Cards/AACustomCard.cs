using AALUND13Card.Extensions;
using AALUND13Card.Handlers;
using AALUND13Card.Handlers.ExtraPickHandlers;
using JARL.Bases;

namespace AALUND13Card.Cards {
    public class AACustomCard : CustomUnityCard {
        public virtual void OnRegister(CardInfo cardInfo) { }

        public override void OnReassignCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            AAStatModifers statModifers = GetComponent<AAStatModifers>();
            if(statModifers != null) {
                statModifers.OnReassign(player);
            }
        }

        public override string GetModName() {
            return AALUND13_Cards.ModInitials;
        }
    }
}