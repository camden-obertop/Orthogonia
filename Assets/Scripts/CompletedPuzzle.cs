using UnityEngine;

public class CompletedPuzzle : MonoBehaviour
{
    public enum Puzzle
    {
        Air,
        Earth,
        Worm,
        Rock,
        Sun,
        Ocean,
        Clouds,
        River,
        Grass,
        Flower,
        Bee,
        Tree,
        Bush,
        Fruit,
        Fungus,
        Fish,
        Bird,
        Squirrel,
        Fox,
        Human,
    }

    private Puzzle _puzzleType = Puzzle.Fox;
    public Puzzle PuzzleType
    {
        get => _puzzleType;
        set => _puzzleType = value;
    }

    private void Start()
    {
        GameObject.FindGameObjectWithTag("ProgressManager").GetComponent<ProgressManager>().ActivatePuzzleInWorld(_puzzleType);
    }

    private void LateUpdate()
    {
        //Destroy(gameObject);
    }
}
