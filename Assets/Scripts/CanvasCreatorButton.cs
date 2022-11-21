#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(GuiCode))]
public class CanvasCreatorButton : Editor
{
    //private string a
    public override void OnInspectorGUI()
    {
        
        GuiCode guiScript = (GuiCode)target;
        GUILayout.Label("Content Creation Buttons");
        if (GUILayout.Button("Create High Scores Display"))
        {
            guiScript.CreateHighScoresDisplay();
            guiScript.IsAHighScoresScreen = true;
        }
        if (GUILayout.Button("Create Scene Navigation"))
        {
            guiScript.CreateNavigation();
        }
        if (GUILayout.Button("Create PauseUI"))
        {
            guiScript.CreatePauseUI();
            guiScript.IsItAPausableScene = true;
        }
        DrawDefaultInspector();
        
    }
}
#endif