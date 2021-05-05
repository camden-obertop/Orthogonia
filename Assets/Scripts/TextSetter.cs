using TMPro;
using UnityEngine;

public class TextSetter : MonoBehaviour
{
    [SerializeField] private string textToSet;
    [SerializeField] private TextMeshProUGUI text1;
    [SerializeField] private TextMeshProUGUI text2;

    private void Start()
    {
        text1.text = textToSet;
        text2.text = textToSet;
    }
}
