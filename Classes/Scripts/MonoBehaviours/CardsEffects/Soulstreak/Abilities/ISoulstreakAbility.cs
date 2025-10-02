namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak.Abilities {
    public interface ISoulstreakAbility {
        void OnBlock(SoulstreakMono soulstreak);
        void OnReset(SoulstreakMono soulstreak);
        void OnUpdate(SoulstreakMono soulstreak);
    }
}
