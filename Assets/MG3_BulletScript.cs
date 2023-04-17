using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG3_BulletScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }

}
