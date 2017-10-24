using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour {
    public float Speed;

    private void OnCollisionEnter(Collision collision) {
        var player = collision.gameObject.GetComponent<Player>();
        if (player) {
            player.GetComponent<Rigidbody>().AddForce(transform.up * Speed, ForceMode.VelocityChange);
        }
    }

}