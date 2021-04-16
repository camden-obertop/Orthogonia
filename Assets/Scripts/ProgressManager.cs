using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private GameObject foxes;

    private void Start()
    {
        GameObject completedPuzzleObject = GameObject.FindGameObjectWithTag("CompletedPuzzle");
        if (completedPuzzleObject != null)
        {
            ActivatePuzzleInWorld(completedPuzzleObject.GetComponent<CompletedPuzzle>().PuzzleType);
        }
    }

    private void ActivatePuzzleInWorld(CompletedPuzzle.Puzzle puzzleType)
    {
        switch (puzzleType)
        {
            case CompletedPuzzle.Puzzle.Fox:
                foxes.SetActive(true);
                break;
            case CompletedPuzzle.Puzzle.Sun:
                break;
        }
    }
}
