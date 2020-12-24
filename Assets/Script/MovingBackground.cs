using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackground : MonoBehaviour
{
    float scrollSpeed = -0.65f;
    Vector2 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newPos = Mathf.Repeat(Time.time * scrollSpeed, 20);
        transform.position = startPos + Vector2.right * newPos;
    }
}
