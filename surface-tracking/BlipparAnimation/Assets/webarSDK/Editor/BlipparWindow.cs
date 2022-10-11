using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditorInternal;
using System;
using System.IO;
using System.Linq;

public class BlipparWindow : EditorWindow
{
    [SerializeField]
    private string LicenseKey = "xxxxxxxx-1111-2222-3333-yyyyyyyyyyyy";
    private string defaultLicense = "xxxxxxxx-1111-2222-3333-yyyyyyyyyyyy";
    [SerializeField]
    //private string sdkURL = "https://webar-sdk.blippar.com/releases/beta/cdn/v1.3.0-unity/blippar/webar-sdk-v1.3.0-beta.min.js";
    //private string defaultURL = "https://webar-sdk.blippar.com/releases/beta/cdn/v1.3.0-unity/blippar/webar-sdk-v1.3.0-beta.min.js";
    private string sdkPath = "sdk/blippar-webar-sdk-v1.4.2/blippar/webar-sdk-v1.4.2.min.js";    private string defaultPath = "sdk/blippar-webar-sdk-v1.4.2/blippar/webar-sdk-v1.4.2.min.js";

    private string defaultBuildLocation;
    private string buildLocation;

    [SerializeField]    public static string ServerAliasName = "localserver";    [SerializeField]    public static string RelativeHostPath = "/";
    private Texture2D b_Logo = null;    [SerializeField]    public static string LocalServerPort = "8888";

    public string[] options = new string[] { "None", "Surface Tracking", "Marker Tracking" };
    public int index = 0;    private bool build = false;    private string applicationDataPath;    private string buildPath;    public UnityEngine.Object myCamera;    public UnityEngine.Object myStage;

    string saveFile = "Assets/webarSDK/Editor/CustomData.json";
    CustomData customData = new CustomData();

    private const string _helpText = "Cannot find 'Physical Simulation List' component on any GameObject in the scene!";

    private static Vector2 _windowsMinSize = Vector2.one * 500f;
    private static Rect _helpRect = new Rect(0f, 0f, 400f, 100f);
    private static Rect _listRect = new Rect(new Vector2(140, 235), _windowsMinSize);

    string projectNameBuild;

    Vector2 scrollPosition = Vector2.zero;

    SerializedObject _objectSO = null;


    SerializedProperty serializedProperty;

    //enum displayFieldType { DisplayAsAutomaticFields, DisplayAsCustomizableGUIFields }
    //displayFieldType DisplayFieldType;
    int ListSize;

    BlipparManager blipparManager;

    MarkerList _simulatorList;
    private static int markerCount;    [MenuItem("Blippar/ Settings")]    public static void showLisanceWindow()    {        GetWindow<BlipparWindow>("Blippar");    }    [MenuItem("Blippar/Webar Object/Camera")]    public static void addBlipparCamera()    {         GameObject obj = Instantiate(Resources.Load("webarCamera")) as GameObject;        obj.name = "webarCamera";    }

