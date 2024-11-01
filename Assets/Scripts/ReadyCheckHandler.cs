using UnityEngine;

public class ReadyCheckHandler : MonoBehaviour
{
    private RollHandler rollHandler;

    private void Start()
    {
        rollHandler = GameObject.Find("RollSpawner").GetComponent<RollHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Invoke(nameof(ReadyCheck), 0.5f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CancelInvoke();
    }

    private void ReadyCheck()
    {
        rollHandler.CheckWin();
        CancelInvoke();
    }
}
