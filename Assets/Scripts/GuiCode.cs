/* GUI CODE SCRIPT FOR CC STARTER KIT v2.0.2.1
 * 
 * INSTRUCTIONS FOR USE
 * 1. Import GuiCode.cs, ScoreData.cs, Storage.cs, and CanvasCreatorButton.cs into your project
 * 2. Add the GuiCode script to an object in every scene of your project. This can be an empty GameObject or the camera.
 *      (avoid attaching this to an object that gets destroyed)
 * 3. Ensure that your scenes are added to the Build Settings Menu under Scenes in Build.
 * 4. a) Make sure that your first scene is the first listed in the build order,
 *    b) and that the last scene is your end scene (with your high scores and credits),
 *    c) make sure that your first games scene is the second listed scene
 * 5. Use the buttons to create the objects needed to work the starter kit.
 *      a) Create High Scores Display - Creates the textboxes responsible for displaying the scores,
 *         This only works if the scene is a high scores scene
 *      b) Create Scene Navigation - Creates two buttons for play and quit - these buttons can be modified
 *      c) Create Pause UI - Creates a PauseOverlay Canvas for player instructions and a sprite to tint the game screen.
 *         Items in the Pause UI are enabled and disabled by a list of GameObjects that can be adjusted in the inspector
 * 6. The first loaded scene must have the Is First Scene Toggle checked.
 * 7. Any scene that shows a high score must have the Is A High Scores Scene checked.
 * 8. If a scene shows the timer or the score display (not high scores, but a score) check the proper box.
 * 9. If the scene can be paused, check the Is a Pauseable Scene box.
 *      a) Any code the runs on update should be enclosed in an if statement that says if (!GuiCode.isPaused) {} this will prevent
 *         the code from running while the game is paused.
 * 10. The Uses Application Timer box should be checked. It will close your game after five minutes of inactivity.
 * 11. Adjust the Game Countdown and Game over display sliders if you are using the built in timer.
 * 12. Storage.Score is the global static variable for storing score.
 * 13. GuiCode.isGameOver is the global public static variable for stating that the game is over.
 *
 * FAQS:
 * q. I use my own navigation. What do I do?
 * a. You MUST use UI Buttons for the GuiCode to work properly. If you have your own means of navigation like a push any key
 *    then the following lines of code will be of use to you.
 *    GameObject CodeHolder = GameObject.Find("Main Camera");
 *    CodeHolder.GetComponent<GuiCode>().PlayGame();
 *    CodeHolder.GetComponent<GuiCode>().QuitGame();
 *    IF you do this, you will need to ensure that QuitGame is called. It is responsible for writing the high scores.
 *    
 * If you have your own Canvas Items, you can ignore the content creation buttons and use the fields in the script.
 * 
 */


using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
public class GuiCode : MonoBehaviour {

    #region "variables used"
    [Header("Pause UI and Settings")]
    public bool isAPauseableScene;
    [SerializeField] private KeyCode pauseKey = KeyCode.F1, quitKey = KeyCode.Escape;
    [SerializeField] List<GameObject> pauseUI;      //Overlay for the Pause Feature
    
    private int sIndex;                         //Index holder for new high score

    [Header("Scene Management")]
    public GameObject quitButton;
    public GameObject playButton;
    public static string startScene, gameScene, endScene;                      //game over screen name
    

    [Header("High Scores Display")]
    [SerializeField] private bool isAHighScoresScene = true;  //Have this checked if a high score display is present in the scene regardless of if you have objects or not
    private bool isFirstScene = true; //check this on title/first high score screen available
    [SerializeField] GameObject newScoreInput;   //Object reference for button
    Text newScoreName;
    public string defaultHighScoreName = "JBOB";
    public GameObject HighScoresNameDisplay, HighScoresScoreDisplay;
    private float displayYOffset, displayXOffset;
    [SerializeField] private bool usesScoreDisplay = true;
    [SerializeField] bool isLowerScoreBetter = false;
    [SerializeField] int defaultHighScoreValue = 0;

    [Header("Timers and Timeout")]
    [SerializeField] private bool usesApplicationTimeOut = true;
    [SerializeField] private bool usesInGameTimer = true;
    [Range(0, 240)] public int gameCountDownTime = 10;
    [SerializeField] private bool delaySceneLoadingOnGameOver = true;
    [Range(0, 10)] public int gameOverDisplayTime = 2;

