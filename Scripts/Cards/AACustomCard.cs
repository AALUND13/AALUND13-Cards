using AALUND13Card.Extensions;
using AALUND13Card.Handlers;
using AALUND13Card.Handlers.ExtraPickHandlers;
using JARL.Bases;
using ModdingUtils.Utils;

namespace AALUND13Card.Cards {
    public class AACustomCard : CustomUnityCard {
        public virtual void OnRegister(CardInfo cardInfo) { }

        public override void OnSetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block) {
            LoggerUtils.LogInfo($"[{GetModName()}][Card] {GetTitle()} has been setup.");
        }

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