using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXHandler : MonoBehaviour
{
    [SerializeField] float _destroyAfterTime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,_destroyAfterTime);
    }
}
