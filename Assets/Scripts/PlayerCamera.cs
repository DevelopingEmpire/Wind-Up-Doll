using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float rotationSpeed = 5.0f;
    public GameObject target;
    private float mouseY;
    private float rotationY = 0f; // Added to store the accumulated vertical rotation
    public float minY = -60f; // Minimum vertical angle
    public float maxY = 80f; // Maximum vertical angle



    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        rotationY = angles.x;
    }

    // Update is called once per frame
    void Update()
    {
        mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Calculate new rotation, clamping in the process
        rotationY += mouseY;
        rotationY = Mathf.Clamp(rotationY, minY, maxY);

        // Apply the calculated and clamped rotation along the X axis for vertical tilt,
        // while keeping the current Y (horizontal) and Z (roll) angles the same.
        transform.position = target.transform.position; // Follow the target
        transform.rotation = Quaternion.Euler(-rotationY, target.transform.eulerAngles.y, 0);
    }
}