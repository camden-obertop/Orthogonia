using System;
using System.Collections;
using System.Collections.Generic;
using CsharpVoxReader;
using CsharpVoxReader.Chunks;
using UnityEngine;

public class VoxToPuzzle : MonoBehaviour
{
    private void Start()
    {
        Loader loader = new Loader();
        VoxReader reader = new VoxReader(@"C:\Users\jgoter2\Documents\GitHub\vr-nonogram\Assets\Puzzles\Vox\Grass.vox", loader);
        reader.Read();
    }
}

public class Loader : IVoxLoader
{
    public void LoadModel(Int32 sizeX, Int32 sizeY, Int32 sizeZ, byte[,,] data)
    {
        bool loaded = true;
        // Create a model
    }

    public void LoadPalette(UInt32[] palette)
    {
        // Set palette
        bool howdy = true;

        // You can use extension method ToARGB from namespace CsharpVoxReader
        // To get rgba values
        // ie: palette[1].ToARGB(out a, out r, out g, out b)
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