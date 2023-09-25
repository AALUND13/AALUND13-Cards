using System.Linq;
using TMPro;
using UnityEngine;

public class AAStatsModifiers : MonoBehaviour
{
    public CardInfo CardInfo;
    public CharacterStatModifiers CharacterStatModifiers;
    public ApplyCardStats ApplyCardStats;
    public Gun Gun;
    public Block Block;



    // Start is called before the first frame update
    void Awake()
    {
        CardInfo = GetComponent<CardInfo>();
        CharacterStatModifiers = GetComponent<CharacterStatModifiers>();
        ApplyCardStats = GetComponent<ApplyCardStats>();
        Gun = GetComponent<Gun>();
        Block = GetComponent<Block>();

        // add mod name text
        // create blank object for text, and attach it to the canvas
        GameObject modNameObj = new GameObject("ModNameText");
        // find bottom left edge object
        RectTransform[] allChildrenRecursive = gameObject.GetComponentsInChildren<RectTransform>();
        var edgeTransform = allChildrenRecursive.FirstOrDefault(obj => obj.gameObject.name == "EdgePart (2)");
        if (edgeTransform != null)
        {
            GameObject bottomLeftCorner = edgeTransform.gameObject;
            modNameObj.gameObject.transform.SetParent(bottomLeftCorner.transform);
        }

        TextMeshProUGUI modText = modNameObj.gameObject.AddComponent<TextMeshProUGUI>();
        modText.text = AALUND13_Cards.modInitials;
        modNameObj.transform.localEulerAngles = new Vector3(0f, 0f, 135f);

        modNameObj.transform.localScale = Vector3.one;
        modNameObj.AddComponent<SetLocalPos>();
        modText.alignment = TextAlignmentOptions.Bottom;
        modText.alpha = 0.1f;
        modText.fontSize = 54;

        OnSetup(CardInfo, Gun, ApplyCardStats, CharacterStatModifiers, Block);


    }

    // Declare OnSetup as a virtual method
    public virtual void OnSetup(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
    {
        
    }

    // Declare OnAddCard as a virtual method
    public virtual void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
    {
        
    }

    // Declare OnRemoveCard as a virtual method
    public virtual void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
    {

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
}
