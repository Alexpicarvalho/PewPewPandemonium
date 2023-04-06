using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Coin : MonoBehaviour
{
    [SerializeField] float turnSpeed = 90f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Obstacle>() != null)
        {
            Destroy(gameObject);
            return;
        }

        //Check if object collided is player
        if (other.gameObject.name != "Player") return;


        Minigame_manager2.inst.IncrementScore();


        //destroy coin 
        Destroy(gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
    }
}
