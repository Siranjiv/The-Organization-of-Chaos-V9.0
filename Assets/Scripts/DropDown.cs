using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropDown : MonoBehaviour
{

    public TextMeshProUGUI output;

    public Weapon shoot;
    public Player keyboard;
    public VoiceRecognition voicereg;
    public FaceController faceCon;
    public FaceDetection faceDetection; // Reference to FaceDetection component

    void Start()
    {
        // Automatically assign all controller references
        shoot = FindObjectOfType<Weapon>();
        keyboard = FindObjectOfType<Player>();
        voicereg = FindObjectOfType<VoiceRecognition>();
        faceCon = FindObjectOfType<FaceController>();
        faceDetection = FindObjectOfType<FaceDetection>();

        if (shoot == null || keyboard == null || voicereg == null || faceCon == null || faceDetection == null)
        {
            Debug.LogError("One or more controller scripts could not be found. Check your scene setup.");
        }

        HandleInputData(0); // Default to keyboard & mouse on start
    }
    public void HandleInputData(int val)
    {
        DisableAllControllers();

        if (val == 0)
        {
            Debug.Log("Keyboard mode enabled");
            keyboard.enabled = true;
            shoot.enabled = true;

            faceDetection.StopCamera();
        }
        else if (val == 1)
        {
            Debug.Log("Voice mode enabled");
            voicereg.enabled = true;

            faceDetection.StopCamera();
        }
        else if (val == 2)
        {
            Debug.Log("Face mode enabled");
            faceCon.enabled = true;
            faceDetection.StartCamera();
        }
    }
    private void DisableAllControllers()
    {
        keyboard.enabled = false;
        shoot.enabled = false;
        voicereg.enabled = false;
        faceCon.enabled = false;

        // Optional safety reset
        faceDetection.StopCamera();
    }


    #region OldCode

    //public void HandleInputData(int val)
    //{
    //    if (val == 0)
    //    {
    //        //to enable keyboard and mouse controller and disable
    //        //other controllers

    //        keyboard.enabled = true;
    //        shoot.enabled = true;
    //        voicereg.enabled = false;
    //        faceCon.enabled = false;
    //    }
    //    if (val == 1)
    //    {
    //        //To enable voice recognition controller and disable
    //        //other controllers
    //        keyboard.enabled = false;
    //        shoot.enabled = false;
    //        voicereg.enabled = true;
    //        faceCon.enabled = false;

    //    }
    //    if (val == 2)
    //    {
    //        //to enable face recognition controller and disable
    //        //other controllers
    //        keyboard.enabled = false;
    //        shoot.enabled = false;
    //        voicereg.enabled = false;
    //        faceCon.enabled = true;


    //    }

    //}
    #endregion
}
