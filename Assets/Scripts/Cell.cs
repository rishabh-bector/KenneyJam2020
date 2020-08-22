using UnityEngine;
using System.Collections.Generic;

public struct DirChange {
    public Vector3 position;
    public float radius;
    public Vector2 velocity;
}

public class Cell : MonoBehaviour {
    // References
    public GameObject grass;
    public GameObject dirt;
    public GameObject pathStraight;
    public GameObject pathTurn;
    public GameObject rangeHoopMesh;

    // Config
    public GameObject[] tiles;

    // State
    public int[] tileTypes;
    public Vector2 spawnVelocity;

    public GameObject towerSelected;
    public bool hovered;

    private void Start() {
        InitTiles();
    }

    private void InitTiles() {
        tiles = new GameObject[5] { null, dirt, grass, pathStraight, pathTurn };
    }

    public void SetTile(int tileType, int rotation, bool upper = false) {
        // Remove old mesh
        var child = transform.Find("lowerTile");
        if (upper) child = transform.Find("upperTile");
        if (child != null) {
            Destroy(child.gameObject);
            child.parent = null;
        }

        // Create new mesh
        if (tileType > 0) {
            if (tiles.Length == 0) InitTiles();
            var newT = Instantiate(tiles[tileType]);
            newT.transform.parent = transform;
            newT.name = "lowerTile";
            if (upper) newT.name = "upperTile";
            if (tileTypes.Length == 0) tileTypes = new int[2];
            if (upper) tileTypes[1] = tileType;
            else tileTypes[0] = tileType;

            newT.transform.position = new Vector3(0, 0, 0);
            newT.transform.localPosition = new Vector3(0, 0, 0);
            if (upper) {
                newT.transform.position = new Vector3(0, 0, 0);
                newT.transform.localPosition = new Vector3(0, 0.2f, 0);
            }

            if (rotation == 1) newT.transform.Rotate(new Vector3(0, 90, 0));
            if (rotation == 2) newT.transform.Rotate(new Vector3(0, 180, 0));
            if (rotation == 3) newT.transform.Rotate(new Vector3(0, 270, 0));
        }
    }

    public void SetPos(Vector3 pos) { transform.position = pos; }

    public void CreateTower() {
        if (transform.parent.GetComponent<Map>().towerSelected == null) return;
        var tower = Instantiate(transform.parent.GetComponent<Map>().towerSelected);
        tower.transform.parent = transform.parent;
        tower.GetComponent<Tower>().map = transform.parent.GetComponent<Map>();
        tower.GetComponent<Tower>().cell = this;
        tower.name = "towerMesh";
        tower.transform.position = transform.position;
        var pos = transform.localPosition;
        pos.y = -1.0f;
        tower.transform.localPosition = pos;
        transform.parent.GetComponent<Map>().towerSelected = null;
    }

    public void RangeHoop(float range, bool on = true) {
        if (on) {
            var rangeHoop = Instantiate(rangeHoopMesh);
            rangeHoop.transform.parent = transform.parent;
            rangeHoop.name = "rangeHoop";
            rangeHoop.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            rangeHoop.transform.localScale = new Vector3(range, 1.0f, range);
            return;
        }
        Destroy(transform.parent.Find("rangeHoop").gameObject);
    }

    public void Shade(bool on = true) {
        Color color;
        if (on) {
            color = transform.Find("upperTile").GetComponent<MeshRenderer>().materials[1].color;
            color -= Color.grey;
            transform.Find("upperTile").GetComponent<MeshRenderer>().materials[1].color = color;
            return;
        }
        color = transform.Find("upperTile").GetComponent<MeshRenderer>().materials[1].color;
        color += Color.grey;
        transform.Find("upperTile").GetComponent<MeshRenderer>().materials[1].color = color;
    }

    void OnMouseEnter() {
        if (transform.parent.GetComponent<Map>().towerSelected != null && tileTypes[1] == 2) {
            var range = (float)transform.parent.GetComponent<Map>().towerSelected.GetComponent<Tower>().range;
            RangeHoop(range);
            Shade();
            hovered = true;
        }
    }

    void OnMouseExit() {
        if (transform.parent.GetComponent<Map>().towerSelected != null && tileTypes[1] == 2) {
            RangeHoop(0f, false);
            Shade(false);
            hovered = false;
        }
    }

    void OnMouseDown() {
        if (transform.parent.GetComponent<Map>().towerSelected && hovered) {
            hovered = false;
            Shade(false);
            Destroy(transform.parent.Find("rangeHoop").gameObject);
            CreateTower();
        }
    }
}
