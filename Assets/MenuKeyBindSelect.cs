using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuKeyBindSelect : MonoBehaviour
{

    private PlayerInputHandler playerInputs;


    // Start is called before the first frame update
    void Start()
    {
        playerInputs = FindObjectOfType<PlayerInputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetNewKeyBind(int bindId)
    {
        //   KeyCode newBind = Input.get;
       KeyCode OnGUI(){


            return KeyCode.E;
       }

    }

    //private void OnGUI()
    //{
    //    Event keyPress = Event.current;
    //}

}
