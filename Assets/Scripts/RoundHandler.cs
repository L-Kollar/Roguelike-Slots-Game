using TMPro;
using UnityEngine;

public class RoundHandler : MonoBehaviour
{
    private int rolls = 3;
    private int rerolls = 5;
    private float currentRoundScore = 0;
    private float currentRoundTarget = 30;

    [SerializeField] RollHandler rollHandler;
    [SerializeField] ShopHandler shopHandler;

    [SerializeField] TMP_Text rerollsText;
    [SerializeField] TMP_Text rollsText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text targetText;

    private int currentReward = 5;
    private float currentMoney = 0;

    [SerializeField] TMP_Text moneyText;

    [SerializeField] GameObject lossDialog;
    [SerializeField] GameObject winDialog;

    public void StartRound(Round round)
    {
        this.rolls = round.rolls;
        this.rerolls = round.rerolls;
        this.currentReward = round.reward;
        this.currentRoundTarget = round.target;

        moneyText.text = currentMoney + " + " + (currentReward + rerolls + (rolls * 2));
        rerollsText.text = "Rerolls: " + rerolls;
        scoreText.text = currentRoundScore.ToString();
        rollsText.text = "Rolls: " + rolls;
    }

    public void AddCurrentScore()
    {
        currentRoundScore += rollHandler.GetScore();
        rolls--;
        moneyText.text = currentMoney + " + " + (currentReward + rerolls + (rolls*2));
        scoreText.text = currentRoundScore.ToString();
        rollsText.text = "Rolls: " + rolls;
        if (rolls <= 0)
        {
            if (currentRoundScore >= currentRoundTarget)
            {
                Win();
            }
            else
            {
                Lose();
            }
        }
        if (currentRoundScore >= currentRoundTarget)
        {
            Win();
        }
        rollHandler.UnlockColumns();
    }

    private void Win()
    {
        currentMoney += currentMoney / 10;
        currentMoney += currentReward;
        currentMoney += rolls * 2;
        currentReward += rerolls;
        shopHandler.GeneratePinUpgrades();
        winDialog.SetActive(true);
    }

    private void Lose()
    {
        lossDialog.SetActive(true);
    }

    public void SetRolls(int rolls)
    {
        this.rolls = rolls;
        rollsText.text = "Rolls: " + this.rolls;
    }

    public bool UseReroll()
    {
        if(rerolls > 0)
        {
            rerolls--;
            moneyText.text = currentMoney + " + " + (currentReward + rerolls + (rolls * 2));
            rerollsText.text = "Rerolls: " + rerolls;
            return true;
        }
        else
        {
            return false;
        }
    }

    public float GetCurrentMoney()
    {
        return currentMoney;
    }

    public void Pay(float price)
    {
        currentMoney -= price;
    }
}
