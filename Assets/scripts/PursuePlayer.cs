using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuePlayer : MonoBehaviour {

    public float Speed;
    Vector3 target;

    Rigidbody _rigidbody;
    Transform player;
    public float damping;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	void FixedUpdate () {
        var lookPos = player.position - transform.position;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);

        var newSpeed = transform.forward * Speed;
        _rigidbody.velocity = newSpeed;
	}
}
