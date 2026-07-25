using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// One-off Editor tool that adds a "Level X (Y/Z)" intro banner to each level
/// scene. Idempotent - re-running it replaces rather than duplicates the banner.
/// </summary>
public static class LevelIntroBannerSetup
{
    [MenuItem("Tools/Level Setup/Add Intro Banners To Scenes")]
    public static void AddBannersToScenes()
    {
        AddBannerToScene("Assets/Space Shooter Template FREE/Scenes/Demo_Scene.unity", "Level 1 (1/2)");
        AddBannerToScene("Assets/Space Shooter Template FREE/Scenes/Level2.unity", "Level 2 (2/2)");
    }

    static void AddBannerToScene(string scenePath, string label)
    {
        var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

        var existing = Object.FindFirstObjectByType<LevelIntroBanner>();
        if (existing != null)
            Object.DestroyImmediate(existing.gameObject);

        var canvasGO = new GameObject("LevelIntroCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        var textGO = new GameObject("LevelLabelText", typeof(Text));
        textGO.transform.SetParent(canvasGO.transform, false);
        var text = textGO.GetComponent<Text>();
        text.font = AssetDatabase.GetBuiltinExtraResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 64;
        text.fontStyle = FontStyle.Bold;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        var rt = textGO.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(900, 200);
        rt.anchoredPosition = Vector2.zero;

        var banner = canvasGO.AddComponent<LevelIntroBanner>();
        banner.levelLabel = label;
        banner.showDuration = 2f;

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
    }
}
