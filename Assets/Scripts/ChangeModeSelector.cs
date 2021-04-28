using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ChangeModeSelector : MonoBehaviour
{

    public enum ModeSelectButton {
        Mark,
        Destroy,
        Build
    };

    public ModeSelectButton currentModeSelectButton;

    private GameObject _voxelManagerGameObject;

    private void OnTriggerEnter(Collider other) {


        if (other.gameObject.CompareTag("Interactor")) {
            if (_voxelManagerGameObject == null) {
                _voxelManagerGameObject = GameObject.FindGameObjectWithTag("VoxelManager");
            }

            VoxelManager _voxelManager = _voxelManagerGameObject.GetComponent<VoxelManager>();

            if (currentModeSelectButton == ModeSelectButton.Mark) {
                if (_voxelManager.CurrentGameMode == VoxelManager.GameMode.Mark) {
                    Debug.Log("Don't do anything!");
                } else {
                    _voxelManager.CurrentGameMode = VoxelManager.GameMode.Mark;
                }
            }

            if (currentModeSelectButton == ModeSelectButton.Destroy) {
                if (_voxelManager.CurrentGameMode == VoxelManager.GameMode.Destroy) {
                    Debug.Log("Don't do anything!");
                } else {
                    _voxelManager.CurrentGameMode = VoxelManager.GameMode.Destroy;
                }
            }

            if (currentModeSelectButton == ModeSelectButton.Build) {
                if (_voxelManager.CurrentGameMode == VoxelManager.GameMode.Build) {
                    Debug.Log("Don't do anything!");
                } else {
                    _voxelManager.CurrentGameMode = VoxelManager.GameMode.Build;
                }
            }
        }

    }
}
