using UnityEngine;

public class StartPuzzle : MonoBehaviour
{
    [SerializeField] private Puzzle puzzle;
    public Puzzle PuzzleObject => puzzle;
    
    [SerializeField] private GameObject levelLoader;

    private void Start()
    {
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }
    
    public void GoToPuzzle()
    {
        levelLoader.SetActive(true);
    }
}
