using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Shows a brief "Level X (Y/Z)" banner centered on screen when a level scene
/// starts, then hides itself. Purely a UI overlay - does not pause or delay
/// any gameplay logic underneath it.
/// </summary>
public class LevelIntroBanner : MonoBehaviour
{
    [Tooltip("Text shown in the banner, e.g. \"Level 1 (1/2)\"")]
    public string levelLabel = "Level 1 (1/2)";

    [Tooltip("How long the banner stays visible before it hides itself")]
    public float showDuration = 2f;

    Text bannerText;

    void Awake()
    {
        bannerText = GetComponentInChildren<Text>();
    }

    void Start()
    {
        if (bannerText != null)
            bannerText.text = levelLabel;
        StartCoroutine(HideAfterDelay());
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(showDuration);
        gameObject.SetActive(false);
    }
}
