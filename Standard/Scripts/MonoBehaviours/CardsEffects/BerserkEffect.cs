using AALUND13Cards.Core.Handlers;
using ModsPlus;
using UnityEngine;
using UnityEngine.Events;

namespace AALUND13Cards.Standard.MonoBehaviours.CardsEffects {
    public class BerserkEffect : MonoBehaviour, IOnDoDamageEventOverridable {
        public const string BERSEAK_START_KEY = "berseak_start";
        public const string BERSEAK_END_KEY = "berseak_end";
        public const float BERSERK_TIME = 10;

        public UnityEvent OnBerserkEvent;

        private AttackLevel attackLevel;
        private CharacterData characterData;
        private ChildRPC childRPC;

        private float timeToReset = 0f;
        private bool berserkActive = false;

        private float regenAdded = 0f;
        private StatChangeTracker tracker;

        private int remainingBerserkModeAmount = 0;


        private void Start() {
            characterData = GetComponentInParent<CharacterData>();
            childRPC = GetComponentInParent<ChildRPC>();
            attackLevel = GetComponent<AttackLevel>();

            characterData.healthHandler.reviveAction += Reset;
            DamageEventHandler.Instance.RegisterDamageEvent(this, characterData.player);

            childRPC.childRPCs.Add(BERSEAK_START_KEY, RPCA_BerserStart);
            childRPC.childRPCs.Add(BERSEAK_END_KEY, RPCA_BerserEnd);
        }

        private void OnDestroy() {
            characterData.healthHandler.reviveAction -= Reset;
            DamageEventHandler.Instance.UnregisterDamageEvent(this, characterData.player);

            childRPC.childRPCs.Remove(BERSEAK_START_KEY);
            childRPC.childRPCs.Remove(BERSEAK_END_KEY);

            Reset();
        }

        private void Update() {
            if(!characterData.player.data.view.IsMine) return;

            if(berserkActive && Time.time >= timeToReset) {
                berserkActive = false;
                childRPC.CallFunction(BERSEAK_END_KEY);
            }
        }


        public DamageInfo OnDamage(DamageInfo info) {
            float healthAfterDamage = characterData.health - info.Damage.magnitude;
            if(info.IsLethal && healthAfterDamage <= characterData.maxHealth * 0.5f && remainingBerserkModeAmount > 0) {
                remainingBerserkModeAmount--;
                OnBerserkMode();

                info.IsLethal = false;
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

            StopAllCoroutines();
        }

        public void OnBerserkMode() {
            if(!characterData.player.data.view.IsMine) return;

            timeToReset = Time.time + BERSERK_TIME;
            berserkActive = true;

            childRPC.CallFunction(BERSEAK_START_KEY);
        }


        private void RPCA_BerserStart() {
            characterData.block.RPCA_DoBlock(true);
            OnBerserkEvent?.Invoke();

            characterData.healthHandler.regeneration -= regenAdded;

            regenAdded = characterData.maxHealth / BERSERK_TIME / 2;
            characterData.healthHandler.regeneration += regenAdded;

            if(tracker == null) {
                tracker = StatManager.Apply(characterData.player, new StatChanges() {
                    Damage = 2f,
                    MovementSpeed = 1.5f,
                });
            }
        }

        private void RPCA_BerserEnd() {
            characterData.healthHandler.regeneration -= regenAdded;
            regenAdded = 0f;

            StatManager.Remove(tracker);
            tracker = null;
        }
    }
}
