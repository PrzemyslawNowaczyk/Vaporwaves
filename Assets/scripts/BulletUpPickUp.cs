using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletUpPickUp : MonoBehaviour {

    public GameObject root;

    Player player;

    private void OnTriggerEnter(Collider other) {
        player = other.GetComponentInParent<Player>();
        if (player) {
            player.IncreaseBullets();
        }
        Destroy(root);
    }

}