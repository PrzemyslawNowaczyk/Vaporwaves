using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadDown : MonoBehaviour {

    public GameObject root;
    Player player;

    private void OnTriggerEnter(Collider other) {
        player = other.GetComponentInParent<Player>();
        if (player) {
            player.DecreaseSpread();
        }
        Destroy(root);
    }
}