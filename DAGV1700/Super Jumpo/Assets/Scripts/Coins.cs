using System;
using UnityEngine;

public class Coins : MonoBehaviour, IItem
{
    public static event Action<int> OnCollect;
    private bool hasBeenCollected = false;
    public int worth = 1;

    public void Collect()
    {
        // Only run the collection logic if the coin has not already been collected
        if (!hasBeenCollected)
        {
            // Set the flag immediately to prevent further calls
            hasBeenCollected = true;

            if (OnCollect != null)
            {
                SoundEffectManager.Instance.PlaySound("CoinSFX1");
                OnCollect.Invoke(worth);
            }

            // Destroy the coin GameObject after collection
            Destroy(gameObject);
        }
    }
}
