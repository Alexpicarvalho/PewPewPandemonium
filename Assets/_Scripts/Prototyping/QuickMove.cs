using CustomClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickMove : MonoBehaviour
{
    public float speed;
    public GameObject movementDirectionIndicator;
    private Animator anim;
    public GameObject bullet;
    public GameObject muzzleFlash;
    public Transform firePoint;
    public GameObject mouseTracker;
    private float vel = 0;
    private ITime iTime;

    bool _sleepAnimFloat = false;
    bool _canMove = true;
    Rigidbody rb;
    [SerializeField] private LayerMask layerMask;

    [Header("Dodge Values")]
    [SerializeField] private float dodgeSpeed;
    [SerializeField] private float dodgeDuration;
    [SerializeField] private AnimationCurve speedCurve;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        iTime = GetComponent<ITime>();
    }

    // Update is called once per frame
    void Update()
    {
        float xMov = Input.GetAxis("Horizontal");
        float zMov = Input.GetAxis("Vertical");
        Vector3 mousePos = MousePosition();

        Vector3 moveDirection = new Vector3(xMov, 0f, zMov).normalized;


        //Animation

        float zVelocity = Vector3.Dot(moveDirection, transform.forward);
        float xVelocity = Vector3.Dot(moveDirection, transform.right);


        if (!_sleepAnimFloat)
        {
            anim.SetFloat("zMov", zVelocity, 0.1f, Time.deltaTime);
            anim.SetFloat("xMov", xVelocity, 0.1f, Time.deltaTime);
        }



        if (moveDirection.magnitude != 0.0f && _canMove)
        {
            movementDirectionIndicator.GetComponent<Animator>().SetBool("On", true);
            movementDirectionIndicator.transform.rotation = Quaternion.LookRotation(moveDirection);
            transform.position += moveDirection * speed * iTime.personalTimeScale * Time.deltaTime;
            //anim.SetBool("Moving", true);

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                anim.Play("Rollin Blend Tree");
                Dodge();
            }
            CalculateAnimatorMovementValue(moveDirection);
        }
        else
        {
            movementDirectionIndicator.GetComponent<Animator>().SetBool("On", false);
            //anim.SetBool("Moving", false);
            float angle = Mathf.SmoothDamp(anim.GetFloat("MovBlend"), 1000, ref vel, 1f);
            anim.SetFloat("MovBlend", 1000);

        }
        if (_canMove) transform.rotation = Quaternion.LookRotation(mousePos);

    }

    public void Dodge()
    {
        StartCoroutine(DodgeMove());
    }
    IEnumerator DodgeMove()
    {
        _sleepAnimFloat = true;
        _canMove = false;
        float startTime = Time.time;
        while (Time.time < startTime + dodgeDuration)
        {
            transform.position += movementDirectionIndicator.transform.forward * dodgeSpeed * iTime.personalTimeScale
                * (speedCurve.Evaluate(Time.time - startTime) / dodgeDuration)
                * Time.deltaTime;

            yield return null;
        }
        _sleepAnimFloat = false;
        _canMove = true;
    }

    private Vector3 MousePosition()
    {
        Camera camera = Camera.main;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            mouseTracker.transform.position = hit.point;
            return new Vector3(hit.point.x - transform.position.x, 0, hit.point.z - transform.position.z);

        }
        else return Vector3.zero;
    }

    private void CalculateAnimatorMovementValue(Vector3 moveDir)
    {
        float value = /*Mathf.Sin(*/Vector3.Angle(transform.forward, moveDir)/*)*/;
        if (moveDir.x > 0) value += 180;
        anim.SetFloat("MovBlend", value);
    }
}


