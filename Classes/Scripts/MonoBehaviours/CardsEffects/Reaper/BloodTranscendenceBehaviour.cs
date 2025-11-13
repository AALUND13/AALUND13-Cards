using AALUND13Cards.Classes.Cards;
using AALUND13Cards.Core;
using AALUND13Cards.Core.Extensions;
using UnityEngine;

namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Reaper {
    public class BloodTranscendenceBehaviour : MonoBehaviour {
        public const string BLOOD_TRANSCENDENCE_KEY = "BloodTranscendence";
        public const string BLOOD_TRANSCENDENCE_ACTIVATE_KEY = BLOOD_TRANSCENDENCE_KEY + "Activate";
        public const string BLOOD_TRANSCENDENCE_DEACTIVATE_KEY = BLOOD_TRANSCENDENCE_KEY + "Deactivate";

        [Header("Effect Objects")]
        public ParticleSystem Effect;

        [Header("Settings")]
        public float SecondsOfInvincibilityAtFullBlood = 5f;
        public float PrecentageBloodInvincibilityStop = 0.1f;
        public float CooldownTime = 10f;


        private CharacterData data;
        private ChildRPC rpc;
        private bool hasTrigger;
        private float lastTriggerTime;


        public BloodlustStats BloodlustStats => data.GetCustomStatsRegistry().GetOrCreate<BloodlustStats>();

        public void Trigger() {
            float minimumBloodRequirement = BloodlustBehaviour.DEFAULT_MAX_BLOOD * PrecentageBloodInvincibilityStop;
            if(data.view.IsMine
                && BloodlustStats.Blood > minimumBloodRequirement
                && !hasTrigger
                && Time.time > lastTriggerTime + CooldownTime
            ) {
                rpc.CallFunction(BLOOD_TRANSCENDENCE_ACTIVATE_KEY);
            }
        }

        private void RPCA_Activate() {
            if(hasTrigger) return;
            float bloodUseRate = (BloodlustBehaviour.DEFAULT_MAX_BLOOD * (1f - PrecentageBloodInvincibilityStop)) / SecondsOfInvincibilityAtFullBlood;

            BloodlustStats.AddBloodDrain(BLOOD_TRANSCENDENCE_KEY, bloodUseRate);
            BloodlustStats.DisableDamageGain = true;
            ClassesStats.MakeInvulnerable(data.player);
            Effect.Play();

            hasTrigger = true;
            LoggerUtils.LogInfo("\"BloodTranscendence\" has been activated");
        }

        private void RPCA_Deactivate() {
            if(!hasTrigger) return;
            float bloodUseRate = (BloodlustBehaviour.DEFAULT_MAX_BLOOD * PrecentageBloodInvincibilityStop) / SecondsOfInvincibilityAtFullBlood;

            BloodlustStats.RemoveBloodDrain(BLOOD_TRANSCENDENCE_KEY);
            BloodlustStats.DisableDamageGain = false;
            ClassesStats.MakeVulnerable(data.player);
            Effect.Stop();

            hasTrigger = false;
            LoggerUtils.LogInfo("\"BloodTranscendence\" has been deactivated");
        }

        private void Update() {
            float minimumBloodRequirement = BloodlustBehaviour.DEFAULT_MAX_BLOOD * PrecentageBloodInvincibilityStop;
            if(data.view.IsMine && BloodlustStats.Blood < minimumBloodRequirement && hasTrigger) {
                rpc.CallFunction(BLOOD_TRANSCENDENCE_DEACTIVATE_KEY);
            }
        }

        private void Start() {
            data = GetComponentInParent<CharacterData>();
            rpc = GetComponentInParent<ChildRPC>();
            rpc.childRPCs.Add(BLOOD_TRANSCENDENCE_ACTIVATE_KEY, RPCA_Activate);
            rpc.childRPCs.Add(BLOOD_TRANSCENDENCE_DEACTIVATE_KEY, RPCA_Deactivate);
        }

        private void OnDestroy() {
            rpc.childRPCs.Remove(BLOOD_TRANSCENDENCE_ACTIVATE_KEY);
            rpc.childRPCs.Remove(BLOOD_TRANSCENDENCE_DEACTIVATE_KEY);
        }
    }
}
