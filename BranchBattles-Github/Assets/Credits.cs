using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public float initialSpeed = 100f;   // Initial movement speed.
    public float acceleratedSpeed = 300f; // Accelerated movement speed when mouse is held.
    //public float moveDuration = 2f;      // Total movement duration in seconds.
    public float positionToLeaveCredits;

    private RectTransform rectTransform;
    private float yOffset;
    //private float startTime;
    private float currentSpeed;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        
        yOffset = 0;
    }

    private void Update()
    {
        yOffset += currentSpeed * Time.deltaTime;

        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yOffset);

        if (Input.GetMouseButton(0))
        {
            currentSpeed = acceleratedSpeed;
        }
        else
        {
            currentSpeed = initialSpeed;
        }

        if (yOffset > positionToLeaveCredits) {
            SceneManager.LoadScene("LevelSelect");
        }
    }
}
