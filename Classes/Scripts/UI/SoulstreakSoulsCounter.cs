using AALUND13Cards.Classes.Armors;
using AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak;
using AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak.Abilities;
using JARL.Armor;
using System.Collections.Generic;
using System.Linq;
using TabInfo;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AALUND13Cards.Classes.UI {
    public class SoulstreakSoulsCounter : MonoBehaviour {
        [Header("Souls Text")]
        public TMP_Text SoulstreakSoulsText;
        public bool UseFullText = true;

        [Header("Single Bar")]
        public GameObject SingleBarObject;
        public Image SinglePercentage;

        [Header("Double Bar")]
        public GameObject DoubleBarObject;
        public Image DoubleBarPercentageFirst;
        public Image DoubleBarPercentageSecond;

        [Header("Color")]
        public Color BarNonActiveColor;
        public Color BarActiveColor;

        [Header("Animation")]
        [SerializeField] private float fillSmoothTime = 0.15f;

        private float firstCurrentFillVelocity;
        private float firstTargetFillAmount;

        private float secondCurrentFillVelocity;
        private float secondTargetFillAmount;

        [HideInInspector] public SoulstreakMono soulstreakMono;

        private void Update() {
            if(soulstreakMono == null)
                return;

            UpdateSoulsText();
            UpdateArmorUI();
        }

        private void LateUpdate() {
            if(SingleBarObject == null || DoubleBarObject == null)
                return;

            // Single Bar
            SinglePercentage.fillAmount = Mathf.SmoothDamp(
                SinglePercentage.fillAmount,
                firstTargetFillAmount,
                ref firstCurrentFillVelocity,
                fillSmoothTime
            );

            // Double Bars
            DoubleBarPercentageFirst.fillAmount = Mathf.SmoothDamp(
                DoubleBarPercentageFirst.fillAmount,
                firstTargetFillAmount,
                ref firstCurrentFillVelocity,
                fillSmoothTime
            );

            DoubleBarPercentageSecond.fillAmount = Mathf.SmoothDamp(
                DoubleBarPercentageSecond.fillAmount,
                secondTargetFillAmount,
                ref secondCurrentFillVelocity,
                fillSmoothTime
            );
        }


        private void UpdateSoulsText() {
            if(SoulstreakSoulsText == null)
                return;

            uint souls = soulstreakMono.SoulstreakStats.Souls;

            if(UseFullText) {
                string label = souls == 1 ? "Soul" : "Souls";
                SoulstreakSoulsText.text = $"{label}: {souls}";
            } else {
                SoulstreakSoulsText.text = souls.ToString();
            }
        }

        private void UpdateArmorUI() {
            if(SingleBarObject == null || DoubleBarObject == null)
                return;

            RenderBars(soulstreakMono.SoulstreakStats.Abilities.ToArray());
        }

        private void RenderBars(ISoulstreakAbility[] soulstreakAbilities) {
            int neededBars = 0;

            List<AbilityBarInfo> abilitiesWithBars = new List<AbilityBarInfo>();
            foreach(var ability in soulstreakAbilities) {
                AbilityBarInfo barInfo = ability.GetBarInfo();
                if(barInfo.ShowBar) {
                    abilitiesWithBars.Add(barInfo);
                    neededBars++;
                }
            }

            if(neededBars > 2) {
                throw new System.Exception("More then 2 abilities requested bars, but counter only support TWO bars");
            }

            switch(neededBars) {
                case 1:
                    SingleBarObject.SetActive(true);
                    DoubleBarObject.SetActive(false);

                    SinglePercentage.color = abilitiesWithBars[0].IsActive ? BarActiveColor : BarNonActiveColor;
                    firstTargetFillAmount = 0.5f + (Mathf.Clamp01(abilitiesWithBars[0].CurrentValue / abilitiesWithBars[0].MaxValue * 0.5f));
                    return;
                case 2:
                    SingleBarObject.SetActive(false);
                    DoubleBarObject.SetActive(true);

                    DoubleBarPercentageFirst.color = abilitiesWithBars[0].IsActive ? BarActiveColor : BarNonActiveColor;
                    firstTargetFillAmount = 0.5f + (Mathf.Clamp01(abilitiesWithBars[0].CurrentValue / abilitiesWithBars[0].MaxValue * 0.25f));

                    DoubleBarPercentageSecond.color = abilitiesWithBars[1].IsActive ? BarActiveColor : BarNonActiveColor;
                    secondTargetFillAmount = 0.5f + (Mathf.Clamp01(abilitiesWithBars[1].CurrentValue / abilitiesWithBars[1].MaxValue * 0.25f));
                    return;
                default:
                    SingleBarObject.SetActive(true);
                    DoubleBarObject.SetActive(false);


                    SinglePercentage.color = BarNonActiveColor;
                    SinglePercentage.fillAmount = 0.5f;
                    return;
            }
        }
    }
}
