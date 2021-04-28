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

    private bool performAction;
    private float performActionFloat;
    private MeshRenderer _meshRenderer;
    public ModeSelectButton currentModeSelectButton;
    private GameObject _voxelManagerGameObject;
    private bool _hovering = false;
    [SerializeField] private Material _unselectedMaterial;
    [SerializeField] private Material _selectedMaterial;
    [SerializeField] private Material _hoverMaterial;
    private bool _selected = false;

    private void Start() {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update() {

        performActionFloat = SteamVR_Actions.picross.PerformActionFloat[SteamVR_Input_Sources.Any].axis;
        performAction = performActionFloat > 0.8f;

        if (_hovering && performAction) {
            if (_voxelManagerGameObject == null) {
                _voxelManagerGameObject = GameObject.FindGameObjectWithTag("VoxelManager");
            }

            VoxelManager _voxelManager = _voxelManagerGameObject.GetComponent<VoxelManager>();

            if (currentModeSelectButton == ModeSelectButton.Mark) {
                if (_voxelManager.CurrentGameMode == VoxelManager.GameMode.Mark) {
                    Debug.Log("Don't do anything!");
                } else {
                    GameObject.FindGameObjectWithTag("Destroy").GetComponent<MeshRenderer>().material = _unselectedMaterial;
                    GameObject.FindGameObjectWithTag("Build").GetComponent<MeshRenderer>().material = _unselectedMaterial;
                    _voxelManager.CurrentGameMode = VoxelManager.GameMode.Mark;
                    _meshRenderer.material = _selectedMaterial;
                }
            }

            if (currentModeSelectButton == ModeSelectButton.Destroy) {
                if (_voxelManager.CurrentGameMode == VoxelManager.GameMode.Destroy) {
                    Debug.Log("Don't do anything!");
                } else {
                    GameObject.FindGameObjectWithTag("Mark").GetComponent<MeshRenderer>().material = _unselectedMaterial;
                    GameObject.FindGameObjectWithTag("Build").GetComponent<MeshRenderer>().material = _unselectedMaterial;
                    _voxelManager.CurrentGameMode = VoxelManager.GameMode.Destroy;
                    _meshRenderer.material = _selectedMaterial;
                }
            }

            if (currentModeSelectButton == ModeSelectButton.Build) {
                if (_voxelManager.CurrentGameMode == VoxelManager.GameMode.Build) {
                    Debug.Log("Don't do anything!");
                } else {
                    GameObject.FindGameObjectWithTag("Destroy").GetComponent<MeshRenderer>().material = _unselectedMaterial;
                    GameObject.FindGameObjectWithTag("Build").GetComponent<MeshRenderer>().material = _unselectedMaterial;
                    _voxelManager.CurrentGameMode = VoxelManager.GameMode.Build;
                    _meshRenderer.material = _selectedMaterial;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Interactor")) {
            _hovering = true;
            _meshRenderer.material = _hoverMaterial;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Interactor")) {
            _hovering = false;
        }
    }
}
