using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoardScript : MonoBehaviour {

    int[,] BoardTop = new int[3, 3];

    int[,,] Board = new int[6, 3, 3];

    public static bool PlayStart = false;

    int[,] CurrentBlock = new int[3, 3];
    List<GameObject> BlocksOnFace01 = new List<GameObject>();
    List<GameObject> BlocksOnFace02 = new List<GameObject>();
    List<GameObject> BlocksOnFace03 = new List<GameObject>();
    List<GameObject> BlocksOnFace04 = new List<GameObject>();
    List<GameObject> BlocksOnFace05 = new List<GameObject>();
    List<GameObject> BlocksOnFace06 = new List<GameObject>();

    public enum Faces
    {
        Face01,
        Face02,
        Face03,
        Face04,
        Face05,
        Face06,
    };

    public static Faces TopFace;

    //public gameObjects 
    public GameObject blockData;
    BlockData BData;
    public GameObject Block;
    public GameObject BlockBody;
    public GameObject Rotatingmesh;
    //for maze Rotation
    Vector3 localLeft;
    Vector3 localForward;
    Vector3 localDown;
    public Transform refTransform;
    List<GameObject> blockSet = new List<GameObject>();
    int[,] blockArray = new int[3, 3];
    public bool canRotate;
    public Collider BottomTrigger;

    public Material BlockMaterial01;
    public Material BlockMaterial02;
    public Material BlockMaterial03;
    public Material BlockMaterial04;
    public Material BlockMaterial05;
    Material currentMaterial;

    public GameObject ScoreUI;

    bool canMoveRight = true;
    bool canMoveLeft = true;
    bool canMoveForward = true;
    bool canMoveBack = true;


    public Text text01;
    public Text text02;
    public Text text03;
    public Text text04;
    public Text text05;
    public Text text06;
    public Text text07;
    public Text text08;
    public Text text09;
    public Text text10;
    public Text text11;
    public Text text12;
    public Text text13;
    public Text text14;
    public Text text15;
    public Text text16;

    public static bool gameOver = false;

    BlockControler BC;
    public GameObject Bcontroler;

    public static int score;
    private void Awake()
    {
        for(int i = 0;i<6;i++)
        {
            for(int j = 0;j<3;j++)
            {
                for(int k = 0; k<3;k++)
                {
                    Board[i, j, k] = 0;
                }
            }
        }
        Screen.orientation = ScreenOrientation.Portrait;
        BData = blockData.GetComponent<BlockData>();
        //List<GameObject> temp = new List<GameObject>();
        
        currentMaterial = BlockMaterial01;
        BC = Bcontroler.GetComponent<BlockControler>();
        score = 0;
        localDown = new Vector3(0, 1, 0);
        localLeft = new Vector3(1, 0, 0);
        localForward = new Vector3(0, 0, -1);
        canRotate = true;
        
    }
    private void Update()
    {
        if (PlayStart)
        {
            CreateBlock();
            PlayStart = false;
        }

        text01.text = CurrentBlock[0, 0].ToString();
        text02.text = CurrentBlock[0, 1].ToString();
        text03.text = CurrentBlock[0, 2].ToString();
        text05.text = CurrentBlock[1, 0].ToString();
        text06.text = CurrentBlock[1, 1].ToString();
        text07.text = CurrentBlock[1, 2].ToString();
        text09.text = CurrentBlock[2, 0].ToString();
        text10.text = CurrentBlock[2, 1].ToString();
        text11.text = CurrentBlock[2, 2].ToString();
        
        int count = 0; 
        for(int i = 0; i< 3;i++)
        {
            for (int j = 0; j < 3; j++)
            {
                count += Board[(int)TopFace,i, j];
            }
        }
        if (count >= 9)
        {
            for(int i = 0;i<9;i++)
            {
                switch(TopFace)
                {
                    case Faces.Face01:
                        Destroy(BlocksOnFace01[i]);
                        break;
                    case Faces.Face02:
                        Destroy(BlocksOnFace02[i]);
                        break;
                    case Faces.Face03:
                        Destroy(BlocksOnFace03[i]);
                        break;
                    case Faces.Face04:
                        Destroy(BlocksOnFace04[i]);
                        break;
                    case Faces.Face05:
                        Destroy(BlocksOnFace05[i]);
                        break;
                    case Faces.Face06:
                        Destroy(BlocksOnFace06[i]);
                        break;
                }
            }
            switch (TopFace)
            {
                case Faces.Face01:
                    BlocksOnFace01 = new List<GameObject>();
                    break;
                case Faces.Face02:
                    BlocksOnFace02 = new List<GameObject>();
                    break;
                case Faces.Face03:
                    BlocksOnFace03 = new List<GameObject>();
                    break;
                case Faces.Face04:
                    BlocksOnFace04 = new List<GameObject>();
                    break;
                case Faces.Face05:
                    BlocksOnFace05 = new List<GameObject>();
                    break;
                case Faces.Face06:
                    BlocksOnFace06 = new List<GameObject>();
                    break;
            }


            for (int i = 0;i<3;i++)
            {
                for(int j = 0;j<3;j++)
                {
                    Board[(int)TopFace, i, j] = 0;
                    BoardTop[i, j] = 0;
                }
            }
        }


        if (BottomCollisinTrigger.reachedNoRotationZone)
        {
            canRotate = false;
        }
        if (BottomCollisinTrigger.reachedDown)
        {
            if (!gameOver)
            {
                CreateBlock();
                score++;
            }
        }
        
        if (canRotate)
        {
            if ((GameUIScript.TopTap || Input.GetKeyDown(KeyCode.Space)) && !BoardRotation.isRotating)
            {
                Debug.Log("Rotating Top");
                BoardRotation.rotationDestination = localDown;
                BoardRotation.isRotating = true;
                GameUIScript.TopTap = false;
                GameUIScript.taped = false;
                int[,] temp = new int[3, 3];
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        temp[i, j] = CurrentBlock[2-j,i];
                    }
                }
                CurrentBlock = temp;
            }
            if ((GameUIScript.RightTap || Input.GetKeyDown(KeyCode.RightArrow)) && !BoardRotation.isRotating)
            {
                GameUIScript.RightTap = false;
                GameUIScript.taped = false;
                BoardRotation.rotationDestination = localLeft;
                BoardRotation.isRotating = true;
            }
            if ((GameUIScript.LeftTap || Input.GetKeyDown(KeyCode.LeftArrow)) && !BoardRotation.isRotating)
            {
                GameUIScript.LeftTap = false;
                GameUIScript.taped = false;
                BoardRotation.rotationDestination = localForward;
                BoardRotation.isRotating = true;
            }
        }
        CheckEdge();

        if ((SwipeInput.swipeRight || Input.GetKeyDown(KeyCode.D))) 
        {
            if(canMoveRight)
            {
                MoveRight();
            }
            SwipeInput.swipeRight = false;
            SwipeInput.swipeFlag = false;
        }
        if((SwipeInput.swipeLeft || Input.GetKeyDown(KeyCode.A)))
        {
            if (canMoveLeft)
            {
                MoveLeft();
            }
            SwipeInput.swipeLeft = false;
            SwipeInput.swipeFlag = false;
        }
        if ((SwipeInput.swipeForward || Input.GetKeyDown(KeyCode.W)))
        {
            if (canMoveForward)
            {

                MoveForward();
            }
            SwipeInput.swipeForward = false;
            SwipeInput.swipeFlag = false;
        }
        if ((SwipeInput.swipeBack || Input.GetKeyDown(KeyCode.S)))
        {
            if (canMoveBack)
            {
                MoveBack();
            }
            SwipeInput.swipeFlag = false;
            SwipeInput.swipeBack = false;
        }

    }
    void MoveBack()
    {
        for (int i = 0;i < 2;i++)
        {
            for (int j = 0; j < 3; j++)
            {
                CurrentBlock[i, j] = CurrentBlock[i+1, j];
            }
        }
        for (int j = 0; j < 3; j++)
        {
            CurrentBlock[2, j] = 0;
        }
        BC.MoveBack();
    }
    void MoveForward()
    {

        for (int i = 2; i > 0; i--)
        {
            for (int j = 0; j < 3; j++)
            {
                CurrentBlock[i, j] = CurrentBlock[i-1, j];
            }
        }
        for (int j = 0; j < 3; j++)
        {
            CurrentBlock[0, j] = 0;
        }
        BC.MoveForward();

    }
    void MoveLeft()
    {

        for (int i = 0; i < 3; i++)
        {
            for (int j = 2; j > 0; j--)
            {
                CurrentBlock[i, j] = CurrentBlock[i, j-1];
            }
        }
        for (int j = 0; j < 3; j++)
        {
            CurrentBlock[j, 0] = 0;
        }
        BC.MoveLeft();

    }
    void MoveRight()
    {

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                CurrentBlock[i, j] = CurrentBlock[i, j+1];
            }
        }
        for (int j = 0; j < 3; j++)
        {
            CurrentBlock[j, 2] = 0;
        }
        BC.MoveRight();
    }

    void CheckEdge()
    {
        canMoveBack = true;
        canMoveForward = true;
        canMoveRight = true;
        canMoveLeft = true;
        for(int i = 0; i<3;i++)
        {
            if(CurrentBlock[i,2] == 1)
            {
                canMoveLeft = false;
                Debug.Log("x - left");
            }
            if (CurrentBlock[i, 0] == 1)
            {
                canMoveRight = false;
                Debug.Log("x - right");
            }
            if (CurrentBlock[0, i] == 1)
            {
                canMoveBack = false;
                Debug.Log("x - back");
            }
            if (CurrentBlock[2, i] == 1)
            {
                canMoveForward = false;
                Debug.Log("x - forward");
            }
        }

    }
    void CreateBlock()
    {
        BottomCollisinTrigger.reachedNoRotationZone = false;
        canRotate = true;
        BlockBody.GetComponent<BlockControler>().onBody = true;
        BottomCollisinTrigger.reachedDown = false;
        GameObject tempObj = new GameObject();

        for(int i = 0;i<3;i++)
        {
            for(int j = 0;j<3; j++)
            {
                Board[(int)TopFace, i,j] += blockArray[i, j];
            }
        }
        for (int i = 0; i < blockSet.Count; i++)
        {
            blockSet[i].transform.position = new Vector3(blockSet[i].transform.position.x, 2.0f, blockSet[i].transform.position.z);
            tempObj = Instantiate(Block, blockSet[i].transform.position, Quaternion.identity, Rotatingmesh.transform);
            tempObj.GetComponent<Renderer>().material = currentMaterial;
            tempObj.tag = "OnBody";

            switch (TopFace)
            {
                case Faces.Face01:
                    BlocksOnFace01.Add(tempObj);
                    break;
                case Faces.Face02:
                    BlocksOnFace02.Add(tempObj);
                    break;
                case Faces.Face03:
                    BlocksOnFace03.Add(tempObj);
                    break;
                case Faces.Face04:
                    BlocksOnFace04.Add(tempObj);
                    break;
                case Faces.Face05:
                    BlocksOnFace05.Add(tempObj);
                    break;
                case Faces.Face06:
                    BlocksOnFace06.Add(tempObj);
                    break;
            }

            Destroy(blockSet[i]);
        }
        int r = Random.Range(0, 5);
        switch(r)
        {
            case 0:
                currentMaterial = BlockMaterial01;
                break;
            case 1:
                currentMaterial = BlockMaterial02;
                break;
            case 2:
                currentMaterial = BlockMaterial03;
                break;
            case 3:
                currentMaterial = BlockMaterial04;
                break;
            case 4:
                currentMaterial = BlockMaterial05;
                break;

        }
        blockSet = new List<GameObject>();
        CurrentBlock = new int[3, 3];
        blockArray = new int[3, 3];
        //blockArray = BData.GetBlock();
        for (int i = 0; i < 3; i++)
        {
            for(int j = 0;j < 3; j++)
            {
                if(blockArray[i,j] == 1)
                {
                    float x = i - 1.0f;
                    float y = j - 1.0f;
                    CurrentBlock[i, j] = 1;
                    blockSet.Add(Instantiate(Block, new Vector3(x, 18, y), Quaternion.identity, BlockBody.transform));
                    blockSet[blockSet.Count -1].GetComponent<Renderer>().material = currentMaterial;
                }
                else
                {
                    CurrentBlock[i, j] = 0;
                }
            }
            BlockBody.GetComponent<BlockControler>().onBody = false;
        }
    }
}
