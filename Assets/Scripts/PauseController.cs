using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class PauseController : MonoBehaviour
{

    public static bool gameIsPaused;
    public GameObject pausedText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }

    void PauseGame()
    {
        if (gameIsPaused)
        {
            Debug.Log("Game is paused");
            Time.timeScale = 0;
            pausedText.SetActive(true);
        } 
        else
        {
            Debug.Log("Game is un-paused");
            Time.timeScale = 1;
            pausedText.SetActive(false);
        }
    }
}
