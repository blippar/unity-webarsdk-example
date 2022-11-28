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


    Vector2 scrollPosition = Vector2.zero;

    int index;
    int lastIndex = -1;
    private List<string> list = new List<string>();
    private List<string> tempList;
    private Texture2D b_Logo = null;
    private Texture2D button_tex;
    private GUIContent button_tex_con;

    //public List<String> options = new string[] { GetTemplateFiles(). };

    LoadingScreenData defaultScreenData;
    LoadingScreenData selectedScreenData;
    LoadingScreenData dataToSave = new LoadingScreenData();
    LoadingData loadingData = new LoadingData();
    CustomData customData = new CustomData();
    BlipparManager blipparManager;

    Regex r = new Regex("^[a-zA-Z0-9]*$");

    string loadigScreenPath = "Assets/webarSDK/Editor/LoadingScreenTemplates/DefaultLoadingScreen.json";
    string _selectedTemplate;
    string _buildPath;
    string _applicationDataPath;
    string _templateName = "";
    bool showGuideView;
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
    string alert_box_font_size;
    string alert_box_height;
    string alert_box_width;
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

    string auto_scan_instruction;
    string auto_scan_instruction_detect;
    string auto_scan_instruction_idle;
    string auto_scan_instruction_text_color;
    string auto_scan_style_display;
    string auto_scan_style_position;
    string auto_scan_style_top;
    string auto_scan_style_left;
    string auto_scan_style_transform_translate;
    string auto_scan_style_z_index;
    string auto_scan_style_width;
    string auto_scan_style_height;
    string auto_scan_text_align;

    string scan_button_display;
    string scan_btn_height;
    string scan_btn_width;
    string scan_btn_img_height;
    string scan_btn_img_width;
    string scan_btn_img_src;
    string scan_btn_img_transform_trsnslate;
    string scan_btn_img_x_coordinate;
    string scan_btn_img_y_coordinate ;

    string scan_btn_instruction;
    string scan_btn_instruction_color;

    string scan_btn_progress_bar_color;
    string scan_btn_progress_bar_cx_coordinate;
    string scan_btn_progress_bar_cy_coordinate;
    string scan_btn_progress_bar_radius;
    string scan_btn_progress_bar_rotate_x;
    string scan_btn_progress_bar_rotate_y;
    string scan_btn_progress_bar_rotate_z;

    string scan_btn_progress_circle_cx_coordinate;
    string scan_btn_progress_circle_cy_coordinate ;
    string scan_btn_progress_circle_radius;
    string scan_btn_progress_circle_style_fill;
    string scan_btn_progress_circle_style_stroke;
    string scan_btn_progress_circle_style_stroke_width;

    [MenuItem("Blippar/ Loading Screen")]
    public static void showLisanceWindow()
    {
        GetWindow<LoadingScreen>("LoadingScreen");
    }

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

    public void ReadLoadingDataFile()
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

    public void ReadCustomDataFile()
    {
        TextAsset txtAsset = (TextAsset)Resources.Load("CustomData", typeof(TextAsset));
        string tileFile = txtAsset.text;
        customData = JsonUtility.FromJson<CustomData>(tileFile);
    }

    public void writeLoadingDataFile()
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
            alert_box_font_size = selectedScreenData.alert_box_font_size;
            alert_box_height = selectedScreenData.alert_box_height;
            alert_box_width = selectedScreenData.alert_box_width;
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

            auto_scan_instruction = selectedScreenData.auto_scan_instruction;
            auto_scan_instruction_detect = selectedScreenData.auto_scan_instruction_detect;
            auto_scan_instruction_idle = selectedScreenData.auto_scan_instruction_idle;
            auto_scan_instruction_text_color = selectedScreenData.auto_scan_instruction_text_color;
            auto_scan_style_display = selectedScreenData.auto_scan_style_display;
            auto_scan_style_position = selectedScreenData.auto_scan_style_position;
            auto_scan_style_top = selectedScreenData.auto_scan_style_top;
            auto_scan_style_left = selectedScreenData.auto_scan_style_left;
            auto_scan_style_transform_translate = selectedScreenData.auto_scan_style_transform_translate;
            auto_scan_style_z_index = selectedScreenData.auto_scan_style_z_index;
            auto_scan_style_width = selectedScreenData.auto_scan_style_width;
            auto_scan_style_height = selectedScreenData.auto_scan_style_height;
            auto_scan_text_align = selectedScreenData.auto_scan_text_align;

            scan_button_display = selectedScreenData.scan_button_display;
            scan_btn_height = selectedScreenData.scan_btn_height;
            scan_btn_width = selectedScreenData.scan_btn_width;
            scan_btn_img_height = selectedScreenData.scan_btn_img_height;
            scan_btn_img_width = selectedScreenData.scan_btn_img_width;
            scan_btn_img_src = selectedScreenData.scan_btn_img_src;
            scan_btn_img_transform_trsnslate = selectedScreenData.scan_btn_img_transform_trsnslate;
            scan_btn_img_x_coordinate = selectedScreenData.scan_btn_img_x_coordinate;
            scan_btn_img_y_coordinate = selectedScreenData.scan_btn_img_y_coordinate;

            scan_btn_instruction = selectedScreenData.scan_btn_instruction;
            scan_btn_instruction_color = selectedScreenData.scan_btn_instruction_color;

            scan_btn_progress_bar_color = selectedScreenData.scan_btn_progress_bar_color;
            scan_btn_progress_bar_cx_coordinate = selectedScreenData.scan_btn_progress_bar_cx_coordinate;
            scan_btn_progress_bar_cy_coordinate = selectedScreenData.scan_btn_progress_bar_cy_coordinate;
            scan_btn_progress_bar_radius = selectedScreenData.scan_btn_progress_bar_radius;
            scan_btn_progress_bar_rotate_x = selectedScreenData.scan_btn_progress_bar_rotate_x;
            scan_btn_progress_bar_rotate_y = selectedScreenData.scan_btn_progress_bar_rotate_y;
            scan_btn_progress_bar_rotate_z = selectedScreenData.scan_btn_progress_bar_rotate_z;

            scan_btn_progress_circle_cx_coordinate = selectedScreenData.scan_btn_progress_circle_cx_coordinate;
            scan_btn_progress_circle_cy_coordinate = selectedScreenData.scan_btn_progress_circle_cy_coordinate;
            scan_btn_progress_circle_radius = selectedScreenData.scan_btn_progress_circle_radius;
            scan_btn_progress_circle_style_fill = selectedScreenData.scan_btn_progress_circle_style_fill;
            scan_btn_progress_circle_style_stroke = selectedScreenData.scan_btn_progress_circle_style_stroke;
            scan_btn_progress_circle_style_stroke_width = selectedScreenData.scan_btn_progress_circle_style_stroke_width;

            showGuideView = selectedScreenData.showGuideView;
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

        ReadLoadingDataFile();
        if (loadingData != null)
        {
            index = loadingData._selectedIndex;
        }
    }

    private void Update()
    {
        //CreateFile();
        //WriteFile();
    }

    private int GetSelectedIndex(string tempName)
    {
        tempList = GetTemplateFiles();
        for (int i = 0; i < tempList.Count; i++)
        {
            string tmpName = tempList[i];
            if (tmpName == tempName)
            {
                return i;
            }
        }
        return 0;
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
        blipparManager = GameObject.FindObjectOfType<BlipparManager>();
        if (blipparManager == null)
        {
            GUILayout.Space(5);
            GUILayout.Label("* Please select tracking system in Blippar setting");
            return;
        }

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
            blipparManager.selectedTemplpate = _selectedTemplate;
            ReadLoadingDataFile();
            loadingData._selectedTemplate = _selectedTemplate;
            loadingData._selectedAnimURL = _trackingAnimURL;
            loadingData._selectedIndex = index;
            writeLoadingDataFile();
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

        if (blipparManager.selectedTracking == "Marker Tracking")
        {
            OnInspectorGUI();
        }

        GUILayout.EndScrollView();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Show Guide View", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        showGuideView = EditorGUILayout.Toggle("", showGuideView);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);


        GUILayout.BeginHorizontal();
        GUILayout.Label("Tracking Animation URL", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(216));
        _trackingAnimURL = GUILayout.TextField(_trackingAnimURL);
        blipparManager.animURL = _trackingAnimURL;
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
                    ReadLoadingDataFile();
                    loadingData._selectedAnimURL = _trackingAnimURL;
                    writeLoadingDataFile();
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
            if (File.Exists("Assets/webarSDK/Editor/LoadingScreenTemplates/" + _templateName + ".json"))
            {
                EditorUtility.DisplayDialog("Warning", "File name already exist, Please select different name", "ok");
                GUILayout.EndHorizontal();
                return;
            }

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
                    blipparManager.selectedTemplpate = _templateName;
                    _selectedTemplate = _templateName;
                    _templateName = "";
                    ReadSelectedFile();
                    StoreTempData();

                    //loadingData._selectedTemplate = _selectedTemplate;
                    //loadingData._selectedAnimURL = _trackingAnimURL;
                    //loadingData._selectedIndex = GetSelectedIndex(_selectedTemplate);
                    //writeCustomFile();

                    index = GetSelectedIndex(_selectedTemplate);

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

    private void OnInspectorGUI()
    {
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Auto Scan Instruction", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        auto_scan_instruction = GUILayout.TextField(auto_scan_instruction);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Auto Scan Instruction to Detect", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        auto_scan_instruction_detect = GUILayout.TextField(auto_scan_instruction_detect);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Auto Scan Instruction Idle", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        auto_scan_instruction_idle = GUILayout.TextField(auto_scan_instruction_idle);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Auto Scan Instruction Text Color", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        auto_scan_instruction_text_color = GUILayout.TextField(auto_scan_instruction_text_color);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Auto Scan Style Display", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        auto_scan_style_display = GUILayout.TextField(auto_scan_style_display);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Auto Scan Style Position", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        auto_scan_style_position = GUILayout.TextField(auto_scan_style_position);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Auto Scan Style Top", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        auto_scan_style_top = GUILayout.TextField(auto_scan_style_top);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Auto Scan Style Left", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        auto_scan_style_left = GUILayout.TextField(auto_scan_style_left);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Auto Scan Style Transform Translate", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        auto_scan_style_transform_translate = GUILayout.TextField(auto_scan_style_transform_translate);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Auto Scan Style Z index", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        auto_scan_style_z_index = GUILayout.TextField(auto_scan_style_z_index);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Auto Scan Style Width", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        auto_scan_style_width = GUILayout.TextField(auto_scan_style_width);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Auto Scan Style Height", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        auto_scan_style_height = GUILayout.TextField(auto_scan_style_height);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Auto Scan Text Allignment", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        auto_scan_text_align = GUILayout.TextField(auto_scan_text_align);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.Label("* Following fields are applicable only when the Auto-Marker-Detection is disabled in Blippar Setting", EditorStyles.boldLabel, GUILayout.MaxWidth(1000));
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Display", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_button_display = GUILayout.TextField(scan_button_display);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Height", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_height = GUILayout.TextField(scan_btn_height);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Width", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_width = GUILayout.TextField(scan_btn_width);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Image Height", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_img_height = GUILayout.TextField(scan_btn_img_height);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Image Width", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_img_width = GUILayout.TextField(scan_btn_img_width);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Image", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_img_src = GUILayout.TextField(scan_btn_img_src);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Image Transform-Translate", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_img_transform_trsnslate = GUILayout.TextField(scan_btn_img_transform_trsnslate);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Image X Coordinate", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_img_x_coordinate = GUILayout.TextField(scan_btn_img_x_coordinate);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Image Y Coordinate", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_img_y_coordinate = GUILayout.TextField(scan_btn_img_y_coordinate);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Instruction", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_instruction = GUILayout.TextField(scan_btn_instruction);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Instruction Color", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_instruction_color = GUILayout.TextField(scan_btn_instruction_color);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Progress bar Color", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_progress_bar_color = GUILayout.TextField(scan_btn_progress_bar_color);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Progress cx coordinate", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_progress_bar_cx_coordinate = GUILayout.TextField(scan_btn_progress_bar_cx_coordinate);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Progress cy coordinate", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_progress_bar_cy_coordinate = GUILayout.TextField(scan_btn_progress_bar_cy_coordinate);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Progress bar Radius", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_progress_bar_radius = GUILayout.TextField(scan_btn_progress_bar_radius);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Progress Rotate-X", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_progress_bar_rotate_x = GUILayout.TextField(scan_btn_progress_bar_rotate_x);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Progress Rotate-Y", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_progress_bar_rotate_y = GUILayout.TextField(scan_btn_progress_bar_rotate_y);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Progress Rotate-Z", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_progress_bar_rotate_z = GUILayout.TextField(scan_btn_progress_bar_rotate_z);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Progress circle cx coordinate", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_progress_circle_cx_coordinate = GUILayout.TextField(scan_btn_progress_circle_cx_coordinate);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Progress circle cy coordinate", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_progress_circle_cy_coordinate = GUILayout.TextField(scan_btn_progress_circle_cy_coordinate);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Progress Circle Radius", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_progress_circle_radius = GUILayout.TextField(scan_btn_progress_circle_radius);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Progress Circle Style fill", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_progress_circle_style_fill = GUILayout.TextField(scan_btn_progress_circle_style_fill);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Progress Circle Style stroke", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_progress_circle_style_stroke = GUILayout.TextField(scan_btn_progress_circle_style_stroke);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scan Button Progress Circle Style stroke width", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(275));
        scan_btn_progress_circle_style_stroke_width = GUILayout.TextField(scan_btn_progress_circle_style_stroke_width);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
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
        dataToSave.alert_box_font_size = alert_box_font_size;
        dataToSave.alert_box_height = alert_box_height;
        dataToSave.alert_box_width = alert_box_width;
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

        dataToSave.auto_scan_instruction = auto_scan_instruction;
        dataToSave.auto_scan_instruction_detect = auto_scan_instruction_detect;
        dataToSave.auto_scan_instruction_idle = auto_scan_instruction_idle;
        dataToSave.auto_scan_instruction_text_color = auto_scan_instruction_text_color;
        dataToSave.auto_scan_style_display = auto_scan_style_display;
        dataToSave.auto_scan_style_position = auto_scan_style_position;
        dataToSave.auto_scan_style_top = auto_scan_style_top;
        dataToSave.auto_scan_style_left = auto_scan_style_left;
        dataToSave.auto_scan_style_transform_translate = auto_scan_style_transform_translate;
        dataToSave.auto_scan_style_z_index = auto_scan_style_z_index;
        dataToSave.auto_scan_style_width = auto_scan_style_width;
        dataToSave.auto_scan_style_height = auto_scan_style_height;
        dataToSave.auto_scan_text_align = auto_scan_text_align;

        dataToSave.scan_button_display = scan_button_display;
        dataToSave.scan_btn_height = scan_btn_height;
        dataToSave.scan_btn_width = scan_btn_width;
        dataToSave.scan_btn_img_height = scan_btn_img_height;
        dataToSave.scan_btn_img_width = scan_btn_img_width;
        dataToSave.scan_btn_img_src = scan_btn_img_src;
        dataToSave.scan_btn_img_transform_trsnslate = scan_btn_img_transform_trsnslate;
        dataToSave.scan_btn_img_x_coordinate = scan_btn_img_x_coordinate;
        dataToSave.scan_btn_img_y_coordinate = scan_btn_img_y_coordinate;

        dataToSave.scan_btn_instruction = scan_btn_instruction;
        dataToSave.scan_btn_instruction_color = scan_btn_instruction_color;

        dataToSave.scan_btn_progress_bar_color = scan_btn_progress_bar_color;
        dataToSave.scan_btn_progress_bar_cx_coordinate = scan_btn_progress_bar_cx_coordinate;
        dataToSave.scan_btn_progress_bar_cy_coordinate = scan_btn_progress_bar_cy_coordinate;
        dataToSave.scan_btn_progress_bar_radius = scan_btn_progress_bar_radius;
        dataToSave.scan_btn_progress_bar_rotate_x = scan_btn_progress_bar_rotate_x;
        dataToSave.scan_btn_progress_bar_rotate_y = scan_btn_progress_bar_rotate_y;
        dataToSave.scan_btn_progress_bar_rotate_z = scan_btn_progress_bar_rotate_z;

        dataToSave.scan_btn_progress_circle_cx_coordinate = scan_btn_progress_circle_cx_coordinate;
        dataToSave.scan_btn_progress_circle_cy_coordinate = scan_btn_progress_circle_cy_coordinate;
        dataToSave.scan_btn_progress_circle_radius = scan_btn_progress_circle_radius;
        dataToSave.scan_btn_progress_circle_style_fill = scan_btn_progress_circle_style_fill;
        dataToSave.scan_btn_progress_circle_style_stroke = scan_btn_progress_circle_style_stroke;
        dataToSave.scan_btn_progress_circle_style_stroke_width = scan_btn_progress_circle_style_stroke_width;

        dataToSave.showGuideView = showGuideView;
        dataToSave._trackingAnimURL = _trackingAnimURL;
    }
}
