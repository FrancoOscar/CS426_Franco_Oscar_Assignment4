using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour {
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float rotationSpeed = 90;
    [SerializeField]
    private float jumpForce = 2.5f;
    [SerializeField]
    private float jumpCool = 1f;
    [SerializeField]
    private float shootCool = 1f;

    [SerializeField]
    private List<Color> colors = new List<Color>();
    [SerializeField]
    private GameObject cannon;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private AudioListener audioListener;
    [SerializeField]
    private Camera playerCamera;

    private float rotationInput;
    private float jumpTime;
    private float shootTime;
    private Vector3 moveDirection;

    void Start() {
        moveDirection = Vector3.zero;
        rotationInput = 0f;
    }

    void Update() {
        if (!IsOwner) return;

        if (Input.GetKey(KeyCode.W))
            moveDirection = transform.forward;
        else if (Input.GetKey(KeyCode.S))
            moveDirection = -transform.forward;
        else
            moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
            rotationInput = -1f;
        else if (Input.GetKey(KeyCode.D))
            rotationInput = 1f;
        else
            rotationInput = 0f;

        if (moveDirection != Vector3.zero) // Move the player relative to its forward direction
            transform.position += moveDirection * speed * Time.deltaTime;

        // Rotate the player relative to its current forward direction
        transform.Rotate(Vector3.up, rotationInput * rotationSpeed * Time.deltaTime, Space.Self);

        if (jumpTime <= 0 && Input.GetKeyDown(KeyCode.Space)) {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpTime = jumpCool;
        }

        if (shootTime <= 0 && Input.GetButtonDown("Fire1")) {
            BulletSpawningServerRpc(cannon.transform.position, cannon.transform.rotation);
            shootTime = shootCool;
        }

        if (jumpTime > 0) jumpTime -= Time.deltaTime;
        if (shootTime > 0) shootTime -= Time.deltaTime;
    }

    public override void OnNetworkSpawn() {
        GetComponent<MeshRenderer>().material.color = colors[(int)OwnerClientId];

        if (!IsOwner) return;

        audioListener.enabled = true;
        playerCamera.enabled = true;
    }

    [ServerRpc]
    private void BulletSpawningServerRpc(Vector3 position, Quaternion rotation) {
        BulletSpawningClientRpc(position, rotation);
    }

    [ClientRpc]
    private void BulletSpawningClientRpc(Vector3 position, Quaternion rotation) {
        GameObject newBullet = Instantiate(bullet, position, rotation);
        newBullet.GetComponent<Rigidbody>().velocity += Vector3.up * 2;
        newBullet.GetComponent<Rigidbody>().AddForce(newBullet.transform.forward * 1500);
    }
}