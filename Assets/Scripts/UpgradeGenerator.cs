using UnityEngine;
using UnityEngine.UI;

public class UpgradeGenerator : MonoBehaviour
{
    [SerializeField] RoundHandler roundHandler;
    [SerializeField] ShopHandler shopHandler;
    [SerializeField] float price;

    [SerializeField] int type;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ShowUpgrades);
    }

    private void ShowUpgrades()
    {
        if (roundHandler.GetCurrentMoney() > price)
        {
            gameObject.SetActive(false);
            roundHandler.Pay(price);
            switch (type)
            {
                case 0:
                    shopHandler.GenerateMultiplierUpgrades();
                    break;
                case 1:
                    shopHandler.GenerateChancesUpgrades();
                    break;
                case 2:
                    shopHandler.GenerateUtilityUpgrades();
                    break;
                default:
                    break;
            }
        }
    }
}
