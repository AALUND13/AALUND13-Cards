using AALUND13Cards.Cards.Effects;
using JARL.Bases;

namespace AALUND13Cards.Cards {
    public class AACustomCard : CustomUnityCard {
        public override void OnSetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block) {
            LoggerUtils.LogInfo($"[{GetModName()}][Card] {GetTitle()} has been setup.");
        }

        public override void OnReassignCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            AAStatModifers statModifers = GetComponent<AAStatModifers>();
            if(statModifers != null) {
                statModifers.OnReassign(player);
            }
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            OnAddedEffect[] onAddedEffects = GetComponents<OnAddedEffect>();
            foreach(OnAddedEffect onAddedEffect in onAddedEffects) {
                onAddedEffect.OnAdded(player, gun, gunAmmo, data, health, gravity, block, characterStats);
            }
        }

        public override string GetModName() {
            return AALUND13_Cards.ModInitials;
        }
    }
}