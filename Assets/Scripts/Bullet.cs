using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed, damage;
    public float lifeTime = 2;
    public LayerMask canDamage;
    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject,lifeTime);
    }

    private void Update () {
        rb.velocity = transform.forward * speed;
	}

    private void OnTriggerEnter(Collider col) {
        if (((1 << col.gameObject.layer) & canDamage) != 0) {
            col.SendMessage("ReceiveDamage", damage);
        }
    }
}
