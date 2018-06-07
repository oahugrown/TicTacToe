// YYeung
// Launch.cs

using UnityEngine;

public class Launch : MonoBehaviour 
{
    public float minSpeed = 1.0f;
    public float maxSpeed = 8.0f;
    private float speed = 1.0F;

    [HideInInspector]
    public Transform startMarker;
    [HideInInspector]
    public Transform endMarker;

    public float lifeSpan;

    void Start()
    {
        this.transform.position = startMarker.transform.position;
        speed = Random.Range(minSpeed, maxSpeed);

        Invoke("Destroy", lifeSpan);
    }
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, endMarker.transform.position, step);
    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }
}
