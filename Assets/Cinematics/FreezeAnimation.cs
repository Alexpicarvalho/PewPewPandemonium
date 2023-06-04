using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FreezeAnimation : MonoBehaviour
{
    private Animator anim;
    public float _freezeTime;
    public string _animationName;
    bool _freeze = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.Play(_animationName, -1, _freezeTime);
        
    }
}
