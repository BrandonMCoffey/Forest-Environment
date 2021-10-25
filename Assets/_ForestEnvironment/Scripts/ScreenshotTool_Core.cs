//==============================================
// Timothy M. Lewis
// Screenshot Tool Core
// Designed for use in Virtual Environments I and II
// Version: 2.1
// Release Date: 4/24/2018
//=============================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[DisallowMultipleComponent]
public class ScreenshotTool_Core : MonoBehaviour {
    [HideInInspector]
    public enum AntiAliasLevel {
        One,
        Two,
        Four,
        Eight
    };


    public IEnumerator ScreenshotAtEndOfFrame(Camera _camera, int _width, int _height, string _saveLocation, AntiAliasLevel _antiAliasLevel = AntiAliasLevel.One)
    {
        bool cameraState = _camera.gameObject.activeSelf;
        bool audioListenerState = false;
        //Since we are enabling the game object, we should disable it's audio listener, just in case;
        AudioListener tempAL = _camera.gameObject.GetComponent<AudioListener>();
        if (tempAL != null) {
            audioListenerState = tempAL.enabled;
            tempAL.enabled = false;
        }
        //Enable camera's gameobject in case there is post or other effects
        _camera.gameObject.SetActive(true);

        int aaLevel = 1;

        switch (_antiAliasLevel) {
            case AntiAliasLevel.One:
                aaLevel = 1;
                break;
            case AntiAliasLevel.Two:
                aaLevel = 2;
                break;
            case AntiAliasLevel.Four:
                aaLevel = 4;
                break;
            case AntiAliasLevel.Eight:
                aaLevel = 8;
                break;
            default:
                aaLevel = 1;
                break;
        }

        yield return new WaitForEndOfFrame();

        RenderTexture rt = new RenderTexture(_width, _height, 24);
        rt.antiAliasing = aaLevel;
        _camera.targetTexture = rt;
        _camera.Render();
        RenderTexture.active = rt;
        Texture2D screenCaptureTexture = new Texture2D(_width, _height, TextureFormat.RGB24, false);
        screenCaptureTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        screenCaptureTexture.Apply();
        RenderTexture.active = null;
        _camera.targetTexture = null;
        byte[] bytes = screenCaptureTexture.EncodeToPNG();
        File.WriteAllBytes(_saveLocation, bytes);


        //Reset camera's gameobject to previous state
        _camera.gameObject.SetActive(cameraState);
        //Reset audiolistener to previous state
        if (tempAL != null) {
            tempAL.enabled = audioListenerState;
        }
        Debug.Log("Screenshot Finished: " + _saveLocation);
    }
}