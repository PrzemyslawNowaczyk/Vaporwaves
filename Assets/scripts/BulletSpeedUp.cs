using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpeedUp : MonoBehaviour {

    public GameObject root;
    Player player;

    private void OnTriggerEnter(Collider other) {
        player = other.GetComponentInParent<Player>();
        if (player) {
            player.IncreaseBulletSpeed();
        }
        Destroy(root);
    }

}
