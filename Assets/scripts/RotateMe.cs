using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMe : MonoBehaviour {

    public Vector3 Rotation;
	
	void Update () {
        transform.Rotate(Rotation);
	}
}
