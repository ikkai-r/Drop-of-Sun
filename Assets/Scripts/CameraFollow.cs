using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;  // Reference to the player
    [SerializeField] private float smoothSpeed = 5f; // Camera follow smoothness
    [SerializeField] private float yOffset = 1f; // Offset to keep the player centered
    [SerializeField] private float minY = 0f;

    private void LateUpdate()
    {
        if (player == null) return;

        // Target position: Keep X and Z fixed, only follow Y
        float targetY = player.position.y + yOffset;

        // Apply the camera limit (don't go below minY)
        targetY = Mathf.Max(targetY, minY);

        // Smoothly transition to the target position
        Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
