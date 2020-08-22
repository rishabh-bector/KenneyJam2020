using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    // References
    public GameObject bulletMesh;
    public Map map;
    // Config
    public float shotRate;
    public float bulletSize;
    public float bulletSpeed;
    public int range;

    // State
    private float shotTimer;
    private bool loaded;

    // Start is called before the first frame update
    void Start() {
        map = transform.parent.GetComponent<Map>();
        shotRate = 2;
        bulletSize = 0.3f;
        bulletSpeed = 10f;
        range = 3;
        shotTimer = shotRate;
    }

    // Update is called once per frame
    void Update() {
        var enemy = TargetEnemy();
        transform.LookAt(enemy.transform);
        if (loaded) {
            var bullet = Instantiate(bulletMesh);
            bullet.transform.parent = transform.parent;
            bullet.transform.position = transform.position;
            var travelVec = enemy.transform.position - bullet.transform.position;
            bullet.GetComponent<Rigidbody>().velocity = Vector3.Normalize(travelVec) * bulletSpeed;
            var travelTime = travelVec.magnitude / bulletSpeed;
            var off = enemy.GetComponent<Rigidbody>().velocity * travelTime;
            var go = new GameObject();
            go.transform.position = enemy.transform.position + off;
            bullet.transform.LookAt(enemy.transform);
            travelVec = go.transform.position - bullet.transform.position;
            Destroy(go);
            bullet.GetComponent<Rigidbody>().velocity = Vector3.Normalize(travelVec) * bulletSpeed;
            bullet.transform.Rotate(new Vector3(90, 0, 0));
            loaded = false;
            shotTimer = shotRate;
            return;
        }
        shotTimer -= Time.deltaTime;
        if (shotTimer <= 0) {
            loaded = true;
        }
    }

    GameObject TargetEnemy() {
        Enemy closest = map.enemies[0];
        double lowestDistance = 1e99;
        foreach (var enemy in map.enemies)
        {
            var distance = (enemy.transform.position - transform.position).magnitude;
            if (distance < lowestDistance) {
                closest = enemy;
                lowestDistance = distance;
            }
        }
        return closest.gameObject;
    }
}
