using System.Collections;
using UnityEngine;
using TMPro;

public class TextUIElement : MonoBehaviour {

    TextMeshProUGUI _text;
    RectTransform _rect;
    Color startColor;
    public string TextBeforeValue;
    public string Format;
    Coroutine handle;

	void Initialize () {
        _text = GetComponent<TextMeshProUGUI>();
        startColor = _text.color;
        _rect = GetComponent<RectTransform>();
	}
	
	public void UpdateTextValue(float f) {
        if (_text == null) {
            Initialize();
        }
        _text.text = TextBeforeValue + f.ToString(Format);

        if (handle != null) {
            StopCoroutine(handle);
        }
        
        handle = StartCoroutine(Expose());
    }

    private IEnumerator Expose() {
        for (float i = 2.0f; i >= 1.0f; i -= Time.deltaTime) {
            _rect.localScale = Vector3.one * i;
            _text.color = Color.Lerp(startColor, Color.magenta, i - 1.0f);
            yield return null;
        }
        _rect.localScale = Vector3.one;
        _text.color = startColor;
        handle = null;
    }
}
