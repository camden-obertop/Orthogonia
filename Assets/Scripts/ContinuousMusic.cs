using UnityEngine;

public class ContinuousMusic : MonoBehaviour
{
    [SerializeField] private GameObject BGM;

    private void Start()
    {
        GameObject[] existingBGM = GameObject.FindGameObjectsWithTag("BGMPlayer");
        if (existingBGM.Length == 0)
        {
            GameObject newBGMObject = Instantiate(BGM);
            DontDestroyOnLoad(newBGMObject);
        }
    }
}
