using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollisionTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "OnBody" || other.tag == "Block")
        {
            GameBoardScript.gameOver = true;
            Debug.Log("hit...");
        }
    }
}
