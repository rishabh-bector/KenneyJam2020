using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour {
    // References
    public Map map;

    // Config
    private double _range;
    public abstract double range {get; set;}
}
