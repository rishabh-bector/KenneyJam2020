using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    // References
    public Rigidbody body;

    public void SetVelocity(Vector2 velocity) {
        var v = body.velocity;
        v.x = velocity.x;
        v.z = velocity.y;
        body.velocity = v;
    }
}
