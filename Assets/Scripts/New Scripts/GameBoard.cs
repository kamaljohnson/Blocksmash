using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class GameBoard : MonoBehaviour {

    public static bool extraLif = false;

    public GameObject Score;
    public GameObject HighScore;
    Text HighScoreText;
    Animator highScreAnimator;
    Text scoreText;
    Animator scoreAnimator;
    public static int score = 0;
    int highScore;

    public GameObject blockPrefab;
    public GameObject ReferenceCubePrefab;
    public GameObject BoardBody;
    public static Animator BodyAnimation;
    public GameObject BlockBody;

    public GameObject MagicCube01;
    public GameObject MagicCube02;
    public GameObject MagicCube03;

    public Material BlockType01;
    public Material BlockType02;
    public Material BlockType03;
    public Material BlockType04;
    public Material BlockType05;
    public Material BlockType06;
    public Material BlockType07;
    public Material BlockType08;

    public Transform BottomRotationTriggerTransform;

    public List<Gradient> ColorGradioant;

    public Material BlockTypeNull;

    public Material CanPlaceReferenceMaterial;
    public Material CanNotPlaceReferenceMaterial;

    Vector3 localLeft;
    Vector3 localForward;
    Vector3 localDown;
    public static bool canRotate;

    List<Material> BlockMaterials = new List<Material>();

    List<GameObject> Board = new List<GameObject>();
    List<GameObject> toDestroyBlocks = new List<GameObject>();  //the blocks which are to be deleted are stored here
    List<GameObject> BlockSet = new List<GameObject>();

    List<GameObject> ReferenceBlock = new List<GameObject>();
    bool ReferenCecreated;

    public static float speed = 1f;
    float speedFlag = 0.05f;

    public float snapOffset;    // when this offset is reached the block will snap to the body

    public static bool FastMotionFlag = false;

    bool specialCubeFlag = false;
    int threshold = 0;
    bool AllBlastFlag = false;

    
    private void Awake()
    {
        BodyAnimation = BoardBody.GetComponent<Animator>();

        HighScoreText = HighScore.GetComponent<Text>();
        highScreAnimator = HighScore.GetComponent<Animator>();
        scoreAnimator = Score.GetComponent<Animator>();
        scoreText = Score.GetComponent<Text>();

        if (PlayerPrefs.HasKey("highScore"))
            highScore = PlayerPrefs.GetInt("highScore");
        else
            highScore = 0;
        HighScoreText.text = highScore.ToString();

        Screen.orientation = ScreenOrientation.Portrait;

        localDown = new Vector3(0, 1, 0);
        localLeft = new Vector3(1, 0, 0);
        localForward = new Vector3(0, 0, -1);
        canRotate = true;
        
        BlockMaterials = new List<Material>{
            BlockType01,
            BlockType02,
            BlockType03,
            BlockType04,
            BlockType05,
            BlockType06,
            BlockType07,
            BlockType08
        };   //adding all the materials to this list 
        
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            if(GameManager.isPlaying)
                FindObjectOfType<GameManager>().Pause();
        }
    }

    private void Update()
    {
        if (GameManager.isPlaying)
        {
            if (highScore < score)
            {
                highScore = score;
                PlayerPrefs.SetInt("highScore", highScore);
                HighScoreText.text = highScore.ToString();
                highScreAnimator.Play("ScoreUpAnimation", -1, 0f);
            }
            if (speed > 10 && BlockBody.transform.position.y < -10)
            {
                canRotate = false;
            }
            if (!isReached())
            {
                BlockBody.transform.position += new Vector3(0, -1, 0) * speed * Time.deltaTime;

            }
            if (canRotate)
            {
                if (BoardBody.transform.position.y > -2)
                    DestroyReferenceCube();
                if ((GameUIScript.TopTap || Input.GetKeyDown(KeyCode.Space)) && !BoardRotation.isRotating)
                {
                    BoardRotation.rotationDestination = localDown;
                    BoardRotation.isRotating = true;
                    GameUIScript.TopTap = false;
                    GameUIScript.taped = false;
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
            if (!BoardRotation.isRotating)
            {
                DestroyReferenceCube();
                if ((SwipeInput.swipeRight || Input.GetKeyDown(KeyCode.D)))
                {
                    MoveRight();
                    SwipeInput.swipeRight = false;
                    SwipeInput.swipeFlag = false;
                }
                if ((SwipeInput.swipeLeft || Input.GetKeyDown(KeyCode.A)))
                {
                    MoveLeft();
                    SwipeInput.swipeLeft = false;
                    SwipeInput.swipeFlag = false;
                }
                if ((SwipeInput.swipeForward || Input.GetKeyDown(KeyCode.W)))
                {
                    MoveForward();
                    SwipeInput.swipeForward = false;
                    SwipeInput.swipeFlag = false;
                }
                if ((SwipeInput.swipeBack || Input.GetKeyDown(KeyCode.S)))
                {
                    MoveBack();
                    SwipeInput.swipeFlag = false;
                    SwipeInput.swipeBack = false;
                }
                if ((SwipeInput.TopTap || Input.GetKeyDown(KeyCode.DownArrow)) && !FastMotionFlag)
                {
                    for (int i = 0; i < BlockSet.Count; i++)
                        BlockSet[i].GetComponent<TrailRenderer>().time = 0.2f;
                    speed = 100;
                    FastMotionFlag = true;
                    SwipeInput.TopTap = false;
                }
            }
            if (!ReferenCecreated && !BoardRotation.isRotating && BoardBody.transform.position.y > -2)
                CreateReferenceCube();

            DestroyToBeDestroyedBlocks();

            if (BottomCollisinTrigger.reachedNoRotationZone || (FastMotionFlag && BlockBody.transform.position.y < -10.0f))
            {
                DestroyReferenceCube();
                canRotate = false;
                if (isBlockWithBlockHit())
                {
                    canRotate = true;
                    if (GameManager.shownAd == false)
                    {       
                        if(Advertisement.IsReady("rewardedVideo"))
                            FindObjectOfType<GameManager>().AdUI();
                        else
                        {
                            Debug.Log("actual game over");
                            FindObjectOfType<GameManager>().GameOver();
                        }
                    }
                    else
                    {
                        Debug.Log("actual game over");
                        FindObjectOfType<GameManager>().GameOver();

                    }
                }
            }
        }
    }

    public void Reset()
    {
        for(int i = 0; i < Board.Count; i++)
            Destroy(Board[i]);
        Board = new List<GameObject>();
        for (int i = 0; i < BlockSet.Count; i++)
            Destroy(BlockSet[i]);
        BlockSet = new List<GameObject>();
        score = 0;
        speed = 1f;
        speedFlag = 0.05f;
        threshold = 0;
        BlockBody.transform.position = Vector3.zero;
        canRotate = true;
    }

    public void ResetInputControls()
    {
        if (FastMotionFlag)
        {
            speed = 1 + speedFlag;
            FastMotionFlag = false;
        }
    }
    bool isBlockWithBlockHit()
    {
        for (int i = 0; i < Board.Count; i++)
        {
            for (int j = 0; j < BlockSet.Count; j++)
            {
                if (Board[i].transform.position.y > 2 && BlockBody.transform.position.y < -16.51f)
                {
                    if (Mathf.Abs(Board[i].transform.position.x - BlockSet[j].transform.position.x) < 0.5 && Mathf.Abs(Board[i].transform.position.z - BlockSet[j].transform.position.z) < 0.5)
                    {

                        BlockBody.transform.position = new Vector3(BlockBody.transform.position.x, -16.5f, BlockBody.transform.position.z);
                        FindObjectOfType<AudioManager>().Play("BlockHitWithBlock");
                        if (!FastMotionFlag)
                            BoardAnimation("AnimationAtBlockDrop");
                        else
                            BoardAnimation("AnimationAtBlockDropFast");
                        return true;
                    }
                }
            }
        }
        return false;
    }

    bool isReached()
    {
        if (BlockBody.transform.position.y < -17.4f)
        {
            speedFlag += 0.05f;
            FindObjectOfType<AudioManager>().Play("BlockHitDown");
            BoardBody.transform.position = new Vector3(0, 0, 0);
            BlockBody.transform.position = new Vector3(BlockBody.transform.position.x, -17.5f, BlockBody.transform.position.z);
            for (int i = 0; i < BlockSet.Count; i++)
            {
                Destroy(BlockSet[i].GetComponent<Collider>());
                Destroy(BlockSet[i].GetComponent<TrailRenderer>());
                BlockSet[i].transform.parent = BoardBody.transform;
                Board.Add(BlockSet[i]);
            }
            BlockBody.transform.position = Vector3.zero;

            int count = 0;
            count += CheckFaceBlast();
            count += CheckSpecialBlast(BlockSet[0].tag);
            CheckAllBlast();
            LogMessage.Log(GetMessage(count));
            if(count > 0 && GetMessage(count) != "")
                LogMessage.Log( "+" + (count * 100).ToString());

            BottomCollisinTrigger.reachedNoRotationZone = false;
            score += 100;
            scoreAnimator.Play("ScoreUpAnimation", -1, 0f);
            if (!FastMotionFlag)
            {
                BoardAnimation("AnimationAtBlockDrop");
                Debug.Log("playing animation --> 1");
            }
            else
            {
                BoardAnimation("AnimationAtBlockDropFast");
                Debug.Log("playing animation --> 2");
            }
           
            CheckIfSpecialCubeFlagActivation();
            CreateBlockSet();
            return true;
        }
        else
            return false;
    }

    //funtions handeling the reference block creation
    void CreateReferenceCube()
    {
        ReferenCecreated = true;
        Vector3 position = new Vector3();
        float y = 2.5f;
        bool flag = false;
        for (int i = 0; i < BlockSet.Count; i++)
        {
            for (int j = 0; j < Board.Count; j++)
            {
                y = 2.5f;
                if (Mathf.Abs(Board[j].transform.position.x - BlockSet[i].transform.position.x) < 0.5 && Mathf.Abs(Board[j].transform.position.z - BlockSet[i].transform.position.z) < 0.5 && Board[j].transform.position.y > 0)
                {
                    y = 3.5f;
                    flag = true;
                    break;
                }
            }
            position = new Vector3(BlockSet[i].transform.position.x, y, BlockSet[i].transform.position.z);
            ReferenceBlock.Add(Instantiate(ReferenceCubePrefab, position, Quaternion.identity, BoardBody.transform));
            if (y == 2.5f)
                ReferenceBlock[ReferenceBlock.Count-1].GetComponent<Renderer>().material = CanPlaceReferenceMaterial;
            else
                ReferenceBlock[ReferenceBlock.Count - 1].GetComponent<Renderer>().material = CanNotPlaceReferenceMaterial;
        }
        if(flag)
        {
            for(int i = 0; i<ReferenceBlock.Count; i++)
            {
                ReferenceBlock[i].transform.position = new Vector3(ReferenceBlock[i].transform.position.x, 3.5f, ReferenceBlock[i].transform.position.z);

                ReferenceBlock[i].GetComponent<Renderer>().material = CanNotPlaceReferenceMaterial;
            }
        }
    }
    void DestroyReferenceCube()
    {
        for (int i = 0; i < ReferenceBlock.Count; i++)
        {
            Destroy(ReferenceBlock[i]);
        }
        ReferenceBlock = new List<GameObject>();
        ReferenCecreated = false;
    }

    //funtions to remove the blocks when group formation
    int CheckSpecialBlast(string MagicTag)
    {
        int count = 0;
        switch (MagicTag)
        {
            case "Magic01":
                for (int i = 0; i< Board.Count;i++)
                {
                    if (Board[i].transform.position.y > 2.4)
                    {
                        toDestroyBlocks.Add(Board[i]);
                        count++;
                    }
                }
                break;
            case "Magic02":
                int count1 = 0;
                int count2 = 0;
                int count3 = 0;
                int count4 = 0;
                int count5 = 0;
                List<string> tag = new List<string>();
                for(int i = 0; i< Board.Count; i++)
                {
                    if(Board[i].transform.position.y > 2.4)
                    {
                        switch(Board[i].tag)
                        {
                            case "Blue":
                                count1++;
                                break;
                            case "Green":
                                count2++;
                                break;
                            case "Yellow":
                                count3++;
                                break;
                            case "Red":
                                count4++;
                                break;
                            case "Violet":
                                count5++;
                                break;
                        }
                    }
                }

                if (count1 + count2 + count3 + count4 + count5 != 0)
                {
                    if (count1 >= count2 && count1 >= count3 && count1 >= count4 && count1 >= count5)
                        tag.Add("Blue");
                    if (count2 >= count1 && count2 >= count3 && count2 >= count4 && count2 >= count5)
                        tag.Add("Green");
                    if (count3 >= count1 && count3 >= count2 && count3 >= count4 && count3 >= count5)
                        tag.Add("Yellow");
                    if (count4 >= count1 && count4 >= count2 && count4 >= count3 && count4 >= count5)
                        tag.Add("Red");
                    if (count5 >= count1 && count5 >= count2 && count5 >= count3 && count5 >= count4)
                        tag.Add("Violet");
                    
                    
                    for (int i = 0; i < Board.Count; i++)
                    {
                        for(int j = 0; j < tag.Count; j++)
                        {
                            if (Board[i].CompareTag(tag[j]) || Board[i].CompareTag("Magic02"))
                            {
                                toDestroyBlocks.Add(Board[i]);
                                count++;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Board.Count; i++)
                    {
                        if (Board[i].CompareTag("Magic02"))
                        {
                            toDestroyBlocks.Add(Board[i]);
                            count++;
                        }
                    }
                }
                break;
            case "Magic03":
                for (int i = 0; i < Board.Count; i++)
                {
                    
                    toDestroyBlocks.Add(Board[i]);
                    count++;
                }
                break;
        }
        return count;
    }   //checks if a special type of formation is formed and retures a list of gameObjects that are to be blasted 
    int CheckFaceBlast()
    {
        int count = 0;
        List<GameObject> tempList = new List<GameObject>();
        for (int i = 0; i <Board.Count;i++)
        {
            if(Board[i].transform.position.y > 2.4f)
            {
                count++;
                tempList.Add(Board[i]);
            }
        }
        if(count == 16)
        {
            score += 500;
            for(int i = 0; i< tempList.Count; i++)
            {
                toDestroyBlocks.Add(tempList[i]);
            }
        }
        for (int i = 0; i < tempList.Count; i++)
        {
            tempList.Remove(tempList[i]);
        }
        if (count == 16)
            return count;
        else
            return 0;
    }   //checks if a complete row is occupied and retures a list of gameObjects that are to be blasted
    void CheckAllBlast()
    {
        if (Board.Count == 0 && score != 0 && !AllBlastFlag)
        {
            if (score < 1000)
            {
                score += 300;
            }
            else if (score < 2500)
            {
                score += 600;
            }
            else if (score < 5000)
            {
                score += 900;
            }
            else if (score < 10000)
            {
                score += 1500;
            }

            else if (score < 20000)
            {
                score += 2600;
            }
            else if (score < 40000)
            {
                score += 3700;
            }
            else
            {
                score += 60000;
            }
            AllBlastFlag = true;
        }
    }
    void DestroyToBeDestroyedBlocks()
    {
        int count = toDestroyBlocks.Count;
        for(int i = 0; i<toDestroyBlocks.Count;i++)
        {
            Board.Remove(toDestroyBlocks[i]);
            if (i % 2 == 0)
            {
                scoreAnimator.Play("ScoreUpAnimation", -1, 0f);
                score += 100;
                scoreText.text = score.ToString();
            }
            if (toDestroyBlocks[i].gameObject != null)
            {
                toDestroyBlocks[i].GetComponent<Explode>().explode(toDestroyBlocks[i].GetComponent<Renderer>().material);
            }
            toDestroyBlocks.Remove(toDestroyBlocks[i]);
        }

    }
    
    public void CreateBlockSet(int index = -1)
    {   //TODO: add rotation and position change to the newly creaed blockset 

        FastMotionFlag = false;
        scoreText.text = score.ToString();
        speed = 1 + speedFlag;
        BottomRotationTriggerTransform.transform.position = new Vector3(0, 4.5f, 0);
        BlockSet = new List<GameObject>();
        List<Vector3> BlockSetArray;
        if(index == -1)
            BlockSetArray = BlockData.GetBlock();
        else
        {
            BlockSetArray = BlockData.GetBlock(index);
        }
        string tag = "";
        BlockType type = new BlockType();

        bool flag = false;
        if (specialCubeFlag)
        {
            specialCubeFlag = false;
            flag = true;
            type = SelectSpecialTag();
        }
        else
            type = (BlockType)Random.Range(0, (int)BlockType.Count - 3);
        
        switch (type)   //picking a random type of block 
        {
            case BlockType.Blue:
                tag = "Blue";
                break;
            case BlockType.Green:
                tag = "Green";
                break;
            case BlockType.Yellow:
                tag = "Yellow";
                break;
            case BlockType.Red:
                tag = "Red";
                break;
            case BlockType.Violet:
                tag = "Violet";
                break;
            case BlockType.Magic01:
                tag = "Magic01";
                break;
            case BlockType.Magic02:
                tag = "Magic02";
                break;
            case BlockType.Magic03:
                tag = "Magic03";
                break;            
        }
        for (int i = 0; i < BlockSetArray.Count; i++)
        {
            if (!flag)
            {
                BlockSet.Add(Instantiate(blockPrefab, BlockSetArray[i], Quaternion.identity, BlockBody.transform));
                BlockSet[i].GetComponent<Renderer>().material = BlockMaterials[(int)type];
                BlockSet[i].GetComponent<TrailRenderer>().colorGradient = ColorGradioant[(int) type];

            }
            else
            {
                switch (tag)
                {
                    case "Magic01":
                        BlockSet.Add(Instantiate(MagicCube01, BlockSetArray[i], Quaternion.identity, BlockBody.transform));
                        break;
                    case "Magic02":
                        BlockSet.Add(Instantiate(MagicCube02, BlockSetArray[i], Quaternion.identity, BlockBody.transform));
                        break;
                    case "Magic03":
                        BlockSet.Add(Instantiate(MagicCube03, BlockSetArray[i], Quaternion.identity, BlockBody.transform));
                        break;
                }
            }
            BlockSet[i].tag = tag;
            AllBlastFlag = false;

        }   // initializing the block set through itrating with values 
        canRotate = true;
    }      //creates a set of blocks when the older set has reached the body
    
    BlockType SelectSpecialTag()
    {
        BlockType type;
        int r = Random.Range(0, 100);
        if (r % 10 == 0)
            type = BlockType.Magic03;
        else if (r % 3 == 0)
            type = BlockType.Magic02;
        else
            type = BlockType.Magic01;
        return type;
    }

    void CheckIfSpecialCubeFlagActivation()
    {
        if(score < 1000)
        {
            threshold += 30;
        }
        else if(score < 2500)
        {
            threshold += 40;
        }
        else if (score < 5000)
        {
            threshold += 50;
        }
        else if (score < 10000)
        {
            threshold += 58;
        }

        else if (score < 20000)
        {
            threshold += 64;
        }
        else if (score < 40000)
        {
            threshold += 68;
        }
        else
        {
            threshold += 70;
        }
        if(threshold >= 350)
        {
            specialCubeFlag = true;
            threshold = 0;
        }
    }   //sets the specialCubeFlag to true if the threshold value is attained


    // funtions to drag the block
    int MoveRight()
    {
        //checking if movement is possible 
        for(int i = 0; i< BlockSet.Count; i++)
        {
            if(BlockSet[i].transform.position.z < -1.4)
            {
                return 0;
            }
            if (BottomCollisinTrigger.reachedNoRotationZone)
            {
                for(int j = 0; j<Board.Count;j++)
                {
                    if(Board[j].transform.position.y > 1.4f)
                    {
                        if(Mathf.Abs(Board[j].transform.position.x - BlockSet[i].transform.position.x)<0.1f && Mathf.Abs(Board[j].transform.position.z - (BlockSet[i].transform.position.z-1)) < 0.1f)
                        {
                            return 0;
                        }
                    }
                }
            }
        }

        //making the movement
        for (int i = 0;i<BlockSet.Count; i++)
        {
            BlockSet[i].transform.position = new Vector3(BlockSet[i].transform.position.x, BlockSet[i].transform.position.y, BlockSet[i].transform.position.z - 1);
        }
        return 1;
    }
    int MoveLeft()
    {
        //checking if movement is possible 
        for (int i = 0; i < BlockSet.Count; i++)
        {
            if (BlockSet[i].transform.position.z > 1.4)
            {
                return 0;
            }
            if (BottomCollisinTrigger.reachedNoRotationZone)
            {
                for (int j = 0; j < Board.Count; j++)
                {
                    if (Board[j].transform.position.y > 1.4f)
                    {
                        if (Mathf.Abs(Board[j].transform.position.x - BlockSet[i].transform.position.x) < 0.1f && Mathf.Abs(Board[j].transform.position.z - (BlockSet[i].transform.position.z + 1)) < 0.1f)
                        {
                            return 0;
                        }
                    }
                }
            }
        }

        //making the movement
        for (int i = 0; i < BlockSet.Count; i++)
        {
            BlockSet[i].transform.position = new Vector3(BlockSet[i].transform.position.x, BlockSet[i].transform.position.y, BlockSet[i].transform.position.z + 1);
        }
        return 1;
    }
    int MoveForward()
    {
        //checking if movement is possible 
        for (int i = 0; i < BlockSet.Count; i++)
        {
            if (BlockSet[i].transform.position.x  > 1.4)
            {
                return 0;
            }
            if (BottomCollisinTrigger.reachedNoRotationZone)
            {
                for (int j = 0; j < Board.Count; j++)
                {
                    if (Board[j].transform.position.y > 1.4f)
                    {
                        if (Mathf.Abs(Board[j].transform.position.x - (BlockSet[i].transform.position.x + 1)) < 0.1f && Mathf.Abs(Board[j].transform.position.z - BlockSet[i].transform.position.z) < 0.1f)
                        {
                            return 0;
                        }
                    }
                }
            }
        }
        //making the movement
        for (int i = 0; i < BlockSet.Count; i++)
        {
            BlockSet[i].transform.position = new Vector3(BlockSet[i].transform.position.x + 1, BlockSet[i].transform.position.y, BlockSet[i].transform.position.z);
        }
        return 1;

    }
    int MoveBack()
    {
        //checking if movement is possible 
        for (int i = 0; i < BlockSet.Count; i++)
        {
            if (BlockSet[i].transform.position.x < -1.4)
            {
                return 0;
            }
            if (BottomCollisinTrigger.reachedNoRotationZone)
            {
                for (int j = 0; j < Board.Count; j++)
                {
                    if (Board[j].transform.position.y > 1.4f)
                    {
                        if (Mathf.Abs(Board[j].transform.position.x - (BlockSet[i].transform.position.x - 1)) < 0.1f && Mathf.Abs(Board[j].transform.position.z - BlockSet[i].transform.position.z) < 0.1f)
                        {
                            return 0;
                        }
                    }
                }
            }
        }
        //making the movement
        for (int i = 0; i < BlockSet.Count; i++)
        {
            BlockSet[i].transform.position = new Vector3(BlockSet[i].transform.position.x - 1, BlockSet[i].transform.position.y, BlockSet[i].transform.position.z);
        }
        return 1;

    }

    string GetMessage(int count)
    {
        string message = "";
        List<string> messageList = new List<string>();
        if(count > 24)
        {
            messageList = new List<string>
            {
                "Fabulous!!",
                "Astonishing!!",
                "Gorgeous!!",
                "Epic!!",
                
            };
        }
        else if(count > 18)
        {
            messageList = new List<string>
            {
                "Unbelievable!!",
                "Breathtaking!!",
                "Incredible!!",

            };
        }
        else if(count > 15)
        {
            messageList = new List<string>
            {
                "Amazing!",
                "Fantastic!",
                "Incredible!",
            };
        }
        else if(count > 10)
        {
            messageList = new List<string>
            {
                "Staggering!",
                "Splendid!",
                "Stunning!",
            };
        }
        else if(count > 6)
        {
            messageList = new List<string>
            {
                "Sweet",
                "Cool",
                "Good",
                "Nice",
                "Bravo",
                "Neat",
            };
        }
        if (count > 6)
        {
            int r = Random.Range(0, messageList.Count);
            message = messageList[r];
        }
        else
            message = "";
        return (message);
    }       //generates the messages to be shown on the screen during block breaking 

    public void ExtraLife()
    {
        for (int i = 0; i < BlockSet.Count; i++)
        {
            //Destroy(BlockSet[i]);
            BlockSet[i].GetComponent<Explode>().explode(BlockSet[i].GetComponent<Renderer>().material);
        }
        BlockBody.transform.position = Vector3.zero;
        BottomCollisinTrigger.reachedNoRotationZone = false;
        CreateBlockSet();
    }
    
    public static void BoardAnimation(string animaiton)
    {
        Debug.Log("here inside animaiton");
        BodyAnimation.Play(animaiton, -1, 0f);
    }
}