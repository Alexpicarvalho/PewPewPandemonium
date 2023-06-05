using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutoScroll : MonoBehaviour
{
    float speed = 100.0f;
    float textPosBegin;
    float textEnd = 1396.0f;

    RectTransform myGoRectTransform;
    [SerializeField]
    TextMeshProUGUI mainText;

    [SerializeField]
    bool isLooping = false;




    void Start()
    {
        textPosBegin = -611.0f;
        myGoRectTransform = gameObject.GetComponent<RectTransform>();
        StartCoroutine(AutoScrollText());
    }

    
   IEnumerator AutoScrollText()
    {
        while (myGoRectTransform.localPosition.y < textEnd)
        {
            myGoRectTransform.Translate(Vector3.up * speed * Time.deltaTime);
            if(myGoRectTransform.localPosition.y > textEnd)
            {
                if (isLooping)
                {
                    myGoRectTransform.localPosition = Vector3.up * textPosBegin;
                }
                else
                {
                    break;
                }
            }    
            
            yield return null;
        }
    }
}
