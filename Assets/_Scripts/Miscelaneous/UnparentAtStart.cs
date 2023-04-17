using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnparentAtStart : MonoBehaviour
{
    private Quaternion _saveRotation;

    // Start is called before the first frame update
    void Start()
    {
        _saveRotation = transform.localRotation;
        transform.parent = null;
        transform.rotation = _saveRotation;
    }
}
