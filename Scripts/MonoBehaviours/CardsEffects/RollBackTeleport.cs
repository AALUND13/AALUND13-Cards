using Sonigon;
using SoundImplementation;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.MonoBehaviours.CardsEffects {
    public class RollBackTeleport : MonoBehaviour {
        [Header("Sounds")]
        public SoundEvent SoundTeleport;

        [Header("Teleport Settings")]
        public GameObject SaveTeleportPositionPrefab;

        [Header("Particle Systems")]
        public ParticleSystem[] parts;
        public ParticleSystem[] remainParts;

        private GameObject saveTeleportPosition;
        private CharacterData data;

        private Vector2 SavePosition;
        private bool DoTeleport;

        private void Start() {
            data = GetComponentInParent<CharacterData>();

            SoundTeleport.variables.audioMixerGroup = SoundVolumeManager.Instance.audioMixer.FindMatchingGroups("SFX")[0];

            GetComponentInParent<Block>().SuperFirstBlockAction += OnBlock;
            data.healthHandler.reviveAction += ResetTeleport;

            if(data.view.IsMine) {
                saveTeleportPosition = Instantiate(SaveTeleportPositionPrefab);
                saveTeleportPosition.SetActive(false);
            }
        }

        private void OnDestroy() {
            GetComponentInParent<Block>().SuperFirstBlockAction -= OnBlock;
            data.healthHandler.reviveAction -= ResetTeleport;

            if(saveTeleportPosition != null) {
                Destroy(saveTeleportPosition);
            }
        }

        private void OnDisable() {
            ResetTeleport();
        }

        public void OnBlock(BlockTrigger.BlockTriggerType triggerType) {
            if(triggerType != BlockTrigger.BlockTriggerType.Default) return;

            if(DoTeleport) {
                Vector2 currentPosition = data.transform.position;

                GetComponentInParent<PlayerCollision>().IgnoreWallForFrames(2);
                data.transform.position = SavePosition;

                for(int j = 0; j < remainParts.Length; j++) {
                    remainParts[j].transform.position = currentPosition;
                    remainParts[j].Play();
                }
                for(int k = 0; k < parts.Length; k++) {
                    parts[k].transform.position = SavePosition;
                    parts[k].Play();
                }
                SoundManager.Instance.Play(SoundTeleport, data.transform);

                data.playerVel.SetFieldValue("velocity", Vector2.zero);
                data.sinceGrounded = 0f;
                ResetTeleport();
            } else {
                if(saveTeleportPosition != null) {
                    saveTeleportPosition.transform.position = data.transform.position;
                    saveTeleportPosition.SetActive(true);
                }

                SavePosition = data.transform.position;
                DoTeleport = true;
            }
        }

        public void ResetTeleport() {
            if(saveTeleportPosition != null) {
                saveTeleportPosition.SetActive(false);
            }
            DoTeleport = false;
            SavePosition = Vector2.zero;
        }
    }
}
