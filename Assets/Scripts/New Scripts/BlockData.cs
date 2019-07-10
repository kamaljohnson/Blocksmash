using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockData:MonoBehaviour {
    
    const int numberOfBlocks = 6;
    const float heightOffset = 20;
    static List<List<Vector3>> blocks = new List<List<Vector3>> 
    {
        new List<Vector3>
        {
            new Vector3(1.5f,heightOffset,-1.5f),
            new Vector3(1.5f,heightOffset,-0.5f),
            new Vector3(0.5f,heightOffset,-1.5f)
        },
        new List<Vector3>
        {
            new Vector3(1.5f,heightOffset,-1.5f),
            new Vector3(1.5f,heightOffset,-0.5f),
            new Vector3(0.5f,heightOffset,-1.5f),
            new Vector3(0.5f,heightOffset,-0.5f)
        
        },
        new List<Vector3>
        {
            new Vector3(1.5f,heightOffset,-1.5f),
            new Vector3(1.5f,heightOffset,-0.5f),
            new Vector3(0.5f,heightOffset,-1.5f),
            new Vector3(1.5f,heightOffset,0.5f)
        },
        new List<Vector3>
        {
            new Vector3(1.5f,heightOffset,-1.5f),
            new Vector3(1.5f,heightOffset,-0.5f),
            new Vector3(1.5f,heightOffset,0.5f)
        },
        new List<Vector3>
        {
            new Vector3(0.5f,heightOffset,-1.5f),
            new Vector3(0.5f,heightOffset,-0.5f),
            new Vector3(0.5f,heightOffset,0.5f),
            new Vector3(1.5f,heightOffset,-1.5f)

        },
        new List<Vector3>
        {
             new Vector3(0.5f,heightOffset,-1.5f),
            new Vector3(0.5f,heightOffset,-0.5f),
            new Vector3(0.5f,heightOffset,0.5f),
            new Vector3(1.5f,heightOffset,-0.5f)
        },
    };
    public static List<Vector3> GetBlock()
    {
        int r = Random.Range(0, numberOfBlocks);
        return blocks[r];
    }
    public static List<Vector3> GetBlock(int index)
    {
        return blocks[index];
    }
}
