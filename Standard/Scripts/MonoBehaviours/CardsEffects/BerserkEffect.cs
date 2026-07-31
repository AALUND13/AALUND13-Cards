using AALUND13Cards.Core.Handlers;
using InControl;
using ModsPlus;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace AALUND13Cards.Standard.MonoBehaviours.CardsEffects {
    public class BerserkEffect : MonoBehaviour, IOnDoDamageEventOverridable {
        public const float BERSERK_TIME = 10;

        public UnityEvent OnBerserkEvent;

        private AttackLevel attackLevel;
        private CharacterData characterData;


        private StatChangeTracker tracker;


        private float timeToReset = 0f;
        private bool berserkActive = false;

        private float regenAdded = 0f;
        private int remainingBerserkModeAmount = 0;


        private void Start() {
            characterData = GetComponentInParent<CharacterData>();
            attackLevel = GetComponent<AttackLevel>();

            characterData.healthHandler.reviveAction += Reset;
            DamageEventHandler.Instance.RegisterDamageEvent(this, characterData.player);
        }

        private void OnDestroy() {
            characterData.healthHandler.reviveAction -= Reset;
            DamageEventHandler.Instance.UnregisterDamageEvent(this, characterData.player);

            Reset();
        }

        private void Update() {
            if(berserkActive && Time.time >= timeToReset) {
                berserkActive = false;
                BerserEnd();
            }
        }


        public DamageInfo OnDamage(DamageInfo info) {
            float healthAfterDamage = characterData.health - info.Damage.magnitude;
            if(info.IsLethal && healthAfterDamage <= characterData.maxHealth * 0.5f && remainingBerserkModeAmount > 0) {
                info.IsLethal = false;
                OnBerserkMode();
            }
            return info;
        }


        public void Reset() {
            if(tracker != null) {
                StatManager.Remove(tracker);
                tracker = null;
            }

            characterData.healthHandler.regeneration -= regenAdded;
            remainingBerserkModeAmount = attackLevel.attackLevel;
            regenAdded = 0f;
        }

        public void OnBerserkMode() {
            if (berserkActive) return;

            BerserStart();
        }


        private void BerserStart() {
            berserkActive = true;
            remainingBerserkModeAmount--;

            timeToReset = Time.time + BERSERK_TIME;
            

            characterData.block.RPCA_DoBlock(true);
            OnBerserkEvent?.Invoke();

            characterData.healthHandler.regeneration -= regenAdded;
            regenAdded = characterData.maxHealth / BERSERK_TIME / 2;
            characterData.healthHandler.regeneration += regenAdded;


            if(tracker != null) {
                tracker = StatManager.Apply(characterData.player, new StatChanges() {
                    Damage = 2f,
                    MovementSpeed = 1.5f,
                });
            }
        }

        private void BerserEnd() {
            characterData.healthHandler.regeneration -= regenAdded;
            regenAdded = 0f;

            StatManager.Remove(tracker);
            tracker = null;
        }
    }
}
