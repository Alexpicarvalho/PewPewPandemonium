using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_CamFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    Vector3 offset;


    void Start()
    {
        offset = transform.position - player.position;   
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + offset;
    }
}
