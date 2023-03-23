using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniGame_manager : MonoBehaviour
{

    bool running = false;

    bool game_started = false;
    public GameObject player;
    public GameObject camera;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (running){
            player.transform.position -= new Vector3(0.5f * Time.deltaTime, 0, 0);
            camera.transform.position -= new Vector3(0.5f * Time.deltaTime, 0, 0 );
        }
        if (Input.GetKeyDown(KeyCode.Space) && !game_started) {
            running = true;
            game_started = true;
            StartCoroutine(Sing());
        }



    }



    IEnumerator Sing()
    {
        //audio play
        yield return new WaitForSeconds(4.5f);
        //animator -> look
        yield return new WaitForSeconds(2);
        //check player move
        if (running){
            Debug.Log("shoot");
        }

        yield return new WaitForSeconds(2);
        //idle
        yield return new WaitForSeconds(1);
        //audio stop
        StartCoroutine(Sing());


    }
}
