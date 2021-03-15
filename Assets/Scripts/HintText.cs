using TMPro;
using UnityEngine;

public class HintText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI voxelCountHintText;
    [SerializeField] private TextMeshProUGUI gapCountHintText;

    public void SetHintText(int voxelCount, int gapCount)
    {
        voxelCountHintText.text = voxelCount.ToString();
        gapCountHintText.text = gapCount.ToString();
    }
}
