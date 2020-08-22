using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Tower
{
    // References
    public GameObject bulletMesh;
    // Config
    public float shotRate;
    public float bulletSpeed;
    public float bulletDamage;
    public override double range {get; set;} = 3;
    public float[] rangeUpgrades = {1.5f, 2f, 3f};
    public float[] speedUpgrades = {1.5f, 2f, 3f};
    public float[] rateUpgrades = {1.5f, 2f, 3f};
    // State
    private float shotTimer;
    private bool loaded;

    // Start is called before the first frame update
    void Start() {
        map = transform.parent.GetComponent<Map>();
        shotRate = 2;
        bulletDamage = 0.55f;
        bulletSpeed = 7f;
        shotTimer = shotRate;
    }

    // Update is called once per frame
    void Update() {
        var enemy = TargetEnemy();
        if (enemy != null) {
            transform.LookAt(enemy.transform);
        }
        if (loaded) {
            if (enemy == null) {
                return;
            }
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

    public override void Upgrade() {
        level += 1;
        var rangeMul = rangeUpgrades[level];
        var speedMul = speedUpgrades[level];
        var rateMul = rateUpgrades[level];
        range *= rangeMul;
        bulletSpeed *= speedMul;
        shotRate /= rateMul;
    }
}
