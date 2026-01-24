using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak.Abilities {
    public struct AbilityBarInfo {
        public bool ShowBar;
        public bool IsActive;

        public float CurrentValue;
        public float MaxValue;

        public AbilityBarInfo(bool showBar, bool isActive, float currentValue, float maxValue) {
            ShowBar = showBar;
            IsActive = isActive;
            CurrentValue = currentValue;
            MaxValue = maxValue;
        }
    }

    public interface ISoulstreakAbility {
        void OnBlock();
        void OnRevive();
        void OnUpdate();

        void OnSoulsAdded(uint addedSouls);
        void OnSoulsReset(uint remainingSouls);

        void OnRemove();

        AbilityBarInfo GetBarInfo();
    }
}
