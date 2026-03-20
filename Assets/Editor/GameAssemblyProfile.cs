using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor-only profile used by GameAssemblyWindow to keep project wiring references.
/// </summary>
public class GameAssemblyProfile : ScriptableObject
{
    [Header("Main Scenes")]
    public List<SceneAsset> mainScenes = new List<SceneAsset>();

    [Header("Primary Prefabs")]
    public GameObject mainPlayerPrefab;

    [Header("Managers")]
    public MonoScript gameManagerScript;
    public MonoScript audioManagerScript;
    public MonoScript uiManagerScript;
    public MonoScript adManagerScript;

    [Header("Skins / Cosmetics")]
    public List<GameObject> skins = new List<GameObject>();
}
