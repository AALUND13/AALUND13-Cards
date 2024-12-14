using System.Linq;
using TMPro;

namespace AALUND13Card.Cards {
    public class BuildNegativeStatCard : AACustomCard {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block) {
            TextMeshProUGUI[] allChildren = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
            if(allChildren.Length > 0) {
                allChildren.Where(obj => obj.gameObject.name == "Text_Name").FirstOrDefault().GetComponent<TextMeshProUGUI>().text = "Defective Card";
            }
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
        }

        public override bool GetEnabled() {
            return false;
        }
    }
}
