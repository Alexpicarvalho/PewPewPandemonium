//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;
//using System;

//public class MenuKeyBindSelect : MonoBehaviour
//{

//    public TextMeshProUGUI buttonLabel;


//    // Start is called before the first frame update
//    public void Start()
//    {
//      //  buttonLabel.text = PlayerPrefs.GetString("CustomKey");
//    }

//    // Update is called once per frame
//    public void Update()
//    {
//        if (buttonLabel.text == "Press key to bind")
//        {
//            foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
//            {
//                buttonLabel.text = keycode.ToString();
//                PlayerPrefs.SetString("CustomKey", keycode.ToString());
//                PlayerPrefs.Save();
//            }
//        }
//    }

//    public void GetNewKeyBind()
//    {
//        buttonLabel.text = "Press key to bind";

//    }


//}
