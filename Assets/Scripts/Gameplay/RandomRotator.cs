// YYeung
// RandomRotator.cs

using UnityEngine;

public class RandomRotator : MonoBehaviour 
{
    public float minDegrees = -5;
    public float maxDegrees = -1;
    private float degrees;

    private void Awake()
    {
        degrees = Random.Range(minDegrees, minDegrees);
    }
    private void Update()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime * degrees);
    }
}
