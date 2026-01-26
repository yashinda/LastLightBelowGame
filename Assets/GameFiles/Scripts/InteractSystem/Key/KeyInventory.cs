using UnityEngine;

public class KeyInventory : MonoBehaviour
{
    public static KeyInventory Instance { get; private set; }

    public bool HasKey { get; private set; }

    private int keyCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void AddKey()
    {
        keyCount++;
        HasKey = true;
    }

    public void UseKey()
    {
        keyCount--;
        if (keyCount == 0)
            HasKey = false;
    }
}
