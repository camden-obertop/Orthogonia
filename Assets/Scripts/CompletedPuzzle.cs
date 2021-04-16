using UnityEngine;

public class CompletedPuzzle : MonoBehaviour
{
    public enum Puzzle
    {
        Fox,
        Sun,
    }

    private Puzzle _puzzleType = Puzzle.Fox;
    public Puzzle PuzzleType
    {
        get => _puzzleType;
        set => _puzzleType = value;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
