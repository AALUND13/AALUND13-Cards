using JARL.Armor;
using JARL.Armor.Bases;
using System.Collections.Generic;
using UnboundLib;
using UnityEngine;
using UnityEngine.UI;

namespace AALUND13Cards.Armors.Armors {
    public class TitaniumArmor : ArmorBase {
        public int SegmentsCount = 4;
        public float RegenThresholdPercent = 1.5f;

        private float segmentThresholdHealth = 0;
        private Image segmentThresholdBar;

        public TitaniumArmor() {
            ArmorTags.Add("CanArmorPierce");
            ArmorRegenCooldownSeconds = 5;
        }

        public override BarColor GetBarColor() {
            return new BarColor(Color.magenta * 0.6f, Color.magenta * 0.45f);
        }

        public override void OnUpdate() {
            TryCreateArmorMinBar();

            float segmentMaxHealth = MaxArmorValue / SegmentsCount;
            while(CurrentArmorValue >= segmentThresholdHealth + (segmentMaxHealth * RegenThresholdPercent)) {
                segmentThresholdHealth += segmentMaxHealth;
            }

            // Update UI bar
            if(segmentThresholdBar != null) {
                segmentThresholdBar.fillAmount = segmentThresholdHealth / MaxArmorValue;
            }
        }

        public override void OnRespawn() {
            segmentThresholdHealth = MaxArmorValue - (MaxArmorValue / SegmentsCount);
        }

        public override DamageArmorInfo OnDamage(float damage, Player DamagingPlayer, ArmorDamagePatchType? armorDamagePatchType) {
            var info = base.OnDamage(damage, DamagingPlayer, armorDamagePatchType);

            float segmentedArmorValue = Mathf.Max(info.Armor, segmentThresholdHealth);
            if(segmentedArmorValue <= segmentThresholdHealth + 0.1f && segmentThresholdHealth > 0) {
                info = new DamageArmorInfo(0, segmentThresholdHealth);
                segmentThresholdHealth = Mathf.Max(segmentThresholdHealth - (MaxArmorValue / SegmentsCount), 0);
            }

            return info;
        }

        private void TryCreateArmorMinBar() {
            if(segmentThresholdBar != null) return;

            Dictionary<ArmorBase, GameObject> armorHealthBars = (Dictionary<ArmorBase, GameObject>)ArmorHandler.GetFieldValue("armorHealthBars");
            if(armorHealthBars != null && armorHealthBars.ContainsKey(this)) {
                var originalBar = armorHealthBars[this];
                GameObject armorHealthBarImage = originalBar.transform.Find("Healthbar(Clone)/Canvas/Image").gameObject;
                if(armorHealthBarImage.transform.Find("MinArmorBar") == null) {
                    armorHealthBarImage.AddComponent<Mask>();

                    var armorMinArmorBarObj = GameObject.Instantiate(armorHealthBarImage.transform.Find("Health").gameObject, armorHealthBarImage.transform);
                    armorMinArmorBarObj.name = "MinArmorBar";
                    armorMinArmorBarObj.transform.SetAsLastSibling();

                    var barImage = armorMinArmorBarObj.GetComponent<Image>();
                    barImage.color = new Color(0.45f, 0f, 0.45f, 1f);
                    barImage.fillAmount = segmentThresholdHealth / MaxArmorValue;
                    segmentThresholdBar = barImage;

                    // This for Titanium Armor to have a segmented look
                    GameObject armorBarGrid = armorHealthBarImage.transform.Find("Grid").gameObject;
                    armorBarGrid.transform.SetAsLastSibling();
                    armorBarGrid.GetComponent<HorizontalLayoutGroup>().spacing = -10;

                    GameObject[] gridChildren = new GameObject[armorBarGrid.transform.childCount];
                    for(int i = 0; i < armorBarGrid.transform.childCount; i++) {
                        gridChildren[i] = armorBarGrid.transform.GetChild(i).gameObject;
                    }

                    // The grid objects have 8 children, so we need disable a few of them
                    int objectToDisable = 8 - SegmentsCount;
                    for(int i = 0; i < objectToDisable; i++) {
                        gridChildren[i].SetActive(false);
                    }

                    armorBarGrid.SetActive(true);
                } else {
                    segmentThresholdBar = armorHealthBarImage.transform.Find("MinArmorBar").GetComponent<Image>();
                    segmentThresholdBar.fillAmount = segmentThresholdHealth / MaxArmorValue;
                }
            }
        }
    }
}
