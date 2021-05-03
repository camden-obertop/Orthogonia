using UnityEngine;

public class StartPuzzle : MonoBehaviour
{
    [SerializeField] private Puzzle puzzle;
    [SerializeField] private GameObject voxelManager;

    public void GoToPuzzle()
    {
        GameObject vrManagerObject = GameObject.FindGameObjectWithTag("VRManager");
        if (vrManagerObject != null)
            Destroy(vrManagerObject);

        GameObject modeSelector = GameObject.FindGameObjectWithTag("ModeSelector");
        if (modeSelector != null)
        {
            foreach (Transform child in modeSelector.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        
        GameObject voxelManagerInstance = Instantiate(voxelManager, transform.position + transform.forward * 2f,
            Quaternion.identity * Quaternion.Euler(0f, 180f, 0f));
        voxelManagerInstance.GetComponent<VoxelManager>().BeginPuzzle(puzzle);
    }
}