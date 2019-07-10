using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Yellow,
    Green,
    Red,
    Blue,
    Violet,
    Magic01,
    Magic02,
    Magic03,
    Count
};  // all the types of blocks in the game   


public class Block:MonoBehaviour{
    public GameObject obj;
    public BlockType type;
    public bool OnBody;
    public bool state;
    /*public void OnCreate(GameObject obj, BlockType type,bool OnBody, bool state)
    {
        this.obj = obj;
        this.type = type;
        this.OnBody = OnBody;
        this.state = state;
    }*/
    public void OnDestroy()
    {
        Destroy(this.obj);   
    }
}
