using UnityEngine;
using UnityEngine.SceneManagement;

// Developed by: Abdullah Al-husini
using System.Collections;

/// <summary>
/// IntroManager: مدير مشهد Intro
/// يعرض شاشة البداية لمدة 3 ثوان ثم ينتقل للواجهة الرئيسية
/// </summary>
public class IntroManager : MonoBehaviour
{
    [SerializeField] private float introDuration = 3f;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void Start()
    {
        // تأكد من وجود CanvasGroup
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        // بدء الـ Intro
        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        // الانتظار لمدة Intro Duration
        yield return new WaitForSeconds(introDuration);

        // تلاشي تدريجي (Fade Out)
        yield return StartCoroutine(FadeOut(0.5f));

        // الانتقال للواجهة الرئيسية
        // استخدام XPlayManager لضمان تحميل المشهد بشكل صحيح وإدارة الحالة
        if (XPlayManager.Instance != null)
        {
            XPlayManager.Instance.LoadScene(mainMenuSceneName);
        }
        else
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }

    private IEnumerator FadeOut(float duration)
    {
        if (canvasGroup == null)
            yield break;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}
