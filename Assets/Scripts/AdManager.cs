using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// AdManager: مدير الإعلانات
/// يدير: البنر السفلي (تغيير كل 7 ثوان)، والبوب-أب (كل 20 ثانية)
/// </summary>
public class AdManager : MonoBehaviour
{
    [SerializeField] private Image bannerImage;
    [SerializeField] private Button bannerCloseButton;
    [SerializeField] private float bannerChangeInterval = 7f;

    [SerializeField] private GameObject popupAdPrefab;
    [SerializeField] private Canvas popupCanvas;
    [SerializeField] private float popupInterval = 20f;
    [SerializeField] private float popupDuration = 5f;

    [SerializeField] private List<Sprite> bannerSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> popupSprites = new List<Sprite>();

    private int currentBannerIndex = 0;
    private float bannerTimer = 0f;
    private float popupTimer = 0f;
    private bool isPopupActive = false;

    private void Start()
    {
        // التحقق من الـ References
        if (bannerImage == null)
            Debug.LogError("Banner Image is not assigned!");

        if (bannerCloseButton != null)
            bannerCloseButton.onClick.AddListener(OnBannerClicked);

        // إضافة إعلانات وهمية إذا لم تكن موجودة
        if (bannerSprites.Count == 0)
            GenerateDummyBanners();

        if (popupSprites.Count == 0)
            GenerateDummyPopups();
    }

    private void Update()
    {
        // تحديث البنر
        bannerTimer += Time.deltaTime;
        if (bannerTimer >= bannerChangeInterval)
        {
            ChangeBanner();
            bannerTimer = 0f;
        }

        // تحديث البوب-أب
        popupTimer += Time.deltaTime;
        if (popupTimer >= popupInterval && !isPopupActive)
        {
            ShowPopupAd();
            popupTimer = 0f;
        }
    }

    private void ChangeBanner()
    {
        if (bannerSprites.Count == 0)
            return;

        currentBannerIndex = (currentBannerIndex + 1) % bannerSprites.Count;
        if (bannerImage != null)
            bannerImage.sprite = bannerSprites[currentBannerIndex];
    }

    private void OnBannerClicked()
    {
        // يمكن إضافة منطق لفتح رابط الإعلان هنا
        Debug.Log("Banner clicked!");
    }

    private void ShowPopupAd()
    {
        if (popupSprites.Count == 0)
            return;

        isPopupActive = true;
        StartCoroutine(DisplayPopupCoroutine());
    }

    private IEnumerator DisplayPopupCoroutine()
    {
        // اختيار إعلان عشوائي
        int randomIndex = Random.Range(0, popupSprites.Count);
        Sprite selectedAd = popupSprites[randomIndex];

        // إنشاء الـ Popup
        GameObject popup = Instantiate(popupAdPrefab, popupCanvas.transform);
        Image popupImage = popup.GetComponent<Image>();
        if (popupImage != null)
            popupImage.sprite = selectedAd;

        // الانتظار لمدة popupDuration
        yield return new WaitForSeconds(popupDuration);

        // إغلاق الـ Popup
        Destroy(popup);
        isPopupActive = false;
    }

    private void GenerateDummyBanners()
    {
        // إنشاء إعلانات وهمية (في الإنتاج، سيتم استبدالها برسومات حقيقية)
        for (int i = 0; i < 3; i++)
        {
            Texture2D dummyTexture = new Texture2D(1080, 200, TextureFormat.RGB24, false);
            Color[] colors = new Color[1080 * 200];
            
            // ألوان عشوائية
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            for (int j = 0; j < colors.Length; j++)
                colors[j] = randomColor;

            dummyTexture.SetPixels(colors);
            dummyTexture.Apply();

            Sprite sprite = Sprite.Create(dummyTexture, new Rect(0, 0, 1080, 200), Vector2.zero);
            bannerSprites.Add(sprite);
        }
    }

    private void GenerateDummyPopups()
    {
        // إنشاء بوب-أب وهمية (في الإنتاج، سيتم استبدالها برسومات حقيقية)
        for (int i = 0; i < 4; i++)
        {
            Texture2D dummyTexture = new Texture2D(600, 800, TextureFormat.RGB24, false);
            Color[] colors = new Color[600 * 800];
            
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            for (int j = 0; j < colors.Length; j++)
                colors[j] = randomColor;

            dummyTexture.SetPixels(colors);
            dummyTexture.Apply();

            Sprite sprite = Sprite.Create(dummyTexture, new Rect(0, 0, 600, 800), Vector2.zero);
            popupSprites.Add(sprite);
        }
    }

    public void AddBanner(Sprite banner)
    {
        if (banner != null)
            bannerSprites.Add(banner);
    }

    public void AddPopup(Sprite popup)
    {
        if (popup != null)
            popupSprites.Add(popup);
    }
}
