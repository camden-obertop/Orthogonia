using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private GameObject overworldVFX;
    [SerializeField] private GameObject picrossVFX;

    public void SwitchToPicrossMode()
    {
        overworldVFX.SetActive(false);
        picrossVFX.SetActive(true);
    }

    public void SwitchToOverworldMode()
    {
        overworldVFX.SetActive(true);
        picrossVFX.SetActive(false);
    }
}
