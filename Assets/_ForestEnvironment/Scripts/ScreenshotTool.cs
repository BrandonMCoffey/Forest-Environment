//==============================================
// Timothy M. Lewis
// Screenshot Tool
// Designed for use in Virtual Environments I and II
// Version: 2.1
// Release Date: 4/24/2018
//=============================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(ScreenshotTool_Core))]
public class ScreenshotTool : MonoBehaviour {
    [System.Serializable]
    public class Cameras {
        [SerializeField]
        public Camera playerCamera1;
        public Camera playerCamera2;

        [SerializeField]
        public List<CameraData> data = new List<CameraData>();

        public void AddCamera()
        {
            data.Add(new CameraData());
        }

        public void RemoveCamera(int _index)
        {
            data.RemoveAt(_index);
        }

        public void DisableAllCameras()
        {
            if (playerCamera1 != null) playerCamera1.gameObject.SetActive(false);
            if (playerCamera2 != null) playerCamera2.gameObject.SetActive(false);
            foreach (CameraData CD in data) {
                if (CD.camera != null)
                    CD.camera.gameObject.SetActive(false);
            }
        }

        public bool firstPlayerCamera = true;
    }

    [System.Serializable]
    public class CameraData {
        [SerializeField]
        public bool active = true;
        [SerializeField]
        public bool visible = true;
        [SerializeField]
        public bool appendTimestamp = true;
        [SerializeField]
        public int width = 1920;
        [SerializeField]
        public int height = 1080;
        [SerializeField]
        public string filename = "";
        [SerializeField]
        public Camera camera;
        [SerializeField]
        public ScreenshotTool_Core.AntiAliasLevel aaLevel;

        public CameraData()
        {
            active = true;
        }

        public bool Useable()
        {
            if (camera == null)
                return false;

            return active;
        }
    }

    [System.Serializable]
    public class ProjectSettings {
        [SerializeField]
        public bool active = false;
        [SerializeField]
        public bool visible = true;
        [SerializeField]
        public string firstName;
        [SerializeField]
        public string lastName;

        [SerializeField]
        public enum ProjectNumber {
            Project1,
            Project2,
            Project3,
            Project4
        };

        [SerializeField]
        public ProjectNumber projectNumber;

        [SerializeField]
        public enum WeekNumber {
            Week1,
            Week2,
            Week3,
            Week4,
            Week5
        };

        [SerializeField]
        public WeekNumber weekNumber;

        public string GetWeekNumber()
        {
            string sWeekNumber = "";

            switch (weekNumber) {
                case WeekNumber.Week1:
                    sWeekNumber = "01";
                    break;
                case WeekNumber.Week2:
                    sWeekNumber = "02";
                    break;
                case WeekNumber.Week3:
                    sWeekNumber = "03";
                    break;
                case WeekNumber.Week4:
                    sWeekNumber = "04";
                    break;
                case WeekNumber.Week5:
                    sWeekNumber = "05";
                    break;
                default:
                    sWeekNumber = "01";
                    break;
            }

            return sWeekNumber;
        }

        public string GetProjectNumber()
        {
            string sProjectNumber = "";

            switch (projectNumber) {
                case ProjectNumber.Project1:
                    sProjectNumber = "01";
                    break;
                case ProjectNumber.Project2:
                    sProjectNumber = "02";
                    break;
                case ProjectNumber.Project3:
                    sProjectNumber = "03";
                    break;
                case ProjectNumber.Project4:
                    sProjectNumber = "04";
                    break;
                default:
                    sProjectNumber = "01";
                    break;
            }

            return sProjectNumber;
        }
    }

    [System.Serializable]
    public class ToolSettings {
        [SerializeField]
        public bool visible = true;
        [SerializeField]
        public KeyCode playerCameraKey = KeyCode.F1;
        [SerializeField]
        public KeyCode previousCameraKey = KeyCode.F2;
        [SerializeField]
        public KeyCode nextCameraKey = KeyCode.F3;
        [SerializeField]
        public KeyCode takeScreenshotKey = KeyCode.SysReq;
    }


    private GameObject goLight;
    private Light spotLight;
    private int cameraViewingIndex = -1;
    private ScreenshotTool_Core core;

    [SerializeField]
    public ProjectSettings projectSettings = new ProjectSettings();
    [SerializeField]
    public Cameras cameras = new Cameras();
    [SerializeField]
    public ToolSettings toolSettings = new ToolSettings();

