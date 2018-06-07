// YYeung
// Gravity.cs

using UnityEngine;

public class Gravity : MonoBehaviour 
{
    public float minScale = 0.05f;
    public float maxScale = 0.5f;
    private float gravity = 0;
    private float gravityScale = 0.01f;
    private float scale = 1;

    private bool hasGravity = true;
    private float gravityCap = 1000;

    private void Awake()
    {
        scale = Random.Range(Mathf.Abs(minScale), Mathf.Abs(maxScale));
        if (scale < 0)
            hasGravity = false;
    }

    void Update () 
    {
        if (!hasGravity)
            return;


        // Get where the current y is now to translate it further
        float currentY = this.transform.position.y;
        gravity += gravityScale;
        if (gravity > gravityCap)
            gravity = gravityCap;
        currentY -= gravity * scale;
        Vector2 newPosition = new Vector2(this.transform.position.x, currentY);
        this.transform.position = newPosition;
	}
}
