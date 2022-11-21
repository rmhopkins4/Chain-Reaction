#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[CustomEditor(typeof(FormScript))]
public class FormCreatorScript : Editor
{
    string buttonQuestionText;
    int state = 1, questionState = 1;
    GameObject newForm = null;
    string CreateFormText = "Create Form",
           choice1="", choice2 = "", choice3 = "",
           choice4 = "", choice5 = "", qText="",
           minVal="", maxVal="", CreateButtonText= "Create Form Show Button";
    GameObject obj= null;
    public override void OnInspectorGUI()
    {
        FormScript formScript = (FormScript)target;

        if (GameObject.FindObjectOfType<FormScript>() != null && formScript.gameObject.activeInHierarchy)
        {
            GUILayout.Label("What type of form would you like to create?");
            //EditorGUILayout.BeginToggleGroup("Form Type", true);

            GUILayout.BeginHorizontal(GUILayout.Height(50));
            if (formScript.transform.childCount == 0)
            {
                GUILayout.BeginVertical();
                if (EditorGUILayout.Toggle("Custom", state == 1 ? true : false, EditorStyles.radioButton)) state = 1;
                if (EditorGUILayout.Toggle("Bug Report", state == 2 ? true : false, EditorStyles.radioButton)) state = 2;
                if (EditorGUILayout.Toggle("Feedback", state == 3 ? true : false, EditorStyles.radioButton)) state = 3;
                GUILayout.EndVertical();
            }
            else
            {
                newForm = formScript.gameObject.transform.GetComponentInChildren<Canvas>().gameObject;
                
                if (newForm.gameObject.transform.Find("OpenForm") != null)
                {
                    //Debug.Log(newForm.gameObject.transform.Find("OpenForm"));
                    obj = newForm.gameObject.transform.Find("OpenForm").gameObject;
                    CreateButtonText = "Remove Form Show Button";
                }
                else
                {
                    CreateButtonText = "Create Form Show Button";
                }
                formScript.FindContent();
                CreateFormText = "Destroy Form";
            }

            if (GUILayout.Button(CreateFormText, GUILayout.ExpandHeight(true)))
            {

                if (state != 0 || formScript.transform.childCount != 0)
                {

                    if (newForm == null)
                    {
                        CreateFormText = "Destroy Form";
                        newForm = Instantiate(formScript.forms[state - 1], formScript.gameObject.transform);
                        if (state == 1) newForm.name = "CustomForm"; else if (state == 2) newForm.name = "BugReport"; else if (state == 3) newForm.name = "Feedback";
                        formScript.FindContent();
                        foreach (RectTransform rt in formScript.gameObject.GetComponentsInChildren<RectTransform>())
                        {
                            if (rt.name == "TitleText")
                            {
                                string temp = rt.gameObject.GetComponent<Text>().text;

                                rt.gameObject.GetComponent<Text>().text = temp.Substring(0, temp.IndexOf('@')) + " " + Application.productName +
                                    temp.Substring(temp.IndexOf('@') + 1, temp.Length - temp.IndexOf('@') - 1);
                            }
                        }
                        if(Component.FindObjectOfType<GuiCode>() == null)
                        {
                            GameObject newSystemObject = new GameObject();
                            newSystemObject.AddComponent<GuiCode>();
                            newSystemObject.name = "GUICode Holder Object";
                        }
                        if (FindObjectOfType<EventSystem>() == null)
                        {
                            GameObject newEventMan = new GameObject("EventSystem", typeof(EventSystem));
                            newEventMan.AddComponent<StandaloneInputModule>();
                        }
                        newForm.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
                        newForm.GetComponent<Canvas>().worldCamera = Camera.main;
                    }
                    else if (newForm != null)
                    {
                        DestroyImmediate(newForm);
                        CreateFormText = "Create Form";
                    }
                }
            }
            GUILayout.EndHorizontal();
            if (newForm != null && newForm.name == "CustomForm")
            {

                GUILayout.Label("----Add Questions----");
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                if (EditorGUILayout.Toggle("3 Choice Rating", questionState == 1 ? true : false, EditorStyles.radioButton)) questionState = 1;
                if (EditorGUILayout.Toggle("5 Choice Rating", questionState == 2 ? true : false, EditorStyles.radioButton)) questionState = 2;
                if (EditorGUILayout.Toggle("5 Choice Rating w/Other", questionState == 3 ? true : false, EditorStyles.radioButton)) questionState = 3;
                if (EditorGUILayout.Toggle("Two Line Open Response", questionState == 4 ? true : false, EditorStyles.radioButton)) questionState = 4;
                if (EditorGUILayout.Toggle("Paragraph Open Response", questionState == 5 ? true : false, EditorStyles.radioButton)) questionState = 5;
                if (EditorGUILayout.Toggle("Slider Rating", questionState == 6 ? true : false, EditorStyles.radioButton)) questionState = 6;
                GUILayout.EndVertical();

                if (GUILayout.Button("Create Question", GUILayout.ExpandHeight(true)))
                {
                    GameObject newQuestion = Instantiate(formScript.questions[questionState - 1], formScript.Content.transform);
                    newQuestion.GetComponent<Question>().Created(qText, choice1, choice2, choice3, choice4, choice5, minVal, maxVal);
                    choice1 = "";
                    choice2 = "";
                    choice3 = "";
                    choice4 = "";
                    choice5 = "";
                    minVal = "";
                    maxVal = "";
                    qText = "";
                }
                GUILayout.EndHorizontal();

                GUILayout.Label("---- Question Settings ----");
                if (questionState > 0)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Question Text");
                    qText = GUILayout.TextArea(qText, 150);
                    //qText = GUILayout.TextField(qText, 150, GUILayout.MaxWidth(100));
                    GUILayout.EndHorizontal();
                }

                if (questionState <= 3 && questionState > 0)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Choice 1");
                    choice1 = GUILayout.TextField(choice1, 15);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Choice 2");
                    choice2 = GUILayout.TextField(choice2, 15);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Choice 3");
                    choice3 = GUILayout.TextField(choice3, 15);
                    GUILayout.EndHorizontal();
                }
                if (questionState <= 3 && questionState > 1)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Choice 4");
                    choice4 = GUILayout.TextField(choice4, 15);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Choice 5");
                    choice5 = GUILayout.TextField(choice5, 15);
                    GUILayout.EndHorizontal();
                }
                if (questionState == 6)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Minimum Value");
                    minVal = GUILayout.TextField(minVal, 15);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Maximum Value");
                    maxVal = GUILayout.TextField(maxVal, 15);
                    GUILayout.EndHorizontal();
                }


                if (formScript.Content.transform.childCount > 0)
                {
                    GUILayout.Label("---- Delete Questions ----");
                    int qIndex = 0;

                    foreach (Transform ConQue in formScript.Content.transform)
                    {
                        buttonQuestionText = ConQue.GetComponent<Question>().QuestionText;
                        if (buttonQuestionText.Length > 36) buttonQuestionText = buttonQuestionText.Substring(0, 36) + "...?";
                        if (GUILayout.Button(buttonQuestionText == "" ? "Q" + qIndex++ : buttonQuestionText))
                        {
                            DestroyImmediate(ConQue.gameObject);
                        }

                    }
                }

            }
            if (newForm != null)
            {
                if (newForm.name == "Feedback" || formScript.formShowKey == KeyCode.None)
                {
                    if (GUILayout.Button(CreateButtonText))
                    {
                        if (CreateButtonText == "Create Form Show Button")
                        {
                            obj = Instantiate(formScript.showFormButton, newForm.gameObject.transform);
                            obj.name = "OpenForm";
                            obj.transform.SetAsFirstSibling();
                            CreateButtonText = "Remove Form Show Button";
                        }
                        else
                        {
                            CreateButtonText = "Create Form Show Button";
                            DestroyImmediate(obj);
                        }

                    }
                }
            }
            DrawDefaultInspector();
        }
    }

    public void OnGUI()
    {
        
    }
}

#endif