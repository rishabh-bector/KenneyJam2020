using UnityEngine;
using System.Collections.Generic;


public class Map : MonoBehaviour {
    // References
    public GameObject cellPrefab;
    public GameObject enemyPrefab;

    // Config
    public int mapWidth;
    public int mapHeight;

    // State
    public GameObject[,] data;
    public List<DirChange> dirChanges;

    public static (int, int)[,] emptyLevel = {
        {(2, 0), (2, 0), (2, 0), (2, 0), (3, 1), (2, 0), (2, 0), (2, 0), (2, 0), (2, 0) },
        {(2, 0), (2, 0), (2, 0), (2, 0), (3, 1), (2, 0), (2, 0), (2, 0), (2, 0), (2, 0) },
        {(2, 0), (2, 0), (2, 0), (2, 0), (3, 1), (2, 0), (2, 0), (2, 0), (2, 0), (2, 0) },
        {(2, 0), (2, 0), (2, 0), (2, 0), (3, 1), (2, 0), (2, 0), (2, 0), (2, 0), (2, 0) },
        {(2, 0), (2, 0), (2, 0), (2, 0), (3, 1), (2, 0), (2, 0), (2, 0), (2, 0), (2, 0) },
        {(2, 0), (2, 0), (2, 0), (2, 0), (3, 1), (2, 0), (2, 0), (2, 0), (2, 0), (2, 0) },
        {(2, 0), (2, 0), (2, 0), (2, 0), (4, 0), (3, 0), (3, 0), (3, 0), (4, 2), (2, 0) },
        {(2, 0), (2, 0), (2, 0), (2, 0), (2, 0), (2, 0), (2, 0), (2, 0), (3, 1), (2, 0) },
        {(2, 0), (2, 0), (2, 0), (2, 0), (2, 0), (2, 0), (2, 0), (2, 0), (3, 1), (2, 0) },
        {(2, 0), (2, 0), (2, 0), (2, 0), (2, 0), (2, 0), (2, 0), (2, 0), (3, 1), (2, 0) },
    };

    private void Start() {
        data = new GameObject[mapWidth, mapHeight];
        dirChanges = new List<DirChange>();
        
        for (int x = 0; x < mapWidth; x++) {
            for (int y = 0; y < mapHeight; y++) {
                data[x, y] = Instantiate(cellPrefab);
                data[x, y].transform.parent = transform;
                data[x, y].transform.position = new Vector3(x, 0, y);
            }
        }

        data[0, 4].GetComponent<Cell>().SetSpawner(enemyPrefab, 3, new Vector2(1, 0));

        LoadLevel(emptyLevel);
    }

    private void Update() {
        // Manage enemy map movement
        DirChangeUpdate();
    }

    private void DirChangeUpdate() {
        for (int i = 0; i < dirChanges.Count; i++) {

        }
    }

    private void IsColliding(DirChange dirChange, Enemy enemy) {

    }

    private void LoadLevel((int, int)[,] level) {
        for (int x = 0; x < mapWidth; x++) {
            for (int y = 0; y < mapHeight; y++) {
                data[x, y].GetComponent<Cell>().SetTile(level[x, y].Item1, level[x, y].Item2, upper: true);
                data[x, y].GetComponent<Cell>().SetTile(level[x, y].Item1, level[x, y].Item2, upper: false);
            }
        }
    }

    

    public void AddDirChange(DirChange dirChange) {
        if (dirChanges == null) dirChanges = new List<DirChange>();
        dirChanges.Add(dirChange);
    }
}
