using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Perk/Prowler", fileName = "ProwlerPerk")]
public class ProwlerPerk : Perk
{
    [SerializeField] LayerMask _prowlerTrackLayer;
    [SerializeField] LayerMask _normalTrackLayer;
    [SerializeField] float _speedOnTracksLvl1;
    [SerializeField] float _speedOnTracksLvl2;
    public override void UpdateEffects(GameObject player)
    {
        if (_reachedMaxLevel) return;
        base.UpdateEffects(player);

        switch (_points)
        {
            case 1:
                ActivateTrackerPrints(player);
                break;

            case 2:
                AddTrackSpeed1(player);
                break;

            case 3:
                AddTrackSpeed2(player);
                break;

            default:
                break;
        }
    }

    private void AddTrackSpeed2(GameObject player)
    {
        Debug.Log("Have speed 1");
        var moveScript = player.GetComponent<PlayerMovement>();
        if (moveScript != null) moveScript._trackSpeed = _speedOnTracksLvl2;
        Debug.Log("Am Speedy boy");
    }

    private void AddTrackSpeed1(GameObject player)
    {
        Debug.Log("Have speed 2");
        var moveScript = player.GetComponent<PlayerMovement>();
        if (moveScript != null) moveScript._trackSpeed = _speedOnTracksLvl1;
        Debug.Log("Am Speedy boy");
    }

    private void ActivateTrackerPrints(GameObject player)
    {
        if (_cameraControler != null) _cameraControler.AddRemoveRenderLayers(_prowlerTrackLayer, _normalTrackLayer);
    }
}
