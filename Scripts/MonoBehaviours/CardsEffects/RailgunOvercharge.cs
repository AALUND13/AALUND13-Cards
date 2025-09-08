using Sonigon;
using SoundImplementation;
using System;
using UnityEngine;

namespace AALUND13Cards.MonoBehaviours.CardsEffects {
    public class RailgunOvercharge : MonoBehaviour {
        [SerializeField] private SoundEvent soundEmpowerSpawn;

        private CharacterData data;

        private ParticleSystem[] parts;
        private Transform particleTransform;

        private RailgunMono railgun;
        private bool isOn;

        private void Start() {
            parts = GetComponentsInChildren<ParticleSystem>();
            particleTransform = base.transform.GetChild(0);

            railgun = transform.parent.GetComponentInChildren<RailgunMono>();
            data = GetComponentInParent<CharacterData>();

            data.block.BlockAction += Block;
            data.healthHandler.reviveAction += ResetOvercharge;

            soundEmpowerSpawn.variables.audioMixerGroup = SoundVolumeManager.Instance.audioMixer.FindMatchingGroups("SFX")[0];
        }

        private void OnDestroy() {
            data.block.BlockAction -= Block;
            data.healthHandler.reviveAction -= ResetOvercharge;

            railgun.RailgunStats.AllowOvercharge = false;
        }

        public void Block(BlockTrigger.BlockTriggerType trigger) {
            if(trigger != BlockTrigger.BlockTriggerType.Echo && trigger != BlockTrigger.BlockTriggerType.Empower && trigger != BlockTrigger.BlockTriggerType.ShieldCharge) {
                railgun.RailgunStats.AllowOvercharge = true;
            }
        }

        private void ResetOvercharge() {
            railgun.RailgunStats.AllowOvercharge = false;
        }

        private void Update() {
            foreach(var part in parts) {
                if(railgun.RailgunStats.AllowOvercharge) {
                    particleTransform.transform.position = data.weaponHandler.gun.transform.position;
                    particleTransform.transform.rotation = data.weaponHandler.gun.transform.rotation;

                    if(!isOn) {
                        SoundManager.Instance.PlayAtPosition(soundEmpowerSpawn, SoundManager.Instance.GetTransform(), base.transform);
                        part.Play();
                        isOn = true;
                    }
                } else if(!railgun.RailgunStats.AllowOvercharge && isOn) {
                    part.Stop();
                    isOn = false;
                }
            }
        }
    }
}
