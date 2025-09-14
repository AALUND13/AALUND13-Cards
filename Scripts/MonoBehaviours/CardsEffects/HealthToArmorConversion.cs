using JARL.Armor;
using JARL.Armor.Bases;
using JARL.Armor.Builtin;
using System.Collections.Generic;
using UnityEngine;

namespace AALUND13Cards.MonoBehaviours.CardsEffects {
    public class HealthToArmorConversion : MonoBehaviour {
        public float HealthToArmorConversions = 1f;

        private Dictionary<ArmorBase, float> armorAdded = new Dictionary<ArmorBase, float>();
        private CharacterData characterData;
        private ArmorHandler armorHandler;


        private float oldHealth;
        private int oldArmorCount;


        private void Start() {
            characterData = GetComponentInParent<CharacterData>();
            armorHandler = GetComponentInParent<ArmorHandler>();

            oldHealth = characterData.maxHealth;
            oldArmorCount = Mathf.Max(armorHandler.ActiveArmors.Count, 1);
            UpdateArmorStats();
        }

        private void Update() {
            if(characterData.maxHealth != oldHealth || armorHandler.ActiveArmors.Count != oldArmorCount) {
                oldHealth = characterData.maxHealth;
                oldArmorCount = Mathf.Max(armorHandler.ActiveArmors.Count, 1);
                UpdateArmorStats();
            }
        }

        private void OnDestroy() {
            foreach(var armorAdded in armorAdded) {
                armorAdded.Key.MaxArmorValue -= armorAdded.Value;
                armorAdded.Key.CurrentArmorValue -= armorAdded.Value;
            }
        }

        private void UpdateArmorStats() {
            var armorAddedCloned = new Dictionary<ArmorBase, float>(armorAdded);
            foreach(var armorAdded in armorAdded) {
                armorAdded.Key.MaxArmorValue -= armorAdded.Value;
            }
            armorAdded.Clear();

            if(armorHandler.ActiveArmors.Count == 0) {
                float addedArmor = characterData.maxHealth * HealthToArmorConversions;
                ArmorBase defaultArmor = armorHandler.GetArmorByType<DefaultArmor>();

                float armorChnage = addedArmor;
                if(armorAddedCloned.ContainsKey(defaultArmor)) armorChnage = addedArmor - armorAddedCloned[defaultArmor];

                defaultArmor.MaxArmorValue += addedArmor;
                defaultArmor.CurrentArmorValue = armorChnage;
                armorAdded[defaultArmor] = addedArmor;
            } else {
                float addedArmor = characterData.maxHealth * HealthToArmorConversions / armorHandler.ActiveArmors.Count;
                foreach(var armor in armorHandler.ActiveArmors) {
                    float armorChnage = addedArmor;
                    if(armorAddedCloned.ContainsKey(armor)) armorChnage = addedArmor - armorAddedCloned[armor];

                    armor.MaxArmorValue += addedArmor;
                    armor.CurrentArmorValue += armorChnage;
                    armorAdded[armor] = addedArmor;
                }
            }

            LoggerUtils.LogInfo($"Added armors to player with a id of {characterData.player.playerID}");
        }
    }
}
