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
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float xMov = Input.GetAxis("Horizontal");
        float zMov = Input.GetAxis("Vertical");
        Vector3 mousePos = MousePosition();

        Vector3 moveDirection = new Vector3(xMov, 0f,zMov);
        Vector3.Normalize(moveDirection);

        anim.SetFloat("xMov", xMov);
        anim.SetFloat("zMov", zMov);
        

        if (moveDirection.magnitude != 0.0f)
        {
            movementDirectionIndicator.GetComponent<Animator>().SetBool("On", true);
            movementDirectionIndicator.transform.rotation = Quaternion.LookRotation(moveDirection);
            transform.position += moveDirection * speed * Time.deltaTime;
            //anim.SetBool("Moving", true);
            CalculateAnimatorMovementValue(moveDirection);
        }
        else
        {
            movementDirectionIndicator.GetComponent<Animator>().SetBool("On", false);
            //anim.SetBool("Moving", false);
            float angle = Mathf.SmoothDamp(anim.GetFloat("MovBlend"), 1000,ref vel, 1f);
            anim.SetFloat("MovBlend", 1000);

        }
        transform.rotation = Quaternion.LookRotation(mousePos);
        

        //shoot

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

    }

    private Vector3 MousePosition()
    {
        Camera camera = Camera.main;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Plane plane = new Plane(Vector3.up, transform.position);

        if (Physics.Raycast(ray,out hit))
        {
            mouseTracker.transform.position = hit.point;
            return new Vector3(hit.point.x - transform.position.x,0,hit.point.z - transform.position.z);
        }
        else return Vector3.zero;
    }

    private void CalculateAnimatorMovementValue(Vector3 moveDir)
    {
        float value = /*Mathf.Sin(*/Vector3.Angle(transform.forward, moveDir)/*)*/;
        if (moveDir.x > 0) value += 180;
        anim.SetFloat("MovBlend", value);
    }

    public void Shoot()
    {
        var mf = Instantiate(muzzleFlash, firePoint.position, Quaternion.LookRotation(transform.forward));
        var bt = Instantiate(bullet, firePoint.position, Quaternion.LookRotation(MousePosition()));

        Destroy(mf, 2);
        Destroy(bt, 4);
    }
}


