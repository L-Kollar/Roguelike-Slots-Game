using UnityEngine;

public class GroundHandler : MonoBehaviour
{
    private bool locked = false;

    public bool GetLocked()
    {
        return locked;
    }

    public void SetLocked(bool locked)
    {
        this.locked = locked;
    }
}
