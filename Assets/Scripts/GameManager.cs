using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum Shape
{
    RECTANGLE = 0,
    TRAPEZIUM = 2,
    TRIANGLE = 1,
    CYLINDER = 3
}
public class Container
{
    public GameObject gameObject;       //related game object
    public Shape shape;                 //shape name
    float minimumHeight;                
    float maximumHeight;
    float minimumWidth;
    float maximumWidth;
    public float actualHeightRatio, actualWidthRatio;  //actual ratios of given object / smaller than 1
    public float height, width,area;            //generated height, width, area
    public Container(GameObject gameObject,Shape shape,float maxH,float minH,float maxW, float minW,float ahr=1f, float awr=1f)
    {
        this.gameObject = gameObject;
        this.shape = shape;
        minimumHeight = minH;
        maximumHeight = maxH;
        minimumWidth = minW;
        maximumWidth = maxW;
        actualHeightRatio = ahr;
        actualWidthRatio = awr;
    }
    public void GenerateShape()
    {
        height = Random.Range(minimumHeight, maximumHeight);
        width = Random.Range(minimumWidth, maximumWidth) ;

        gameObject.GetComponent<RectTransform>().localScale = new Vector3(width, height, 1f);

        width *= gameObject.GetComponent<RectTransform>().sizeDelta.x * actualWidthRatio;
        height *= gameObject.GetComponent<RectTransform>().sizeDelta.y * actualHeightRatio;

        //Debug.Log(gameObject.GetComponent<RectTransform>().sizeDelta);

        gameObject.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;

        area = Area();

    }
    float Area()
    {
        float containerMaxArea;
        switch (shape)
        {
            case Shape.RECTANGLE:
                containerMaxArea = height * width;
                break;
            case Shape.TRIANGLE:
                containerMaxArea = height * width * 0.5f;
                break;
            case Shape.TRAPEZIUM:
                containerMaxArea = height * (1.412f * width) * 0.5f;
                break;
            case Shape.CYLINDER:
                containerMaxArea = height * width + (width * width) * (Mathf.PI / 8f - 1f / 2f);
                break;
            default: containerMaxArea = 0f;
                break;
        }
        //Debug.Log("Area is " + containerMaxArea +" of shape "+shape);
        return containerMaxArea;
    }
    public void SetFillLevel(float filled)
    {
        float fill=0;
        switch (shape)
        {
            case Shape.RECTANGLE:
                fill = filled / area;
                break;
            case Shape.CYLINDER:
                fill = filled / area;
                break;
            case Shape.TRIANGLE:
                fill = Mathf.Sqrt(filled / area);
                break;
            case Shape.TRAPEZIUM:
                fill = ((filled / area) * 2.427f + Mathf.Sqrt(filled / area)) / 3.427f ;//(filled / area + Mathf.Sqrt(filled / area))/2;
                break;
        }
        gameObject.transform.GetChild(0).GetComponent<Image>().fillAmount = fill;
    }
    public float GetFillLevel()
    {
        return gameObject.transform.GetChild(0).GetComponent<Image>().fillAmount;
    }

