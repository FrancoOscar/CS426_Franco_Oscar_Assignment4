using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera mainCamera;

    void Start()
    {
        // Find the camera within the "Player" prefab
        GameObject playerPrefab = GameObject.Find("Player");
        
        if (playerPrefab != null)
        {
            mainCamera = playerPrefab.GetComponentInChildren<Camera>();
            
            if (mainCamera == null)
            {
                Debug.LogError("Camera not found in the 'Player' prefab.");
            }
        }
        else
        {
            Debug.LogError("Player prefab not found in the scene.");
        }
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            // Face the camera
            transform.LookAt(mainCamera.transform);

            // Optionally, you can remove the following line if it's causing issues
            // transform.Rotate(0, 180, 0);
        }
    }
}
