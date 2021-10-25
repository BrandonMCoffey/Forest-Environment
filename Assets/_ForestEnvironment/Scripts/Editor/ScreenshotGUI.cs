//==============================================
// Timothy M. Lewis
// Screenshot Tool GUI
// Designed for use in Virtual Environments I and II
// Version: 2.1
// Release Date: 4/24/2018
//=============================================


using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(ScreenshotTool))]
public class ScreenshotGUI : Editor {
    private int TAB_SIZE = 10; // pixel size of 4 spaces?
    private int indentLevel = 0;
    private ScreenshotTool myScreenshotTool;
    private SerializedObject mySO;
    private SerializedProperty cameras_playerCamera1_Property;
    private SerializedProperty cameras_playerCamera2_Property;
    private SerializedProperty cameras_data_Property;
    private SerializedProperty projectSettings_visible_Property;
    private SerializedProperty projectSettings_active_Property;
    private SerializedProperty projectSettings_firstname_Property;
    private SerializedProperty projectSettings_lastname_Property;
    private SerializedProperty projectSettings_projecNumber_Property;
    private SerializedProperty projectSettings_weekNumber_Property;
    private SerializedProperty stupidProperty;
    private SerializedProperty toolSettings_visible_Property;
    private SerializedProperty toolSettings_playerCameraKey_Property;
    private SerializedProperty toolSettings_previousCameraKey_Property;
    private SerializedProperty toolSettings_nextCameraKey_Property;
    private SerializedProperty toolSettings_takeScreenshotKey_Property;

    void OnEnable()
    {
        myScreenshotTool = (ScreenshotTool) target;
        mySO = new SerializedObject(myScreenshotTool);
        cameras_playerCamera1_Property = mySO.FindProperty("cameras.playerCamera1");
        cameras_playerCamera2_Property = mySO.FindProperty("cameras.playerCamera2");
        cameras_data_Property = mySO.FindProperty("cameras.data");
        projectSettings_visible_Property = mySO.FindProperty("projectSettings.visible");
        projectSettings_active_Property = mySO.FindProperty("projectSettings.active");
        projectSettings_firstname_Property = mySO.FindProperty("projectSettings.firstName");
        projectSettings_lastname_Property = mySO.FindProperty("projectSettings.lastName");
        projectSettings_projecNumber_Property = mySO.FindProperty("projectSettings.projectNumber");
        projectSettings_weekNumber_Property = mySO.FindProperty("projectSettings.weekNumber");
        toolSettings_visible_Property = mySO.FindProperty("toolSettings.visible");
        toolSettings_playerCameraKey_Property = mySO.FindProperty("toolSettings.playerCameraKey");
        toolSettings_previousCameraKey_Property = mySO.FindProperty("toolSettings.previousCameraKey");
        toolSettings_nextCameraKey_Property = mySO.FindProperty("toolSettings.nextCameraKey");
        toolSettings_takeScreenshotKey_Property = mySO.FindProperty("toolSettings.takeScreenshotKey");
    }

    public override void OnInspectorGUI()
    {
        mySO.Update();


        GUIStyle CameraStyle = new GUIStyle();
        CameraStyle.normal.background = MakeTex(4, 4, new Color(0.0f, 0.0f, 0.0f, 0.15f));
        CameraStyle.margin = new RectOffset(0, 0, 0, 4);
        CameraStyle.padding = new RectOffset(4, 4, 4, 4);

        GUIContent content;

        content = new GUIContent("Player Camera 1", "The camera that is used during any given section of the game. In most cases, the playerController camera.");
        EditorGUILayout.PropertyField(cameras_playerCamera1_Property, content);
        content = new GUIContent("Player Camera 2", "The camera that is used during any given section of the game. In most cases, the playerController camera.");
        EditorGUILayout.PropertyField(cameras_playerCamera2_Property, content);
        GUILayout.Space(5);

        indentLevel++;
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(TAB_SIZE * indentLevel);
        EditorGUILayout.BeginVertical();
        toolSettings_visible_Property.boolValue = EditorGUILayout.Foldout(toolSettings_visible_Property.boolValue, "Hotkeys");
        if (toolSettings_visible_Property.boolValue) {
            EditorGUIUtility.labelWidth = 120;
            content = new GUIContent("Player Camera", "This button will always switch back to the player controller camera.");
            EditorGUILayout.PropertyField(toolSettings_playerCameraKey_Property, content);
            content = new GUIContent("Next Camera", "Switch to the next camera in the composition camera list.");
            EditorGUILayout.PropertyField(toolSettings_previousCameraKey_Property, content);
            content = new GUIContent("Previous Camera", "Switch to the previous camera in the composition camera list.");
            EditorGUILayout.PropertyField(toolSettings_nextCameraKey_Property, content);
            content = new GUIContent("Take Screenshots", "Cycle through all the composition cameras and take screeenshots.");
            EditorGUILayout.PropertyField(toolSettings_takeScreenshotKey_Property, content);
            if (myScreenshotTool.toolSettings.takeScreenshotKey == KeyCode.Print) {
                EditorGUILayout.HelpBox("WARNING: You are using Print. This is usually because a user desires to use Print Screen. Print screen is the shift value of System Request on a keyboard. It is likely that you actually want SysReq instead of Print. If this is the case, you can use the button below to change to SysReq", MessageType.Warning);

                GUILayout.Space(5);
                if (GUILayout.Button("Switch to SysReq")) {
                    mySO.ApplyModifiedProperties();
                    myScreenshotTool.toolSettings.takeScreenshotKey = KeyCode.SysReq;
                    mySO.Update();
                }
            }
        }
        EditorGUILayout.EndVertical();
        indentLevel--;
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);


