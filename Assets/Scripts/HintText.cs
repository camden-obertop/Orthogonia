using System;
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

    public void ClearHintText()
    {
        voxelCountHintText.text = String.Empty;
        gapCountHintText.text = String.Empty;
    }
}
