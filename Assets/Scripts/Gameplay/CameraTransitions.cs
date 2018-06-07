using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransitions : MonoBehaviour 
{

    public Transform[] uis;
    public void TransitionCamera(int index)
    {
        Vector3 newPosition = uis[index].position;
        newPosition.z = -10;
        StartCoroutine(DoLerp(newPosition, this.transform.position, Time.deltaTime + 1));
    }

    IEnumerator DoLerp(Vector3 endPosition, Vector3 startPosition, float time)
    {
        float elapsedTime = 0;
        float percentage = elapsedTime / time;
        while (percentage < 1)
        {
            elapsedTime += Time.deltaTime;
            percentage = elapsedTime / time;
            transform.position = Vector3.Lerp(startPosition, endPosition, percentage);
            yield return null;
        }
    }
}
