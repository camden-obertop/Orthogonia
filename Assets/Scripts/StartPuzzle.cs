using UnityEngine;

public class StartPuzzle : MonoBehaviour
{
    [SerializeField] private Puzzle puzzle;
    public Puzzle PuzzleObject => puzzle;

    [SerializeField] private VoxelManager voxelManager;
    [SerializeField] private GameObject controlScheme;
    [SerializeField] private GameObject player;

    private void Start()
    {
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }
    
    public void GoToPuzzle()
    {
        Instantiate(voxelManager, transform.position + transform.forward * 5f, Quaternion.identity);
        // Destroy(controlScheme);
        // Destroy(player);
        // levelLoader.SetActive(true);
    }
}