    public static bool lockSceneNames = false;
    private float timeTicker;                   //Time tracker for timeout
    private bool alreadyChecked = false,        //Flag for checking if is a HighScore once
                 highScore = false;                     //Flag for if it is a highscore
    int displayTime;                            //Container for the displayed time
    string stringTime;                          //Temp space for displaying time
    string scenesData;
    #endregion
    string gameName;                            //GameName, populated by data folder
                //are high scores measured by low or high
    public static bool isPaused;                //IS THE GAME PAUSED?      
    public static bool isGameOver;
    public static float guiTime;                //Time the game runs for

    

    [SerializeField] private bool debugMode = false;
    Font displayfont;
    

    public bool DebugMode
    {
        get { return debugMode; }
    }

    public string DefaultHighScoreName
    {
        get { return defaultHighScoreName; }
    }

    public bool IsAHighScoresScreen
    {
        get { return isAHighScoresScene; }
        set { isAHighScoresScene = value; }
    }

    public bool IsItAPausableScene
    {
        get { return isAPauseableScene; }
        set { isAPauseableScene = value; }

    }

    // Use this for initialization
    void Start ()
    {
        string startS, gameS, endS;
       
        if (startScene == null)
        {
            startS = SceneUtility.GetScenePathByBuildIndex(0);
            startScene = startS.Substring(startS.LastIndexOf('/') + 1, startS.Length - startS.LastIndexOf('/') - 7);
            if (SceneManager.sceneCountInBuildSettings > 1)
            {
                gameS = SceneUtility.GetScenePathByBuildIndex(1);
                gameScene = gameS.Substring(gameS.LastIndexOf('/') + 1, gameS.Length - gameS.LastIndexOf('/') - 7);
                if (SceneManager.sceneCountInBuildSettings > 2)
                {
                    endS = SceneUtility.GetScenePathByBuildIndex(SceneManager.sceneCountInBuildSettings - 1);
                    endScene = endS.Substring(endS.LastIndexOf('/') + 1, endS.Length - endS.LastIndexOf('/') - 7);
                }
                else
                {
                    endS = SceneUtility.GetScenePathByBuildIndex(0);
                    endScene = endS.Substring(startS.LastIndexOf('/') + 1, startS.Length - startS.LastIndexOf('/') - 7);
                }
            }
            else
            {
                gameS = SceneUtility.GetScenePathByBuildIndex(0);
                gameScene = gameS.Substring(gameS.LastIndexOf('/') + 1, gameS.Length - gameS.LastIndexOf('/') - 7);
                endS = SceneUtility.GetScenePathByBuildIndex(0);
                endScene = endS.Substring(startS.LastIndexOf('/') + 1, startS.Length - startS.LastIndexOf('/') - 7);
            }



        }
        displayXOffset = 0;
        displayYOffset = 0;
        if(SceneManager.GetActiveScene().name == startScene)
        {
            isFirstScene = true;
        }
        else
        {
            isFirstScene = false;
        }


        if (defaultHighScoreName.Length > 4) defaultHighScoreName = defaultHighScoreName.Substring(0, 4);
        gameName = Application.productName;
        if ((isAHighScoresScene || usesScoreDisplay || usesInGameTimer) && displayfont == null)
        {
            displayfont = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }
        isGameOver = false;
        if(SceneManager.GetActiveScene().name == gameScene)
        {
            guiTime = gameCountDownTime;
            if (!usesInGameTimer && delaySceneLoadingOnGameOver)
                guiTime = 0;
        }
        #region "High Scores"

        // Since this script reloads every scene.
        if (Storage.highScoreData == null && isFirstScene)
        {
            Storage.highScoreData = new ScoresData(defaultHighScoreName);
            Storage.highScoreData.GameName = gameName;
            Storage.score = defaultHighScoreValue;
#if UNITY_STANDALONE
            WriteManagerIni();
#endif
#if UNITY_EDITOR
            WriteManagerIni();
#endif
            
            timeTicker = 0;
            
            Storage.highScoreData.ReadData();
           
        }

#if UNITY_EDITOR
        if (Storage.highScoreData == null)
        {
            Storage.highScoreData = new ScoresData(defaultHighScoreName);
            Storage.highScoreData.GameName = gameName; 
            Storage.highScoreData.ReadData();
        }
#endif

        if (isAHighScoresScene)
        {
            
            CreateHighScoresDisplay();
        }
        if (GameObject.Find("btn_Play") != null)
            GameObject.Find("btn_Play").GetComponent<Button>().onClick.AddListener(PlayGame);
        else if (playButton != null)
            playButton.GetComponent<Button>().onClick.AddListener(PlayGame);
        #endregion
    }

