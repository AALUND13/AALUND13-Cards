using AALUND13Cards.Classes.Cards;

namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak.Abilities {
    public abstract class SoulstreakAbility<TAbility> : ISoulstreakAbility
        where TAbility : SoulstreakAbility<TAbility> 
    {
        public SoulStreakStats SoulstreakStats { get; internal set; }

        public virtual void OnBlock() { }
        public virtual void OnRevive() { }
        public virtual void OnUpdate() { }

        public virtual void OnSoulsAdded(uint addedSouls) { }
        public virtual void OnSoulsReset(uint removedSouls) { }

        public virtual void OnRemove() { }
    }
}
