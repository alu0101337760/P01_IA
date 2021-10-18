using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float AccelerationMod;
    public float sensitivity;
    public float DecelerationMod;

    [Range(0, 89)] public float MaxXAngle = 60f;       

    public float maximumMovementSpeed = 1f;
        
    private Vector3 movementSpeed;


    private void Start()
    {
        movementSpeed = Vector3.zero;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1)) {
            MouseRotation();
        }
        var acceleration = CalculateAcceleration();

        movementSpeed += acceleration;

        CalculateDeceleration(acceleration);

        if (movementSpeed.magnitude > maximumMovementSpeed)
        {
            movementSpeed = movementSpeed.normalized * maximumMovementSpeed;
        }

        transform.Translate(movementSpeed);
    }

    private Vector3 CalculateAcceleration()
    {
        var acceleration = Vector3.zero;

        //key input detection
        if (Input.GetKey("w"))
        {
            acceleration.z += 1;
        }

        if (Input.GetKey("s"))
        {
            acceleration.z -= 1;
        }

        if (Input.GetKey("a"))
        {
            acceleration.x -= 1;
        }

        if (Input.GetKey("d"))
        {
            acceleration.x += 1;
        }
              

        return acceleration.normalized * AccelerationMod;
    }

    private float _rotationX;

    private void MouseRotation()
    {
        //mouse input
        float rotationHorizontal = sensitivity * Input.GetAxis("Mouse X");
        float rotationVertical = sensitivity * Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up * rotationHorizontal, Space.World);

        var rotationY = transform.localEulerAngles.y;

        _rotationX += rotationVertical;
        _rotationX = Mathf.Clamp(_rotationX, -MaxXAngle, MaxXAngle);

        transform.localEulerAngles = new Vector3(-_rotationX, rotationY, 0);
    }

    private void CalculateDeceleration(Vector3 acceleration)
    {
        if (Mathf.Approximately(Mathf.Abs(acceleration.x), 0))
        {
            if (Mathf.Abs(movementSpeed.x) < DecelerationMod)
            {
                movementSpeed.x = 0;
            }
            else
            {
                movementSpeed.x -= DecelerationMod * Mathf.Sign(movementSpeed.x);
            }
        }

        if (Mathf.Approximately(Mathf.Abs(acceleration.y), 0))
        {
            if (Mathf.Abs(movementSpeed.y) < DecelerationMod)
            {
                movementSpeed.y = 0;
            }
            else
            {
                movementSpeed.y -= DecelerationMod * Mathf.Sign(movementSpeed.y);
            }
        }

        if (Mathf.Approximately(Mathf.Abs(acceleration.z), 0))
        {
            if (Mathf.Abs(movementSpeed.z) < DecelerationMod)
            {
                movementSpeed.z = 0;
            }
            else
            {
                movementSpeed.z -= DecelerationMod * Mathf.Sign(movementSpeed.z);
            }
        }
    }
}