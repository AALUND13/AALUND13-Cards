using AALUND13Cards.Extensions;
using AALUND13Cards.Handlers;
using AALUND13Cards.MonoBehaviours;
using ModsPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AALUND13Cards.MonoBehaviours.CardsEffects {
    public class StoreDamageEffect : MonoBehaviour, IOnDoDamageEvent {
        public float DamageToStorePercentage = 0.5f;
        public float MaxStoredDamageMultiplier = 5f;
        [Tooltip("Cooldown time in seconds before damage can be stored again after releasing stored damage.")]
        public float CoolddownTime = 5f;

        private float maxStoredDamage = 100f;
        private float storedDamage = 0f;
        private float cooldownTimer = 0f;
        private bool damageReductionApplied = false;

        private Player player;
        private DamageSpawnObjects damageSpawnObjects;

        private CustomHealthBar storedDamageBar;

        public void ReleaseStoredDamage() {
            if(storedDamage > 0f) {
                damageSpawnObjects.SpawnDamage(Vector2.up * storedDamage);
                ResetStoredDamage();

                cooldownTimer = CoolddownTime;
            }
        }

        public void OnDamage(Vector2 damage) {
            if(cooldownTimer > 0f) return;

            float rawDamage = damage.magnitude / (1f - DamageToStorePercentage);
            storedDamage = Mathf.Min(maxStoredDamage, storedDamage + rawDamage * DamageToStorePercentage);
            if(storedDamage == maxStoredDamage && damageReductionApplied) {
                player.data.GetAdditionalData().DamageReduction -= DamageToStorePercentage;
                damageReductionApplied = false;
            }

            storedDamageBar.CurrentHealth = storedDamage;
        }

        public void ResetStoredDamage() {
            storedDamage = 0f;
            storedDamageBar.CurrentHealth = storedDamage;

            if(!damageReductionApplied) {
                player.data.GetAdditionalData().DamageReduction += DamageToStorePercentage;
                damageReductionApplied = true;
            }
        }

        private void Start() {
            player = GetComponentInParent<Player>();
            damageSpawnObjects = GetComponent<DamageSpawnObjects>();
            storedDamageBar = CreateStoredDamageBar();

            DamageEventHandler.RegisterDamageEvent(this, player);
            player.data.healthHandler.reviveAction += OnRevive;

            maxStoredDamage = player.data.maxHealth * MaxStoredDamageMultiplier;
            storedDamageBar.SetValues(0f, maxStoredDamage);

            ResetStoredDamage();
        }

        private void OnDestroy() {
            DamageEventHandler.UnregisterDamageEvent(this, player);
            player.data.healthHandler.reviveAction -= OnRevive;
        }


        private void Update() {
            maxStoredDamage = player.data.maxHealth * MaxStoredDamageMultiplier;
            storedDamageBar.MaxHealth = maxStoredDamage;
            if(cooldownTimer > 0f) {
                cooldownTimer = Mathf.Max(0f, cooldownTimer - TimeHandler.deltaTime);
                if(damageReductionApplied) {
                    player.data.GetAdditionalData().DamageReduction -= DamageToStorePercentage;
                    damageReductionApplied = false;
                } else if(cooldownTimer <= 0f && !damageReductionApplied) {
                    player.data.GetAdditionalData().DamageReduction += DamageToStorePercentage;
                    damageReductionApplied = true;
                }
            }

            if(storedDamage > maxStoredDamage) {
                storedDamage = maxStoredDamage;

                storedDamageBar.CurrentHealth = storedDamage;
            }
        }
        private CustomHealthBar CreateStoredDamageBar() {
            GameObject storedDamageBarObj = new GameObject("Railgun Charge Bar");

            CustomHealthBar storedDamageBar = storedDamageBarObj.AddComponent<CustomHealthBar>();
            storedDamageBar.SetColor(new Color(1f, 0.6470588235f, 0f, 1f) * 0.8f);

            player.AddStatusIndicator(storedDamageBarObj);

            Destroy(storedDamageBarObj.transform.Find("Healthbar(Clone)/Canvas/Image/White").gameObject);

            return storedDamageBar;
        }

        private void OnRevive() {
            ResetStoredDamage();
            cooldownTimer = 0f;
        }
    }
}