    [MenuItem("Blippar/Webar Object/Manager")]    public static void addBlipparManager()    {        GameObject obj = Instantiate(Resources.Load("webarManager"), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)) as GameObject;        obj.name = "webarManager";    }

    [MenuItem("Blippar/Webar Object/Stage")]    public static void addBlipparStage()    {        GameObject obj = Instantiate(Resources.Load("webarStage"), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)) as GameObject;        obj.name = "webarStage";    }

    [MenuItem("Blippar/Webar Object/Marker")]    public static void addBlipparMarker()    {        GameObject obj = Instantiate(Resources.Load("webarMarker"), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)) as GameObject;        markerCount = markerCount + 1;        obj.name = "webarMarker" + markerCount;    }

    public void readFile()
    {
        // Does the file exist?
        if (File.Exists(saveFile))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(saveFile);

            // Deserialize the JSON data 
            //  into a pattern matching the GameData class.
            customData = JsonUtility.FromJson<CustomData>(fileContents);
        }
    }

    public void writeFile()
    {
        // Serialize the object into JSON and save string.
        string jsonString = JsonUtility.ToJson(customData);

        // Write JSON to file.
        File.WriteAllText("Assets/webarSDK/Editor/CustomData.json", jsonString);
    }

    private void CreateFile()
    {
        if (File.Exists(saveFile))
            return;

        saveFile = "Assets/webarSDK/Editor/CustomData.json";
    }

    void OnEnable()
    {
        b_Logo = (Texture2D)Resources.Load("logo", typeof(Texture2D));
        markerCount = 0;
        applicationDataPath = Application.dataPath;
        buildPath = applicationDataPath.Replace("Assets", "");

        if (GameObject.Find("webarManager") == null)
        {
            GameObject obj = Instantiate(Resources.Load("webarManager"), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
            obj.name = "webarManager";
        }

        readFile();
        if (customData._licenseKey != null)
        {
            LicenseKey = customData._licenseKey;
        }

        if (customData._selectedTracking == "Surface Tracking")
        {
            index = 1;
        }
        else if (customData._selectedTracking == "Marker Tracking")
        {
            index = 2;
        }

        projectNameBuild = PlayerSettings.productName;
        //defaultBuildLocation = buildPath + projectNameBuild;
        defaultBuildLocation = buildPath;
        buildLocation = defaultBuildLocation;
        if (customData._selectedBuildLocation != null)
        {
            buildLocation = customData._selectedBuildLocation;
        }
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }

    private void OnGUI()
    {
        blipparManager = GameObject.FindObjectOfType<BlipparManager>();
        _simulatorList = FindObjectOfType<MarkerList>();

        GUILayout.Label(b_Logo, EditorStyles.centeredGreyMiniLabel);

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();

        GUILayout.Label("WebAR SDK Path", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(130));        sdkPath = GUILayout.TextField(sdkPath);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("License Key", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(130));
        LicenseKey = GUILayout.TextField(LicenseKey);
        CreateFile();
        customData._licenseKey = LicenseKey;
        writeFile();

        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Build Location", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(130));
        buildLocation = GUILayout.TextField(buildLocation);

        if (GUILayout.Button("Browse"))
        {
            string path = EditorUtility.OpenFolderPanel("Select folder for build", "", "");
            if (path == "")
            {
                GUILayout.EndHorizontal();
                return;
            }
            buildLocation = path;
            customData._selectedBuildLocation = buildLocation;
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Space(200);
        GUILayout.Label("                      ");
        if (GUILayout.Button("Reset"))
        {
            sdkPath = defaultPath;
            LicenseKey = defaultLicense;
            buildLocation = defaultBuildLocation;
        }
        //GUILayout.Space(10);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Select Tracking", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(130));
        index = EditorGUILayout.Popup(index, options);
           
        GUILayout.EndHorizontal();
        GUILayout.Space(15);
        if (blipparManager != null)
        {
            OnInspectorGUI(index);
            if (!build)
            {
                if (index == 1)
                {
                    blipparManager.selectedTracking = "Surface Tracking";
                    customData._selectedTracking = "Surface Tracking";
                }
                else if (index == 2)
                {
                    blipparManager.selectedTracking = "Marker Tracking";
                    customData._selectedTracking = "Marker Tracking";
                }
                else
                {
                    customData._selectedTracking = "None";
                }
            }
        }    }

    private void OnInspectorGUI(int index)
    {
        switch (index)
        {
            case 0:

                break;
            case 1:
                DrawSurfaceInspector();
                BuildBlippar();
                break;
            case 2:
                DrawMarkerInspector();
                BuildBlippar();
                break;
            default:
                Debug.LogError("Unrecognized Option");
                break;
        }
    }

    void DrawSurfaceInspector()
    {
        GUILayout.Space(5);
        GUILayout.Label("Scene References", EditorStyles.boldLabel, GUILayout.MaxWidth(130));
        GUILayout.BeginHorizontal();
        GUILayout.Label("webar Camera", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(130));
        myCamera = EditorGUILayout.ObjectField(myCamera, typeof(Camera), true);

        SetDefaults();

        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Label("webar Stage", /*EditorStyles.boldLabel,*/ GUILayout.MaxWidth(130));
        myStage = EditorGUILayout.ObjectField(myStage, typeof(GameObject), true);
        GUILayout.EndHorizontal();
        GUILayout.Space(20);
        SetDefaults();
    }

    void DrawMarkerInspector()
    {
        GUILayout.Space(5);

        GUILayout.Label("Scene References", EditorStyles.boldLabel, GUILayout.MaxWidth(130));
        GUILayout.BeginHorizontal();
        GUILayout.Label("webar Camera", EditorStyles.boldLabel, GUILayout.MaxWidth(130));
        myCamera = EditorGUILayout.ObjectField(myCamera, typeof(Camera), true);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        SetDefaults();

        EditorGUILayout.LabelField("Enter the number of marker(s)", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        ListSize = EditorGUILayout.IntField("Marker Count", ListSize);
        _objectSO = new SerializedObject(_simulatorList);
        serializedProperty = _objectSO.FindProperty("markerElements"); // Find the List in our script and create a refrence of it
        _objectSO.Update();
        if (ListSize == 0 && serializedProperty.arraySize != 0)
        {
            ListSize = serializedProperty.arraySize;
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Add/Update"))
        {
            ListSize = EditorGUILayout.IntField("Marker Size", ListSize);
            if (ListSize > 10 || ListSize < 0)
            {
                EditorUtility.DisplayDialog("Warning", "Please enter a value between 0 and 10", "ok");
                GUILayout.EndHorizontal();
                return;
            }
            if (ListSize != serializedProperty.arraySize)
            {
                while (ListSize > serializedProperty.arraySize)
                {
                    serializedProperty.InsertArrayElementAtIndex(serializedProperty.arraySize);
                }
                while (ListSize < serializedProperty.arraySize)
                {
                    serializedProperty.DeleteArrayElementAtIndex(serializedProperty.arraySize - 1);
                }
            }
            _objectSO.ApplyModifiedProperties();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        //scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, GUILayout.Width(625), GUILayout.Height(400));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);
       // EditorGUILayout.Space();

        for (int i = 0; i < serializedProperty.arraySize; i++)
        {
            SerializedProperty MyListRef = serializedProperty.GetArrayElementAtIndex(i);
            SerializedProperty MyID = MyListRef.FindPropertyRelative("markerID");
            SerializedProperty MyGO = MyListRef.FindPropertyRelative("markerObject");

            EditorGUILayout.LabelField("Marker " + (i + 1), EditorStyles.boldLabel);

            {
                MyID.stringValue = EditorGUILayout.TextField("Marker ID", MyID.stringValue);
                MyGO.objectReferenceValue = EditorGUILayout.ObjectField("Marker Object", MyGO.objectReferenceValue, typeof(GameObject), true);
            }

            //EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("        ");
            if (GUILayout.Button("Remove Marker"))
            {
                serializedProperty.DeleteArrayElementAtIndex(i);
                ListSize = ListSize - 1;
            }
            GUILayout.EndHorizontal();
        }
        _objectSO.ApplyModifiedProperties();

        GUILayout.EndScrollView();
        GUILayout.Space(5);
    }

    void SetDefaults()
    {
        //Adding Camera
        if (GameObject.Find("webarCamera") == null)
        {
            GameObject obj = Instantiate(Resources.Load("webarCamera")) as GameObject;
            obj.name = "webarCamera";
        }

        if (GameObject.Find("webarCamera") != null)
        {
            myCamera = GameObject.Find("webarCamera");
        }

        if (index == 1)
        {
            if (GameObject.Find("webarStage") == null)
            {
                GameObject obj = Instantiate(Resources.Load("webarStage"), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
                obj.name = "webarStage";
            }

            if (GameObject.Find("webarStage") != null)
            {
                myStage = GameObject.Find("webarStage");
            }
        }

        if (index == 2)
        {
            //if (GameObject.FindGameObjectWithTag("Marker") == null)
            //{
            //    GameObject obj = Instantiate(Resources.Load("webarMarker"), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
            //    obj.name = "webarMarker1";

            //    //if (GameObject.FindGameObjectsWithTag("Marker") != null)
            //    //{
            //    //    Markers markers = new Markers
            //    //    {
            //    //        markerID = "",
            //    //        markerObject = obj
            //    //    };
            //    //    _simulatorList.markerElements.Add(markers);
            //    //}
            //}
        }
   
    }

    static string GetScenes()
    {
        //return EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
        return SceneManager.GetActiveScene().path;
    }

    void BuildBlippar()
    {
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Build Scene"))
        {
            build = true;
            if (!UIValidation())
            {
                GUIUtility.ExitGUI();
                return;
            }

            PlayerSettings.WebGL.template = "PROJECT:Blippar";

            if (options.GetValue(index).ToString() == "Surface Tracking")
            {
                blipparManager.selectedTracking = "Surface Tracking";
                blipparManager.ReadString(true, sdkPath, LicenseKey, myCamera.name, myStage.name);
            }
            else if (options.GetValue(index).ToString() == "Marker Tracking")
            {
                blipparManager.selectedTracking = "Marker Tracking";
                blipparManager.ReadMarkers(true, sdkPath, LicenseKey, myCamera.name, _simulatorList);
            }
            Debug.Log("### BUILDING ###");
            PlayerSettings.WebGL.decompressionFallback = true;
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
            //PlayerSettings.WebGL.decompressionFallback = true;

            var report = BuildPipeline.BuildPlayer(
                new[] {GetScenes()},
                buildLocation + "/" + "WebAR-" + projectNameBuild ,
                BuildTarget.WebGL,
                BuildOptions.None);

            blipparManager = FindObjectOfType<BlipparManager>();
            if (options.GetValue(index).ToString() == "Surface Tracking")
            {
                blipparManager.UndoMarker();
            }
            else if (options.GetValue(index).ToString() == "Marker Tracking")
            {
                blipparManager.UndoMarker();
            }
            build = false;
            Debug.Log("### Build Process Completed ###");
            if (report.summary.result.ToString() == "Succeeded")            {                EditorUtility.DisplayDialog("Congratulations", "Build completed successfully", "ok");            }            else            {                EditorUtility.DisplayDialog("Sorry", "Build not completed, check console log", "ok");            }
            GUIUtility.ExitGUI();
            EditorApplication.Exit(1);
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.LabelField("Note:", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        Scene scenes = SceneManager.GetActiveScene();
        //var projectNameBuild = PlayerSettings.productName;
        EditorGUILayout.LabelField("Build Path:   " + buildLocation, EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        var style = GUI.skin.label;
        style.richText = true;
        string caption = "How to host the build on Local HTTPS server?";
        caption = string.Format("<color=#0096FF>{0}</Color>", caption);

        bool bClicked = GUILayout.Button(caption, style);
        var rect = GUILayoutUtility.GetLastRect();
        rect.width = style.CalcSize(new GUIContent(caption)).x;
        EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);

        if (bClicked)
        {
            Application.OpenURL("https://support.blippar.com/hc/en-us/articles/4407844463763-Develop-Locally");
        }

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        string hostServer = "How to create the Marker ID for marker tracking?";
        hostServer = string.Format("<color=#0096FF>{0}</Color>", hostServer);

        bool hostClicked = GUILayout.Button(hostServer, style);
        var hostRect = GUILayoutUtility.GetLastRect();
        hostRect.width = style.CalcSize(new GUIContent(hostServer)).x;
        EditorGUIUtility.AddCursorRect(hostRect, MouseCursor.Link);
        if (hostClicked)
        {
            Application.OpenURL("https://support.blippar.com/hc/en-us/articles/4410413461011-Building-a-Basic-Marker-Tracking-Experience#h_01FMG1FSPBGRT23CAWR8K8YJJX");
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("        ");
    }

    private bool UIValidation()
    {
        if (LicenseKey == "")
        {
            EditorUtility.DisplayDialog("Warning", "Please enter license key", "ok");
            return false;
        }
        if (sdkPath == "")
        {
            EditorUtility.DisplayDialog("Warning", "Please enter sdk URL", "ok");
            return false;
        }
        if (myCamera == null)
        {
            EditorUtility.DisplayDialog("Warning", "Please assign wearCamera in inspector window", "ok");
            return false;
        }
        if (myStage == null && options.GetValue(index).ToString() == "Surface Tracking")
        {
            EditorUtility.DisplayDialog("Warning", "Please assign webarStage in inspector window", "ok");
            return false;
        }
        if (FindObjectOfType<BlipparManager>() == null)
        {
            EditorUtility.DisplayDialog("Warning", "Please add webarManager object from Blipper menu", "ok");
            return false;
        }
        if (FindObjectOfType<WEBARSDK>() == null)
        {
            EditorUtility.DisplayDialog("Warning", "Please add WEBAR script to the object", "ok");
            return false;
        }

        if (options.GetValue(index).ToString() == "Marker Tracking" && markerCount == 0)
        {
            EditorUtility.DisplayDialog("Warning", "Please add marker data in blippar settings", "ok");
            return false;
        }

        if (buildLocation == "" )
        {
            EditorUtility.DisplayDialog("Warning", "Please provide build location in inspector window", "ok");
            return false;
        }

        if (_simulatorList.markerElements.Count > 0)
        {
            for (int i = 0; i < _simulatorList.markerElements.Count; i++)
            {
                Markers mark = _simulatorList.markerElements[i];
                if (mark.markerID =="")
                {
                    EditorUtility.DisplayDialog("Warning", "Please enter marker id in inspector window", "ok");
                    return false;
                }
            }
        }
        //if (GetScenes().Length == 0)
        //{
        //    EditorUtility.DisplayDialog("Warning", "No active scenes in player settings", "ok");
        //    return false;
        //}
        return true;
    }
}