    private void WriteManagerIni()
    {
        string path = Storage.highScoreData.pathForDocumentsFile(gameName + "_Info.ini");
        if(!File.Exists(path))
        {
            StreamWriter sw;
            try
            {
                sw = new StreamWriter(path);
                sw.WriteLine("techTitle=" + gameName);
                sw.WriteLine("displayTitle=" + gameName);
                sw.WriteLine("authors=" + Application.companyName);
                sw.WriteLine("description=Your Description goes here");
                sw.WriteLine("played=0");
                sw.WriteLine("players=1");
                sw.WriteLine("favYear=");
                sw.WriteLine("year="+DateTime.Now.Year);
                sw.WriteLine("tags=comma,seperated,tags,here");
                sw.WriteLine("artRating=0");
                sw.WriteLine("gameRating=0");
                sw.WriteLine("genRating=0");
                sw.WriteLine("votes=0");
                sw.WriteLine("CabinetArt=" + gameName + "_Art.png");
                sw.WriteLine("movieFile=" + gameName + "_Movie.mp4");
                sw.Close();
            }
            catch
            {
                Debug.Log("error");
            }
        }
        path = Storage.highScoreData.pathForDocumentsFile(gameName + "_Controls.ini");
        if (!File.Exists(path))
        {
            StreamWriter sw;
            try
            {
                sw = new StreamWriter(path);
                sw.WriteLine("[" + gameName + "]");
                sw.WriteLine("//Can add as mean headers as needed, and name the headers whatever they want");
                sw.WriteLine("//by putting a dash on both sides of a header. See \" - Keyboard - \" or \" - Controller - \"");
                sw.WriteLine("//Each header can support a maximum of 10 characters.");
                sw.WriteLine("//This does  not include the dashes.");
                sw.WriteLine("//Each list after any header can be a maximum of 16 lines long, with each line supporting up to 25 characters.");
                sw.WriteLine("- Keyboard-");
                sw.WriteLine("UpArrow, LeftArrow, RightArrow: Move");
                sw.WriteLine("- Controller-");
                sw.WriteLine("Left Stick: Move");
                sw.Close();
            }
            catch
            {
                Debug.Log("error");
            }
        }
        path = Storage.highScoreData.pathForDocumentsFile(gameName + ".ini");
        if (!File.Exists(path))
        {
            StreamWriter sw;
            try
            {
                sw = new StreamWriter(path);
                sw.WriteLine("[" + gameName + "]");

                if (!isLowerScoreBetter)
                {
                    sw.WriteLine("name1=Carv");
                    sw.WriteLine("hscore1=10");
                    sw.WriteLine("name2=erAr");
                    sw.WriteLine("hscore2=8");
                    sw.WriteLine("name3=cade");
                    sw.WriteLine("hscore3=6");
                    sw.WriteLine("name4=HiSc");
                    sw.WriteLine("hscore4=4");
                    sw.WriteLine("name5=ore");
                    sw.WriteLine("hscore5=2");
                }
                else
                {
                    sw.WriteLine("name1=Carv");
                    sw.WriteLine("hscore1=99995");
                    sw.WriteLine("name2=erAr");
                    sw.WriteLine("hscore2=99996");
                    sw.WriteLine("name3=cade");
                    sw.WriteLine("hscore3=99997");
                    sw.WriteLine("name4=HiSc");
                    sw.WriteLine("hscore4=99998");
                    sw.WriteLine("name5=ore");
                    sw.WriteLine("hscore5=99999");
                }
                sw.Close();
            }
            catch
            {
                Debug.Log("error");
            }
        }
    }

public void PauseGame(bool result)
    {
        if (isAPauseableScene)
        {
            foreach (GameObject gO in pauseUI)
            {
                if (gO.GetComponent<Canvas>() == null)
                {
                    if (gO.GetComponent<SpriteRenderer>() != null)
                    {
                        gO.GetComponent<SpriteRenderer>().enabled = result;
                    }
                }
                else
                {
                    gO.GetComponent<Canvas>().enabled = result;
                }
            }
            
            foreach(FormScript form in Component.FindObjectsOfType<FormScript>())
            {
                
                if (form.Visible && result == false)
                    form.Hide(); 
            }

            if (result)
            {
                Time.timeScale = 0;
            }
            else if (!result)
            {
                Time.timeScale = 1;
            }
            isPaused = result;
        }
    }

