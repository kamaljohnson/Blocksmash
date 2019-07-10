using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardRotation : MonoBehaviour
{
    public static bool isTop = false;
    public static bool isRotating = true;
    public static Vector3 rotationDestination;
    float counter = 0;

    private void Start()
    {
        isRotating = false;
    }

    void FixedUpdate()
    {
        if (isRotating && !GameBoard.FastMotionFlag)
        {
            if (counter < 15)
            {
                counter++;
                if (isTop)
                {
                    transform.transform.Rotate(rotationDestination * 6, Space.World);
                }
                else
                {
                    transform.Rotate(rotationDestination * 6, Space.World);
                }
            }
            else
            {
                isRotating = false;
                counter = 0;
                isTop = false;
            }

            //rotationFlag = true;
        }
    }
}
