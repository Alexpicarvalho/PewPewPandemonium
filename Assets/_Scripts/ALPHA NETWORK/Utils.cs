using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils 
{
    public static Vector3 GetRandomSpawnPoint()
    {
        return new Vector3(Random.Range(-80, 80), 30, Random.Range(-80, 80));
    }
}
