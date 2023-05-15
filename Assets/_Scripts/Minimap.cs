using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Minimap : MonoBehaviour
{
    [SerializeField] public Transform target;
    // Start is called before the first frame update
    private void Awake()
    {
       // target = transform.parent.parent;
        //if (!target.GetComponent<NetworkObject>().HasInputAuthority && target.GetComponent<NetworkObject>().HasStateAuthority) Destroy(transform.parent);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 pos = target.position;
        pos.y = transform.position.y;
        transform.position = pos;
    }
}
