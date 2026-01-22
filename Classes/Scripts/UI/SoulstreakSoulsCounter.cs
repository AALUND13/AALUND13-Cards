using AALUND13Cards.Classes.Armors;
using AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak;
using AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak.Abilities;
using JARL.Armor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AALUND13Cards.Classes.UI {
    public class SoulstreakSoulsCounter : MonoBehaviour {
        [Header("Souls Text")]
        public TMP_Text SoulstreakSoulsText;
        public bool UseFullText = true;

        [Header("Armor Percentage")]
        public Image SoulArmorPercentage;
        public Color SoulArmorNonActiveColor;
        public Color SoulArmorActiveColor;

        [Header("Animation")]
        [SerializeField] private float fillSmoothTime = 0.15f;

        private float currentFillVelocity;
        private float targetFillAmount;

        [HideInInspector] public SoulstreakMono soulstreakMono;

        private const float UI_HALF_FILL = 0.5f;

        private void Update() {
            if(soulstreakMono == null)
                return;

            UpdateSoulsText();
            UpdateArmorUI();
        }

        private void LateUpdate() {
            if(SoulArmorPercentage == null)
                return;

            SoulArmorPercentage.fillAmount = Mathf.SmoothDamp(
                SoulArmorPercentage.fillAmount,
                targetFillAmount,
                ref currentFillVelocity,
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
            if(SoulArmorPercentage == null)
                return;

            ArmorAbility armorAbility =
                soulstreakMono.SoulstreakStats.GetAbility<ArmorAbility>();

            if(armorAbility == null) {
                ResetArmorUI();
                return;
            }

            SoulArmor soulArmor = ArmorFramework.ArmorHandlers[
                soulstreakMono.Data.player
            ].GetArmorByType(typeof(SoulArmor)) as SoulArmor;

            if(soulArmor == null) {
                ResetArmorUI();
                return;
            }

            float percentage = CalculateArmorPercentage(soulArmor, armorAbility);
            ApplyArmorUI(percentage, !soulArmor.IsActive);
        }

        private float CalculateArmorPercentage(
            SoulArmor soulArmor,
            ArmorAbility armorAbility
        ) {
            if(!soulArmor.IsActive && armorAbility.AbilityCooldownTime > 0f) {
                return Mathf.Clamp01(
                    (armorAbility.AbilityCooldownTime - armorAbility.AbilityCooldown) /
                    armorAbility.AbilityCooldownTime
                );
            }

            if(soulArmor.IsActive && soulArmor.MaxArmorValue > 0f) {
                return Mathf.Clamp01(
                    soulArmor.CurrentArmorValue / soulArmor.MaxArmorValue
                );
            }

            return 0f;
        }

        private void ApplyArmorUI(float percentage, bool isDisabled) {
            SoulArmorPercentage.color =
                isDisabled ? SoulArmorNonActiveColor : SoulArmorActiveColor;

            targetFillAmount =
                percentage > 0f
                    ? UI_HALF_FILL + (percentage * UI_HALF_FILL)
                    : UI_HALF_FILL;
        }

        private void ResetArmorUI() {
            SoulArmorPercentage.fillAmount = UI_HALF_FILL;
            SoulArmorPercentage.color = SoulArmorNonActiveColor;
        }
    }
}
