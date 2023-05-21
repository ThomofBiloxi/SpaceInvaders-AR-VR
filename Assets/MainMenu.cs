using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    // public void OnPointerDown(PointerEventData eventData)
    // {
    //     string buttonName = gameObject.name;
    //     Debug.Log("Button Name: " + buttonName);

    //     // Call the respective functions based on the button name.
    //     if (buttonName == "StartButton")
    //     {
    //         PlayGame();
    //     }
    //     else if (buttonName == "QuitButton")
    //     {
    //         QuitGame();
    //     }
    // }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