    public float GetFallLevel()
    {
        Debug.Log(GetFillLevel() * height);
        return GetFillLevel() * height ;
    }
    public Image GetHighSpill()
    {
        return gameObject.transform.GetChild(2).GetComponent<Image>();
    }
    public Image GetLowSpill()
    {
        return gameObject.transform.GetChild(3).GetComponent<Image>();
    }

}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    // Use this for initialization
    public int highestLevelReached,highestScoreReached;
    public const float FRAME_DURATION = 0.03f;          // (time in seconds)duration between each fill
    public const float PER_SECOND_FILL = 3000;          // value filled at each duration
    private const float MAX_FILL_VOLUME = 0.85f;
    private Vector4 orange = new Vector4(1f, 0.69f, 0.15f, 1f);     //default color to set to


    public const float COMPOSITION_MARGIN = 0.06f;      //6 points on both sides
    public const float GOOD_COMPOSITION_MARGIN = 0.02f; //2 points on both sides 
    public const float MINIMUM_FILL_REQUIRED = 0.8f;    //Container should be filled 80 percent    
    public const float GOOD_FILL_MARGIN_BELOW = 0.04f;  //4 points from below
    public const float FILL_MARGIN = 0.02f;             //2 points from below //upper than up belower than below
    public const float PITCHER_SPEED=1.1f;                //pitcher speed modifier
    private float m_Saturation, m_Hue, m_Value;         //default hsv to set to

    public GameObject hotPitcher, coldPitcher;
    public GameObject rectangleGlass, trapeziumGlass, triangleGlass, cylinderGlass;
    public AudioClip pouringWater, goodSound, badSound;
    public GameObject linePrime, lineNadir;

    Container[] allGlass;
    Container rectangle, triangle, trapezium, cylinder;

    float containerComposition;

    public bool isRunning;
    public Container currentContainer;


    public bool firstTime, soundPlaying=false;
    public int hotValue, coldValue;
    public int counter;
    public bool isStarted;
    
    public string endMessage;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        if (PlayerPrefs.HasKey("HighestLevelReached"))
        {
            highestLevelReached = PlayerPrefs.GetInt("HighestLevelReached");
            highestScoreReached = PlayerPrefs.GetInt("HighestScoreReached");
        }
        else
        {
            PlayerPrefs.SetInt("HighestLevelReached", 0);
            PlayerPrefs.SetInt("HighestScoreReached", 0);
        }
        
        if (PlayerPrefs.GetInt("SaveLater") == 1)
        {
            PlayerPrefs.SetInt("SaveLater",0);
    //        LeaderBoard leaderBoard = new LeaderBoard();
    //        leaderBoard.Post(PlayerPrefs.GetInt("HighestScoreReached"));
        }
    }

    public void Save()
    {
        PlayerPrefs.SetInt("HighestLevelReached", highestLevelReached);
        PlayerPrefs.SetInt("HighestScoreReached", highestScoreReached);
    }

    public int levelNo,score;
    
    public void Start()
    {

        //need more calibration
        rectangle = new Container(rectangleGlass,Shape.RECTANGLE, 1.2f, 0.6f, 1.2f, 0.65f,0.812f,0.794f);
        trapezium = new Container(trapeziumGlass,Shape.TRAPEZIUM, 1.3f, 0.65f, 1.2f, 0.7f,0.9f,0.9f);
        triangle = new Container(triangleGlass,Shape.TRIANGLE, 1.5f, 0.7f, 1.5f, 0.7f,0.375f);
        cylinder = new Container(cylinderGlass,Shape.CYLINDER, 1.2f, 0.6f, 1.2f, 0.8f,0.466f);

        allGlass = new Container[] { rectangle, trapezium, triangle, cylinder };

        Color.RGBToHSV(rectangleGlass.transform.Find("Fill").GetComponent<Image>().color, out m_Hue, out m_Saturation, out m_Value);
        InvokeRepeating("MyUpdate", FRAME_DURATION, FRAME_DURATION);

        Input.simulateMouseWithTouches = true;
   //     StartGame();
    }

    public void StartGame()
    {
        levelNo = score = 0;
        soundPlaying = false;
        UIController.Instance.levelText.text = "0";
        NextLevel();
        isRunning = false;
        Invoke("WaitFirstTime", 1f);
        //UIController.Instance.MessagePopUp("bellow");
    }
    public void WaitFirstTime()
    {
        isRunning = true;
    }
    public void NextLevel()
    {
        /*int counter=0;
        float avg=0f;
        while (counter < 100)
        {
            currentContainer = allGlass[Random.Range(0, 4)];
            currentContainer.GenerateShape();
            avg += currentContainer.area;
            counter++;
        }
        print("Average is " + avg / counter);
        */

        isRunning = true;

        hotPitcher.GetComponent<Pitcher>().SetVal(0f);
        coldPitcher.GetComponent<Pitcher>().SetVal(0f);
        hotPitcher.transform.parent.gameObject.SetActive(true);
        coldPitcher.transform.parent.gameObject.SetActive(false);

        firstTime = isStarted = false;
        counter = 0;

        levelNo++;
        //assining probability for selection of glasses
        float rand = Random.Range(0f, 1f);
        if (rand < 0.35f)
        {
            currentContainer = allGlass[0];
        }
        else if (rand < 0.6f)
        {
            currentContainer = allGlass[1];
        }
        else if (rand < 0.82f)
        {
            currentContainer = allGlass[2];
        }
        else
        {
            currentContainer = allGlass[3];
        }
        //if level is below 4 generate rectangle glass
        if (levelNo < 4)
        {
            currentContainer = allGlass[0];
        }

        currentContainer.GenerateShape();
       
        foreach(Container container in allGlass)
        {
            if(container == currentContainer)
            {
                container.gameObject.SetActive(true);
                container.gameObject.transform.GetChild(2).gameObject.SetActive(false);
                container.gameObject.transform.GetChild(3).gameObject.SetActive(false);
                container.gameObject.transform.GetChild(4).gameObject.SetActive(false);

            }
            else
            {
                container.gameObject.SetActive(false);
            }
        }

        currentContainer.gameObject.transform.Find("Fill").GetComponent<Image>().color = orange;
        currentContainer.gameObject.transform.Find("Fill").GetComponent<Image>().color = Color.HSVToRGB(m_Hue, m_Saturation, m_Value);

        firstTime = true;
        containerComposition = Random.Range(4, 17) * 5f/100f;
        UIController.Instance.compositionValText.text = (containerComposition * 100).ToString() + "%";
        print("This is level " + levelNo + ". Composition required is " + containerComposition);
        print("Composition Area " + currentContainer.area);

    }

    public void EndLevel()
    {
        isRunning = false;
        coldPitcher.transform.parent.gameObject.SetActive(false);
        hotPitcher.transform.parent.gameObject.SetActive(false);
        hotPitcher.transform.parent.GetComponent<FallAnimation>().StopAnimation();
        coldPitcher.transform.parent.GetComponent<FallAnimation>().StopAnimation();

        currentContainer.GetFallLevel();
        print("HotLevel " + hotValue + "\t ColdLevel" + coldValue);
        float madeComposition = (float)hotValue / (hotValue + coldValue);
        float madeFill = (hotValue + coldValue) * FRAME_DURATION * PER_SECOND_FILL;
        //UIController.Instance.callThisBitch(currentContainer, madeFill / currentContainer.area);
        int levelScore = 0;
        //passes the level if within the fill margin and composition margin
        if (madeFill < (MINIMUM_FILL_REQUIRED - FILL_MARGIN) * currentContainer.area || madeFill > (1f + FILL_MARGIN) * currentContainer.area ||
            madeComposition < containerComposition - COMPOSITION_MARGIN || madeComposition > containerComposition + COMPOSITION_MARGIN)
        {

            print("Container Area is " + currentContainer.area + " made fill is " + madeFill);

            print("Container composition is " + containerComposition + " Made composition" + madeComposition);

            if (madeFill < (MINIMUM_FILL_REQUIRED - FILL_MARGIN) * currentContainer.area)
            {
                print((MINIMUM_FILL_REQUIRED - FILL_MARGIN) * currentContainer.area);
                endMessage = "UnderFill";
            }
            else if (madeFill > (1f + FILL_MARGIN) * currentContainer.area)
            {
                endMessage = "OverFill";
                if (madeFill > (1f + 0.05f) * currentContainer.area) {
                    currentContainer.GetHighSpill().GetComponent<FillAnimation>().Animate();
                    currentContainer.gameObject.transform.GetChild(4).GetComponent<Image>().color = currentContainer.gameObject.transform.Find("Fill").GetComponent<Image>().color;
                    currentContainer.gameObject.transform.GetChild(4).gameObject.SetActive(true);
                    currentContainer.GetHighSpill().GetComponent<Image>().color = currentContainer.gameObject.transform.Find("Fill").GetComponent<Image>().color;
                }
                else
                {
                    currentContainer.gameObject.transform.GetChild(4).gameObject.SetActive(true);
                    currentContainer.gameObject.transform.GetChild(4).GetComponent<Image>().color = currentContainer.gameObject.transform.Find("Fill").GetComponent<Image>().color;
                    currentContainer.GetLowSpill().GetComponent<FillAnimation>().Animate();
                    currentContainer.GetLowSpill().GetComponent<Image>().color = currentContainer.gameObject.transform.Find("Fill").GetComponent<Image>().color;
                }
            }
            else if (madeComposition < containerComposition - COMPOSITION_MARGIN)
            {
                endMessage = "Too Cold";
            }
            else if (madeComposition > containerComposition + COMPOSITION_MARGIN)
            {
                endMessage = "Too Hot";
            }
            else {
                endMessage = "Dont Know Why";
                currentContainer.gameObject.transform.GetChild(2).gameObject.SetActive(false);
                currentContainer.gameObject.transform.GetChild(3).gameObject.SetActive(false);
                currentContainer.gameObject.transform.GetChild(4).gameObject.SetActive(false);

            }
            if (score > highestScoreReached)
            {
                endMessage += "\n <color=#ff0000ff>HIGH SCORE BROKEN!!</color>";
            }
            UIController.Instance.goMessage.text = endMessage;
            print(endMessage);

            EndGame();
        }
        else
        {
            levelScore++;
            if (madeComposition < containerComposition + GOOD_COMPOSITION_MARGIN && madeComposition > containerComposition - GOOD_COMPOSITION_MARGIN)
            {
                levelScore++;
            }
            if (madeFill > (1 - GOOD_FILL_MARGIN_BELOW) * currentContainer.area && madeFill < (1 + FILL_MARGIN) * currentContainer.area)
            {
                levelScore++;
            }
            //bro print rewarding messages from here
            if (levelScore == 1)
            {
                print("Good");
                endMessage = "Good";
            }else if (levelScore == 2)
            {
                print("Great");
                endMessage = "Great";
            }
            else if(levelScore==3)
            {
                print("Awesome");
                endMessage = "Awesome";
            }
            UIController.Instance.MessagePopUp(endMessage);
            if (UIController.Instance.soundState == 1)
                UIController.Instance.soundSystem.GetComponent<AudioSource>().PlayOneShot(goodSound);
            score += levelScore;
            StartCoroutine(PauseNextLevel());
            hotPitcher.SetActive(true);
        }

        float hPrime = currentContainer.height * madeFill;
        GameObject prLine = Instantiate(linePrime, currentContainer.gameObject.transform.position, Quaternion.identity);
        prLine.transform.SetParent(currentContainer.gameObject.transform);
        prLine.transform.localPosition= Vector3.zero;
        prLine.transform.localPosition = prLine.transform.localPosition + new Vector3(0f,hPrime,0f);
 

        //  Destroy(prLine, 2f);

        //UIController.Instance.messagePopUp("Testes");

    }
    public IEnumerator PauseNextLevel()
    {
        UIController.Instance.deetsPanel.GetComponent<Animation>().Play("scoreIncrease");
        UIController.Instance.levelText.text = (score).ToString();
        yield return new WaitForSeconds(1);
        NextLevel();
    }

    public void EndGame()
    {
        PourOff();
        Handheld.Vibrate();
        if(UIController.Instance.soundState==1)
            UIController.Instance.soundSystem.GetComponent<AudioSource>().PlayOneShot(badSound);
        print("Game is Finished. Your score is " + levelNo);

        if (levelNo > highestLevelReached)
        {
            highestLevelReached = levelNo;
            Save();
        }
        //if highest score reached. Save highest score
        //use score value to show at the top
        if(score > highestScoreReached)
        {
     //       LeaderBoard leaderBoard = new LeaderBoard();
     //       leaderBoard.Post(score);
            highestScoreReached = score;
            Save();
            UIController.Instance.UpdateHighScore(score);
        }
        //  StopCoroutine(MyUpdate());
        //StopAllCoroutines();
        UIController.Instance.homePanel.GetComponent<Animation>().Play("gameOver");
        UIController.Instance.gameElements.GetComponent<Animation>().Play("gamesPanelAnimationGameToHome1");
    }

    public GameObject PitcherParent;
    public void MyUpdate()
    {
        if (isRunning)
        {
            if (Input.GetMouseButton(0))
            //if (Input.touchCount > 0)
            {
                //isStarted is use to count the first register //otherwise falls directly 
                isStarted = true;
                counter++;
                if (firstTime)
                {
                    if (!hotPitcher.transform.parent.GetComponent<FallAnimation>().isRunning)
                    {
                        hotPitcher.transform.parent.GetComponent<FallAnimation>().StartAnimation();
                    }
                    PourOn();
                    hotPitcher.GetComponent<Pitcher>().SetVal((float)counter * FRAME_DURATION * PER_SECOND_FILL * PITCHER_SPEED);
                    currentContainer.SetFillLevel((float)counter * FRAME_DURATION * PER_SECOND_FILL);
                    //print(counter);
                    if ((float)counter * FRAME_DURATION * PER_SECOND_FILL > 1.10f*currentContainer.area)
                    {
                        hotValue = counter;
                        PourOff();
                        EndLevel();
                    }
                }
                else
                {
                    if (!coldPitcher.transform.parent.GetComponent<FallAnimation>().isRunning)
                    {
                        coldPitcher.transform.parent.GetComponent<FallAnimation>().StartAnimation();
                    }
                   
                    PourOn();
                    coldPitcher.GetComponent<Pitcher>().SetVal((float)counter * FRAME_DURATION * PER_SECOND_FILL * PITCHER_SPEED);
                    currentContainer.SetFillLevel((float)(counter+hotValue) * FRAME_DURATION * PER_SECOND_FILL);
                    if ((float)(counter + hotValue) * FRAME_DURATION * PER_SECOND_FILL > 1.10f * currentContainer.area)
                    {
                        coldValue = counter;
                        PourOff();
                        EndLevel();
                    }
                    currentContainer.gameObject.transform.GetChild(0).GetComponent<glassColorController>().MoheRangDe(counter);

                }
                //print(counter);
            }
            else
            {
                if(hotPitcher.transform.parent.GetComponent<FallAnimation>().isRunning)
                {
                    hotPitcher.transform.parent.GetComponent<FallAnimation>().StopAnimation();
                }
                if (coldPitcher.transform.parent.GetComponent<FallAnimation>().isRunning)
                {
                    coldPitcher.transform.parent.GetComponent<FallAnimation>().StopAnimation();
                }
                if (isStarted)
                {
                    if (firstTime)
                    {
                        PourOff();
                        firstTime = false;
                        hotValue = counter;
                        hotPitcher.transform.parent.gameObject.SetActive(false);
                        coldPitcher.transform.parent.gameObject.SetActive(true);
                        currentContainer.gameObject.transform.GetChild(0).GetComponent<glassColorController>().GetHotting(hotValue);
                    }
                    else
                    {
                        PourOff();
                        firstTime = true;
                        coldValue = counter;
                        EndLevel();
                    }
                    counter = 0;
                    isStarted = false;
                }
                //change pitcher
            }
            
            //yield  return new WaitForSeconds(FRAME_DURATION);

        }
    }

    void PourOn()
    {
        if (UIController.Instance.soundState == 1 && !soundPlaying)
        {
            soundPlaying = true;
            UIController.Instance.effectSystem.GetComponent<AudioSource>().volume = MAX_FILL_VOLUME;
        }
    }

    void PourOff()
    {
        soundPlaying = false;
        UIController.Instance.effectSystem.GetComponent<AudioSource>().volume = 0f;
    }
}
