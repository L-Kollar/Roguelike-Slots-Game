using Unity.VisualScripting;
using UnityEngine;

public class BlockChecker : MonoBehaviour
{
    private int column;
    private RollHandler rollHandler;

    private void Start()
    {
        rollHandler = GameObject.Find("RollSpawner").GetComponent<RollHandler>();
    }

    public void LoadRollMap()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down);
        int row = -1;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Block"))
            {
                if (row == -1 || row >= rollHandler.GetRowCount())
                {
                    row++;
                    continue;
                }
                rollHandler.GetRollMap()[column, row] = hit.transform.gameObject;
                row++;
            }
        }
    }

    public void SetColumn(int column)
    {
        this.column = column;
    }
}
