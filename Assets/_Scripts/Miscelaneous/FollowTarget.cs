using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform _followTarget;
    [SerializeField] bool _targetIsChild = false;
    [SerializeField] bool _targetIsSibling = false;
    [SerializeField] int _targetChildIndex;
    [SerializeField] int _targetSiblingIndex;
    [SerializeField] private Vector3 _startOffset;
    [SerializeField] bool _autoOffset = true;
    // Start is called before the first frame update
    void Start()
    {
        if(_autoOffset) _startOffset = transform.position;

        if(_targetIsChild) _followTarget = transform.GetChild(_targetChildIndex);
        else if (_targetIsSibling) _followTarget = transform.parent.GetChild(_targetSiblingIndex);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _startOffset + _followTarget.localPosition;
    }
}
