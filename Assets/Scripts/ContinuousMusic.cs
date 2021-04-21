using UnityEngine;

public class ContinuousMusic : MonoBehaviour
{
    [SerializeField] private GameObject BGM;

    private void Start()
    {
        GameObject existingBGM = GameObject.Find(BGM.name);
        if (existingBGM == null)
        {
            GameObject newBGMObject = Instantiate(BGM);
            DontDestroyOnLoad(newBGMObject);
        }
    }
}
