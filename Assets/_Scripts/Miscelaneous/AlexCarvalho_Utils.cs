using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AlexCarvalho_Utils
{
    public static class My_Utils
    {
        /// <summary>
        /// Raycasts in a cone and returns a List of hit transforms (no duplicates)
        /// </summary>
        /// <param name="num1">The first number to add.</param>
        /// <param name="num2">The second number to add.</param>
        /// <returns>The sum of num1 and num2.</returns>

        public static List<Transform> ConeRaycast(float coneAngle,Vector3 pointOfOrigin, Vector3 startDirection
            ,float maxRange = Mathf.Infinity, int numberOfRaycasts = 20, LayerMask targetLayer = default, int maxReturnSize = int.MaxValue)
        {
            Debug.Log("CONE RAYCAST");
            List<Transform> hitsList = new List<Transform>();

            for (int i = 0; i < numberOfRaycasts / 2; i++)
            {
                Quaternion rotation = Quaternion.AngleAxis((coneAngle / numberOfRaycasts) * i, Vector3.up);
                Vector3 rayCastDir = (rotation * startDirection).normalized;
                Ray ray = new Ray(pointOfOrigin, rayCastDir);

                if (Physics.Raycast(ray, out RaycastHit hit, maxRange/*, targetLayer*/))
                {
                    if(!hitsList.Contains(hit.collider.transform)) hitsList.Add(hit.collider.transform);
                    Debug.Log("Hit Something");
                }
            }

            for (int i = 0; i < numberOfRaycasts / 2; i++)
            {
                Quaternion rotation = Quaternion.AngleAxis((coneAngle / numberOfRaycasts) * i, Vector3.up);
                Vector3 rayCastDir = (Quaternion.Inverse(rotation) * startDirection).normalized;
                Ray ray = new Ray(pointOfOrigin, rayCastDir);

                if (Physics.Raycast(ray, out RaycastHit hit, maxRange/*, targetLayer*/))
                {
                    if(!hitsList.Contains(hit.collider.transform)) hitsList.Add(hit.collider.transform);
                    Debug.Log("Hit Something");
                }
            }

            return hitsList;
        }

    }
    

}

