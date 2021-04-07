using UnityEngine;

public class StartPuzzle : MonoBehaviour
{
    [SerializeField] private Puzzle puzzle;
    public Puzzle PuzzleObject => puzzle;
    
    [SerializeField] private GameObject levelLoader;
    [SerializeField] private GameObject controlScheme;
    [SerializeField] private GameObject player;

    private void Start()
    {
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }
    
    public void GoToPuzzle()
    {
        Destroy(controlScheme);
        Destroy(player);
        levelLoader.SetActive(true);
    }
}