    void PauseGame()
    {
        if (isAPauseableScene)
        {
            if (Time.timeScale == 0)
            {
                isPaused = false;
                Time.timeScale = 1;
            }
            else
            {
                isPaused = true;
                Time.timeScale = 0;
            }
            foreach (GameObject gO in pauseUI)
            {
                if(gO.GetComponent<Canvas>() == null)
                {
                    if(gO.GetComponent<SpriteRenderer>() != null)
                    {
                        gO.GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
                else
                {
                    gO.GetComponent<Canvas>().enabled = true;
                }
            }
        }
    }



    void GameScreen()
    {
        string gameOverMsg = "Game Over!";
        checkIfOver();

        if (debugMode && Input.GetKey(KeyCode.F10))
        {
            Storage.score++;
        }

        if (usesScoreDisplay)
        {
            GameObject aCanvas = CreateCanvas();
            if (GameObject.Find("txt_Score") == null)
            {
                CreateTextBox("txt_Score", aCanvas.transform, "Score" + Storage.score, displayfont, Color.white, 30,
                    new Vector2(0, 1), new Vector2(0, 1), Vector3.one, new Vector2(0, 1), new Vector3(10, -10, 0), new Vector2(200, 100),
                    Vector3.zero, TextAnchor.UpperLeft);

            }
            GameObject.Find("txt_Score").GetComponent<Text>().text = "Score: " + Storage.score;
        }

        if (usesInGameTimer)
        {
            GameObject aCanvas = CreateCanvas();
            if (GameObject.Find("txt_TimeDisplay") == null)
            {
                CreateTextBox("txt_TimeDisplay", aCanvas.transform, "Score" + Storage.score, displayfont, Color.white, 50,
                    new Vector2(.5f, 1), new Vector2(.5f, 1), Vector3.one, new Vector2(.5f, 1), new Vector3(10, -10, 0), new Vector2(400, 100),
                    Vector3.zero, TextAnchor.UpperCenter);
            }
            if (guiTime > 0)
            {
                GameObject.Find("txt_TimeDisplay").GetComponent<Text>().text = "Time: " + Mathf.RoundToInt(guiTime).ToString();
            }
            else if (guiTime <= 0 && GameObject.Find("txt_TimeDisplay").GetComponent<Text>().text != gameOverMsg)
            {
                GameObject.Find("txt_TimeDisplay").GetComponent<Text>().text = gameOverMsg;
            }
        }
    }

    void TitleScreen()
    {
        if (debugMode && Input.GetKeyDown(KeyCode.F10))
            PlayGame();
    }

    void HighScoresScreen()
    {
        if (debugMode && Input.GetKeyDown(KeyCode.F10))
            PlayGame();

        if (SceneManager.GetActiveScene().name == endScene && isAHighScoresScene && (newScoreInput == null || newScoreName == null))
        {
            //IPF NEW SCORE NEEDS TO BE DEVISIBLED NOT DISABLED 10/23 zig - happens when you escape quit out of a game
            newScoreInput = GameObject.Find("ipf_newScore");
            if (newScoreInput.transform.childCount > 0)
                newScoreName = newScoreInput.GetComponentsInChildren<Text>()[1];
            else
                newScoreName = newScoreInput.GetComponent<Text>();
        }
        #region "High Scores"
        if (!alreadyChecked)
        {
            alreadyChecked = true;
            sIndex = Storage.highScoreData.CheckScore(Storage.score, isLowerScoreBetter);
            if (sIndex != -1) // there is a new high score
            {
                highScore = true;
                Storage.highScoreData.AddNewScore(Storage.score, defaultHighScoreName, sIndex);
            }
        }
        if (GameObject.Find("btn_Quit") != null)
            GameObject.Find("btn_Quit").GetComponent<Button>().onClick.AddListener(QuitGame);
        else if (quitButton != null)
        {
            quitButton.GetComponent<Button>().onClick.AddListener(QuitGame);
        }
        string message = "YOU HAVE A HIGH SCORE!\nEnter your initials"; //newName = "";
        if (highScore && SceneManager.GetActiveScene().name == endScene)
        {
            newScoreInput.SetActive(true);
            if (isAHighScoresScene)
            {
                string acceptedInput = "abcdefghijklmnopqrstuvwxyz";
                if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete))
                {
                    if (newScoreName.text.Length > 1)
                        newScoreName.text = newScoreName.text.Substring(0, newScoreName.text.Length - 1);
                    else if (newScoreName.text.Length == 1)
                        newScoreName.text = "";
                    UpdateScoreDisplay();
                }
                else if (Input.anyKeyDown)
                {
                    bool allowed = false;

                    if (newScoreName.text.Length < 4)
                    {
                        for (int i = 0; i < acceptedInput.Length; i++)
                        {
                            if (Input.inputString != null && Input.inputString != "")
                            {
                                if (Input.inputString[0].ToString().ToLower() == acceptedInput[i].ToString())
                                    allowed = true;
                            }
                        }
                        if (allowed)
                        {
                            newScoreName.text += Input.inputString[0];
                        }
                    }
                    else if (newScoreName.text.Length == 4 && newScoreName.text == defaultHighScoreName)
                    {
                        newScoreName.text = "";
                        for (int i = 0; i < acceptedInput.Length; i++)
                        {
                            if (Input.inputString != null && Input.inputString != "")
                            {
                                if (Input.inputString[0].ToString().ToLower() == acceptedInput[i].ToString())
                                    allowed = true;
                            }
                        }
                        if (allowed)
                        {
                            newScoreName.text += Input.inputString[0];
                        }
                    }


                    UpdateScoreDisplay();
                }
            }

            if (isAHighScoresScene)
            {
                GameObject.Find("txt_HighScoreMsg").GetComponent<Text>().text = message;
            }
        }
        else
        {
            if (SceneManager.GetActiveScene().name == endScene && !highScore)
            {
                if (newScoreInput.activeInHierarchy)
                {
                    newScoreInput.SetActive(false);
                }
                message = "GAME OVER";
                GameObject.Find("txt_HighScoreMsg").GetComponent<Text>().text = message;
            }
        }
        #endregion
    }


