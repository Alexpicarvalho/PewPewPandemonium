using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCircle : MonoBehaviour
{
	[SerializeField] GameObject zoneWall;
	[SerializeField] float xRadius;
	[SerializeField] float speed;
    [SerializeField] float timeToStart;
    [SerializeField] float timeToEnd;
    [SerializeField] float mul;

    bool canUpdateCircle;
	float next;

    private void Start()
    {
        EndCircle();
    }

    private void Update()
    {
        zoneWall.transform.localScale =
                Vector3.Lerp(zoneWall.transform.localScale, new Vector3(xRadius * mul, 1, xRadius * mul), 10 * Time.deltaTime);

        if (canUpdateCircle)
        {   
            xRadius = Mathf.Lerp(xRadius, next, speed * Time.deltaTime);
            xRadius = Mathf.Clamp(xRadius, 0, 500);
        }
    }

    void StartCircle()
    {
        Invoke("EndCircle", timeToEnd);
        next = xRadius / 2;
        canUpdateCircle = true;
    }

    void EndCircle()
    {
        canUpdateCircle = false;
        Invoke("StartCircle", timeToStart);
        next = xRadius / 2;
    }

}
