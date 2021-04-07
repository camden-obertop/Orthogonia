using System;
using System.Collections.Generic;
using CsharpVoxReader;
using CsharpVoxReader.Chunks;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
#endif

public class VoxToPuzzle : MonoBehaviour
{
    [SerializeField] private string newPuzzleName = "New Puzzle";
    [SerializeField] private string nameOfVoxFile = "Fox2Test.vox";

    private void Start()
    {
        Loader loader = new Loader();
        string path = Application.dataPath + "\\Puzzles\\Vox\\" + nameOfVoxFile;
        VoxReader reader = new VoxReader(@path, loader);
        reader.Read();

#if UNITY_EDITOR
        Puzzle newPuzzle = ScriptableObject.CreateInstance<Puzzle>();
        
        byte[] flatData = new byte[loader.SizeX * loader.SizeY * loader.SizeZ];
        for (int i = 0; i < loader.SizeX; i++)
        {
            for (int j = 0; j < loader.SizeY; j++)
            {
                for (int k = 0; k < loader.SizeZ; k++)
                {
                    flatData[k + j * loader.SizeZ + i * loader.SizeY * loader.SizeZ] = loader.Data[i, j, k];
                }
            }
        }

        newPuzzle.Palette = loader.Palette;
        newPuzzle.Data = flatData;
        newPuzzle.SizeX = loader.SizeX;
        newPuzzle.SizeY = loader.SizeY;
        newPuzzle.SizeZ = loader.SizeZ;
        
        string name = AssetDatabase.GenerateUniqueAssetPath("Assets/Puzzles/" + newPuzzleName + ".asset");
        AssetDatabase.CreateAsset(newPuzzle, name);
        AssetDatabase.SaveAssets();
#endif
    }
}

public class Loader : IVoxLoader
{
    public byte[,,] Data;
    public UInt32[] Palette;
    public Int32 SizeX;
    public Int32 SizeY;
    public Int32 SizeZ;

    public void LoadModel(Int32 sizeX, Int32 sizeY, Int32 sizeZ, byte[,,] data)
    {
        SizeX = sizeX;
        SizeY = sizeY;
        SizeZ = sizeZ;
        Data = data;
    }

    public void LoadPalette(UInt32[] palette)
    {
        Palette = palette;
    }

    public void NewGroupNode(int id, Dictionary<string, byte[]> attributes, int[] childrenIds)
    {
        throw new NotImplementedException();
    }

    public void NewLayer(int id, Dictionary<string, byte[]> attributes)
    {
        throw new NotImplementedException();
    }

    public void NewMaterial(int id, Dictionary<string, byte[]> attributes)
    {
        throw new NotImplementedException();
    }

    public void NewShapeNode(int id, Dictionary<string, byte[]> attributes, int[] modelIds, Dictionary<string, byte[]>[] modelsAttributes)
    {
        throw new NotImplementedException();
    }

    public void NewTransformNode(int id, int childNodeId, int layerId, Dictionary<string, byte[]>[] framesAttributes)
    {
        throw new NotImplementedException();
    }

    public void SetMaterialOld(int paletteId, MaterialOld.MaterialTypes type, float weight, MaterialOld.PropertyBits property, float normalized)
    {
        throw new NotImplementedException();
    }

    public void SetModelCount(int count)
    {
        throw new NotImplementedException();
    }
}