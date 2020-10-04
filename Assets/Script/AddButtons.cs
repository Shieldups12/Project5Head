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
        for(int i = 0; i < 12; i++)
        {
            GameObject button = Instantiate(puzzleButton);
            button.name = "Button " + i;
            button.transform.SetParent(puzzleField, false);
        }
    }
}
