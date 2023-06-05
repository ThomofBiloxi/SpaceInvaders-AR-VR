using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class PauseController : MonoBehaviour
{

    public static bool gameIsPaused;
    public GameObject pausedText;
    public GameObject quitButton;


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
            quitButton.SetActive(true);
        } 
        else
        {
            Debug.Log("Game is un-paused");
            Time.timeScale = 1;
            pausedText.SetActive(false);
            quitButton.SetActive(false);
        }
    }

    public void QuitToMain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
