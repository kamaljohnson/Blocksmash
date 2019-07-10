using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockControler : MonoBehaviour {

    public bool onBody;
    public GameObject blocK;
    private void Start()
    {
        onBody = true;
    }
    private void FixedUpdate()
    {
        if(!onBody)
        {
            transform.position += new Vector3(0, -0.05f, 0);
        }
        
    }
    public void MoveForward()
    {
        blocK.transform.position += new Vector3(1, 0, 0);
    }
    public void MoveBack()
    {

        blocK.transform.position -= new Vector3(1, 0, 0);

    }
    public void MoveLeft()
    {

        blocK.transform.position += new Vector3(0, 0, 1);

    }
    public void MoveRight()
    {

        blocK.transform.position -= new Vector3(0, 0, 1);

    }
}
