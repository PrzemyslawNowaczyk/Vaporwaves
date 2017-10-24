using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetStartingPosition : MonoBehaviour {

    public Image img;
    public float distanceFromCamera;

	void FixedUpdate () {
        var ray = Camera.main.ScreenPointToRay(img.rectTransform.position);
        transform.position = ray.origin + ray.direction.normalized * distanceFromCamera;
    }
	

}