    void AlwaysOnScript()
    {
        bool formShown;
        if (usesApplicationTimeOut)
        {
            #region "Timeout Scripts"
            float timeToTimeout = 240;
            if (!Input.anyKeyDown && Input.GetAxis("Mouse X") == 0 && Input.GetAxis("Mouse Y") == 0)
            {
                timeTicker += Time.deltaTime;
                if (timeTicker > timeToTimeout && !isFirstScene)
                {
                    Application.Quit();
                }
                else if (timeTicker > timeToTimeout && isFirstScene)
                {
#if UNITY_EDITOR
                    if (Application.isEditor)
                        EditorApplication.isPlaying = false;
#endif
                    if (!Application.isEditor)
                        Application.Quit();

                }
            }
            else
            {
                timeTicker = 0;
            }
            #endregion
        }

        //if they press the button on the frame, pending the scene and the scene is pausable.
        
        if (isPaused)
        {
            if (Input.GetKeyUp(quitKey) || Input.GetButtonUp("Cancel"))
            {
                formShown = false;
                foreach(FormScript fr in Component.FindObjectsOfType<FormScript>())
                {
                    if(fr.Visible)
                    {
                        formShown = true;
                        fr.Hide();
                    }
                }
                
                if (!formShown)
                {
                    PauseGame(false);
                    SceneManager.LoadScene(endScene);
                }
            }
            else if (Input.GetKeyUp(pauseKey))
            {
                PauseGame(false);
            }
        }
        else
        {
            if ((Input.GetKeyUp(quitKey) || Input.GetButtonUp("Cancel")) && !isAPauseableScene)
            {
                
                if (SceneManager.GetActiveScene().name == endScene)
                {
                    QuitGame();
                }
                else if (SceneManager.GetActiveScene().name == startScene)
                {
                    PauseGame(false);
                    SceneManager.LoadScene(endScene);
                }
                
            }
            if (isAPauseableScene)
            {
                if (Input.GetKeyUp(pauseKey) || Input.GetKeyUp(quitKey) || Input.GetButtonUp("Cancel"))
                {
                    PauseGame(true);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene != endScene && currentScene != startScene)
            GameScreen();
        if (isAHighScoresScene)
            HighScoresScreen();
        TitleScreen();
        AlwaysOnScript();
    }

    public void CreatePauseUI()
    {
        if (pauseUI == null)
        {
            pauseUI = new List<GameObject>();
        }
        pauseUI.Clear();
        if (displayfont == null)
        {
            displayfont = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }
        GameObject pauseTint;
        GameObject pauseOverlay = CreateCanvas("PauseOverlay");
        pauseOverlay.GetComponent<Canvas>().sortingOrder = 101;
        //print(pauseKey);
        
        CreateTextBox("PauseText", pauseOverlay.transform, "PAUSED\nPress F1 to Resume\nPress ESC to Quit Game",
            displayfont, Color.white, 66, new Vector2(.5f, .5f), new Vector2(.5f, .5f), Vector3.one, new Vector2(.5f, .5f),
            new Vector3(4, -14, 0), new Vector2(744.3f, 230f), Vector3.zero, TextAnchor.MiddleCenter);
        if (GameObject.Find("PauseTint") == null)
        {
            pauseTint = new GameObject("PauseTint", typeof(SpriteRenderer));
            pauseTint.transform.SetAsFirstSibling();
            pauseTint.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, .627f);
#if UNITY_EDITOR
            pauseTint.GetComponent<SpriteRenderer>().sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
#endif
            pauseTint.GetComponent<SpriteRenderer>().sortingOrder = 100;
            pauseTint.transform.localScale = new Vector3(5000, 5000, 1);
            pauseTint.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            pauseTint = GameObject.Find("pauseTint");
        }
        pauseOverlay.transform.SetAsFirstSibling();
        pauseOverlay.GetComponent<Canvas>().enabled = false;
        pauseUI.Add(pauseOverlay);
        pauseUI.Add(pauseTint);
    }


    private bool CreateTextBox(string _name, Transform _parent, string _text, Font _font, Color _color, int _fontsize,
                               Vector2 _aMin, Vector2 _aMax, Vector3 _scale, Vector2 _pivots, Vector3 _position, Vector2 _size,
                               Vector3 _offset, TextAnchor _alignment)
    {
        bool result = false;
        if (GameObject.Find(_name) == null)
        {
            GameObject newTextBox = new GameObject(_name, typeof(Text));
            RectTransform nTB_rt = newTextBox.GetComponent<RectTransform>();
            newTextBox.transform.SetParent(_parent);

            newTextBox.GetComponent<Text>().color = _color;
            newTextBox.GetComponent<Text>().fontSize = _fontsize;
            newTextBox.GetComponent<Text>().font = _font;
            newTextBox.GetComponent<Text>().text = _text;
            newTextBox.GetComponent<Text>().alignment = _alignment;
            newTextBox.transform.localScale = _scale;

            nTB_rt.anchorMin = _aMin;
            nTB_rt.anchorMax = _aMax;
            nTB_rt.pivot = _pivots;
            nTB_rt.anchoredPosition3D = _position + _offset;
            nTB_rt.sizeDelta = _size;
            result = true;
        }
        return result;
    }



    private void CreateButton(string _name, Transform _parent, string _text, Font _font, Color _color, int _fontsize,
                               Vector2 _aMin, Vector2 _aMax, Vector3 _scale, Vector2 _pivots, Vector3 _position, Vector2 _size,
                               Vector3 _offset, TextAnchor _alignment)
    {
        if (GameObject.Find(_name) == null)
        {
            GameObject newButton = new GameObject(_name, typeof(Button));
            newButton.transform.SetParent(_parent);
            Image nB_Img = newButton.AddComponent<Image>();
#if UNITY_EDITOR
            nB_Img.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
#endif
            nB_Img.type = Image.Type.Sliced;
            newButton.GetComponent<Button>().targetGraphic = nB_Img;
            CreateTextBox(_name+"Text", newButton.transform, _text, displayfont, Color.black, 30, new Vector2(0, 0), new Vector2(1, 1), Vector3.one, new Vector2(.5f, .5f), Vector3.zero, Vector2.zero, _offset, TextAnchor.MiddleCenter);
            newButton.transform.localScale = _scale;
            newButton.transform.localPosition = _position;
        }
    }



    public void CreateNavigation()
    {
        GameObject newCanvas = CreateCanvas();
        Vector3 offset = new Vector3(displayXOffset, -displayYOffset, 0);
        if (displayfont == null)
        {
            displayfont = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }
        if(playButton == null)
            CreateButton("btn_Play", newCanvas.transform, "Play", displayfont, Color.white, 30, new Vector2(0, 1), new Vector2(0, 1), Vector3.one, new Vector2(0, 1), new Vector3(-80, -170, 0), new Vector2(100, 200),
                        offset, TextAnchor.UpperLeft);
        if(quitButton == null)
            CreateButton("btn_Quit", newCanvas.transform, "Quit", displayfont, Color.white, 30, new Vector2(0, 1), new Vector2(0, 1), Vector3.one, new Vector2(0, 1), new Vector3(110, -170, 0), new Vector2(100, 200),
                        offset, TextAnchor.UpperLeft);
    }


    public void CreateHighScoresDisplay()
    {
        GameObject newCanvas = CreateCanvas();
        Vector3 offset = new Vector3(displayXOffset, -displayYOffset,0);
        if (displayfont == null)
        {
            displayfont = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }
        CreateCamera();
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject newEventMan = new GameObject("EventSystem", typeof(EventSystem));
            newEventMan.AddComponent<StandaloneInputModule>();
        }
        if (HighScoresNameDisplay == null && HighScoresScoreDisplay == null)
        {
            
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                if (!CreateTextBox("txt_HighNames", newCanvas.transform, Storage.highScoreData.ScoreNames(), displayfont, Color.white, 30,
                    new Vector2(0, 1), new Vector2(0, 1), Vector3.one, new Vector2(0, 1), new Vector3(10, -80, 0), new Vector2(100, 200),
                    offset, TextAnchor.UpperLeft))
                    GameObject.Find("txt_HighNames").GetComponent<Text>().text = Storage.highScoreData.ScoreNames();
                if (!CreateTextBox("txt_HighScore", newCanvas.transform, Storage.highScoreData.ScoreScores(), displayfont, Color.white, 30,
                    new Vector2(0, 1), new Vector2(0, 1), Vector3.one, new Vector2(0, 1), new Vector3(130, -80, 0), new Vector2(200, 200),
                    offset, TextAnchor.UpperRight))
                    GameObject.Find("txt_HighScore").GetComponent<Text>().text = Storage.highScoreData.ScoreScores();
            }
            else
            {
                CreateTextBox("txt_HighNames", newCanvas.transform, "", displayfont, Color.white, 30,
                   new Vector2(0, 1), new Vector2(0, 1), Vector3.one, new Vector2(0, 1), new Vector3(10, -80, 0), new Vector2(100, 200),
                   offset, TextAnchor.UpperLeft);
                CreateTextBox("txt_HighScore", newCanvas.transform, "", displayfont, Color.white, 30,
                    new Vector2(0, 1), new Vector2(0, 1), Vector3.one, new Vector2(0, 1), new Vector3(130, -80, 0), new Vector2(200, 200),
                    offset, TextAnchor.UpperRight);
            }
#else
            CreateTextBox("txt_HighNames", newCanvas.transform, Storage.highScoreData.ScoreNames(), displayfont, Color.white, 30,
                   new Vector2(0, 1), new Vector2(0, 1), Vector3.one, new Vector2(0, 1), new Vector3(10, -80, 0), new Vector2(100, 200),
                   offset, TextAnchor.UpperLeft);
                CreateTextBox("txt_HighScore", newCanvas.transform, Storage.highScoreData.ScoreScores(), displayfont, Color.white, 30,
                    new Vector2(0, 1), new Vector2(0, 1), Vector3.one, new Vector2(0, 1), new Vector3(130, -80, 0), new Vector2(200, 200),
                    offset, TextAnchor.UpperRight);


#endif
            if (SceneManager.GetActiveScene().name == startScene && Application.isPlaying)
            {
                CreateNavigation();
            }
            HighScoresNameDisplay = GameObject.Find("txt_HighNames");
            HighScoresScoreDisplay = GameObject.Find("txt_HighScore");
        
        }
        else
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                HighScoresNameDisplay.GetComponent<Text>().text = Storage.highScoreData.ScoreNames();
                HighScoresScoreDisplay.GetComponent<Text>().text = Storage.highScoreData.ScoreScores();
            }
