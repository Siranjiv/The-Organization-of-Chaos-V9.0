using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GamePlayUIController : MonoBehaviour
{

    private void StopWebcamIfActive()
    {
        FaceDetection faceDetection = FindObjectOfType<FaceDetection>();
        if (faceDetection != null)
        {
            faceDetection.StopCamera();  // This is the public method from your FaceDetection.cs
        }
    }

    public void RestartGame() {

        StopWebcamIfActive();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ScoreScript.scoreValue = 0;
    }

    public void HomeButton() {

        StopWebcamIfActive();

        SceneManager.LoadScene("MainMenu");
        ScoreScript.scoreValue = 0;

    }
}
