// YYeung
// RandomScale.cs

using UnityEngine;

public class RandomScale : MonoBehaviour 
{
    public float minSize = 1.0f;
    public float maxSize = 10.0f;


    // Use this for initialization
    void Start () {
        float rng = Random.Range(minSize, maxSize);
        this.transform.localScale = new Vector3(rng, rng);
	}
}
