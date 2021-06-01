using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoardManager))]
public class BoardManagerEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        BoardManager board = (BoardManager)target;


        if (GUILayout.Button("Generate")) {
            board.SetupGame();
        }
    }
}