    // Use this for initialization
    void Start()
    {
        goLight = new GameObject("specializedLight");
        spotLight = goLight.AddComponent<Light>();
        spotLight.type = LightType.Spot;
        goLight.SetActive(false);


        core = this.GetComponent<ScreenshotTool_Core>();

        ScreenshotTool[] screenshotTools = FindObjectsOfType<ScreenshotTool>();

        if (screenshotTools.Length > 1) {
            Debug.LogWarning("You have " + screenshotTools.Length + " screenshot tool components in your scene! You should only have 1!");
            for (int i = 0; i < screenshotTools.Length; i++)
                Debug.LogWarning(screenshotTools[i]);
        }

        cameras.DisableAllCameras();

        if (cameras.playerCamera1 != null) {
            cameras.playerCamera1.gameObject.SetActive(true);
            if (cameras.playerCamera2 != null) cameras.firstPlayerCamera = true;
        } else {
            if (cameras.playerCamera2 != null) cameras.playerCamera2.gameObject.SetActive(false);
            for (int i = 0; i < cameras.data.Count; i++) {
                if (cameras.data[i].camera != null && cameras.data[i].camera.gameObject.activeSelf == true) {
                    cameras.data[i].camera.gameObject.SetActive(true);
                    i = cameras.data.Count;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(toolSettings.playerCameraKey)) {
            if (!cameras.firstPlayerCamera) {
                if (cameras.playerCamera1 != null) {
                    Debug.Log("Viewing Camera: Player 1");
                    cameras.DisableAllCameras();
                    cameras.playerCamera1.gameObject.SetActive(true);
                    cameraViewingIndex = -1;
                } else {
                    Debug.Log("Player Camera is not set!");
                }
                if (cameras.playerCamera2 != null) cameras.firstPlayerCamera = true;
            } else {
                if (cameras.playerCamera2 != null) {
                    Debug.Log("Viewing Camera: Player 2");
                    cameras.DisableAllCameras();
                    cameras.playerCamera2.gameObject.SetActive(true);
                    cameraViewingIndex = -1;
                } else {
                    Debug.Log("Player Camera 2 is not set!");
                }
                cameras.firstPlayerCamera = false;
            }
        }
        if (Input.GetKeyDown(toolSettings.previousCameraKey)) {
            cameras.DisableAllCameras();
            cameraViewingIndex--;
            if (cameraViewingIndex < 0)
                cameraViewingIndex = cameras.data.Count - 1;

            if (cameras.data[cameraViewingIndex].Useable()) {
                cameras.data[cameraViewingIndex].camera.gameObject.SetActive(true);
                Debug.Log("Viewing Camera: " + cameras.data[cameraViewingIndex].camera.gameObject.name);
            }
        }
        if (Input.GetKeyDown(toolSettings.nextCameraKey)) {
            cameras.DisableAllCameras();
            cameraViewingIndex++;
            if (cameraViewingIndex >= cameras.data.Count)
                cameraViewingIndex = 0;

            if (cameras.data[cameraViewingIndex].Useable()) {
                cameras.data[cameraViewingIndex].camera.gameObject.SetActive(true);
                Debug.Log("Viewing Camera: " + cameras.data[cameraViewingIndex].camera.gameObject.name);
            }
        }

        if (Input.GetKeyDown(toolSettings.takeScreenshotKey)) {
            for (int i = 0; i < cameras.data.Count; i++) {
                if (cameras.data[i].Useable()) {
                    string tempFilename = Application.dataPath + "//Screenshots//";
                    if (projectSettings.active)
                        tempFilename += "Week" + projectSettings.GetWeekNumber() + "//" + projectSettings.lastName + projectSettings.firstName + "_";

                    if (cameras.data[i].filename.Trim() != "")
                        tempFilename += cameras.data[i].filename;
                    else
                        tempFilename += "Camera" + i;

                    if (projectSettings.active)
                        tempFilename += "_P" + projectSettings.GetProjectNumber() + "_W" + projectSettings.GetWeekNumber();
                    if (cameras.data[i].appendTimestamp)
                        tempFilename += "-T" + System.DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss-fff");
                    tempFilename += ".png";

                    StartCoroutine(core.ScreenshotAtEndOfFrame(cameras.data[i].camera, cameras.data[i].width, cameras.data[i].height, tempFilename, cameras.data[i].aaLevel));
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F9)) {
            //Activate Spotlight
            if (!goLight.activeSelf) {
                goLight.SetActive(true);
            } else {
                goLight.SetActive(false);
            }
        }

        if (goLight.activeSelf) {
            try {
                ActivateSpotLight();
            } catch (Exception e) {
                Debug.LogException(e, this);
            }
        }
    }

    private void ActivateSpotLight()
    {
        goLight.transform.rotation = Camera.main.gameObject.transform.rotation;
        goLight.transform.position = Camera.main.gameObject.transform.position;

        bool shift = false;

        if (Input.GetKey(KeyCode.LeftShift)) {
            shift = true;
        }

        bool ctrl = false;

        if (Input.GetKey(KeyCode.LeftControl)) {
            ctrl = true;
        }

        if (Input.GetKey(KeyCode.F10)) {
            if (shift) {
                spotLight.range -= 0.3f;
            } else if (ctrl) {
                spotLight.spotAngle -= 0.3f;
            } else {
                spotLight.intensity -= 0.3f;
            }
        }

        if (Input.GetKey(KeyCode.F11)) {
            if (shift) {
                spotLight.range += 0.3f;
            } else if (ctrl) {
                spotLight.spotAngle += 0.3f;
            } else {
                spotLight.intensity += 0.3f;
            }
        }
    }
}