// Yyeung
// FollowObject.cs

using UnityEngine;

public class FollowObject : MonoBehaviour 
{

    public GameObject objectToFollow;


	void Update () {
        this.transform.position = objectToFollow.transform.position;
	}
}
