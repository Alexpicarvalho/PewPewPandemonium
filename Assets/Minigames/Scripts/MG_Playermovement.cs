using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Playermovement : MonoBehaviour
{
   public float speed = 1f;
   public Rigidbody rb;
   private Vector3 moveDirection = Vector3.zero;

    public float sideSpeed = 5f;


    void Update()
    {
        // move the player front
        moveDirection = transform.forward * speed * Time.deltaTime;

        // move the player to the sides
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            moveDirection += transform.right * -sideSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            moveDirection += transform.right * sideSpeed * Time.deltaTime;
        }
        
        rb.MovePosition(rb.position + moveDirection);
    }
}
