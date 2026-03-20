using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameAssemblyWindow : EditorWindow
{
    private const string ProfileAssetPath = "Assets/Editor/GameAssemblyProfile.asset";

    private GameAssemblyProfile profile;
    private SerializedObject profileSerializedObject;

    [MenuItem("Window/Game Assembly")]
    public static void Open()
    {
        GetWindow<GameAssemblyWindow>("Game Assembly");
    }

    private void OnEnable()
    {
        LoadOrCreateProfile();
    }

    private void OnGUI()
    {
        if (profile == null)
        {
            if (GUILayout.Button("Create Assembly Profile"))
            {
                LoadOrCreateProfile();
            }
            return;
        }

        profileSerializedObject.Update();

        EditorGUILayout.LabelField("Game Assembly", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Manage scenes, manager bindings, skins, and project wiring validation.", MessageType.Info);

        EditorGUILayout.Space(6f);
        DrawProfileFields();
        profileSerializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space(8f);
        DrawActions();
    }

    private void DrawProfileFields()
    {
        EditorGUILayout.PropertyField(profileSerializedObject.FindProperty("mainScenes"), true);
        EditorGUILayout.PropertyField(profileSerializedObject.FindProperty("mainPlayerPrefab"));
        EditorGUILayout.PropertyField(profileSerializedObject.FindProperty("gameManagerScript"));
        EditorGUILayout.PropertyField(profileSerializedObject.FindProperty("audioManagerScript"));
        EditorGUILayout.PropertyField(profileSerializedObject.FindProperty("uiManagerScript"));
        EditorGUILayout.PropertyField(profileSerializedObject.FindProperty("adManagerScript"));
        EditorGUILayout.PropertyField(profileSerializedObject.FindProperty("skins"), true);
    }

    private void DrawActions()
    {
        if (GUILayout.Button("Validate Current Scene"))
        {
            ValidateCurrentScene();
        }

        if (GUILayout.Button("Auto-Fix Current Scene"))
        {
            AutoFixCurrentScene();
        }

        if (GUILayout.Button("Validate Project Scenes"))
        {
            ValidateProjectScenes();
        }
    }

    private void LoadOrCreateProfile()
    {
        profile = AssetDatabase.LoadAssetAtPath<GameAssemblyProfile>(ProfileAssetPath);
        if (profile == null)
        {
            profile = CreateInstance<GameAssemblyProfile>();
            AssetDatabase.CreateAsset(profile, ProfileAssetPath);
            AssetDatabase.SaveAssets();
        }

        profileSerializedObject = new SerializedObject(profile);
    }

    private void ValidateCurrentScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (!scene.IsValid())
        {
            Debug.LogError("No active scene found for validation.");
            return;
        }

        List<string> issues = CollectSceneIssues(scene);
        PrintIssues("Current Scene Validation", scene.path, issues);
    }

    private void AutoFixCurrentScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (!scene.IsValid())
        {
            Debug.LogError("No active scene found for auto-fix.");
            return;
        }

        bool changed = false;
        changed |= EnsureManagerInScene("XPlayManager", profile.gameManagerScript);
        changed |= EnsureManagerInScene("AudioManager", profile.audioManagerScript);
        changed |= EnsureManagerInScene("UIManager", profile.uiManagerScript);
        changed |= EnsureManagerInScene("AdManager", profile.adManagerScript);

        if (changed)
        {
            EditorSceneManager.MarkSceneDirty(scene);
            Debug.Log("Auto-fix applied. Save the scene to persist changes.");
        }
        else
        {
            Debug.Log("Auto-fix found no missing manager bindings.");
        }
    }

    private void ValidateProjectScenes()
    {
        if (profile.mainScenes == null || profile.mainScenes.Count == 0)
        {
            Debug.LogWarning("No scenes assigned in GameAssemblyProfile.");
            return;
        }

        string initialPath = SceneManager.GetActiveScene().path;
        foreach (SceneAsset sceneAsset in profile.mainScenes)
        {
            if (sceneAsset == null)
            {
                Debug.LogWarning("Encountered null SceneAsset entry in profile.");
                continue;
            }

            string path = AssetDatabase.GetAssetPath(sceneAsset);
            Scene scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
            List<string> issues = CollectSceneIssues(scene);
            PrintIssues("Project Scene Validation", path, issues);
        }

        if (!string.IsNullOrEmpty(initialPath))
        {
            EditorSceneManager.OpenScene(initialPath, OpenSceneMode.Single);
        }
    }

    private List<string> CollectSceneIssues(Scene scene)
    {
        var issues = new List<string>();
        GameObject[] roots = scene.GetRootGameObjects();

        if (!HasTypeInScene(typeof(XPlayManager), roots))
        {
            issues.Add("Missing XPlayManager in scene.");
        }

        if (!HasTypeInScene(typeof(AudioManager), roots))
        {
            issues.Add("Missing AudioManager in scene (can still be auto-created at runtime).");
        }

        int missingScriptsCount = 0;
        foreach (GameObject root in roots)
        {
            missingScriptsCount += CountMissingScriptsRecursive(root);
        }

        if (missingScriptsCount > 0)
        {
            issues.Add("Missing script references found: " + missingScriptsCount);
        }

        return issues;
    }

    private static bool HasTypeInScene(Type type, GameObject[] roots)
    {
        foreach (GameObject root in roots)
        {
            if (root.GetComponentInChildren(type, true) != null)
            {
                return true;
            }
        }

        return false;
    }

    private static int CountMissingScriptsRecursive(GameObject root)
    {
        int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(root);
        foreach (Transform child in root.transform)
        {
            count += CountMissingScriptsRecursive(child.gameObject);
        }

        return count;
    }

    private static bool EnsureManagerInScene(string objectName, MonoScript scriptAsset)
    {
        if (scriptAsset == null)
        {
            return false;
        }

        Type managerType = scriptAsset.GetClass();
        if (managerType == null || !typeof(MonoBehaviour).IsAssignableFrom(managerType))
        {
            Debug.LogWarning("Script is not a MonoBehaviour: " + scriptAsset.name);
            return false;
        }

        MonoBehaviour existing = FindFirstObjectByType(managerType) as MonoBehaviour;
        if (existing != null)
        {
            return false;
        }

        GameObject root = GameObject.Find(objectName) ?? new GameObject(objectName);
        if (root.GetComponent(managerType) == null)
        {
            root.AddComponent(managerType);
            return true;
        }

        return false;
    }

    private static UnityEngine.Object FindFirstObjectByType(Type type)
    {
#if UNITY_2023_1_OR_NEWER
        return UnityEngine.Object.FindFirstObjectByType(type);
#else
        UnityEngine.Object[] all = UnityEngine.Object.FindObjectsOfType(type, true);
        return all != null && all.Length > 0 ? all[0] : null;
#endif
    }

    private static void PrintIssues(string title, string scenePath, List<string> issues)
    {
        if (issues.Count == 0)
        {
            Debug.Log($"{title}: OK -> {scenePath}");
            return;
        }

        Debug.LogWarning($"{title}: {scenePath}\n- " + string.Join("\n- ", issues));
    }
}
