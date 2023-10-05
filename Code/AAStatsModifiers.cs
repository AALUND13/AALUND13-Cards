using System.Linq;
using TMPro;
using UnityEngine;

public class AAStatsModifiers : MonoBehaviour
{
    private CardInfo CardInfo;
    private CharacterStatModifiers CharacterStatModifiers;
    private ApplyCardStats ApplyCardStats;
    private Gun Gun;
    private Block Block;
    public bool isClass = false;
    public bool showCardClassName = false;
    public string className = null;

    // Start is called before the first frame update
    void Awake()
    {
        CardInfo = GetComponent<CardInfo>();
        CharacterStatModifiers = GetComponent<CharacterStatModifiers>();
        ApplyCardStats = GetComponent<ApplyCardStats>();
        Gun = GetComponent<Gun>();
        Block = GetComponent<Block>();
        createModText();
        createClassText();
        OnSetup(CardInfo, Gun, ApplyCardStats, CharacterStatModifiers, Block);


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
            if(!showCardClassName)
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

    private void createModText()
    {
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
    }

    // Declare OnSetup as a virtual method
    /// <summary>
    /// This method call when this card spawned
    /// </summary>
    /// <param name="cardInfo"></param>
    /// <param name="gun"></param>
    /// <param name="cardStats"></param>
    /// <param name="statModifiers"></param>
    /// <param name="block"></param>
    public virtual void OnSetup(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
    {
        
    }

    // Declare OnAddCard as a virtual method
    /// <summary>
    /// This method call when this card have be added to the player
    /// </summary>
    /// <param name="player"></param>
    /// <param name="gun"></param>
    /// <param name="gunAmmo"></param>
    /// <param name="data"></param>
    /// <param name="health"></param>
    /// <param name="gravity"></param>
    /// <param name="block"></param>
    /// <param name="characterStats"></param>
    /// <param name="cardInfo"></param>
    public virtual void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats, CardInfo cardInfo)
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
