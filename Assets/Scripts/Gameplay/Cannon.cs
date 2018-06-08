// YYeung
// Canon.cs

using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour 
{
    public GameObject launchedObject;
    public Transform[] endTargets;
    public float launchRate = 0.2f;
    public int maxCoins = 30;
    private List<GameObject> coins = new List<GameObject>();

    private void Start()
    {
        coins = new List<GameObject>();
        for (int i = 0; i < maxCoins; ++i)
        {
            coins.Add(NewSpawn());
        }
    }

    public void FireAway()
    {
        launchRate = Mathf.Abs(launchRate);
        InvokeRepeating("Fire", 0, launchRate);
    }

    public void StopFire()
    {
        CancelInvoke();
        foreach(GameObject coin in coins)
        {
            coin.SetActive(false);
        }
    }
    private void Fire()
    {
        for (int i = 0; i < coins.Count; ++i)
        {
            if (!coins[i].activeInHierarchy)
            {
                coins[i].transform.position = transform.position;
                coins[i].SetActive(true);
                break;
            }
        }
    }

    GameObject NewSpawn()
    {
        GameObject newSpawn = Instantiate(launchedObject);
        Launch newLaunch = newSpawn.GetComponent<Launch>();
        newLaunch.startMarker = this.transform;

        if (endTargets.Length < 1)
        {
            Debug.LogError("No end targets set in Cannon.NewSpawn()");
            return null;
        }

        int rng = Random.Range(0, endTargets.Length);
        newLaunch.endMarker = endTargets[rng];
        newSpawn.SetActive(false);
        return newSpawn;
    }
}
