using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CursoManager : MonoBehaviour
{

    public static CursoManager instance;
    private Texture2D defaultCursor;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of Cursor Manager Found. Destroying Copy!");
            Destroy(gameObject);
        }
        else instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //defaultCursor = PlayerSettings.defaultCursor;
    }
    public void ChangeCursor(Texture2D newCursor)
    {
        Cursor.SetCursor(newCursor, new Vector2(newCursor.width / 2, newCursor.height / 2), CursorMode.Auto);
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(defaultCursor, new Vector2(defaultCursor.width / 2, defaultCursor.height / 2),CursorMode.Auto);
    }
}
