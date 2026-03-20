using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System;

public class SetupEditor : EditorWindow
{
    [MenuItem("X-Play/Setup Project")]
    public static void ShowWindow()
    {
        GetWindow<SetupEditor>("X-Play Setup");
    }

    private void OnGUI()
    {
        GUILayout.Label("X-Play Project Setup", EditorStyles.boldLabel);

        EditorGUILayout.HelpBox(
            "هذه الأداة تقوم بربط السكريبتات الأساسية بكائنات اللعبة في المشاهد تلقائياً. استخدمها بعد استنساخ المشروع أو عند حدوث مشاكل في الروابط.\nDeveloped by: Abdullah Al-husini",
            MessageType.Info
        );

        if (GUILayout.Button("تشغيل الإعداد التلقائي"))
        {
            RunSetup();
        }
    }

    private static void RunSetup()
    {
        Debug.Log("بدء إعداد مشروع X-Play التلقائي...");

        SetupIntroScene();
        SetupMainMenuScene();
        SetupGameScenes();

        Debug.Log("اكتمل إعداد مشروع X-Play التلقائي.");
        EditorUtility.DisplayDialog("X-Play Setup", "تم إعداد المشروع بنجاح!", "موافق");
    }

    private static void SetupIntroScene()
    {
        string introScenePath = "Assets/Scenes/Intro.unity";
        Scene introScene = EditorSceneManager.OpenScene(introScenePath, OpenSceneMode.Single);

        GameObject gameManagerObj = GameObject.Find("XPlayManager");
        if (gameManagerObj == null)
        {
            gameManagerObj = new GameObject("XPlayManager");
            gameManagerObj.AddComponent<XPlayManager>();
        }
        else if (gameManagerObj.GetComponent<XPlayManager>() == null)
        {
            gameManagerObj.AddComponent<XPlayManager>();
        }

        GameObject introManagerObj = GameObject.Find("IntroManager");
        if (introManagerObj == null)
        {
            introManagerObj = new GameObject("IntroManager");
            introManagerObj.AddComponent<IntroManager>();
        }
        else if (introManagerObj.GetComponent<IntroManager>() == null)
        {
            introManagerObj.AddComponent<IntroManager>();
        }

        EditorSceneManager.SaveScene(introScene);
    }

    private static void SetupMainMenuScene()
    {
        string mainMenuScenePath = "Assets/Scenes/MainMenu.unity";
        Scene mainMenuScene = EditorSceneManager.OpenScene(mainMenuScenePath, OpenSceneMode.Single);

        string[] managers = {
            "MainMenuManager",
            "AdManager",
            "BackgroundController",
            "UIManager",
            "SettingsManager"
        };

        foreach (string manager in managers)
        {
            GameObject obj = GameObject.Find(manager);
            if (obj == null)
            {
                obj = new GameObject(manager);
            }

            Type type = FindTypeByName(manager);
            if (type != null && type.IsSubclassOf(typeof(MonoBehaviour)) && obj.GetComponent(type) == null)
            {
                obj.AddComponent(type);
            }
        }

        EditorSceneManager.SaveScene(mainMenuScene);
    }

    private static void SetupGameScenes()
    {
        string[] gameSceneNames = {
            "PulseClicker",
            "GoldenHarvest",
            "NeonPop",
            "CyberSlither",
            "BlockCrush",
            "SkyStacker",
            "NitroRace",
            "MindMatch"
        };

        foreach (string gameName in gameSceneNames)
        {
            string gameScenePath = $"Assets/Scenes/{gameName}.unity";
            Scene gameScene = EditorSceneManager.OpenScene(gameScenePath, OpenSceneMode.Single);

            GameObject gameManagerObj = GameObject.Find("XPlayManager");
            if (gameManagerObj == null)
            {
                gameManagerObj = new GameObject("XPlayManager");
                gameManagerObj.AddComponent<XPlayManager>();
            }
            else if (gameManagerObj.GetComponent<XPlayManager>() == null)
            {
                gameManagerObj.AddComponent<XPlayManager>();
            }

            GameObject gameSpecificManagerObj = GameObject.Find($"{gameName}Manager");
            if (gameSpecificManagerObj == null)
            {
                gameSpecificManagerObj = new GameObject($"{gameName}Manager");
            }

            Type managerType = FindTypeByName($"{gameName}Manager");
            if (managerType != null && managerType.IsSubclassOf(typeof(MonoBehaviour)))
            {
                if (gameSpecificManagerObj.GetComponent(managerType) == null)
                {
                    gameSpecificManagerObj.AddComponent(managerType);
                }
            }
            else
            {
                Debug.LogError($"لم يتم العثور على سكريبت {gameName}Manager");
            }

            EditorSceneManager.SaveScene(gameScene);
        }
    }

    private static Type FindTypeByName(string typeName)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type type = assembly.GetType(typeName);
            if (type != null)
            {
                return type;
            }
        }

        return null;
    }
}
