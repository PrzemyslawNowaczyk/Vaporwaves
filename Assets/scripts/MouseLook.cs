using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour {

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    float rotationX = 0F;
    float rotationY = 0F;
    Quaternion originalRotation;
    Quaternion xQuaternion;
    Quaternion yQuaternion;


    void Update() {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (axes == RotationAxes.MouseXAndY) {
            // Read the mouse input axis
            rotationX += Input.GetAxisRaw("Mouse X") * sensitivityX;
            rotationY += Input.GetAxisRaw("Mouse Y") * sensitivityY;
            rotationX = ClampAngle(rotationX, minimumX, maximumX);
            rotationY = ClampAngle(rotationY, minimumY, maximumY);
            xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
        }
        else if (axes == RotationAxes.MouseX) {
            rotationX += Input.GetAxisRaw("Mouse X") * sensitivityX;
            rotationX = ClampAngle(rotationX, minimumX, maximumX);
            xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        }
        else {
            rotationY += Input.GetAxisRaw("Mouse Y") * sensitivityY;
            rotationY = ClampAngle(rotationY, minimumY, maximumY);
            yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
        }
    }

    private void FixedUpdate() {
        if (axes == RotationAxes.MouseXAndY) {
            transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        }
        else if (axes == RotationAxes.MouseX) {
            transform.localRotation = originalRotation * xQuaternion;
        }
        else {
            transform.localRotation = originalRotation * yQuaternion;
        }
    }

    void Start() {
        originalRotation = transform.localRotation;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static float ClampAngle(float angle, float min, float max) {
        if (angle < -360F)
             angle += 360F;
        if (angle > 360F)
             angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

}