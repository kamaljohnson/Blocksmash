using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastScript : MonoBehaviour {

    private void Update()
    {
        RaycastHit hit;
        
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            switch(hit.collider.tag)
            {
                case "Face01":
                    GameBoardScript.TopFace = GameBoardScript.Faces.Face01;
                    break;
                case "Face02":
                    GameBoardScript.TopFace = GameBoardScript.Faces.Face02;
                    break;
                case "Face03":
                    GameBoardScript.TopFace = GameBoardScript.Faces.Face03;
                    break;
                case "Face04":
                    GameBoardScript.TopFace = GameBoardScript.Faces.Face04;
                    break;
                case "Face05":
                    GameBoardScript.TopFace = GameBoardScript.Faces.Face05;
                    break;
                case "Face06":
                    GameBoardScript.TopFace = GameBoardScript.Faces.Face06;
                    break;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 1000, Color.white);
        }
    }

}
