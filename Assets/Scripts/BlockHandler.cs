using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BlockHandler : MonoBehaviour
{
    private BlockType type;
    private RollHandler rollHandler;
    private bool blocked = false;

    private void FixedUpdate()
    {
        if (rollHandler.GetRolling() && !blocked) {
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().linearVelocityY += -10;
        }
        else
        {
            GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }

    public void SetType(BlockType type)
    {
        this.type = type;
        GetComponent<SpriteRenderer>().sprite = rollHandler.GetGeneratedSprites()[(int)type];
        GetComponent<SpriteRenderer>().size = new Vector2(1, 1);
    }

    public BlockType GetBlockType()
    {
        return type;
    }

    public void SetRollHandler(RollHandler rollHandler)
    {
        this.rollHandler = rollHandler;
    }

    public void SetBlocked(bool blocked)
    {
        this.blocked = blocked;
    }
}
