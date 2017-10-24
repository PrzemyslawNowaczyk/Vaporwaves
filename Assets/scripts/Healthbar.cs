using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour {

    RectTransform _transform;

    private void Awake() {
        _transform = GetComponent<RectTransform>();
    }

    public void SetHeartsCount(float count) {
        _transform.sizeDelta = new Vector2(16.0f * count, 16.0f);
    }
}
