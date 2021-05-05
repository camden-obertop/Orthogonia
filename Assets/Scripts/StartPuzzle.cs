using UnityEngine;

public class StartPuzzle : MonoBehaviour
{
    [SerializeField] private Puzzle puzzle;
    [SerializeField] private GameObject voxelManager;
    [SerializeField] private GameObject overworldPlayer;
    [SerializeField] private GameObject picrossPlayer;

    public void GoToPuzzle()
    {
        picrossPlayer.transform.position = overworldPlayer.transform.position;
        picrossPlayer.transform.rotation = overworldPlayer.transform.rotation;
        overworldPlayer.SetActive(false);
        picrossPlayer.SetActive(true);

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