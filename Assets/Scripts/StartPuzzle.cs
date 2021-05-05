using UnityEngine;

using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class StartPuzzle : MonoBehaviour
{
    [SerializeField] private Puzzle puzzle;
    [SerializeField] private GameObject voxelManager;
    [SerializeField] private GameObject overworldPlayer;
    [SerializeField] private GameObject picrossPlayer;
    [SerializeField] private bool firstPuzzle;
    [SerializeField] private bool earthPuzzle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandCollider"))
        {
            GoToPuzzle();
        }
    }

    public void GoToPuzzle()
    {
        picrossPlayer.transform.position = overworldPlayer.transform.position;
        picrossPlayer.transform.rotation = overworldPlayer.transform.rotation;
        overworldPlayer.SetActive(false);
        picrossPlayer.SetActive(true);

        if (firstPuzzle)
        {
            GameObject.FindGameObjectWithTag("VO").GetComponent<VOManager>().FirstPuzzleDialogueDriver();
        }
        else if (earthPuzzle)
        {
            GameObject.FindGameObjectWithTag("VO").GetComponent<VOManager>().StartEarthPuzzle();
        }

        GameObject.FindGameObjectWithTag("VFX").GetComponent<VFXManager>().SwitchToPicrossMode();

        GameObject modeSelector = GameObject.FindGameObjectWithTag("ModeSelector");
        if (modeSelector != null)
        {
            foreach (Transform child in modeSelector.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        
        GameObject voxelManagerInstance = Instantiate(voxelManager, transform.position,
            Quaternion.identity * Quaternion.Euler(0f, 180f, 0f));
        voxelManagerInstance.GetComponent<VoxelManager>().BeginPuzzle(puzzle, picrossPlayer, overworldPlayer);
        transform.parent.gameObject.SetActive(false);
    }
}