#else
            HighScoresNameDisplay.GetComponent<Text>().text = Storage.highScoreData.ScoreNames();
            HighScoresScoreDisplay.GetComponent<Text>().text = Storage.highScoreData.ScoreScores();
#endif
        }
        CreateTextBox("txt_HighScoresTitle", newCanvas.transform, "High Scores", displayfont, Color.white, 30,
                        new Vector2(0, 1), new Vector2(0, 1), Vector3.one, new Vector2(0, 1), new Vector3(10, -10, 0),
                        new Vector2(GameObject.Find("txt_HighNames").GetComponent<RectTransform>().sizeDelta.x +
                                    GameObject.Find("txt_HighScore").GetComponent<RectTransform>().sizeDelta.x + 20, 70),
                        offset, TextAnchor.MiddleCenter);
        if (SceneManager.GetActiveScene().name == endScene)
        {
            CreateTextBox("ipf_newScore", newCanvas.transform, defaultHighScoreName, displayfont, Color.white, 30,
                new Vector2(0, 1), new Vector2(0, 1), Vector3.one, new Vector2(0, 1), new Vector3(10, -360, 0),
                new Vector2(GameObject.Find("txt_HighNames").GetComponent<RectTransform>().sizeDelta.x +
                            GameObject.Find("txt_HighScore").GetComponent<RectTransform>().sizeDelta.x + 20, 80),
                offset, TextAnchor.MiddleCenter);
            CreateTextBox("txt_HighScoreMsg", newCanvas.transform, "Game Over", displayfont, Color.white, 10,
                new Vector2(0, 1), new Vector2(0, 1), Vector3.one, new Vector2(0, 1), new Vector3(10, -320, 0),
                new Vector2(GameObject.Find("txt_HighNames").GetComponent<RectTransform>().sizeDelta.x +
                            GameObject.Find("txt_HighScore").GetComponent<RectTransform>().sizeDelta.x + 20, 80),
                offset, TextAnchor.MiddleCenter);
        }
        newScoreInput = GameObject.Find("ipf_newScore");

    }

    private GameObject CreateCamera()
    {
        GameObject result = null;
        if (FindObjectsOfType<Camera>().Length == 0)
        {
            GameObject newCamera = new GameObject("Camera", typeof(Camera));
            newCamera.tag = "MainCamera";
        }
        else
        {
            result = FindObjectOfType<Camera>().gameObject;
            result.tag = "MainCamera";
        }
        return result;
    }

    private GameObject CreateCanvas(string _name)
    {
        GameObject result = null;
        Canvas newCan;
        bool foundNamedCanvas;
        if (FindObjectOfType<Canvas>() == null)
        {
            result = new GameObject(_name, typeof(Canvas));
            result.AddComponent<CanvasScaler>();
            result.AddComponent<GraphicRaycaster>();
            newCan = result.GetComponent<Canvas>();
            newCan.renderMode = RenderMode.ScreenSpaceCamera;
            newCan.GetComponent<Canvas>().worldCamera = Camera.main;
        }
        else
        {
            foundNamedCanvas = false;
            foreach(Canvas theCanvas in FindObjectsOfType<Canvas>())
            {
                if(theCanvas.name == _name)
                {
                    result = theCanvas.gameObject;
                    foundNamedCanvas = true;
                }
            }
            if(!foundNamedCanvas)
            {
                result = new GameObject(_name, typeof(Canvas));
                result.AddComponent<CanvasScaler>();
                result.AddComponent<GraphicRaycaster>();
                newCan = result.GetComponent<Canvas>();
                newCan.renderMode = RenderMode.ScreenSpaceCamera;
                newCan.GetComponent<Canvas>().worldCamera = Camera.main;
                result = newCan.gameObject;
            }
        }
        return result;
    }

    private GameObject CreateCanvas()
    {
        return CreateCanvas("Canvas");
    }
    
    public void UpdateScoreDisplay()
    {
        string newName;
        if (isAHighScoresScene)
            
            newName = newScoreName.text;
       
        else
            newName = newScoreName.text;

        

        if (newName == "")
            Storage.highScoreData.RewriteSingleScore(defaultHighScoreName, sIndex);
        else
            Storage.highScoreData.RewriteSingleScore(newName, sIndex);
        GameObject.Find("txt_HighNames").GetComponent<Text>().text = Storage.highScoreData.ScoreNames();
        GameObject.Find("txt_HighScore").GetComponent<Text>().text = Storage.highScoreData.ScoreScores();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
            if (Application.isEditor)
                EditorApplication.isPlaying = false;
        if (highScore)
        {
            Storage.highScoreData.WriteData();
            highScore = false;
        }
#endif
        if (!Application.isEditor)
        {
            if (highScore)
            {
                Storage.highScoreData.WriteData();
                highScore = false;
            }
            Application.Quit();
        }
    }

    public void ResetGame()
    {
        alreadyChecked = false;
        PauseGame(false);
        if(SceneManager.GetActiveScene().name == endScene)
        {
            isPaused = false;
            Time.timeScale = 1;
        }
        if(highScore)
        {
            Storage.highScoreData.WriteData();
            highScore = false;
        }
        Storage.score = defaultHighScoreValue;
        if(usesInGameTimer)
            guiTime = gameCountDownTime;
        if (!usesInGameTimer && delaySceneLoadingOnGameOver)
            guiTime = 0;
        SceneManager.LoadScene(gameScene);
    }

    public void PlayGame()
    {
        if(SceneManager.GetActiveScene().name == gameScene)
        {
            SceneManager.LoadScene(endScene);
        }
        else 
        ResetGame();
    }

    void checkIfOver()
    {
        if(isGameOver)
        {
            if (!usesInGameTimer && !isPaused && delaySceneLoadingOnGameOver)
            {
                Debug.Log(guiTime);
                guiTime -= Time.deltaTime;
                if (guiTime <= -gameOverDisplayTime)
                {
                    SceneManager.LoadScene(endScene);
                }
            }
            else
            {
                SceneManager.LoadScene(endScene);
            }
        }

        if (usesInGameTimer && !isPaused)
        {
            guiTime -= Time.deltaTime;
            if (delaySceneLoadingOnGameOver)
            {
                if (guiTime <= -gameOverDisplayTime)
                {
                    isGameOver = true;
                }
            }
            else
            {
                if (guiTime <= 0)
                {
                    isGameOver = true;
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        QuitGame();
    }

}

