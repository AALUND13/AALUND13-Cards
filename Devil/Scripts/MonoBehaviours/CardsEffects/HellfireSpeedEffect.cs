using AALUND13Cards.Devil.Patches;
using ModdingUtils.GameModes;
using ModdingUtils.MonoBehaviours;
using System.Collections.Generic;
using UnityEngine;

namespace AALUND13Cards.Devil.MonoBehaviours.CardsEffects {
    public class HellfireSpeedEffect : MonoBehaviour, IPointEndHookHandler {
        public float AttackSpeedPerShootMultiplier = 1.025f;
        public float ReloadTimePerShootMultiplier = 1.025f;

        private List<HellfireDebuffEffect> statChanges = new List<HellfireDebuffEffect>();
        private Player player;

        private void Start() {
            player = GetComponentInParent<Player>();
            
            GunPatch.RegisterShootAction(player, OnShoot);
            InterfaceGameModeHooksManager.instance.RegisterHooks(player);
        }

        private void OnDestroy() {
            GunPatch.DeregisterShootAction(player, OnShoot);
            InterfaceGameModeHooksManager.instance.RemoveHooks(player);
        }

        private void OnShoot() {
            var statAdded = player.gameObject.AddComponent<HellfireDebuffEffect>();
            statAdded.Initialize(this);
            statChanges.Add(statAdded);
        }

        public void OnPointEnd() {
            foreach(var buff in statChanges) {
                if(buff == null) continue;
                Destroy(buff);
            }
            statChanges.Clear();
        }
    }


    internal class HellfireDebuffEffect : ReversibleEffect {
        public HellfireSpeedEffect HellfireSpeedEffect;

        public void Initialize(HellfireSpeedEffect stats) {
            HellfireSpeedEffect = stats;
        }

        public override void OnStart() {
            gunStatModifier.attackSpeed_mult = HellfireSpeedEffect.AttackSpeedPerShootMultiplier;
            gunAmmoStatModifier.reloadTimeMultiplier_mult = HellfireSpeedEffect.ReloadTimePerShootMultiplier;
        }
    }
}
