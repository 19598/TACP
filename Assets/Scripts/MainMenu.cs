using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Transform camera;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//moves to next scene
    }

    public void QuitGame()
    {
        Application.Quit();//quits the game
    }

    public void Update()
    {
        camera.Rotate(0, 0.05f, 0);//slowly rotates the camera, just for fun
    }
}
