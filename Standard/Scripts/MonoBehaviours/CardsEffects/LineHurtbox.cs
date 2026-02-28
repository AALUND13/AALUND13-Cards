using AALUND13Cards.Standard.MonoBehaviours.ProjectilesEffects;
using Photon.Pun;
using Photon.Utilities;
using Sonigon;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AALUND13Cards.Standard.MonoBehaviours.CardsEffects {
    public class LineHurtbox : MonoBehaviour, ITrailUpdatable, IPunInstantiateMagicCallback {
        [Header("Sounds")]
        public SoundEvent SoundDamage;

        [Header("View & Player")]
        [Tooltip("The view & player that own this \"LineHurtbox\"")]
        public PhotonView View;
        public Player Player;

        [Header("Settings")]
        public float DamageDelay = 0.1f;
        public float BaseDamage = 100;

        public float BaseThickness = 1;
        public List<Vector3> Positions = new List<Vector3>();

        [Header("Character Effect")]
        public float Silence = 0;
        public float Slow = 0;
        public bool FastSlow = false;


        [Header("Scale Character Effect")]
        public bool ScaleSilence = false;
        public bool ScaleSlow = false;


        [Header("Scale Settings")]
        public bool ScaleDamage = false;
        public bool ScaleThickness = false;

        private Dictionary<Damagable, float> LastDamageMap = new Dictionary<Damagable, float>();

        private void Update() {
            if(View.IsMine) {
                var collions = GetDamagableBetweenPoints(Positions);
                foreach(var collion in collions) {
                    DoDamage(collion);
                }
            }
        }

        private Damagable[] GetDamagableBetweenPoints(List<Vector3> points) {
            List<Collider2D> hitColliders = new List<Collider2D>();
            List<Damagable> damagables = new List<Damagable>();

            for(int i = 0; i < points.Count - 1; i++) {
                Vector2 start = points[i];
                Vector2 end = points[i + 1];

                float scaleThickness = BaseThickness * (ScaleThickness ? transform.localScale.x : 1f);
                float radius = scaleThickness * 0.5f;

                Vector2 direction = end - start;
                float distance = direction.magnitude;

                if(distance <= 0.0001f)
                    continue;

                Vector2 center = (start + end) * 0.5f;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                Vector2 size = new Vector2(distance + radius * 2f, radius * 2f);

                Collider2D[] overlaps = Physics2D.OverlapCapsuleAll(
                    center,
                    size,
                    CapsuleDirection2D.Horizontal,
                    angle
                );

                foreach(Collider2D col in overlaps) {
                    if(hitColliders.Contains(col)) continue;

                    Damagable dmg = col.GetComponentInParent<Damagable>();
                    if(dmg != null) {
                        hitColliders.Add(col);
                        damagables.Add(dmg);
                    }
                }
            }

            return damagables.ToArray();
        }

        private void DoDamage(Damagable damagable) {
            float lastDamageTime = LastDamageMap.TryGetValue(damagable, out float lastDmgTime) ? lastDmgTime : 0;
            if(Time.time > lastDamageTime + DamageDelay) {
                float scaleDamage = BaseDamage * (ScaleDamage ? transform.localScale.x : 1);

                float scaleSilence = Silence * (ScaleSilence ? transform.localScale.x : 1);
                float scaleSlow = Slow * (ScaleSlow ? transform.localScale.x : 1);

                CharacterData characterData = damagable.gameObject.GetComponentInParent<CharacterData>();
                if(characterData != null) {
                    if(scaleSilence > 0) characterData.view.RPC("RPCA_AddSilence", RpcTarget.All, scaleSilence);
                    if(scaleSlow > 0) characterData.view.RPC("RPCA_AddSlow", RpcTarget.All, scaleSlow, FastSlow);
                }
                damagable.CallTakeDamage(Vector2.up * scaleDamage, damagable.transform.position, damagingPlayer: Player);
                LastDamageMap[damagable] = Time.time;
            }
        }

        public void Update(Vector3[] positions) {
            Positions = positions.ToList();
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info) {
            Player = PlayerManager.instance.players.First(p => p.playerID == (int)info.photonView.InstantiationData[1]);
        }
    }
}
