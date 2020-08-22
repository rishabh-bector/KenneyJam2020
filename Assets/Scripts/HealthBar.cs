using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    // References
    public Enemy parent;
    public Camera mainCamera;
    public Slider slider;

    // Config
    public float vertOffset;

    private void Update() {
        if (parent == null) return;
        transform.position = mainCamera.WorldToScreenPoint(parent.transform.position);
        var locPos = transform.localPosition;
        locPos.y -= vertOffset;
        transform.localPosition = locPos;
    }

    public void SetHealth(float h) { slider.value = h; }
}
