// YYeung
// Launch.cs

using System.Collections;
using UnityEngine;

public class Launch : MonoBehaviour 
{
    public float minSpeed = 1.0f;
    public float maxSpeed = 8.0f;
    private float speed = 1.0F;

    private float startTime;
    private float endTime;

    [HideInInspector]
    public Transform startMarker;
    //[HideInInspector]
    public Transform endMarker;

    public float lifeSpan;

    void OnEnable()
    {
        if (endMarker == null)
            endMarker = this.transform;
        startMarker = this.transform;
        speed = Random.Range(minSpeed, maxSpeed);
        Invoke("Destroy", lifeSpan);
        startTime = Time.time;
        endTime = startTime + speed;
        StartCoroutine(LaunchObject());
    }
    public IEnumerator LaunchObject()
    {
        while (Time.time < endTime)
        {
            float timeSoFar = Time.time - startTime;
            float fractionTime = timeSoFar / speed;

            this.transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionTime);

            yield return null;
        }
    }

    private void Destroy()
    {
        this.gameObject.SetActive(false);
    }
}
