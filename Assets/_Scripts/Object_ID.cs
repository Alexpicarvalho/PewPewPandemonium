using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_ID : MonoBehaviour
{
    public int my_ID;

    public void CreateID(Object_ID _parentID) { my_ID = _parentID.my_ID; }
}
