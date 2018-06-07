// YYeung
// Canon.cs

using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour 
{
    public GameObject launchedObject;
    public Transform[] endTargets;
    public float launchRate = 0.2f;

    public void FireAway()
    {
        launchRate = Mathf.Abs(launchRate);
        InvokeRepeating("NewSpawn", 0, launchRate);
    }

    public void StopFire()
    {
        CancelInvoke();
        GameObject[] coinsLeftInScene = GameObject.FindGameObjectsWithTag("Coin");
        foreach(GameObject coin in coinsLeftInScene)
        {
            Destroy(coin);
        }
    }

    void NewSpawn()
    {
        GameObject newSpawn = Instantiate(launchedObject);
        Launch newLaunch = newSpawn.GetComponent<Launch>();
        newLaunch.startMarker = this.transform;

        if (endTargets.Length < 1)
        {
            Debug.LogError("No end targets set in Cannon.NewSpawn()");
            return;
        }

        int rng = Random.Range(0, endTargets.Length);
        newLaunch.endMarker = endTargets[rng];
    }
}
