using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class LevelIntroBannerTests
{
    static GameObject BuildBanner(string label, float showDuration)
    {
        var root = new GameObject("Banner_Test");
        var textObject = new GameObject("BannerText_Test");
        textObject.transform.SetParent(root.transform);
        Text text = textObject.AddComponent<Text>();

        LevelIntroBanner banner = root.AddComponent<LevelIntroBanner>();
        banner.levelLabel = label;
        banner.showDuration = showDuration;
        return root;
    }

    [UnityTest]
    public IEnumerator LevelIntroBanner_OnStart_SetsTheLabelText()
    {
        GameObject root = BuildBanner("Level 1 (1/2)", showDuration: 10f);
        yield return null;

        Text text = root.GetComponentInChildren<Text>();
        Assert.AreEqual("Level 1 (1/2)", text.text);

        Object.Destroy(root);
    }

    [UnityTest]
    public IEnumerator LevelIntroBanner_AfterShowDuration_HidesItself()
    {
        GameObject root = BuildBanner("Level 2 (2/2)", showDuration: 0.05f);
        yield return null;

        Assert.IsTrue(root.activeSelf, "Should still be visible immediately after starting.");

        yield return new WaitForSeconds(0.2f);

        Assert.IsFalse(root.activeSelf, "Should have hidden itself once showDuration elapsed.");

        Object.Destroy(root);
    }
}
