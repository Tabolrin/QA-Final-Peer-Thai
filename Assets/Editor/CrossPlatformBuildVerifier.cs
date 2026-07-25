using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

/// <summary>
/// One-off build verification helper for cross-platform testing (WebGL/Android).
/// Not part of the CI/CD pipeline (which uses game-ci/unity-builder instead) -
/// this exists so the actual build target modules can be exercised manually
/// and the result logged in a way a batchmode run can grep for.
/// </summary>
public static class CrossPlatformBuildVerifier
{
    private static readonly string[] Scenes = new[]
    {
        "Assets/Space Shooter Template FREE/Scenes/Demo_Scene.unity",
        "Assets/Space Shooter Template FREE/Scenes/Level2.unity",
    };

    [MenuItem("Tools/Build Verification/WebGL")]
    public static void BuildWebGL()
    {
        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = Scenes,
            locationPathName = "Builds/WebGL",
            target = BuildTarget.WebGL,
            options = BuildOptions.None,
        });
        LogResult("WebGL", report);
    }

    [MenuItem("Tools/Build Verification/Android")]
    public static void BuildAndroid()
    {
        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = Scenes,
            locationPathName = "Builds/Android/game.apk",
            target = BuildTarget.Android,
            options = BuildOptions.None,
        });
        LogResult("Android", report);
    }

    private static void LogResult(string platform, BuildReport report)
    {
        var summary = report.summary;
        Debug.Log($"BUILD_VERIFIER_RESULT platform={platform} result={summary.result} " +
                   $"totalErrors={summary.totalErrors} totalWarnings={summary.totalWarnings} " +
                   $"totalSizeBytes={summary.totalSize} totalTimeSeconds={summary.totalTime.TotalSeconds}");
    }
}
