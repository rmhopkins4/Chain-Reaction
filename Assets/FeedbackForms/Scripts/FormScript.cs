using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class FormScript : MonoBehaviour
{
    public string outputFilename="BugReport", outputFileExtension = "txt";
    public bool formVisibleOnStart;
    public KeyCode formShowKey;
    public KeyCode[] alternateFormShowKey;
    [HideInInspector]
    public GameObject[] forms;
    [HideInInspector]
    public GameObject[] questions;
    [HideInInspector]
    public GameObject showFormButton;
    private CanvasGroup cg;
    private Canvas theCanvas;
    GuiCode sys;
    private bool shown;
    private GameObject content= null;
    // Start is called before the first frame update
    public bool Visible
    {
        get { return shown; }
    }

    public GameObject Content
    {
        get { return content; }
        set { content = value; }
    }
    void Start()
    {   
        cg = GetComponentInChildren<CanvasGroup>();
        
        if (cg != null)
        {
            sys = Component.FindObjectOfType<GuiCode>();
            theCanvas = transform.GetComponentInChildren<Canvas>();
            if (cg.alpha == 0 && formVisibleOnStart)
            {
                Show();
            }
            if (theCanvas.renderMode == RenderMode.ScreenSpaceOverlay && theCanvas.worldCamera == null)
            {
                transform.parent.GetComponent<Canvas>().worldCamera = Camera.main;
            }
            if(outputFileExtension != "")
            {
                outputFileExtension = "." + outputFileExtension;
            }
            if (!formVisibleOnStart)
            {
                Hide();
            }
            if (!sys.DebugMode)
            {
                Hide();
            }
            foreach (Button bn in GetComponentsInChildren<Button>())
            {
                if (bn.gameObject.name == "ResetButton") bn.onClick.AddListener(ResetForm);
                if (bn.gameObject.name == "SubmitButton") bn.onClick.AddListener(SubmitForm);
                if (bn.gameObject.name == "OpenForm") bn.onClick.AddListener(Show);
            }
            FindContent();
        }
        else
        {
            enabled = false;
        }
        
    }

    public void FindContent()
    {
        content = GetComponentInChildren<ScrollRect>().content.gameObject;
    }

    public void Hide()
    {

        cg.alpha = 0;
        cg.blocksRaycasts = false;
        shown = false;
    }

    public void Show()
    {
        cg.blocksRaycasts = true;
        cg.alpha = 1;
        shown = true;
    }

    // Update is called once per frame
    void Update()
    {
        bool pressed = false;
        foreach (KeyCode kc in alternateFormShowKey)
        {
            if (Input.GetKeyDown(kc)) pressed = true;
        }
        if (Input.GetKeyDown(formShowKey) || pressed)
        {
            if (Input.GetKeyDown(formShowKey) || pressed)
            {
                if (cg.alpha == 0) Show(); else Hide();
            }
            if (sys.IsItAPausableScene && shown)
            {
                if (!GuiCode.isPaused) sys.PauseGame(true);
            }
        }
    }

    public void SubmitForm()
    {
        Question currentQuestion;
        int count = 0;
        StreamWriter writer;
        StreamWriter sheetWriter;
        string path;

        path = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')) + "/Reports/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        writer = new StreamWriter(path + outputFilename + System.DateTime.Today.Day + System.DateTime.Today.Month + System.DateTime.Today.Year + outputFileExtension, true);
        if (!File.Exists(path + outputFilename + System.DateTime.Today.Day + System.DateTime.Today.Month + System.DateTime.Today.Year + ".csv"))
        {
            sheetWriter = new StreamWriter(path + outputFilename + System.DateTime.Today.Day + System.DateTime.Today.Month + System.DateTime.Today.Year + ".csv", true);
            foreach (Transform content in content.transform)
            {
                currentQuestion = content.GetComponent<Question>();
                sheetWriter.Write(currentQuestion.QuestionText);
                if (count++ < content.transform.childCount - 1) sheetWriter.Write(",");
            }
            sheetWriter.Write(sheetWriter.NewLine);
            count = 0;
        }
        else
        {
            sheetWriter = new StreamWriter(path + outputFilename + System.DateTime.Today.Day + System.DateTime.Today.Month + System.DateTime.Today.Year + ".csv", true);
        }

        foreach (Transform content in content.transform)
        {
            currentQuestion = content.GetComponent<Question>();
            sheetWriter.Write(currentQuestion.ValueText);
            if (count++ < content.transform.childCount - 1) sheetWriter.Write(",");

        }
        sheetWriter.Write(sheetWriter.NewLine);
        sheetWriter.Close();


        writer.WriteLine("*********************************************");
        writer.WriteLine("*********************************************");
        writer.WriteLine(outputFilename);
        writer.WriteLine("For " + Application.productName + " v." + Application.version);
        writer.WriteLine("Developed by " + Application.companyName);
        writer.WriteLine("Submitted by:" + System.Environment.UserName);
        writer.WriteLine("Generated by script on " + System.DateTime.Now);
        writer.WriteLine("----------------------------");

        foreach (Transform content in content.transform)
        {
            currentQuestion = content.GetComponent<Question>();
            writer.WriteLine(currentQuestion.QuestionText);
            writer.WriteLine(currentQuestion.ValueText);
            if (count++ < content.transform.childCount - 1)
                writer.WriteLine("========");
        }
        writer.Close();
        ResetForm();
        Hide();

    }

    public void ResetForm()
    {
        foreach (Transform content in content.transform)
        {
            content.GetComponent<Question>().ResetQuestion();
        }
    }
}
