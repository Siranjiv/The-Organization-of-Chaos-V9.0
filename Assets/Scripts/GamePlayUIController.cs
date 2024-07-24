using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GamePlayUIController : MonoBehaviour
{

    public void RestartGame() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ScoreScript.scoreValue = 0;
    }

    public void HomeButton() {

        SceneManager.LoadScene("MainMenu");
        ScoreScript.scoreValue = 0;

    }
}
