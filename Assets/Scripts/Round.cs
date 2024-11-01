using UnityEngine;

public class Round
{
    public int rolls;
    public int rerolls;
    public int reward;
    public float target;
    
    public Round(int rolls, int rerolls, int reward, float target)
    {
        this.rolls = rolls;
        this.rerolls = rerolls;
        this.reward = reward;
        this.target = target;
    }
}
