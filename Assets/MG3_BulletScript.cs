using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG3_BulletScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }

    }

}
