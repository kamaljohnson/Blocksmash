using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameUIScript : MonoBehaviour {

    public static bool TopTap;
    public static bool LeftTap;
    public static bool RightTap;
    public static bool taped;
    // Use this for initialization

    void Start () {
        TopTap = false;
        LeftTap = false;
        RightTap = false;
        taped = false;
    }

    void Update()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase ==  TouchPhase.Began && GameManager.isPlaying) {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    switch (hit.collider.tag)
                    {
                        case "TopButton":
                            if (!taped )
                            {
                                taped = true;
                                TopTap = true;
                            }
                            break;
                        case "RightButton":
                            if (!taped)
                            {
                                taped = true;
                                RightTap = true;
                            }
                            break;
                        case "LeftButton":
                            if (!taped)
                            {
                                taped = true;
                                LeftTap = true;
                            }
                            break;
                    }
                }
            }
        }
    }
}
