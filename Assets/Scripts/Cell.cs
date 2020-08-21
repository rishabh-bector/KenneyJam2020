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

    // Config
    public GameObject[] tiles;

    // State
    private GameObject spawnChild;
    public Vector2 spawnVelocity;
    public float spawnRate;
    public float spawnTimer;

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

    public void SetSpawner(GameObject spawn, float rate, Vector2 velocity) {
        if (spawn == null) return;
        spawnChild = spawn;
        spawnRate = rate;
        spawnVelocity = velocity;
    }

    public void Spawn() {
        if (spawnChild == null) return;
        var child = Instantiate(spawnChild);
        child.transform.parent = transform.parent;
        child.name = "enemyMesh";
        child.transform.position = transform.position;
        var pos = transform.localPosition;
        pos.y = -0.4f;
        child.transform.localPosition = pos;
        child.GetComponent<Enemy>().SetVelocity(spawnVelocity);
        spawnTimer = spawnRate;
        GetComponentInParent<Map>().AddEnemy(child.GetComponent<Enemy>());
    }

    private void Update() {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0) {
            Spawn();
            spawnTimer = spawnRate;
        }
    }
}
