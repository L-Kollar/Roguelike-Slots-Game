using UnityEngine;
using UnityEngine.UI;

public class ShopHandler : MonoBehaviour
{
    [SerializeField] GameHandler gameHandler;
    [SerializeField] RollHandler rollHandler;
    [SerializeField] RoundHandler roundHandler;

    [SerializeField] Transform multiplierUpgrades;
    [SerializeField] Transform chancesUpgrades;
    [SerializeField] Transform utilityUpgrades;
    [SerializeField] Transform pinUpgrades;

    private void Start()
    {
        GeneratePinUpgrades();
    }

    public void ChangeChance(BlockType blockType)
    {
        rollHandler.ChangeChance(blockType);
    }

    public void ChangeMultiplier(BlockType blockType)
    {
        rollHandler.ChangeMultiplier(blockType);
    }

    public void AddRollModifier(int add)
    {
        gameHandler.AddRollModifier(add);
    }

    public void AddRerollModifier(int add)
    {
        gameHandler.AddRerollModifier(add);
    }

    public void AddRewardModifier(int add)
    {
        gameHandler.AddRewardModifier(add);
    }

    public void AddTargetModifier(int add)
    {
        gameHandler.AddTargetModifier(add);
    }

    public void GenerateMultiplierUpgrades()
    {

        foreach (Transform upgrade in multiplierUpgrades)
        {
            BlockType blocktype = (BlockType)Random.Range(0, 5);
            upgrade.GetComponent<TooltipHandler>().SetText(blocktype + " multiplier", "Upgrades multiplier of " + blocktype + " appearing.");
            upgrade.GetComponent<Button>().onClick.AddListener(delegate { ChangeMultiplier(blocktype); } );
        }
    }

    public void GenerateChancesUpgrades()
    {
        foreach (Transform upgrade in chancesUpgrades)
        {
            BlockType blocktype = (BlockType)Random.Range(0, 6);
            upgrade.GetComponent<TooltipHandler>().SetText(blocktype + " chance", "Upgrades chance of " + blocktype + " appearing.");
            upgrade.GetComponent<Button>().onClick.AddListener(delegate { ChangeChance(blocktype); });
        }
    }

    public void GenerateUtilityUpgrades()
    {
        foreach (Transform upgrade in utilityUpgrades)
        {
            int buffer = Random.Range(0, 3);
            switch (buffer)
            {
                case 0:
                    upgrade.GetComponent<TooltipHandler>().SetText("Add roll", "Add 1 more roll.");
                    upgrade.GetComponent<Button>().onClick.AddListener(delegate { AddRollModifier(1); });
                    break;
                case 1:
                    upgrade.GetComponent<TooltipHandler>().SetText("Add rerolls", "Add 2 more rerolls.");
                    upgrade.GetComponent<Button>().onClick.AddListener(delegate { AddRerollModifier(2); });
                    break;
                case 2:
                    upgrade.GetComponent<TooltipHandler>().SetText("Get more rewards", "Get 10 more $ per win.");
                    upgrade.GetComponent<Button>().onClick.AddListener(delegate { AddRewardModifier(5); });
                    break;
                case 3:
                    upgrade.GetComponent<TooltipHandler>().SetText("Lower target", "Lower target by 10.");
                    upgrade.GetComponent<Button>().onClick.AddListener(delegate { AddTargetModifier(-10); });
                    break;
                default:
                    break;
            }
        }
    }

    public void GeneratePinUpgrades()
    {
        foreach (Transform upgrade in pinUpgrades)
        {
            int buffer = Random.Range(0, System.Enum.GetValues(typeof(Pin)).Length);
            upgrade.GetComponent<TooltipHandler>().SetText("" + (Pin)buffer, "");
        }
    }
}
