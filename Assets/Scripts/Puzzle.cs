using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Puzzle", menuName = "ScriptableObjects/Puzzle", order = 1)] [Serializable]
public class Puzzle : ScriptableObject
{
    public UInt32[] Palette;
    public byte[] Data;
    public Int32 SizeX;
    public Int32 SizeY;
    public Int32 SizeZ;
}