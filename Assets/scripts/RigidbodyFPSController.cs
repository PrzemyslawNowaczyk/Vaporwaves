using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class RigidbodyFPSController : MonoBehaviour {

    [SerializeField] private CharacterTriggerController groundCheck;

    public float GroundSpeed = 10.0f;
    [Range(0.001f, 10.0f)] public float GroundAccelerationRatio = 5.5f;
    [Range(0.001f, 10.0f)] public float GroundDeccelerationRatio = 5.5f;


    public float JumpSpeed;
    public float AirControl;
    public float FreeFlightDumpen;


    private Rigidbody _rigidbody;
    private Command collectedInput;
    private bool queuedJump;

    public void AddExternalForce(Vector3 force, ForceMode fMode) {
        _rigidbody.AddForce(force, fMode);
    }

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update() {
        collectedInput = CollectInput();

        if (collectedInput.JumpDown) {
            queuedJump = true;
        }
        else if (collectedInput.JumpUp) {
            queuedJump = false;
        }
    }

    void FixedUpdate() {
        collectedInput = CollectInput();
        if (groundCheck.IsColliding) {
            MoveGround();
        }
        else {
            MoveAir();
        }
        
    }

    private void MoveGround() {
        var currentSpeed = _rigidbody.velocity;
        currentSpeed.y = 0.0f;
        var wishSpeedHor = collectedInput.MovementInput.normalized * GroundSpeed;
        wishSpeedHor = transform.TransformDirection(wishSpeedHor);

        if(collectedInput.MovementInput.magnitude < 0.1f) {
            _rigidbody.AddForce(-currentSpeed * GroundDeccelerationRatio * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
        else {
            _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, wishSpeedHor, GroundAccelerationRatio * Time.fixedDeltaTime);
        }

        if (queuedJump) {
            var modifiedSpeed = _rigidbody.velocity;
            modifiedSpeed.y = JumpSpeed;
            _rigidbody.velocity = modifiedSpeed;
            queuedJump = false;
        }
    }

    private void MoveAir() {
        var currentSpeed = _rigidbody.velocity;
        var wishSpeed = collectedInput.MovementInput.normalized * GroundSpeed;
        wishSpeed = transform.TransformDirection(wishSpeed);
        wishSpeed.y = currentSpeed.y;
        var wishSpeedXZ = wishSpeed;
        wishSpeedXZ.y = 0.0f;
        if (wishSpeedXZ.magnitude > 0.1f) {
            _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, wishSpeed, GroundAccelerationRatio * AirControl * Time.fixedDeltaTime);
        }
        else {
            _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, wishSpeed, FreeFlightDumpen * Time.fixedDeltaTime);
        }
    }

    private Command CollectInput() {
        return new Command(
            new Vector3(
                Input.GetAxisRaw("Horizontal"),
                0.0f,
                Input.GetAxisRaw("Vertical")),
            Input.GetButtonDown("Jump"),
            Input.GetButtonUp("Jump"));
    }

    struct Command {

        public Command(Vector3 movement, bool jumpDown, bool jumpUp) {
            MovementInput = movement;
            JumpDown = jumpDown;
            JumpUp = jumpUp;
        }

        public readonly Vector3 MovementInput;
        public readonly bool JumpDown;
        public readonly bool JumpUp;
    }
}
