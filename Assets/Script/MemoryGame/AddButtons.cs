using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddButtons : MonoBehaviour
{
    [SerializeField]
    private Transform puzzleField;

    [SerializeField]
    private GameObject puzzleButton;

    public void GenerateGrid(int tileCount)
    {
        for(int i = 0; i < tileCount; i++)
        {
            GameObject button = Instantiate(puzzleButton) as GameObject;
            button.name = "Button " + i;
            button.transform.SetParent(puzzleField, false);
        }
        Debug.Log("Grid Generated");
    }

    public void DestroyGrid()
    {
        var objectsWithTag = GameObject.FindGameObjectsWithTag("PuzzleButton_Tag");
        for (int i = objectsWithTag.Length - 1; i > -1; i--)
            DestroyImmediate(objectsWithTag[i]);
        Debug.Log("Grid Destroyed");
    }
}
