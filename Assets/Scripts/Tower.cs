using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour {
    // References
    public Map map;
    public Cell cell;
    // Config
    public abstract double range {get; set;}

    public int level = 1;
    public abstract void Upgrade ();
    private bool clicked;

    public GameObject TargetEnemy() {
        if (map.enemies.Count == 0) {
            return null;
        }
        Enemy closest = map.enemies[0];
        double lowestDistance = 1e99;
        foreach (var enemy in map.enemies)
        {
            var distance = (enemy.transform.position - transform.position).magnitude;
            if (distance < lowestDistance && distance < range) {
                closest = enemy;
                lowestDistance = distance;
            }
        }
        if (lowestDistance == 1e99) {
            return null;
        }
        return closest.gameObject;
    }

    void OnMouseDown() {
        if (clicked) {
            cell.RangeHoop(0f, false);
            cell.Shade(false);
            clicked = false;
            return;
        }
        cell.RangeHoop((float)range);
        cell.Shade();
        clicked = true;
    }
}