        indentLevel++;
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(TAB_SIZE * indentLevel);
        EditorGUILayout.BeginVertical();
        projectSettings_visible_Property.boolValue = EditorGUILayout.Foldout(projectSettings_visible_Property.boolValue, "Project Settings (Optional)");
        if (projectSettings_visible_Property.boolValue) {
            EditorGUIUtility.labelWidth = 100;
            content = new GUIContent("Enable Project Settings", "Project settings are designed to help keep your screenshots organized for your course. Enter your name, set the Project Number and each week update the week number. Your screenshots will be named appropriatly and organized into weekly folders.");
            projectSettings_active_Property.boolValue = EditorGUILayout.ToggleLeft(content, projectSettings_active_Property.boolValue);
            EditorGUI.BeginDisabledGroup(!projectSettings_active_Property.boolValue);
            EditorGUILayout.PropertyField(projectSettings_firstname_Property);
            EditorGUILayout.PropertyField(projectSettings_lastname_Property);
            GUILayout.Space(5);
            content = new GUIContent("Project", "Which project is this?");
            EditorGUILayout.PropertyField(projectSettings_projecNumber_Property);
            content = new GUIContent("Week", "Which week, during the project, is this?");
            EditorGUILayout.PropertyField(projectSettings_weekNumber_Property);
            EditorGUI.EndDisabledGroup();
        }
        EditorGUILayout.EndVertical();
        indentLevel--;
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);

        if (GUILayout.Button("Add Camera Composition")) {
            mySO.ApplyModifiedProperties();
            myScreenshotTool.cameras.AddCamera();
            mySO.Update();
        }


        EditorGUIUtility.labelWidth = 60;
        for (int i = 0; i < cameras_data_Property.arraySize; i++) {
            SerializedProperty cameraData_Property = cameras_data_Property.GetArrayElementAtIndex(i);

            if (cameraData_Property != null) {
                SerializedProperty cameraData_camera_Property = cameraData_Property.FindPropertyRelative("camera");
                SerializedProperty cameraData_filename_Property = cameraData_Property.FindPropertyRelative("filename");
                SerializedProperty cameraData_active_Property = cameraData_Property.FindPropertyRelative("active");
                SerializedProperty cameraData_visible_Property = cameraData_Property.FindPropertyRelative("visible");
                SerializedProperty cameraData_appendTimestamp_Property = cameraData_Property.FindPropertyRelative("appendTimestamp");
                SerializedProperty cameraData_aaLevel_Property = cameraData_Property.FindPropertyRelative("aaLevel");
                SerializedProperty cameraData_width_Property = cameraData_Property.FindPropertyRelative("width");
                SerializedProperty cameraData_height_Property = cameraData_Property.FindPropertyRelative("height");

                EditorGUILayout.BeginHorizontal(CameraStyle);
                indentLevel++;
                GUILayout.Space(TAB_SIZE * indentLevel);
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                string sectionTitle = ": Unassigned Camera";
                if (cameraData_camera_Property != null && cameraData_camera_Property.objectReferenceValue != null) {
                    sectionTitle = ": " + cameraData_camera_Property.objectReferenceValue.name;
                }
                cameraData_visible_Property.boolValue = EditorGUILayout.Foldout(cameraData_visible_Property.boolValue, i + sectionTitle);
                if (GUILayout.Button("-", GUILayout.Width(20))) {
                    mySO.ApplyModifiedProperties();
                    myScreenshotTool.cameras.RemoveCamera(i);
                    mySO.Update();
                    EditorGUILayout.EndHorizontal();
                } else {
                    EditorGUILayout.EndHorizontal();
                    if (cameraData_visible_Property.boolValue) {
                        GUILayout.Space(5);
                        content = new GUIContent("Enable Camera", "When disabled, cameras will not be used by the tool. No screenshots will be created from them, and you will not be able to switch to them in game.");
                        cameraData_active_Property.boolValue = EditorGUILayout.ToggleLeft(content, cameraData_active_Property.boolValue);
                        EditorGUI.BeginDisabledGroup(!cameraData_active_Property.boolValue);
                        content = new GUIContent("Camera", "The composition camera you will use. Note: The game object which this camera component is attached to will be disabled when not in use by the script");
                        EditorGUILayout.PropertyField(cameraData_camera_Property, content);
                        content = new GUIContent("Filename", "This filename will be added into your screenshot.");
                        EditorGUILayout.PropertyField(cameraData_filename_Property);
                        EditorGUILayout.BeginHorizontal();
                        indentLevel++;
                        GUILayout.Space(TAB_SIZE * indentLevel);
                        EditorGUILayout.BeginVertical();
                        content = new GUIContent("Append Timestamp", "Append a timestamp to the end of your filename. WARNING: Disabling this feature will cause Unity to overwrite your previous screenshots with the same names!");
                        cameraData_appendTimestamp_Property.boolValue = EditorGUILayout.ToggleLeft(content, cameraData_appendTimestamp_Property.boolValue);
                        EditorGUILayout.EndVertical();
                        indentLevel--;
                        EditorGUILayout.EndHorizontal();
                        GUILayout.Label("Resolution:", EditorStyles.boldLabel);
                        EditorGUILayout.PropertyField(cameraData_width_Property);
                        EditorGUILayout.PropertyField(cameraData_height_Property);
                        EditorGUIUtility.labelWidth = 100;
                        content = new GUIContent("Antialiasing", "What level of antialiasing do you want?");
                        EditorGUILayout.PropertyField(cameraData_aaLevel_Property, content);
                        EditorGUIUtility.labelWidth = 60;
                        EditorGUI.EndDisabledGroup();
                    }
                }
                EditorGUILayout.EndVertical();
                indentLevel--;
                EditorGUILayout.EndHorizontal();
            }
        }

        mySO.ApplyModifiedProperties();
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];

        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();

        return result;
    }
}