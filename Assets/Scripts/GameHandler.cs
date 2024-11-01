using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private int currentRound = 0;
    [SerializeField] RoundHandler roundHandler;

    private int rollModifier = 0;
    private int rerollModifier = 0;
    private int rewardModifier = 0;
    private float targetModifier = 0;

    private Round[] rounds = {
        new Round(3, 5, 5, 30),
        new Round(3, 5, 5, 50),
    };

    public void StartRound()
    {
        Round round = rounds[currentRound];

        round.rolls += rollModifier;
        round.rerolls += rerollModifier;
        round.reward += rewardModifier;
        round.target += targetModifier;

        roundHandler.StartRound(round);
    }

    public void NextRound()
    {
        currentRound++;
        StartRound();
    }

    public void Restart()
    {
        currentRound = 0;
        StartRound();
    }

    public void AddRollModifier(int add)
    {
        rollModifier += add;
    }

    public void AddRerollModifier(int add)
    {
        rerollModifier += add;
    }

    public void AddRewardModifier(int add)
    {
        rewardModifier += add;
    }

    public void AddTargetModifier(int add)
    {
        targetModifier += add;
    }
}
