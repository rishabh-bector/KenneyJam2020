using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {
    public GameObject healthPrefab;

    public HealthBar NewHealthBar() {
        var h = Instantiate(healthPrefab);
        h.transform.parent = transform;
        return h.GetComponent<HealthBar>();
    }
}
