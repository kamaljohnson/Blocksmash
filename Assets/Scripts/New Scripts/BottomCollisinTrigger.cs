using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCollisinTrigger : MonoBehaviour {

    public static bool reachedNoRotationZone; // gets activated when the block reaches here 
    public static bool reachedDown;

    private void Start()
    {
        reachedDown = false;
        reachedNoRotationZone = false;
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("reached no rotation zone ");
        reachedNoRotationZone = true;
    }
    void OnTriggerExit(Collider other)
    {
        reachedDown = true;
    }
}
