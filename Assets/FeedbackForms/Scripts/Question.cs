using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Question : MonoBehaviour
{
    public GameObject questionLabel;
    public GameObject value;
    string questionText= "";
    string selectedValue;
    private GameObject defaultToggle = null;
    string questionType ="";
    public bool required;
    // Start is called before the first frame update
    void Start()
    {
        questionText = questionLabel.GetComponent<Text>().text;
        if (value.GetComponent<ToggleGroup>() != null)
        {
            questionType = "toggle";
            if (value.GetComponent<ToggleGroup>().ActiveToggles().FirstOrDefault() != null)
            {
                defaultToggle = value.GetComponent<ToggleGroup>().ActiveToggles().FirstOrDefault().gameObject;
            }
        }
        else
        {
            questionType = "open";
        }
    }

    public void Created(string qText, string c1, string c2, string c3, string c4, string c5, string minVal, string maxVal)
    {
        float min, max, temp;
        if (qText != "") { questionLabel.GetComponent<Text>().text = qText; questionText = qText; }
        else { questionText = questionLabel.GetComponent<Text>().text; }
        if (GetComponentInChildren<Slider>() != null)
        {
            if(!float.TryParse(minVal, out min)) min = 0;
            if (!float.TryParse(maxVal, out max)) max = 1;
            if (min > max)
            {
                temp = max;
                max = min;
                min = temp;
            }
            GetComponentInChildren<Slider>().maxValue = max;
            GetComponentInChildren<Slider>().minValue = min;
            transform.Find("MinimumValue").GetComponent<Text>().text = "" + min;
            transform.Find("MaximumValue").GetComponent<Text>().text = "" + max;
        }
        Toggle[] toggles = GetComponentsInChildren<Toggle>();
        if (toggles.Length > 0)
        {
            if(c1 != "") toggles[0].transform.Find("Label").GetComponent<Text>().text = c1;
            if (c2 != "") toggles[1].transform.Find("Label").GetComponent<Text>().text = c2;
            if (c3 != "") toggles[2].transform.Find("Label").GetComponent<Text>().text = c3;
            if (toggles.Length > 3)
            {
                if (c4 != "") toggles[3].transform.Find("Label").GetComponent<Text>().text = c4;
                if (c5 != "") toggles[4].transform.Find("Label").GetComponent<Text>().text = c5;
            }
        }
    }

    public string QuestionType
    {
        get { return questionType; }
    }

    public void ResetQuestion()
    {
        if (defaultToggle != null)
        {
            defaultToggle.GetComponent<Toggle>().isOn = true;
        }
        else 
        {
            foreach(Toggle t in value.GetComponentsInChildren<Toggle>())
            {
                t.isOn = false;
            }
        }
        if (value.GetComponent<InputField>() != null)  value.GetComponent<InputField>().text = "";
        if (value.GetComponent<Slider>() != null) value.GetComponent<Slider>().value = value.GetComponent<Slider>().minValue;
    }

    private void GrabValues()
    {
        if (value.GetComponent<ToggleGroup>() != null)
        {
            if (value.GetComponent<ToggleGroup>().ActiveToggles().Count()>0)
            {
                
                selectedValue = value.GetComponent<ToggleGroup>().ActiveToggles().FirstOrDefault().gameObject.transform.Find("Label").GetComponent<Text>().text;
                if(selectedValue == "Other")
                {
                    selectedValue = value.GetComponent<ToggleGroup>().ActiveToggles().FirstOrDefault().gameObject.transform.GetComponentInChildren<InputField>().text;
                }
            }
            else
            {
                selectedValue = "No Response";
            }
        }
        else if (value.GetComponent<InputField>() != null)
        {
            selectedValue = value.GetComponent<InputField>().text;
        }
        else if (value.GetComponent<Slider>() != null)
        {
            selectedValue = "" + value.GetComponent<Slider>().value;
        }
    }

    public string QuestionText
    {
        get {
            if(questionText == "")
            {
                if (questionLabel.GetComponent<Text>() != null) questionText = questionLabel.GetComponent<Text>().text;
            }
            return questionText;
        }
    }

    public string ValueText
    {
        get {
            GrabValues();
            return selectedValue;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
