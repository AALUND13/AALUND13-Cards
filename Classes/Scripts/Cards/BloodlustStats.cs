using AALUND13Cards.Core.Utils;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Security.Cryptography;

namespace AALUND13Cards.Classes.Cards {

    public class BloodlustStats : ICustomStats {
        private struct BloodDrain {
            public float Drain;
            public bool Enable;

            public BloodDrain(float drain) : this(drain, true) { }
            public BloodDrain(float drain, bool toggle) {
                Drain = drain;
                Enable = toggle;
            }

            public BloodDrain ToggleDrain(bool toggle) {
                Enable = toggle; 
                return this;
            }

            public static BloodDrain operator +(BloodDrain left, float right) {
                return new BloodDrain(left.Drain + right, left.Enable);
            }

            public static BloodDrain operator -(BloodDrain left, float right) {
                return new BloodDrain(left.Drain - right, left.Enable);
            }
        }

        // Blood Values
        public float MaxBlood;
        public float StartingBlood;
        public float Blood;

        // Blood Healing/Draining
        public float BloodDrainRate;
        public float BloodHealthRegenRate;
        private Dictionary<string, BloodDrain> drainRate = new Dictionary<string, BloodDrain>();

        // Blood Damage
        public float BloodFillPerDamage;
        public float DamageMultiplierFromDamage;
        public float DamageFromNoBlood;
        public bool DisableDamageGain;

        public void ResetStats() {
            // Blood Damage
            MaxBlood = 0;
            StartingBlood = 0;
            Blood = 0;

            // Blood Healing/Draining
            BloodDrainRate = 0;
            BloodHealthRegenRate = 0;
            drainRate.Clear();

            // Blood Damage
            BloodFillPerDamage = 0;
            DamageMultiplierFromDamage = 0;
            DamageFromNoBlood = 0;
            DisableDamageGain = false;
        }


        public float GetBloodDrain(string key) {
            if(drainRate.ContainsKey(key)) {
                return drainRate[key].Drain;
            } else { return 0; }
        }

        public float GetBloodDrain() {
            float bloodDrain = BloodDrainRate;
            foreach(var drain in drainRate.Values.Where(d => d.Enable)) {
                bloodDrain += drain.Drain;
            }
            return bloodDrain;
        }


        public void RemoveBloodDrain(string key) {
            if(drainRate.ContainsKey(key)) drainRate.Remove(key);
        }

        public void RemoveBloodDrain(string key, float amount) {
            if(drainRate.ContainsKey(key)) {
                drainRate[key] -= amount;
                if(drainRate[key].Drain <= 0) drainRate.Remove(key);
            }
        }


        public void AddBloodDrain(string key, float amount) {
            if(!drainRate.ContainsKey(key)) drainRate.Add(key, new BloodDrain(amount));
            else drainRate[key] += amount;
        }


        public void ToggleBloodDrain(string key, bool toggle) {
            if(drainRate.ContainsKey(key)) drainRate[key] = drainRate[key].ToggleDrain(toggle);
        }
    }
}
