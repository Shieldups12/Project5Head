using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddButtons : MonoBehaviour
{
    [SerializeField]
    private Transform puzzleField;

    [SerializeField]
    private GameObject puzzleButton;

    void Awake()
    {
        for(int i = 0; i < 9; i++)
        {
            GameObject button = Instantiate(puzzleButton) as GameObject;
            button.name = "Button " + i;
            button.transform.SetParent(puzzleField, false);
        }
    }
}
