using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeInput : MonoBehaviour
{

    //controls 
    public static bool tapDownLeft = false;
    public static bool tapDownRight = false;
    public static bool tapUp = false;
    public static bool swipeRight = false;
    public static bool swipeLeft = false;
    public static bool swipeForward = false;
    public static bool swipeBack = false;
    public static bool TopTap = false;

    public static bool swipeFlag = false;

    private Vector3 fp;   
    private Vector3 lp;  
    private float dragDistance;
/*    private float tapTimer;
    private float doubleTapTimeInterval = 0.5f;
    private bool singleTapFlag;*/

    bool TuchFlag = false;

    void Start()
    {
/*        singleTapFlag = false;
        tapTimer = 0;*/
        
        dragDistance = Screen.height * 2 / 100; //dragDistance is 15% height of the screen
    }
    private void Update()
    {
/*        if (singleTapFlag)
        {
            tapTimer += Time.deltaTime;
            if (tapTimer > doubleTapTimeInterval)
            {
                tapTimer = 0;
                singleTapFlag = false;
                TopTap = false;
            }
        }*/
        if (GameManager.isPlaying)
        {

            if (Input.touchCount >= 1)
            {
                Touch touch = Input.GetTouch(0);

                
                if (touch.phase == TouchPhase.Began)
                {
                    fp = touch.position;
                    lp = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    lp = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)//&& GameBoard.playFlag)
                {

                    {
                        lp = touch.position;
                        if (Mathf.Abs(lp.x - fp.x) > dragDistance && Mathf.Abs(lp.y - fp.y) > dragDistance && !swipeFlag)// && !GameBoard.GamePaused)
                        {
                            if (lp.x - fp.x > 0 && lp.y - fp.y > 0)
                            {
                                swipeForward = true;
                            }
                            if (lp.x - fp.x < 0 && lp.y - fp.y < 0)
                            {
                                swipeBack = true;
                            }
                            if (lp.x - fp.x < 0 && lp.y - fp.y > 0)
                            {
                                swipeLeft = true;
                            }
                            if (lp.x - fp.x > 0 && lp.y - fp.y < 0)
                            {
                                swipeRight = true;
                            }


                        }
                        else if (!GameBoard.FastMotionFlag && !BoardRotation.isTop && touch.position.y > Screen.height / 2.3 && touch.position.y < Screen.height * 5 / 6)// && !GameBoard.GamePaused)
                        {
/*                            if (singleTapFlag)
                            {*/
                                TopTap = true;
                                /*singleTapFlag = false;
                            }
                            else
                            {
                                singleTapFlag = true;                                
                            }*/
                        }
                        
                    }
                }       
            }

        }
        else
        {
            TopTap = false;
            swipeRight = false;
            swipeLeft = false;
            swipeForward = false;
            swipeBack = false;
            swipeFlag = false;
        }
    }
}
