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
    public bool selected = false;

    private void Start() {
        _meshRenderer = GetComponent<MeshRenderer>();

        if (currentModeSelectButton == ModeSelectButton.Mark) {
            selected = true;
            _meshRenderer.material = _selectedMaterial;
        }
    }

    private void Update() {

        performActionFloat = SteamVR_Actions.picross.PerformActionFloat[SteamVR_Input_Sources.Any].axis;
        performAction = performActionFloat > 0.8f;

        if (_hovering && !selected) {
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
                    GameObject.FindGameObjectWithTag("Destroy").GetComponent<ChangeModeSelector>().selected = false;
                    GameObject.FindGameObjectWithTag("Build").GetComponent<ChangeModeSelector>().selected = false;
                    selected = true;
                    // Change GameMode
                    _voxelManager.CurrentGameMode = VoxelManager.GameMode.Mark;
                    _voxelManager.MakeMarkable();
                    _meshRenderer.material = _selectedMaterial;
                }
            }

            if (currentModeSelectButton == ModeSelectButton.Destroy) {
                if (_voxelManager.CurrentGameMode == VoxelManager.GameMode.Destroy) {
                    Debug.Log("Don't do anything!");
                } else {
                    GameObject.FindGameObjectWithTag("Mark").GetComponent<MeshRenderer>().material = _unselectedMaterial;
                    GameObject.FindGameObjectWithTag("Build").GetComponent<MeshRenderer>().material = _unselectedMaterial;
                    GameObject.FindGameObjectWithTag("Mark").GetComponent<ChangeModeSelector>().selected = false;
                    GameObject.FindGameObjectWithTag("Build").GetComponent<ChangeModeSelector>().selected = false;
                    selected = true;
                    _voxelManager.CurrentGameMode = VoxelManager.GameMode.Destroy;
                    _voxelManager.MakeDestroyable();
                    _meshRenderer.material = _selectedMaterial;
                }
            }

            if (currentModeSelectButton == ModeSelectButton.Build) {
                if (_voxelManager.CurrentGameMode == VoxelManager.GameMode.Build) {
                    Debug.Log("Don't do anything!");
                } else {
                    GameObject.FindGameObjectWithTag("Destroy").GetComponent<MeshRenderer>().material = _unselectedMaterial;
                    GameObject.FindGameObjectWithTag("Mark").GetComponent<MeshRenderer>().material = _unselectedMaterial;
                    GameObject.FindGameObjectWithTag("Destroy").GetComponent<ChangeModeSelector>().selected = false;
                    GameObject.FindGameObjectWithTag("Mark").GetComponent<ChangeModeSelector>().selected = false;
                    selected = true;
                    _voxelManager.CurrentGameMode = VoxelManager.GameMode.Build;
                    _voxelManager.MakeBuildable();
                    _meshRenderer.material = _selectedMaterial;
                }
            }
        }
    }

    public void ChangeModeVisualFromVoxelManager() {
        if (_voxelManagerGameObject == null) {
            _voxelManagerGameObject = GameObject.FindGameObjectWithTag("VoxelManager");
        }

        VoxelManager _voxelManager = _voxelManagerGameObject.GetComponent<VoxelManager>();

        if (_voxelManager.CurrentGameMode == VoxelManager.GameMode.Mark) {
            GameObject.FindGameObjectWithTag("Mark").GetComponent<MeshRenderer>().material = _unselectedMaterial;
            GameObject.FindGameObjectWithTag("Mark").GetComponent<ChangeModeSelector>().selected = false;
            GameObject.FindGameObjectWithTag("Destroy").GetComponent<MeshRenderer>().material = _selectedMaterial;
            GameObject.FindGameObjectWithTag("Destroy").GetComponent<ChangeModeSelector>().selected = true;
        } else if (_voxelManager.CurrentGameMode == VoxelManager.GameMode.Destroy) {
            GameObject.FindGameObjectWithTag("Destroy").GetComponent<MeshRenderer>().material = _unselectedMaterial;
            GameObject.FindGameObjectWithTag("Destroy").GetComponent<ChangeModeSelector>().selected = false;
            GameObject.FindGameObjectWithTag("Mark").GetComponent<MeshRenderer>().material = _selectedMaterial;
            GameObject.FindGameObjectWithTag("Mark").GetComponent<ChangeModeSelector>().selected = true;
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
            if (selected) {
                _meshRenderer.material = _selectedMaterial;
            } else {
                _meshRenderer.material = _unselectedMaterial;
            }
        }
    }

    private void OnMouseOver() {
        _hovering = true;
        _meshRenderer.material = _hoverMaterial;
    }

    private void OnMouseExit() {
        _hovering = false;
        if (selected) {
            _meshRenderer.material = _selectedMaterial;
        } else {
            _meshRenderer.material = _unselectedMaterial;
        }
    }
}
