using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rayCastOnBodyBlocks : MonoBehaviour {


    // Update is called once per frame
    public static bool BlockBelow;
	void Update () {
        RaycastHit hit;

        
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            if (hit.collider.tag == "onBody")
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
                Debug.Log("hit");
            }
            else
            {
                BlockBelow = false;
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 3, Color.white);
            }
        }
        
    }
}
