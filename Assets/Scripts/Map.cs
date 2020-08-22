using UnityEngine;
using System.Collections.Generic;


public class Map : MonoBehaviour {
    // References
    public GameObject cellPrefab;
    public GameObject enemyPrefab;
    public RoundManager roundManager;

    // Config
    public int mapWidth;
    public int mapHeight;

    // State
    public GameObject[,] data;
    public List<DirChange> dirChanges;
    public List<Enemy> enemies;
    public GameObject towerSelected;

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
        enemies = new List<Enemy>();
        
        // Initialize map
        for (int x = 0; x < mapWidth; x++) {
            for (int y = 0; y < mapHeight; y++) {
                data[x, y] = Instantiate(cellPrefab);
                data[x, y].transform.parent = transform;
                data[x, y].transform.position = new Vector3(x, 0, y);
            }
        }

        // Initialize round manager
        EnemyWave r1w1 = roundManager.BuildWave(enemyPrefab, 1, 1);
        Round r1 = roundManager.BuildRound(
            data[0, 4].transform, 
            new Vector2(1, 0), 
            new List<EnemyWave>() { r1w1 }
        );

        roundManager.Init(new List<Round>() { r1 });
        
        dirChanges.Add(BuildDirChange(data[7, 4].transform.position, 1, new Vector2(0, 1)));
        dirChanges.Add(BuildDirChange(data[6, 9].transform.position, 1.5f, new Vector2(1, 0)));

        LoadLevel(emptyLevel);
    }

    private void OnDrawGizmos() {
        // if (data != null) Gizmos.DrawSphere(data[7, 4].transform.position, 1);
        // if (data != null) Gizmos.DrawSphere(data[6, 9].transform.position, 0.75f);
    }

    private void Update() {
        // Manage enemy map movement
        DirChangeUpdate();
    }

    private void DirChangeUpdate() {
        for (int i = 0; i < dirChanges.Count; i++) {
            for (int e = 0; e < enemies.Count; e++) {
                if (IsColliding(dirChanges[i], enemies[e])) {
                    enemies[e].SetVelocity(dirChanges[i].velocity);
                }
            }
        }
    }

    private bool IsColliding(DirChange dirChange, Enemy enemy) {
        float dist = Vector3.Distance(enemy.transform.position, dirChange.position);
        if (dist < dirChange.radius) return true;
        return false;
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

    public void PurchaseTower(GameObject tower) {
        towerSelected = tower;
    }

    private DirChange BuildDirChange(Vector3 position, float radius, Vector2 velocity) {
        var d = new DirChange();
        d.position = position;
        d.position.y += 0.2f;
        d.position.x -= 0.2f;
        d.radius = radius;
        d.velocity = velocity;
        return d;
    }

    public void AddEnemy(Enemy e) { enemies.Add(e); }
}
