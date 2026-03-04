using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Standard.Cards;
using Sonigon;
using SoundImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Standard.MonoBehaviours.CardsEffects {
    public class QuickDashHandler : MonoBehaviour {
        public const string QUICK_DASH_KEY = "quick_dash";

        [Header("Sounds")]
        public SoundEvent DashSound;
        public ParticleSystem DashParticleSystem;

        [Header("Other Settings")]
        public float DoubleKeyPressMaxTiming = 0.3f;
        public float DashForce = 500000;
        public float DashCooldown = 0.25f;

        private PlayerVelocity playerVelocity;
        private CharacterData characterData;
        private GeneralInput input;
        private ChildRPC childRPC;

        private Vector3 LastDirectionInput = Vector3.zero;
        
        private bool isKeyDown = false;

        private float keyPressResetTime = 0;
        private float dashCooldownTime = 0;

        private int inputKeyPressAmount = 0;
        private int quickDashesLeft = 0;

        private void Start() {
            playerVelocity = GetComponentInParent<PlayerVelocity>();
            characterData = GetComponentInParent<CharacterData>();
            input = GetComponentInParent<GeneralInput>();
            childRPC = GetComponentInParent<ChildRPC>();

            DashSound.variables.audioMixerGroup = SoundVolumeManager.Instance.audioMixer.FindMatchingGroups("SFX")[0];
            childRPC.childRPCsVector2.Add(QUICK_DASH_KEY, RPCA_Dash);
        }

        private void OnDestroy() {
            childRPC.childRPCsVector2.Remove(QUICK_DASH_KEY);
        }

        private void Update() {
            if(!characterData.view.IsMine) return;

            if(Time.time > keyPressResetTime) {
                inputKeyPressAmount = 0;
            }

            if(characterData.isGrounded == true)
                quickDashesLeft = characterData.GetCustomStatsRegistry().GetOrCreate<StandardStats>().QuickDashes;

            if(isKeyDown && input.direction.x == 0f) isKeyDown = false;
            else if(isKeyDown && input.direction.x != 0) return;

            if(input.direction.x == 0) return;
            if(quickDashesLeft == 0) return;

            if(LastDirectionInput != input.direction) {
                LastDirectionInput = input.direction;
                inputKeyPressAmount = 0;
            }

            keyPressResetTime = Time.time + DoubleKeyPressMaxTiming;
            inputKeyPressAmount++;
            isKeyDown = true;

            if(inputKeyPressAmount >= 2) {
                if(Time.time > dashCooldownTime) {
                    childRPC.CallFunction(QUICK_DASH_KEY, (Vector2)input.direction);
                    dashCooldownTime = Time.time + DashCooldown;
                    quickDashesLeft--;
                }
            }
        }

        private void RPCA_Dash(Vector2 dir) {
            playerVelocity.SetFieldValue("velocity", new Vector2(((Vector2)playerVelocity.GetFieldValue("velocity")).x, 0));
            playerVelocity.InvokeMethod("AddForce", new Type[] { typeof(Vector2) }, new Vector2(dir.x * DashForce, 0));
            characterData.sinceGrounded = 0f;
            characterData.sinceWallGrab = 0f;

            SoundManager.Instance.Play(DashSound, characterData.transform);

            DashParticleSystem.Play();
            this.ExecuteAfterSeconds(0.25f, () => {
                DashParticleSystem.Stop();
            });
        }
    }
}
