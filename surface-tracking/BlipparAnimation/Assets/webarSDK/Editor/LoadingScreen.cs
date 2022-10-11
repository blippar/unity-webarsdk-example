using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditorInternal;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class LoadingScreen : EditorWindow
{
    [MenuItem("Blippar/ Loading Screen")]
    public static void showLisanceWindow()
    {
        GetWindow<LoadingScreen>("LoadingScreen");
    }

    Vector2 scrollPosition = Vector2.zero;

    int index;
    int lastIndex = -1;
    private List<string> list = new List<string>();
    private Texture2D b_Logo = null;
    private Texture2D button_tex;
    private GUIContent button_tex_con;
    //public List<String> options = new string[] { GetTemplateFiles(). };

    LoadingScreenData defaultScreenData;
    LoadingScreenData selectedScreenData;
    LoadingScreenData dataToSave = new LoadingScreenData();
    LoadingData loadingData = new LoadingData();

    Regex r = new Regex("^[a-zA-Z0-9]*$");

    string loadigScreenPath = "Assets/webarSDK/Editor/LoadingScreenTemplates/DefaultLoadingScreen.json";
    string _selectedTemplate;
    string _buildPath;
    string _applicationDataPath;
    string _templateName = "";
    string _trackingAnimURL;

    string logo_src;
    string logo_width;
    string logo_height;
    string progress_dot_ring_scale;
    string progress_dot_ring_color;
    string progress_ring_scale;
    string progress_ring_color;
    string progress_ring_line_width;

    string alert_border_color;
    string alert_border_width;
    string alert_background_color;
    string alert_message_text_color;
    string alert_button_color;
    string alert_button_text_color;
    string alert_button_height;
    string alert_camera_permission_text;
    string alert_camera_permission_button_text;
    string alert_motion_permission_text;
    string alert_motion_permission_button_text;

    string ui_background_color;
    string ui_text_color;
    string desktop_logo_src;
    string desktop_logo_width;
    string desktop_logo_height;
    string issue_img_src;
    string issue_img_width;
    string issue_img_height;

    private void CreateFile()
    {
        if (File.Exists(loadigScreenPath))
            return;
        loadigScreenPath = "Assets/webarSDK/Editor/LoadingScreenTemplates/DefaultLoadingScreen.json";
    }

    void WriteFile()
    {
        string jsonString = JsonUtility.ToJson(defaultScreenData);
        File.WriteAllText("Assets/webarSDK/Editor/LoadingScreenTemplates/DefaultLoadingScreen.json", jsonString);
    }

    public void readCustomFile()
    {
        if (File.Exists("Assets/webarSDK/Resources/LoadingData.json"))
        {
            //string fileContents = File.ReadAllText("Assets/webarSDK/Resources/LoadingData.json");
            //loadingData = JsonUtility.FromJson<LoadingData>(fileContents);
            TextAsset txtAsset = (TextAsset)Resources.Load("LoadingData", typeof(TextAsset));
            string tileFile = txtAsset.text;
            loadingData = JsonUtility.FromJson<LoadingData>(tileFile);
        }
    }

    public void writeCustomFile()
    {
        string jsonString = JsonUtility.ToJson(loadingData);
        File.WriteAllText("Assets/webarSDK/Resources/LoadingData.json", jsonString);
    }

    void StoreTempData()
    {
        if (selectedScreenData != null)
        {
            _trackingAnimURL = "";

            logo_src = selectedScreenData.logo_src;
            logo_width = selectedScreenData.logo_width;
            logo_height = selectedScreenData.logo_height;
            progress_dot_ring_scale = selectedScreenData.progress_dot_ring_scale;
            progress_dot_ring_color = selectedScreenData.progress_dot_ring_color;
            progress_ring_scale = selectedScreenData.progress_ring_scale;
            progress_ring_color = selectedScreenData.progress_ring_color;
            progress_ring_line_width = selectedScreenData.progress_ring_line_width;

            alert_border_color = selectedScreenData.alert_border_color;
            alert_border_width = selectedScreenData.alert_border_width;
            alert_background_color = selectedScreenData.alert_background_color;
            alert_message_text_color = selectedScreenData.alert_message_text_color;
            alert_button_color = selectedScreenData.alert_button_color;
            alert_button_text_color = selectedScreenData.alert_button_text_color;
            alert_button_height = selectedScreenData.alert_button_height;
            alert_camera_permission_text = selectedScreenData.alert_camera_permission_text;
            alert_camera_permission_button_text = selectedScreenData.alert_camera_permission_button_text;
            alert_motion_permission_text = selectedScreenData.alert_motion_permission_text;
            alert_motion_permission_button_text = selectedScreenData.alert_motion_permission_button_text;

            ui_background_color = selectedScreenData.ui_background_color;
            ui_text_color = selectedScreenData.ui_text_color;
            desktop_logo_src = selectedScreenData.desktop_logo_src;
            desktop_logo_width = selectedScreenData.desktop_logo_width;
            desktop_logo_height = selectedScreenData.desktop_logo_height;
            issue_img_src = selectedScreenData.issue_img_src;
            issue_img_width = selectedScreenData.issue_img_width;
            issue_img_height = selectedScreenData.issue_img_height;
            _trackingAnimURL = selectedScreenData._trackingAnimURL;
        }

    }

    private void OnEnable()
    {
        b_Logo = (Texture2D)Resources.Load("logo", typeof(Texture2D));
        _applicationDataPath = Application.dataPath;
        _buildPath = _applicationDataPath.Replace("Assets", "");

        button_tex = (Texture2D)Resources.Load("Refresh", typeof(Texture2D));
        button_tex_con = new GUIContent(button_tex);
    }

    private void Update()
    {
        //CreateFile();
        //WriteFile();
    }

    public List<String> GetTemplateFiles()
    {
        list.Clear();
        DirectoryInfo dir = new DirectoryInfo("Assets/webarSDK/Editor/LoadingScreenTemplates/");
        FileInfo[] info = dir.GetFiles("*.json");
        foreach (FileInfo f in info)
        {
            string filename = f.Name.Substring(0, f.Name.LastIndexOf("."));
            list.Add(filename);
        }
        return list;
    }

    void ReadSelectedFile()
    {
        if (File.Exists("Assets/webarSDK/Editor/LoadingScreenTemplates/" + _selectedTemplate + ".json"))
        {
            string fileContents = File.ReadAllText("Assets/webarSDK/Editor/LoadingScreenTemplates/" + _selectedTemplate + ".json");
            selectedScreenData = JsonUtility.FromJson<LoadingScreenData>(fileContents);
        }
    }

    private void OnGUI()
    {
        GUILayout.Label(b_Logo, EditorStyles.centeredGreyMiniLabel);
        GUILayout.Space(5);
        GUILayout.Label("Select template for build", EditorStyles.boldLabel, GUILayout.MaxWidth(200));
        GUILayout.BeginHorizontal();
        GUILayout.Label("Select Template", EditorStyles.boldLabel, GUILayout.MaxWidth(216));
        index = EditorGUILayout.Popup(index, GetTemplateFiles().ToArray());
        if (GUILayout.Button(button_tex_con, GUILayout.MaxWidth(18), GUILayout.MaxHeight(18)))
        {
            ReadSelectedFile();
            StoreTempData();
            lastIndex = index;
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(15);
        GUILayout.Label("Customise selected Template", EditorStyles.boldLabel, GUILayout.MaxWidth(200));

        GUILayout.Space(5);
        GUILayout.Label("* Copy required images into the" + " Assets/WEBGLTemplates/Blippar/images", EditorStyles.boldLabel, GUILayout.MaxWidth(1000));

        if (GetTemplateFiles().ToArray().Count() > 0)
        {
            _selectedTemplate = GetTemplateFiles().ToArray()[index].ToString();
            readCustomFile();
            loadingData._selectedTemplate = _selectedTemplate;
            loadingData._selectedAnimURL = _trackingAnimURL;
            writeCustomFile();
            if (lastIndex != index)
            {
                ReadSelectedFile();
                StoreTempData();
                lastIndex = index;
            }
        }
        if (selectedScreenData == null)
            return;
        GUILayout.Space(5);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Logo Name", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        logo_src = GUILayout.TextField(logo_src);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Logo Width", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        logo_width = GUILayout.TextField(logo_width);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Logo Height", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        logo_height = GUILayout.TextField(logo_height);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Progress Dot Ring Scale", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        progress_dot_ring_scale = GUILayout.TextField(progress_dot_ring_scale);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Progress Dot Ring Colour", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        progress_dot_ring_color = GUILayout.TextField(progress_dot_ring_color);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Progress Ring Scale", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        progress_ring_scale = GUILayout.TextField(progress_ring_scale);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Progress Ring Colour", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        progress_ring_color = GUILayout.TextField(progress_ring_color);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Progress Ring Line Width", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        progress_ring_line_width = GUILayout.TextField(progress_ring_line_width);
        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Alert Border Colour", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        alert_border_color = GUILayout.TextField(alert_border_color);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Alert Border Width", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        alert_border_width = GUILayout.TextField(alert_border_width);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Alert Background Colour", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        alert_background_color = GUILayout.TextField(alert_background_color);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Alert Message Text Colour", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        alert_message_text_color = GUILayout.TextField(alert_message_text_color);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Alert Button Colour", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        alert_button_color = GUILayout.TextField(alert_button_color);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Alert Button Text Colour", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        alert_button_text_color = GUILayout.TextField(alert_button_text_color);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Alert Button Height", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        alert_button_height = GUILayout.TextField(alert_button_height);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Alert Camera Permission Text", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        alert_camera_permission_text = GUILayout.TextField(alert_camera_permission_text);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Alert Camera Permission Button Text", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        alert_camera_permission_button_text = GUILayout.TextField(alert_camera_permission_button_text);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Alert Motion Permission Text", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        alert_motion_permission_text = GUILayout.TextField(alert_motion_permission_text);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Alert Motion Permission Button Text", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        alert_motion_permission_button_text = GUILayout.TextField(alert_motion_permission_button_text);
        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        GUILayout.Label("UI Background Colour", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        ui_background_color = GUILayout.TextField(ui_background_color);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("UI Text Colour", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        ui_text_color = GUILayout.TextField(ui_text_color);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Desktop Logo", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        desktop_logo_src = GUILayout.TextField(desktop_logo_src);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Desktop Logo Width", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        desktop_logo_width = GUILayout.TextField(desktop_logo_width);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Desktop Logo Height", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        desktop_logo_height = GUILayout.TextField(desktop_logo_height);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Issue Image", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        issue_img_src = GUILayout.TextField(issue_img_src);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Issue Image Width", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        issue_img_width = GUILayout.TextField(issue_img_width);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Issue Image Height", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        issue_img_height = GUILayout.TextField(issue_img_height);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.EndScrollView();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Tracking Animation URL", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        _trackingAnimURL = GUILayout.TextField(_trackingAnimURL);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Save Template", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));

        if (GUILayout.Button("Save Template"))
        {
            if (loadingData != null)
            {
                if (loadingData._selectedTemplate == "DefaultLoadingScreen")
                {
                    EditorUtility.DisplayDialog("Warning", "No permission to save default template, Try Save as a new", "ok");
                    _trackingAnimURL = "";
                    ReadSelectedFile();
                    StoreTempData();
                    lastIndex = index;
                }
                else
                {
                    SaveData();
                    readCustomFile();
                    loadingData._selectedAnimURL = _trackingAnimURL;
                    writeCustomFile();
                    string jsonString = JsonUtility.ToJson(dataToSave);
                    File.WriteAllText("Assets/webarSDK/Editor/LoadingScreenTemplates/" + _selectedTemplate + ".json", jsonString);

                    EditorUtility.DisplayDialog("Congratulations", "Data Saved successfully", "ok");
                    ReadSelectedFile();
                    StoreTempData();
                }
            }

        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Save as a new Template", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        _templateName = GUILayout.TextField(_templateName);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        if (GUILayout.Button("Save as a new Template"))
        {
            if (_templateName == "")
            {
                EditorUtility.DisplayDialog("Warning", "Please enter a name", "ok");
            }
            else 
            {
                if (r.IsMatch(_templateName))
                {
                    SaveData();

                    string jsonString = JsonUtility.ToJson(dataToSave);
                    File.WriteAllText("Assets/webarSDK/Editor/LoadingScreenTemplates/" + _templateName + ".json", jsonString);


                    EditorUtility.DisplayDialog("Congratulations", "Data Saved successfully", "ok");
                    _selectedTemplate = _templateName;
                    _templateName = "";
                    ReadSelectedFile();
                    StoreTempData();
                }
                else
                {
                    _templateName = "";
                    EditorUtility.DisplayDialog("Warning", "Please enter a name in [a-z]/[A-Z]/[0-9] ", "ok");
                }
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20);
    }

    void SaveData()
    {
        dataToSave.logo_src = logo_src;
        dataToSave.logo_width = logo_width;
        dataToSave.logo_height = logo_height;
        dataToSave.progress_dot_ring_scale = progress_dot_ring_scale;
        dataToSave.progress_dot_ring_color = progress_dot_ring_color;
        dataToSave.progress_ring_scale = progress_ring_scale;
        dataToSave.progress_ring_color = progress_ring_color;
        dataToSave.progress_ring_line_width = progress_ring_line_width;

        dataToSave.alert_border_color = alert_border_color;
        dataToSave.alert_border_width = alert_border_width;
        dataToSave.alert_background_color = alert_background_color;
        dataToSave.alert_message_text_color = alert_message_text_color;
        dataToSave.alert_button_color = alert_button_color;
        dataToSave.alert_button_text_color = alert_button_text_color;
        dataToSave.alert_button_height = alert_button_height;
        dataToSave.alert_camera_permission_text = alert_camera_permission_text;
        dataToSave.alert_camera_permission_button_text = alert_camera_permission_button_text;
        dataToSave.alert_motion_permission_text = alert_motion_permission_text;
        dataToSave.alert_motion_permission_button_text = alert_motion_permission_button_text;

        dataToSave.ui_background_color = ui_background_color;
        dataToSave.ui_text_color = ui_text_color;
        dataToSave.desktop_logo_src = desktop_logo_src;
        dataToSave.desktop_logo_width = desktop_logo_width;
        dataToSave.desktop_logo_height = desktop_logo_height;
        dataToSave.issue_img_src = issue_img_src;
        dataToSave.issue_img_width = issue_img_width;
        dataToSave.issue_img_height = issue_img_height;
        dataToSave._trackingAnimURL = _trackingAnimURL;
    }
}
