using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTriggerController : MonoBehaviour {

    public LayerMask CollisionMask;
    public Vector3 Center;
    public Vector3 GlobalSize;

    public bool IsColliding { get { return _collision; } }
    public float CollidingTime { get; private set; }
    public float TimeSinceLeftCollision { get; private set; }
    public bool LastFrameWasColliding { get; private set; }

    bool _collision = false;

    private void Awake() {
        _collision = false;
    }


    private void FixedUpdate() {
        LastFrameWasColliding = _collision;

        if (Physics.CheckBox(transform.position + Center, GlobalSize/2.0f, transform.rotation, CollisionMask)){
            CollidingTime += Time.fixedDeltaTime;
            _collision = true;
            TimeSinceLeftCollision = 0.0f;
        }
        else {
            _collision = false;
            CollidingTime = 0.0f;
            TimeSinceLeftCollision += Time.fixedDeltaTime;
        }
    }


    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireCube(transform.position + Center, GlobalSize);
    }

}
