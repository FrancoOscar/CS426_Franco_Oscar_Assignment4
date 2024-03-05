using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DestroyDoor : NetworkBehaviour
{
    public GameObject doorToDestroy;

    // This method is called whenever a collision is detected
    private void OnCollisionEnter(Collision collision)
    {
        // Printing if collision is detected on the console
        Debug.Log("Collision Detected");

        // If the collision is detected, destroy the target and the specified door
        if (IsServer)
        {
            DestroyDoorServerRpc();
        }
        else
        {
            // Inform the server to destroy the door
            DestroyDoorClientRpc();
        }
    }

    // ServerRpc to destroy the target and the specified door
    [ServerRpc(RequireOwnership = false)]
    public void DestroyDoorServerRpc()
    {
        // Despawn the target
        GetComponent<NetworkObject>().Despawn(true);
        // Destroy the target game object
        Destroy(gameObject);

        // Check if a door is specified
        if (doorToDestroy != null)
        {
            // Destroy the specified door object
            Destroy(doorToDestroy);
        }
        else
        {
            Debug.LogWarning("Door to destroy is not assigned in the DestroyDoor script.");
        }

        // Inform all clients to destroy the door
        DestroyDoorClientRpc();
    }

    // ClientRpc to destroy the door on all clients
    [ClientRpc]
    public void DestroyDoorClientRpc()
    {
        // Check if a door is specified
        if (doorToDestroy != null)
        {
            // Destroy the specified door object on all clients
            Destroy(doorToDestroy);
        }
        else
        {
            Debug.LogWarning("Door to destroy is not assigned in the DestroyDoor script.");
        }
    }
}
