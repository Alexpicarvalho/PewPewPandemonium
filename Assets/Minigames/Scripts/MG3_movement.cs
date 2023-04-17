using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG3_movement : MonoBehaviour
{
    public float speed = 5f; // The movement speed of the player
    public float cameraDistance = 5f; // The distance of the camera from the player
    public float cameraHeight = 1.5f; // The height of the camera above the player

    private Transform cameraTransform; // The transform of the camera

    void Start()
    {
        // Get the transform of the main camera
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Move left if A key is pressed
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        // Move right if D key is pressed
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        // Move forward if W key is pressed
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        // Move backward if S key is pressed
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }

        // Position camera above and behind player
        Vector3 cameraPosition = transform.position - transform.forward * cameraDistance + Vector3.up * cameraHeight;
        cameraTransform.position = cameraPosition;
        cameraTransform.LookAt(transform.position);
    }
}
