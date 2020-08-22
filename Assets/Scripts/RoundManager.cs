using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public struct EnemyWave {
    public GameObject enemyPrefab;
    public float spawnRate;
    public int waveSize;
}

public struct Round {
    public Transform spawnPoint;
    public Vector2 spawnVelocity;
    public List<EnemyWave> waves;
}

public class RoundManager : MonoBehaviour {
    // References
    public Map map;
    public TMP_Text roundLabel;

    // Config
    public List<Round> rounds;

    // State
    [HideInInspector]
    private int currentRound = -1;
    public int currentWave;
    public int currentEnemy;
    public bool playing;
    public float spawnTimer;

    public void Init(List<Round> rounds) {
        this.rounds = rounds;
        roundLabel.text = "Round: 1";
    }
    
    public void NextRound() {
        currentRound++;
        currentWave = 0;
        currentEnemy = 0;
        playing = true;
        Debug.Log("Starting round: " + currentRound);
        roundLabel.text = "Round: " + (currentRound + 1);
    }

    public Round BuildRound(Transform spawnPoint, Vector2 spawnVelocity, List<EnemyWave> waves) {
        var r = new Round();
        r.spawnPoint = spawnPoint;
        r.spawnVelocity = spawnVelocity;
        r.waves = waves;
        return r;
    }

    public EnemyWave BuildWave(GameObject enemyPrefab, float rate, int size) {
        var e = new EnemyWave();
        e.enemyPrefab = enemyPrefab;
        e.spawnRate = rate;
        e.waveSize = size;
        return e;
    }

    private void NextSpawn() {
        if (currentEnemy == rounds[currentRound].waves[currentWave].waveSize) {
            currentWave++;
            currentEnemy = 0;
            if (currentWave == rounds[currentRound].waves.Count) {
                playing = false;
                return;
            }
        }

        var pos = rounds[currentRound].spawnPoint.localPosition;
        pos.y = -0.4f;

        var child = Instantiate(rounds[currentRound].waves[currentWave].enemyPrefab);
        child.transform.parent = map.transform;
        child.transform.position = rounds[currentRound].spawnPoint.position;
        child.transform.localPosition = pos;
        child.name = "enemyMesh";

        child.GetComponent<Enemy>().SetVelocity(rounds[currentRound].spawnVelocity);
        GetComponentInParent<Map>().AddEnemy(child.GetComponent<Enemy>());

        currentEnemy++;
    }

    private void Update() {
        if (!playing) return;
        if (currentRound >= rounds.Count) return;
        if (currentWave >= rounds[currentRound].waves.Count) return;
        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0) {
            NextSpawn();
            if (currentWave >= rounds[currentRound].waves.Count) return;
            spawnTimer = rounds[currentRound].waves[currentWave].spawnRate;
        }
    }
}
