using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class ScoresData
{
    private List<Score> highScores; //List of Scores for storage
    private List<string> validate;  //database for valid information
    private string defaultName;     //The default name for a score
    private string gameName;        //The gamename as listed by unity
    private string path;            

    string testString;

    //Gamename accessor method
    public string GameName
    {
        get { return gameName; }
        set { gameName = value; }
    }

    //Constructor
    public ScoresData(string _dName)
    {
        //if the name is more than four, trim it to four
        //this shouldn't happen if the UI is set up properly
        if (_dName.Length > 4)
            _dName = _dName.Substring(0, 4);
        // Set the default name
        defaultName = _dName;
        // Instantiate the highscores and validate lists
        highScores = new List<Score>();
        validate = new List<string>();
        // read in the validation database
        ReadValid();
    }

    //Another constructor
    public ScoresData() : this("JBOB")
    {
        
    }

    //Function: ReadValid
    //Parameters: None
    //Description: Reads a list of validation data from a file.
    public bool ReadValid()
    {
        // We assume it read, unless otherwise stated
        bool result = true;
        // Our path to the file
        string filePath;
        if (Application.isEditor) //Only happen if we are in Unity
        {
            filePath = "UnityPatchManager"; //our path leads right here
            filePath = pathForDocumentsFile(filePath); // format the file path properly
        }
        else
        {
            try
            {
                //our path is the datapath, but one directory up.
                filePath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("/"));
                filePath = filePath.Substring(0, filePath.LastIndexOf("/")); //go up again.
                filePath = filePath.Substring(0, filePath.LastIndexOf("/")); //and again
                filePath += "/UnityPatchManager"; // the filename for the validation data.
            }
            catch
            {
                filePath = "UnityPatchManager"; //our path leads right here
                filePath = pathForDocumentsFile(filePath); // format the file path properly
            }
        }
        if (File.Exists(filePath)) //make sure the file exists
        {
            StreamReader file = null; //instantiate
            try
            {
                file = new StreamReader(filePath); //attempt to find the file
                while (!file.EndOfStream)
                {
                    validate.Add(file.ReadLine()); //read in each line
                }
            }
            catch
            {
                result = false; // we didn't load, sadness abound
            }
            finally
            {
                if (file != null) //close the file if the file is open
                    file.Close();
            }
        }
        return result;
    }

    public bool ReadData()
    {
        highScores.Clear();
        bool result = true;
        int scoreCount = 0;
        Score newScore;
        string filePath = gameName + ".ini";
        filePath = pathForDocumentsFile(filePath);
        if (File.Exists(filePath))
        {
            StreamReader file = null;
            try
            {
                file = new StreamReader(filePath);
                string nameLine;
                string scoreLine;
                file.ReadLine(); // throw away the first line of the file, it is a header
                while (!file.EndOfStream && scoreCount < 5)
                {
                    newScore = new Score();
                    nameLine = file.ReadLine();
                    scoreLine = file.ReadLine();
                    newScore.PScore = int.Parse(scoreLine.Substring(scoreLine.IndexOf('=')+1, scoreLine.Length - (scoreLine.IndexOf('=')+1)));
                    newScore.Name = nameLine.Substring(nameLine.IndexOf('=')+1, nameLine.Length - (nameLine.IndexOf('=') + 1));
                    highScores.Add(newScore);
                    scoreCount++;    
                }
                file.Close();
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (file != null)
                    file.Close();
            }
        }
        else
        {
            for(int i =0; i < 5; i++)
            {
                highScores.Add(new Score(defaultName, (5 - i) * 2));
            }

        }
        return result;
    }

    public string ScoreNames()
    {
        string temp = "";
        foreach(Score sc in highScores)
        {
            temp += sc.Name + "\n";
        }
        return temp;
    }

    public string ScoreScores()
    {
        string temp = "";
        foreach (Score sc in highScores)
        {
            temp += sc.PScore.ToString() + "\n";
        }
        return temp;
    }

    public bool WriteData()
    {
        bool result = true;
        string path = pathForDocumentsFile(gameName + ".ini");
        StreamWriter sw;
        try
        {
            sw = new StreamWriter(path, false);
            sw.WriteLine("[" + gameName + "]");
            for (int i = 1; i <= highScores.Count; i++)
            {
                sw.WriteLine("name" + i + "=" + highScores[i - 1].Name);
                sw.WriteLine("hscore" + i + "=" + highScores[i - 1].PScore);
            }
            sw.Close();
        }
        catch
        {
            Debug.Log("ERRRORRRR");
            result = false;
        }
        finally
        {
            
        }
        return result;
    }

    public void RewriteSingleScore(string name, int index)
    {
        bool outs = false;
        foreach(string valid in validate)
        {
            if((name.ToLower() == valid.ToLower() || name.ToLower().Contains(valid.ToLower())) && !outs)
            {
                name = defaultName;
                outs = true;
            }
        }
        highScores[index].Name = name;
    }

    public int CheckScore(int newscore,bool isLowBetter)
    {
        
        int insertPosition = -1;
        for (int i = highScores.Count-1; i >= 0; i--)
        {
            
            if ((highScores[i].PScore < newscore && !isLowBetter)|| (highScores[i].PScore > newscore && isLowBetter))
            {
                insertPosition = i;
            }
        }
        return insertPosition;
    }

    public void AddNewScore(int newScore, string newName, int insertPosition)
    {
        Score insertScore = new Score(newName, newScore);
        for(int i= highScores.Count -1; i >= insertPosition; i--)
        {
            if (i == insertPosition)
                highScores[i] = insertScore;
            else
                highScores[i] = highScores[i - 1];
        }
    }

    public void DebugScores()
    {
        foreach (Score Sc in highScores)
        {
            Debug.Log(Sc.Name + "/" + Sc.PScore);
        }
    }


    public string pathForDocumentsFile(string filename)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(Path.Combine(path, "Documents"), filename);
        }

        else if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }

        else
        {
            string path = Application.dataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
    }
}

public class Score
{
    int pScore;
    string name;

    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            //should sanitize/censor
            name = value;
        }
    }

    public int PScore
    {
        get { return pScore; }
        set { pScore = value; }
    }

    public Score()
    {
        pScore = 0;
        name = "bob";
    }

    public Score(string _name, int _score)
    {
        pScore = _score;
        name = _name;
    }

}
