using UnityEngine;

public class CoinCollectible : MonoBehaviour
{
    bool collected;
    public bool TryCollect()
    {
        if (collected) return false;
        collected = true;
        Destroy(gameObject);
        return true;
    }
}
