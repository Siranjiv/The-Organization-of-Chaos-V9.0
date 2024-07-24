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

    public void HandleInputData(int val)
    {
        if (val == 0)
        {
            //to enable keyboard and mouse controller and disable
            //other controllers

            keyboard.enabled = true;
            shoot.enabled = true;
            voicereg.enabled = false;
            faceCon.enabled = false;
        }
        if (val == 1)
        {
            //To enable voice recognition controller and disable
            //other controllers
            keyboard.enabled = false;
            shoot.enabled = false;
            voicereg.enabled = true;
            faceCon.enabled = false;

        }
        if (val == 2)
        {
            //to enable face recognition controller and disable
            //other controllers
            keyboard.enabled = false;
            shoot.enabled = false;
            voicereg.enabled = false;
            faceCon.enabled = true;


        }

    }
}
