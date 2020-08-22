using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    // References
    public Rigidbody body;
    public HealthBar healthBar;

    public void SetVelocity(Vector2 velocity) {
        var v = body.velocity;
        v.x = velocity.x;
        v.z = velocity.y;
        body.velocity = v;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("bullet")) {
            Destroy(other.gameObject);
            healthBar.SetHealth(healthBar.GetHealth() - 0.55f);
            if (healthBar.GetHealth() <= 0) {
                GetComponentInParent<Map>().RemoveEnemy(this, 0); 
            }
        }

        if (other.gameObject.tag.Equals("end")) {
            GetComponentInParent<Map>().RemoveEnemy(this, 1);
        }
    }
}
