using JARL.Armor;
using JARL.Armor.Bases;
using System.Collections.Generic;
using UnboundLib;
using UnityEngine;
using UnityEngine.UI;

namespace AALUND13Cards.Armors {
    public class TitaniumArmor : ArmorBase {
        public int ArmorSegments = 4;

        private float minArmorHealth = 0;
        private Image armorMinArmorBar;

        public TitaniumArmor() {
            ArmorTags.Add("CanArmorPierce");
            ArmorRegenCooldownSeconds = 5;
        }

        public override BarColor GetBarColor() {
            return new BarColor(Color.magenta * 0.6f, Color.magenta * 0.45f);
        }

        public override void OnUpdate() {
            TryCreateArmorMinBar();
        }

        public override void OnRespawn() {
            minArmorHealth = MaxArmorValue - (MaxArmorValue / ArmorSegments);
            if(armorMinArmorBar != null) {
                armorMinArmorBar.fillAmount = minArmorHealth / MaxArmorValue;
            }
        }

        public override DamageArmorInfo OnDamage(float damage, Player DamagingPlayer, ArmorDamagePatchType? armorDamagePatchType) {
            var info = base.OnDamage(damage, DamagingPlayer, armorDamagePatchType);

            float segmentedArmorValue = Mathf.Max(info.Armor, minArmorHealth);
            if(segmentedArmorValue <= minArmorHealth + 0.1f && minArmorHealth > 0) {
                info = new DamageArmorInfo(0, minArmorHealth);
                minArmorHealth = Mathf.Max(minArmorHealth - (MaxArmorValue / ArmorSegments), 0);

                if(armorMinArmorBar != null) {
                    armorMinArmorBar.fillAmount = minArmorHealth / MaxArmorValue;
                }
            }

            return info;
        }

        private void TryCreateArmorMinBar() {
            if(armorMinArmorBar != null) return;

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
                    barImage.fillAmount = minArmorHealth / MaxArmorValue;
                    armorMinArmorBar = barImage;

                    // This for Titanium Armor to have a segmented look
                    GameObject armorBarGrid = armorHealthBarImage.transform.Find("Grid").gameObject;
                    armorBarGrid.transform.SetAsLastSibling();
                    armorBarGrid.GetComponent<HorizontalLayoutGroup>().spacing = -10;

                    GameObject[] gridChildren = new GameObject[armorBarGrid.transform.childCount];
                    for(int i = 0; i < armorBarGrid.transform.childCount; i++) {
                        gridChildren[i] = armorBarGrid.transform.GetChild(i).gameObject;
                    }

                    // The grid objects have 8 children, so we need disable a few of them
                    int objectToDisable = 8 - ArmorSegments;
                    for(int i = 0; i < objectToDisable; i++) {
                        gridChildren[i].SetActive(false);
                    }

                    armorBarGrid.SetActive(true);
                } else {
                    armorMinArmorBar = armorHealthBarImage.transform.Find("MinArmorBar").GetComponent<Image>();
                    armorMinArmorBar.fillAmount = minArmorHealth / MaxArmorValue;
                }
            }
        }
    }
}
