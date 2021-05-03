using UnityEngine;

public class StartPuzzle : MonoBehaviour
{
    [SerializeField] private Puzzle puzzle;
    [SerializeField] private GameObject voxelManager;
    
    public void GoToPuzzle()
    {
        GameObject voxelManagerInstance = Instantiate(voxelManager, transform.position + transform.forward * 5f, Quaternion.identity);
        voxelManagerInstance.GetComponent<VoxelManager>().BeginPuzzle(puzzle);
    }
}
