using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCostumization : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer[] bodyParts;
    [SerializeField] SkinnedMeshRenderer[] headParts;

    private Mesh[] bodySkins = new Mesh[20];
    private Mesh[] headSkins = new Mesh[20];
    private int currentBodyIndex = 0;
    private int currentHeadIndex = 0;
    private SkinnedMeshRenderer currentBody;
    private SkinnedMeshRenderer currentHead;
    // Start is called before the first frame update
    void Start()
    {
        currentBody = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        currentHead = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        GetSkins();
    }

    private void GetSkins()
    {
        int i = 0;
        int j = 0;
        foreach (var item in bodyParts)
        {
            bodySkins[i] = bodyParts[i].sharedMesh;
            i++;
        }
        foreach (var item in headParts)
        {
            headSkins[j] = headParts[j].sharedMesh;
            j++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwapBodyPiece(true);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwapBodyPiece(false);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SwapHeadPiece(true);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SwapHeadPiece(false);
        }
    }

    void SwapBodyPiece(bool next)
    {
        if (next && currentHeadIndex < bodySkins.Length)
        {
            currentBodyIndex++;
            currentBody.sharedMesh = bodySkins[currentBodyIndex];
        }
        else if(currentHeadIndex > 0)
        {
            currentBodyIndex--;
            currentBody.sharedMesh = bodySkins[currentBodyIndex];
        }
    }

    void SwapHeadPiece(bool next)
    {
        if (next && currentHeadIndex < headSkins.Length)
        {
            currentHeadIndex++;
            currentHead.sharedMesh = headSkins[currentHeadIndex];
        }
        else if(currentHeadIndex > 0)
        {
            currentHeadIndex--;
            currentHead.sharedMesh = headSkins[currentHeadIndex];
        }
    }

}
