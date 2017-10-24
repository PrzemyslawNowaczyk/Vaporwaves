using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DropChance {
    public static float Multiplier =1.0f;
    public GameObject DropPrefab;
    public float DropChanceBase;
    public float DropChancePercent {
        get { return DropChanceBase* Multiplier; }
    }

    public static void ResetMultiplier() {
        Multiplier = 0.0f;
    }
}