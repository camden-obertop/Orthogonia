using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    // Each of these is called after you beat a level. I.e. the earth transition is enabled when you beat the air puzzle
    [Header("Air")] 
    [SerializeField] private GameObject airTransition;
    [SerializeField] private GameObject airParticles;
    [SerializeField] private Material gloomySkybox;
    [SerializeField] private Material normalSkybox;
    [SerializeField] private GameObject earthTransition;
    [SerializeField] private Color initialFogColor;
    [SerializeField] private Light dirLight;
    [SerializeField] private Color initialDirectionalLightColor;

    [Header("Earth")] 
    [SerializeField] private GameObject earthLandscape;
    [SerializeField] private GameObject airLandscape;
    [SerializeField] private GameObject windyAir;
    [SerializeField] private GameObject wormTransition;

    [Header("Worm")] 
    [SerializeField] private GameObject worms;
    [SerializeField] private GameObject rockTransition;

    [Header("Rocks")]
    [SerializeField] private GameObject rocks;
    [SerializeField] private GameObject sunTransition;

    [Header("Sun")]
    [SerializeField] private GameObject oceanTransition;
    [SerializeField] private Material sunSky;
    [SerializeField] private Color sunFogColor;
    [SerializeField] private Color sunDirectionalLightColor;

    [Header("Ocean")]
    [SerializeField] private GameObject cloudsTransition;
    [SerializeField] private GameObject ocean;

    [Header("Clouds")]
    [SerializeField] private GameObject riverTransition;
    [SerializeField] private GameObject clouds;

    [Header("River")]
    [SerializeField] private GameObject grassTransition;
    [SerializeField] private GameObject riverCover;

    [Header("Grass")]
    [SerializeField] private GameObject flowerTransition;
    [SerializeField] private GameObject grassEarth;
    [SerializeField] private Material grassSky;
    [SerializeField] private Material brighterWater;

    [Header("Flower")]
    [SerializeField] private GameObject beeTransition;
    [SerializeField] private GameObject flowerParent;

    [Header("Bee")]
    [SerializeField] private GameObject treeTransition;
    [SerializeField] private GameObject bees;

    [Header("Tree")]
    [SerializeField] private GameObject bushTransition;

    [Header("Bush")]
    [SerializeField] private GameObject fruitTransition;

    [Header("Fruit")]
    [SerializeField] private GameObject fungusTransition;

    [Header("Fungus")]
    [SerializeField] private GameObject fishTransition;

    [Header("Fish")]
    [SerializeField] private GameObject birdTransition;

    [Header("Bird")]
    [SerializeField] private GameObject squirrelTransition;

    [Header("Squirrel")]
    [SerializeField] private GameObject foxTransition;

    [Header("Fox")]
    [SerializeField] private GameObject humanTransition;

    private void Start()
    {
        RenderSettings.skybox = gloomySkybox;
        RenderSettings.fogDensity = 0.18f;
    }

    public void ActivatePuzzleInWorld(CompletedPuzzle.Puzzle puzzleType)
    {
        switch (puzzleType)
        {
            case CompletedPuzzle.Puzzle.Air:
                airTransition.SetActive(false);
                airParticles.SetActive(false);
                RenderSettings.skybox = normalSkybox;
                RenderSettings.fogDensity = 0.007f;
                RenderSettings.fogColor = initialFogColor;
                dirLight.color = initialDirectionalLightColor;
                windyAir.SetActive(true);
                earthTransition.SetActive(true);
                break;
            case CompletedPuzzle.Puzzle.Earth:
                earthTransition.SetActive(false);
                earthLandscape.SetActive(true);
                airLandscape.SetActive(false);
                wormTransition.SetActive(true);
                break;
            case CompletedPuzzle.Puzzle.Worm:
                wormTransition.SetActive(false);
                worms.SetActive(true);
                rockTransition.SetActive(true);
                break;
            case CompletedPuzzle.Puzzle.Rock:
                rockTransition.SetActive(false);
                rocks.SetActive(true);
                sunTransition.SetActive(true);
                break;
            case CompletedPuzzle.Puzzle.Sun:
                sunTransition.SetActive(false);
                oceanTransition.SetActive(true);
                RenderSettings.skybox = sunSky;
                RenderSettings.fogColor = sunFogColor;
                dirLight.color = sunDirectionalLightColor;
                break;
            case CompletedPuzzle.Puzzle.Ocean:
                oceanTransition.SetActive(false);
                cloudsTransition.SetActive(true);
                ocean.SetActive(true);
                break;
            case CompletedPuzzle.Puzzle.Clouds:
                cloudsTransition.SetActive(false);
                riverTransition.SetActive(true);
                clouds.SetActive(true);
                break;
            case CompletedPuzzle.Puzzle.River:
                riverTransition.SetActive(false);
                grassTransition.SetActive(true);
                riverCover.SetActive(false);
                break;
            case CompletedPuzzle.Puzzle.Grass:
                grassTransition.SetActive(false);
                flowerTransition.SetActive(true);
                earthLandscape.SetActive(false);
                grassEarth.SetActive(true);
                RenderSettings.skybox = grassSky;
                ocean.GetComponent<MeshRenderer>().material = brighterWater;
                break;
            case CompletedPuzzle.Puzzle.Flower:
                flowerTransition.SetActive(false);
                beeTransition.SetActive(true);
                break;
            case CompletedPuzzle.Puzzle.Bee:
                beeTransition.SetActive(false);
                treeTransition.SetActive(true);
                bees.SetActive(true);
                break;
            case CompletedPuzzle.Puzzle.Tree:
                treeTransition.SetActive(false);
                bushTransition.SetActive(true);
                break;
            case CompletedPuzzle.Puzzle.Bush:
                bushTransition.SetActive(false);
                fruitTransition.SetActive(true);
                break;
            case CompletedPuzzle.Puzzle.Fruit:
                fruitTransition.SetActive(false);
                fungusTransition.SetActive(true);
                break;
            case CompletedPuzzle.Puzzle.Fungus:
                fungusTransition.SetActive(false);
                fishTransition.SetActive(true);
                break;
            case CompletedPuzzle.Puzzle.Fish:
                fishTransition.SetActive(false);
                birdTransition.SetActive(true);
                break;
            case CompletedPuzzle.Puzzle.Bird:
                birdTransition.SetActive(false);
                squirrelTransition.SetActive(true);
                break;
            case CompletedPuzzle.Puzzle.Squirrel:
                squirrelTransition.SetActive(false);
                foxTransition.SetActive(true);
                break;
            case CompletedPuzzle.Puzzle.Fox:
                foxTransition.SetActive(false);
                humanTransition.SetActive(true);
                break;
            case CompletedPuzzle.Puzzle.Human:
                humanTransition.SetActive(false);
                break;
        }
    }
}