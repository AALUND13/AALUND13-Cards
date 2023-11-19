using System.Linq;
using TMPro;
using UnboundLib.Cards;
using UnityEngine;

public class AACustomCard : CustomCard
{
    public bool isClass = false;
    public bool showCardClassName = false;
    public string className = null;

    private CardInfo Card;

    public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
    {
        createClassText();
        Card = cardInfo;
    }

    private class SetLocalPos : MonoBehaviour
    {
        private readonly Vector3 localpos = new Vector3(-50f, -50f, 0f);

        private void Update()
        {
            if (gameObject.transform.localPosition == localpos) return;
            gameObject.transform.localPosition = localpos;
            Destroy(this, 1f);
        }
    }

    public override string GetModName()
    {
        return AALUND13_Cards.modInitials;
    }

    protected override GameObject GetCardArt()
    {
        return cardInfo.cardArt;
    }

    protected override string GetDescription()
    {
        return cardInfo.cardDestription;
    }

    protected override CardInfo.Rarity GetRarity()
    {
        return cardInfo.rarity;
    }

    protected override CardInfoStat[] GetStats()
    {
        return cardInfo.cardStats;
    }

    protected override CardThemeColor.CardThemeColorType GetTheme()
    {
        return cardInfo.colorTheme;
    }

    protected override string GetTitle()
    {
        return cardInfo.cardName;
    }

    private void createClassText()
    {
        if (isClass)
        {
            // add mod name text
            // create blank object for text, and attach it to the canvas
            GameObject modNameObj = new GameObject("ClassText");
            // find bottom left edge object
            RectTransform[] allChildrenRecursive = gameObject.GetComponentsInChildren<RectTransform>();
            var edgeTransform = allChildrenRecursive.FirstOrDefault(obj => obj.gameObject.name == "EdgePart (1)");
            if (edgeTransform != null)
            {
                GameObject bottomLeftCorner = edgeTransform.gameObject;
                modNameObj.gameObject.transform.SetParent(bottomLeftCorner.transform);
            }

            TextMeshProUGUI modText = modNameObj.gameObject.AddComponent<TextMeshProUGUI>();
            if (!showCardClassName)
            {
                modText.text = "Class";
            }
            else
            {
                modText.text = className;
            }

            modNameObj.transform.localEulerAngles = new Vector3(0f, 0f, 135f);

            modNameObj.transform.localScale = Vector3.one;
            modNameObj.AddComponent<SetLocalPos>();
            modText.alignment = TextAlignmentOptions.Bottom;
            modText.alpha = 0.1f;
            modText.fontSize = 54;
        }
    }

    public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
    {
    }
}
