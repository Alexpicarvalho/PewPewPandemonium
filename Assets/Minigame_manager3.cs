using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_manager3 : MonoBehaviour
{

    public GameObject ui_start;
    public GameObject ui_gameover;
    public GameObject ui_Win;


    void Start()
    {
        ui_Win.SetActive(false);
        ui_gameover.SetActive(false);
        ui_start.SetActive(true);
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            ui_Win.SetActive(true);
        }
    }
}
