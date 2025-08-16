using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [SerializeField] private List<ClickerUpgradeItem> clickerItems = new();
    [SerializeField] private List<PlatformUpgradeItem> platformItems = new();

    void Awake() {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (clickerItems.Count == 0 || platformItems.Count == 0)
        {
            // runtime-safe fallback (or switch to Addressables later)
            clickerItems = new List<ClickerUpgradeItem>(Resources.LoadAll<ClickerUpgradeItem>("ClickerItems"));
            platformItems = new List<PlatformUpgradeItem>(Resources.LoadAll<PlatformUpgradeItem>("PlatformItems"));
        }
    }

    public IReadOnlyList<ClickerUpgradeItem> GetClickerItems() => clickerItems;
    public IReadOnlyList<PlatformUpgradeItem> GetPlatformItems() => platformItems;

#if UNITY_EDITOR
    [ContextMenu("Editor: Auto-Fill From Project Folders")]
    private void EditorAutoFill() {
        clickerItems.Clear(); platformItems.Clear();
        foreach (var p in Directory.GetFiles("Assets/ScriptableObjects/ClickerItems"))
            if (!p.EndsWith(".meta")) clickerItems.Add(UnityEditor.AssetDatabase.LoadAssetAtPath<ClickerUpgradeItem>(p));
        foreach (var p in Directory.GetFiles("Assets/ScriptableObjects/PlatformItems"))
            if (!p.EndsWith(".meta")) platformItems.Add(UnityEditor.AssetDatabase.LoadAssetAtPath<PlatformUpgradeItem>(p));
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}

