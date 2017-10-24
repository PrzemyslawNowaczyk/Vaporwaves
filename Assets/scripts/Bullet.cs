using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public GameObject Decal;
    public Rigidbody Rigid;
    public Renderer Rend;
    public Collider Collid;

    public float Damage;


    void Start () {
        Destroy(gameObject, 10.0f);
	}

    private void OnCollisionEnter(Collision collision) {
        var decal = Instantiate(Decal, collision.contacts[0].point, Quaternion.identity);
        decal.GetComponent<ParticleSystem>().Emit(200);
        Destroy(decal, 3.0f);
        Destroy(gameObject, 3.0f);
        Rigid.velocity = Vector3.zero;
        Collid.enabled = false;
        Rend.enabled = false;

        var enemy = collision.gameObject.GetComponent<EnemyBase>();

        if (!enemy) {
            enemy = collision.gameObject.GetComponentInParent<EnemyBase>();
        }

        if (!enemy) {
            enemy = collision.gameObject.GetComponentInChildren<EnemyBase>();
        }

        if (enemy) {
            enemy.TakeDamage(Damage);
        }
    }

}
