using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AlexCarvalho_Utils
{
    /// <summary>
    /// This class is part of AlexCarvalho_Utils, made by Alexandre Carvalho
    /// </summary>
    public static class My_Utils
    {
        #region Summary
        /// <summary>
        /// Raycasts in a cone and returns a List of hit transforms (no duplicates)
        /// <para><paramref name="coneAngle"/> Angle of the raycast cone </para>
        /// <para><paramref name="maxRange"/> Max range of each idividual raycast</para>
        /// <para><paramref name="maxReturnSize"/> Limit size of the return list</para>
        /// <para><paramref name="numberOfRaycasts"/> The amount of check raycasts to be made, increasing makes the check more accurate, but impacts performance</para>
        /// <para><paramref name="targetLayer"/> What layers are targeted by the raycasts, defaulted to everything</para>
        /// </summary>
        #endregion
        public static List<Transform> ConeRaycast(float coneAngle, Vector3 pointOfOrigin, Vector3 startDirection
            , float maxRange = Mathf.Infinity, int numberOfRaycasts = 20, LayerMask targetLayer = default, int maxReturnSize = int.MaxValue)
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
                    if (!hitsList.Contains(hit.collider.transform)) hitsList.Add(hit.collider.transform);
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
                    if (!hitsList.Contains(hit.collider.transform)) hitsList.Add(hit.collider.transform);
                    Debug.Log("Hit Something");
                }
            }

            return hitsList;
        }

        public static float SnapToGroundGetY(Vector3 raycastPos, LayerMask whatIsGround = default)
        {
            if (whatIsGround == default) whatIsGround = ~0;
            if (Physics.Raycast(raycastPos, Vector3.down, out RaycastHit hit, Mathf.Infinity, whatIsGround))
            {
                Debug.Log("Alex Utils Hit" + hit.collider.name);
                return hit.point.y;
            }
            else
            {
                Debug.Log("Alex Utils Didn't hit ground");
                return raycastPos.y;
            }
        }

        /// <summary>
        /// Returns the given position, with the Y value corresponding to the ground level (ground is layermask, defaulted to everything)
        /// </summary>
        public static Vector3 SnapToGroundGetPosition(Vector3 objectPosition, LayerMask whatIsGround = default)
        {
            return new Vector3(objectPosition.x, SnapToGroundGetY(objectPosition, whatIsGround), objectPosition.z);
        }

        public static Vector3 GetMousePositionNoY(Vector3 objectPosition,LayerMask whatIsGround = default)
        {
            Camera camera = Camera.main;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, whatIsGround))
            {
                return new Vector3(hit.point.x - objectPosition.x, 0, hit.point.z - objectPosition.z);

            }
            else return Vector3.zero;
        }

        public static bool CheckLineOfSight(Transform _this, Transform _checkTarget, LayerMask _blocksLOS = default)
        {
            if (_blocksLOS == default) _blocksLOS = ~0;
            Vector3 raycastDirection = (_checkTarget.position - _this.position).normalized;

            if(Physics.Raycast(_this.position + Vector3.up, raycastDirection,Mathf.Infinity,_blocksLOS)) return false;
            else return true;
        }
    }


}

