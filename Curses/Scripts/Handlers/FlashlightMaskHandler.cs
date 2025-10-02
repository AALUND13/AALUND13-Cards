using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Curses.Cards;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AALUND13Cards.Curses.Handlers {
    public class FlashlightMaskHandler : MonoBehaviour {
        public static FlashlightMaskHandler Instance { get; private set; }

        public Material mat;
        public float RadiusFadeInMultiplier = 0.2f;
        public float RadiusFadeOutMultiplier = 1f;
        public float FadeDuration = 2f;

        private Dictionary<Player, float> currentRadii = new Dictionary<Player, float>();
        private Coroutine fadeCoroutine = null;
        private float globalFade = 0f;

        private bool isBattleActive = false;
        private bool oldIsBattleActive = false;

        private void Awake() {
            Instance = this;

            globalFade = 0f;
            mat.SetFloat("_GlobalFade", globalFade);
        }

        private void OnDestroy() {
            if(fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            Instance = null;
        }

        private void Update() {
            var players = PlayerManager.instance.players.Where(p => p.data.view.IsMine && !p.data.dead).ToList();
            int playerCount = Mathf.Clamp(players.Count, 0, 32);

            isBattleActive = GameManager.instance.battleOngoing && players.Any(p => p.data.GetCustomStatsRegistry().GetOrCreate<CursesStats>().IsBind) && playerCount > 0;
            if(isBattleActive != oldIsBattleActive) {
                oldIsBattleActive = isBattleActive;
                if(isBattleActive) {
                    OnBattleStart();
                } else {
                    OnBattleEnd();
                }
            }

            var posArr = new Vector4[32];
            var radArr = new float[32];
            Camera cam = Camera.main;

            for(int i = 0; i < playerCount; i++) {
                var player = players[i];
                if(player == null || !player.transform) continue;

                Vector3 screen = cam.WorldToViewportPoint(player.transform.position);
                posArr[i] = new Vector4(screen.x, screen.y, 0, 0);

                float targetRadius = CalculateRadius(player);

                if(!currentRadii.ContainsKey(player)) currentRadii[player] = targetRadius;

                float currentRadius = currentRadii[player];

                currentRadius = Mathf.MoveTowards(currentRadius, targetRadius, Time.deltaTime);

                currentRadii[player] = currentRadius;
                radArr[i] = currentRadius;

            }

            for(int i = playerCount; i < 32; i++) {
                posArr[i] = Vector4.zero;
                radArr[i] = 0f;
            }

            mat.SetVectorArray("_LightPosArray", posArr);
            mat.SetFloatArray("_RadiusArray", radArr);
            mat.SetInt("_LightCount", playerCount);

            // Cleanup radii of players no longer present
            var toRemove = currentRadii.Keys.Except(players).ToList();
            foreach(var p in toRemove) currentRadii.Remove(p);
        }

        private float CalculateRadius(Player player) {
            if(player == null || !player.transform) return 0f;

            float camSize = Camera.main.orthographicSize;
            float playerScale = Mathf.Max(player.transform.localScale.x, 2);
            float camFactor = 1f / Mathf.Max(camSize, 0.01f);

            float baseMultiplier = isBattleActive ? RadiusFadeInMultiplier : RadiusFadeOutMultiplier;

            return Mathf.Max(playerScale * camFactor * baseMultiplier, 0.01f);
        }


        private IEnumerator FadeCoroutine(float from, float to, float duration) {
            float elapsed = 0f;
            while(elapsed < duration) {
                elapsed += Time.deltaTime;
                globalFade = Mathf.Lerp(from, to, Mathf.Clamp01(elapsed / duration));
                mat.SetFloat("_GlobalFade", globalFade);
                yield return null;
            }
            globalFade = to;
            mat.SetFloat("_GlobalFade", globalFade);
            fadeCoroutine = null;
        }

        public void OnBattleStart() {
            isBattleActive = true;
            if(fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeCoroutine(globalFade, 1f, FadeDuration));
        }

        public void OnBattleEnd() {
            isBattleActive = false;
            if(fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeCoroutine(globalFade, 0f, FadeDuration));
        }
    }
}
