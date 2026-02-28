using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AALUND13Cards.Standard.MonoBehaviours.ProjectilesEffects {
    public class TrailObject {
        public Vector3 Start;
        public Vector3 End;
        public Vector3 Current;

        public float Lifetime;
        public float ExpireTime;

        public TrailObject(Vector3 start, Vector3 end, float lifetime, float expireTime) {
            Start = start;
            End = end;
            Lifetime = lifetime;
            ExpireTime = expireTime;
            Current = start;
        }

        public void UpdatePosition(float now) {
            float remaining = ExpireTime - now;
            float t = 1f - (remaining / Lifetime);
            Current = Vector3.Lerp(Start, End, t);
        }
    }

    public interface ITrailUpdatable {
        void Update(Vector3[] positions);
    }

    public class TrailEffect : MonoBehaviour, IPunInstantiateMagicCallback {
        public GameObject EffectObject;
        public float Lifetime = 1f;

        private PhotonView view;
        private ParticleSystem effectParticles;

        private readonly Queue<TrailObject> trail = new Queue<TrailObject>();
        private readonly List<ITrailUpdatable> trailUpdatables = new List<ITrailUpdatable>();
        private readonly List<Vector3> positionsBuffer = new List<Vector3>();

        private Vector3 lastPosition;
        private bool active = false;
        private bool sourceFinished = false;

        private void Awake() {
            view = GetComponent<PhotonView>();
            GetComponents(trailUpdatables);
            effectParticles = EffectObject.GetComponent<ParticleSystem>();
        }

        private void Update() {
            if(!active) return;

            float now = (float)Time.time;

            while(trail.Count > 0 && trail.Peek().ExpireTime <= now) {
                trail.Dequeue();
            }

            foreach(var segment in trail) {
                segment.UpdatePosition(now);
            }

            positionsBuffer.Clear();
            foreach(var segment in trail) {
                positionsBuffer.Add(segment.Current);
            }
            if(trail.Count > 0) positionsBuffer.Add(trail.Last().End);

            Vector3[] positionsArray = positionsBuffer.ToArray();

            foreach(var updatable in trailUpdatables) {
                updatable.Update(positionsArray);
            }

            if(sourceFinished && trail.Count == 0 && view.IsMine) {
                PhotonNetwork.Destroy(gameObject);
            }
        }

        [PunRPC]
        public void RPCA_AddPosition(Vector3 position) {
            float now = (float)Time.time;

            if(!active) {
                active = true;
                lastPosition = position;

                EffectObject.transform.position = position;
                effectParticles.Play();
                return;
            }

            trail.Enqueue(new TrailObject(
                lastPosition,
                position,
                Lifetime,
                now + Lifetime
            ));

            lastPosition = position;
            EffectObject.transform.position = position;
        }

        [PunRPC]
        public void RPCA_SourceFinished() {
            sourceFinished = true;
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info) {
            transform.localScale = (Vector3)info.photonView.InstantiationData[0];
        }
    }
}