using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CircleController : NetworkBehaviour
{
    public float startSize = 10f; // the initial size of the circle
    public float endSize = 2f; // the final size of the circle
    public float timeToShrink = 60f; // the time it takes for the circle to shrink from startSize to endSize
    public float mapRadius = 50f; // the maximum distance from the center of the map to spawn the circle

    private float currentTime = 0f;
    private float currentSize = 0f;
    private bool isShrinking = false;

    // Start is called before the first frame update
    void Start()
    {
        currentSize = startSize;
        transform.localScale = new Vector3(currentSize, currentSize, 1f);

        if (Runner.IsServer)
        {
            // Randomly position the circle at the start of the game
            Vector2 randomPos = Random.insideUnitCircle.normalized * mapRadius;
            transform.position = new Vector3(randomPos.x, randomPos.y, 0f);

            // Sync the circle position and size with the clients
            //photonView.RPC("SyncPositionAndSize", RpcTarget.Others, transform.position, transform.localScale);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isShrinking)
        {
            if (currentTime < timeToShrink)
            {
                currentTime += Time.deltaTime;
                currentSize = Mathf.Lerp(startSize, endSize, currentTime / timeToShrink);
                transform.localScale = new Vector3(currentSize, currentSize, 1f);
            }
            else
            {
                if (Runner.IsServer)
                {
                    // Randomly move the circle to a new position
                    Vector2 randomPos = Random.insideUnitCircle.normalized * mapRadius;
                    transform.position = new Vector3(randomPos.x, randomPos.y, 0f);

                    // Sync the circle position and size with the clients
                    //photonView.RPC("SyncPositionAndSize", RpcTarget.Others, transform.position, transform.localScale);
                }

                // Reset the shrinking animation
                currentTime = 0f;
                currentSize = startSize;
                transform.localScale = new Vector3(currentSize, currentSize, 1f);
            }
        }
    }

    public void StartShrinking()
    {
        isShrinking = true;

        // Notify the clients to start the shrinking animation
        //photonView.RPC("StartShrinkingRPC", RpcTarget.Others);
    }

    public void StopShrinking()
    {
        isShrinking = false;

        // Notify the clients to stop the shrinking animation
        //photonView.RPC("StopShrinkingRPC", RpcTarget.Others);
    }

    //[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    //void SyncPositionAndSize(Vector3 position, Vector3 scale)
    //{
    //    transform.position = position;
    //    transform.localScale = scale;
    //}

    //[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    //void StartShrinkingRPC()
    //{
    //    isShrinking = true;
    //}

    //[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    //void StopShrinkingRPC()
    //{
    //    isShrinking = false;
    //}
}

