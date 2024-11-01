using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] GameObject blockPrefab;
    private RollHandler rollHandler;
    private Collider2D spawnCollider;
    private int size = 1;
    private bool blocked = false;
    private bool stopped = false;

    private void Start()
    {
        spawnCollider = GetComponent<Collider2D>();
        rollHandler = GameObject.Find("RollSpawner").GetComponent<RollHandler>();
    }

    private void FixedUpdate()
    {
        if (!stopped && !rollHandler.GetStopped())
        {
            SpawnBlock();
            stopped = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            SpawnBlock();
        }
    }

    public void SpawnBlock()
    {
        if (blocked || rollHandler.GetStopped()) return;
        GameObject buffer = Instantiate(blockPrefab, transform.position, Quaternion.identity);
        buffer.transform.localScale *= size;
        
        //roll chances
        float random = Random.Range(0, 1000);
        int rolledType = 0;
        int bufferType = 0;
        foreach (float chance in rollHandler.GetChances().Values)
        {
            if(random < chance) {
                rolledType = bufferType;
            }
            bufferType++;
        }
        //set roll type
        buffer.GetComponent<BlockHandler>().SetRollHandler(rollHandler);
        buffer.GetComponent<BlockHandler>().SetType((BlockType) rolledType);
    }

    //set size of block
    public void SetSize(int size)
    {
        this.size = size;
        transform.position += new Vector3(spawnCollider.bounds.size.x/2, 0, 0);
        transform.localScale = new Vector2(size, size);
    }


    public void SetBlocked(bool blocked)
    {
        this.blocked = blocked;
    }
}
