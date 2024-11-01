using NUnit.Framework;
using Shapes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RollHandler : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RoundHandler roundHandler;

    [SerializeField] private int rowCount = 5;
    [SerializeField] private int columnCount = 7;
    private GameObject[,] rollMap;

    [SerializeField] GameObject ground;
    [SerializeField] Texture2D[] blockPictures;

    private BlockSpawner[] blockSpawners;
    private BlockChecker[] blockCheckers;
    
    private List<Sprite> generatedSprites = new List<Sprite>();

    private bool rolling = false;
    private bool stopped = true;

    [SerializeField] Transform rollMapTransform;
    [SerializeField] GameObject blockSpawnerPrefab;
    [SerializeField] GameObject destroyHandlerPrefab;
    [SerializeField] GameObject readyCheckPrefab;
    [SerializeField] GameObject groundPrefab;

    [SerializeField] GameObject linePrefab;

    private float aspect = (float)Screen.width / Screen.height;

    private float worldHeight;
    private float worldWidth;

    private List<Polyline> lineVisuals = new List<Polyline>();

    [SerializeField] TMP_Text winCounterText;
    private float score = 0;

    //multipliers
    private Dictionary<BlockType, float> multipliers = new Dictionary<BlockType, float>()
    {
        { BlockType.None, 0.1f },
        { BlockType.Cherry, 1 },
        { BlockType.Lemon, 3 },
        { BlockType.Apple, 5 },
        { BlockType.Melon, 10 },
        { BlockType.Seven, 100 },
        { BlockType.Joker, 100 },
    };


    //Chances
    private Dictionary<BlockType, float> chances = new Dictionary<BlockType, float>()
    {
        { BlockType.None, 500 },
        { BlockType.Cherry, 400 },
        { BlockType.Lemon, 50 },
        { BlockType.Apple, 30 },
        { BlockType.Melon, 10 },
        { BlockType.Seven, 7 },
        { BlockType.Joker, 3 },
    };

    private void Start()
    {
        worldHeight = mainCamera.orthographicSize * 2;
        worldWidth = worldHeight * aspect;

        rollMapTransform.position = new Vector2(-15, worldHeight / 2);
        blockCheckers = new BlockChecker[columnCount];
        blockSpawners = new BlockSpawner[columnCount];
        CreateMap();
        rollMap = new GameObject[columnCount, rowCount];
        foreach (Texture2D texture in blockPictures)
        {
            generatedSprites.Add(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)));
        }
    }

    private void CreateMap()
    {
        GameObject readyCheckBuffer = Instantiate(readyCheckPrefab, rollMapTransform);
        readyCheckBuffer.GetComponent<BoxCollider2D>().size = new Vector2(worldWidth, 1);

        GameObject destroyHandlerBuffer = Instantiate(destroyHandlerPrefab, rollMapTransform);
        destroyHandlerBuffer.transform.position = new Vector2(-15, -worldHeight / 2);
        destroyHandlerBuffer.GetComponent<BoxCollider2D>().size = new Vector2(worldWidth, 1);
        float bufferX = -1.6f * ((columnCount - 1) / 2);
        for (int i = 0; i < columnCount; i++)
        {
            GameObject blockSpawnerBuffer = Instantiate(blockSpawnerPrefab, rollMapTransform);
            blockSpawnerBuffer.transform.position += new Vector3(bufferX + (1.6f * i), 0, 0);
            blockSpawnerBuffer.transform.GetChild(1).GetComponent<BlockChecker>().SetColumn(i);

            blockSpawners[i] = blockSpawnerBuffer.transform.GetChild(0).GetComponent<BlockSpawner>();
            blockCheckers[i] = blockSpawnerBuffer.transform.GetChild(1).GetComponent<BlockChecker>();

            GameObject groundBuffer = Instantiate(groundPrefab, ground.transform);
            groundBuffer.transform.position = new Vector3(blockSpawnerBuffer.transform.position.x, (-worldHeight / 2) - 0.5f, 0);
        }
    }

    public void CheckWin()
    {
        foreach(BlockChecker blockChecker in blockCheckers) {
            blockChecker.LoadRollMap();
        }
        CheckLines();
    }

    private void CheckLines()
    {
        float score = 0;

        for (int i = 0; i < rowCount; i++)
        {
            lineVisuals.Add(Instantiate(linePrefab).GetComponent<Polyline>());
            lineVisuals[lineVisuals.Count - 1].transform.position = rollMap[0, i].transform.position;
            lineVisuals[lineVisuals.Count - 1].AddPoint(Vector3.zero);
            if ((int)rollMap[0, i].GetComponent<BlockHandler>().GetBlockType() != 0)
            {
                float lineScore = CheckHorizontal(0, i, 0);

                if (winCounterText.text != "") winCounterText.text += " + ";
                winCounterText.text += lineScore;

                score += lineScore;
            }
        }
        winCounterText.text += " = " + score;
        this.score = score;
    }

    private float CheckHorizontal(int column, int row, float score)
    {
        if (column + 1 < columnCount && (rollMap[column, row].GetComponent<BlockHandler>().GetBlockType() == rollMap[column + 1, row].GetComponent<BlockHandler>().GetBlockType() || (int)rollMap[column + 1, row].GetComponent<BlockHandler>().GetBlockType() == 6 || (int)rollMap[column, row].GetComponent<BlockHandler>().GetBlockType() == 6))
        {
            lineVisuals.Add(Instantiate(linePrefab).GetComponent<Polyline>());
            lineVisuals[lineVisuals.Count - 1].transform.position = rollMap[column, row].transform.position;
            lineVisuals[lineVisuals.Count - 1].AddPoint(Vector3.zero);
            
            lineVisuals[lineVisuals.Count - 1].AddPoint(lineVisuals[lineVisuals.Count - 1].transform.InverseTransformPoint(rollMap[column + 1, row].transform.position));
            score = CheckHorizontal(column + 1, row, score + multipliers[rollMap[column + 1, row].GetComponent<BlockHandler>().GetBlockType()]);
        } else if (column + 1 < columnCount && row - 1 >= 0 && (rollMap[column, row].GetComponent<BlockHandler>().GetBlockType() == rollMap[column + 1, row - 1].GetComponent<BlockHandler>().GetBlockType() || (int)rollMap[column + 1, row - 1].GetComponent<BlockHandler>().GetBlockType() == 6 || (int)rollMap[column, row].GetComponent<BlockHandler>().GetBlockType() == 6))
        {
            lineVisuals.Add(Instantiate(linePrefab).GetComponent<Polyline>());
            lineVisuals[lineVisuals.Count - 1].transform.position = rollMap[column, row].transform.position;
            lineVisuals[lineVisuals.Count - 1].AddPoint(Vector3.zero);
            
            lineVisuals[lineVisuals.Count - 1].AddPoint(lineVisuals[lineVisuals.Count - 1].transform.InverseTransformPoint(rollMap[column + 1, row - 1].transform.position));
            score = CheckHorizontal(column + 1, row - 1, score + multipliers[rollMap[column + 1, row - 1].GetComponent<BlockHandler>().GetBlockType()]);
        } else if (column + 1 < columnCount && row + 1 < rowCount && (rollMap[column, row].GetComponent<BlockHandler>().GetBlockType() == rollMap[column + 1, row + 1].GetComponent<BlockHandler>().GetBlockType() || (int)rollMap[column + 1, row + 1].GetComponent<BlockHandler>().GetBlockType() == 6 || (int)rollMap[column, row].GetComponent<BlockHandler>().GetBlockType() == 6))
        {
            lineVisuals.Add(Instantiate(linePrefab).GetComponent<Polyline>());
            lineVisuals[lineVisuals.Count - 1].transform.position = rollMap[column, row].transform.position;
            lineVisuals[lineVisuals.Count - 1].AddPoint(Vector3.zero);

            lineVisuals[lineVisuals.Count - 1].AddPoint(lineVisuals[lineVisuals.Count - 1].transform.InverseTransformPoint(rollMap[column + 1, row + 1].transform.position));
            score = CheckHorizontal(column + 1, row + 1, score + multipliers[rollMap[column + 1, row + 1].GetComponent<BlockHandler>().GetBlockType()]);
        }

        return score;
    }

    public void Reroll()
    {
        if (roundHandler.UseReroll()) Roll();
    }

    public void Roll()
    {
        winCounterText.text = "";
        ClearLinesVisuals();
        stopped = false;
        rolling = true;
        int column = 0;
        foreach (Transform groundGameobject in ground.transform)
        {
            if (!groundGameobject.GetComponent<GroundHandler>().GetLocked())
            {
                for (int i = 0; i < rowCount; i++)
                {
                    if(rollMap[column, i]) rollMap[column, i].GetComponent<BlockHandler>().SetBlocked(false);
                }
                blockSpawners[column].SetBlocked(false);
                groundGameobject.gameObject.SetActive(false);
            }
            else
            {
                for (int i = 0; i < rowCount; i++)
                {
                    if (rollMap[column, i]) rollMap[column, i].GetComponent<BlockHandler>().SetBlocked(true);
                }
                blockSpawners[column].SetBlocked(true);
            }
            column++;
        }
        Invoke(nameof(StopRoll), Random.Range(1,2));
    }

    private void ClearLinesVisuals()
    {
        foreach (Polyline line in lineVisuals)
        {
            Destroy(line.gameObject);
        }
        lineVisuals.Clear();
    }

    private void StopRoll()
    {
        rolling = false;
        foreach (Transform groundGameobject in ground.transform)
        {
            groundGameobject.gameObject.SetActive(true);
        }
    }

    public List<Sprite> GetGeneratedSprites()
    {
        return generatedSprites;
    }

    public bool GetRolling()
    {
        return rolling;
    }

    public bool GetStopped()
    {
        return stopped;
    }

    public GameObject[,] GetRollMap()
    {
        return rollMap;
    }

    public int GetRowCount()
    {
        return rowCount;
    }

    public void UnlockColumns()
    {
        for (int i = 0; i < columnCount; i++) {
            ground.transform.GetChild(i).GetComponent<GroundHandler>().SetLocked(false);
        }
    }

    public void LockColumn(int column)
    {
        ground.transform.GetChild(column).GetComponent<GroundHandler>().SetLocked(!ground.transform.GetChild(column).GetComponent<GroundHandler>().GetLocked());
    }

    public GameObject GetGround()
    {
        return ground;
    }

    public float GetScore()
    {
        return score;
    }


    //changes chance of type
    public void ChangeChance(BlockType type)
    {
        int buffer = (int) type;
        float chance = chances[type] / 5;

        for (int i = 0; i < buffer; i++)
        {
            chances[(BlockType)i] -= chance / buffer;
        }
        chances[type] += chance;
    }

    public void ChangeMultiplier(BlockType type)
    {
        float multiplier = multipliers[type];
        multipliers[type] += multiplier;
    }

    public Dictionary<BlockType, float> GetChances()
    {
        return chances;
    }
}
