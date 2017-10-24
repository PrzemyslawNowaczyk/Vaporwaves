using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropChanceManager : MonoBehaviour {

    [Range(1.0f, 100.0f)]
    public float SecondsForPlusOneMultiplier = 10.0f;

	// Update is called once per frame
	void Update () {
        DropChance.Multiplier += Time.deltaTime / SecondsForPlusOneMultiplier;
	}


}
