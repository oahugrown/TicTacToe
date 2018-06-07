using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWin : MonoBehaviour 
{
    public static bool bigWinActive = false;
    public static bool coinCannonActive = false;

    public static void ActivateBigWin(bool active)
    {
        // Activate big win sprite animation
        GameObject.FindWithTag("BigWin").transform.GetChild(0).gameObject.SetActive(active);
        bigWinActive = active;
    }

    public static void ActivateCoins(bool active)
    {
        // Activate coin cannons
        GameObject[] cannons = GameObject.FindGameObjectsWithTag("CoinCannon");
        if (cannons.Length > 0)
        {
            foreach (GameObject cannon in cannons)
            {
                if (active)
                    cannon.GetComponent<Cannon>().FireAway();
                else
                    cannon.GetComponent<Cannon>().StopFire();
            }
        }
        coinCannonActive = active;
    }
}
