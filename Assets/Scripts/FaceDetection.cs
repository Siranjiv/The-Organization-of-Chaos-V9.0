using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;

public class FaceDetection : MonoBehaviour
{
    //Declare empty texture in the plane
   private WebCamTexture _webCamTexture;
    CascadeClassifier cascade;
    OpenCvSharp.Rect Myface;  //Rectange to store face

    //get the axises of the face
    public float faceY;
    public float faceX;

    void Start()
    {
        //To store the array of webcam Device
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length > 0)
        {
            // To load the camera
            _webCamTexture = new WebCamTexture(devices[0].name);

            // To start the camera 
            //_webCamTexture.Play(); -->Error is not to start the camera at the start of the game
        }
        else
        {
            Debug.LogWarning("No webcam found!");
        }

        //Cascade clasifier is a pre-tained clasifier provided by OpenCV plus
        cascade = new CascadeClassifier("Assets/OpenCV+Unity/Demo/Face_Detector/haarcascade_frontalface_default.xml");
    }

    // Update is called once per frame
    void Update()
    {
        if (_webCamTexture == null || !_webCamTexture.isPlaying)
            return;

        //Webcam Texture is used as the render texture
        GetComponent<Renderer>().material.mainTexture = _webCamTexture;

        //To Store the current frame
        Mat frame = OpenCvSharp.Unity.TextureToMat(_webCamTexture);

        findNewFace(frame);
        display(frame);
    }

    public void StartCamera()
    {
        if (_webCamTexture != null && !_webCamTexture.isPlaying)
        {
            _webCamTexture.Play();
            Debug.Log("Webcam started.");
        }
    }

    public void StopCamera()
    {
        if (_webCamTexture != null && _webCamTexture.isPlaying)
        {
            _webCamTexture.Stop();
            Debug.Log("Webcam stopped.");
        }
    }

    void findNewFace(Mat frame)
    {
        //To detect whether there is a matching image in the frame if there is store it as faces
        var faces = cascade.DetectMultiScale(frame, 1.1, 2, HaarDetectionType.ScaleImage);

        //To detect the co-ordinates of the face
        if (faces.Length >= 1)
        {
            Debug.Log(faces[0].Location);
            Myface = faces[0];
            faceY = faces[0].Y;
            faceX = faces[0].X;
        }
    }

    void display(Mat frame)
    {
        //if Myface contains face data then the rectangle will be shown in the frame
        if (Myface != null)
        {
            
            frame.Rectangle(Myface, new Scalar(250, 0, 0), 2);
        }
        Texture newtexture = OpenCvSharp.Unity.MatToTexture(frame);
        GetComponent<Renderer>().material.mainTexture = newtexture;
    }

    // Stop webcam properly when game stops or object is disabled
    void OnDisable()
    {
        if (_webCamTexture != null && _webCamTexture.isPlaying)
        {
            _webCamTexture.Stop();
        }
    }

    void OnApplicationQuit()
    {
        if (_webCamTexture != null && _webCamTexture.isPlaying)
        {
            _webCamTexture.Stop();
        }
    }
}
