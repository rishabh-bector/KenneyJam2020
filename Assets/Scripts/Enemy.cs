using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    // References
    public Rigidbody body;

    public void SetVelocity(Vector2 velocity) {
        body.velocity = velocity;
    }
}
