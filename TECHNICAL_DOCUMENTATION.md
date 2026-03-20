# التوثيق التقني لمشروع X-Play

Developed by: Abdullah Al-husini

## نظرة عامة على البنية المعمارية

### نمط التصميم المستخدم

#### 1. **Singleton Pattern**
استخدام نمط Singleton لـ XPlayManager لضمان وجود نسخة واحدة فقط من مدير المنصة الرئيسي:

```csharp
public static XPlayManager Instance { get; private set; }

private void Awake()
{
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
        return;
    }
    Instance = this;
    DontDestroyOnLoad(gameObject);
}
```

#### 2. **Manager Pattern**
كل لعبة لها مدير خاص بها يتعامل مع منطق اللعبة:
- PulseClickerManager
- GoldenHarvestManager
- NeonPopManager
- إلخ...

#### 3. **Observer Pattern** (في الإعلانات)
AdManager يراقب الوقت ويطلق أحداث الإعلانات:

```csharp
private float bannerTimer = 0f;
if (bannerTimer >= bannerChangeInterval)
{
    ChangeBanner();
    bannerTimer = 0f;
}
```

## هيكل البيانات

### 1. نظام العملات والطاقة

```csharp
public class XPlayManager
{
    private int coins = 0;
    private int energy = 100;
    private int maxEnergy = 100;
    
    public void AddCoins(int amount) { }
    public bool SpendCoins(int amount) { }
    public bool ConsumeEnergy(int amount) { }
    public void RestoreEnergy(int amount) { }
}
```

### 2. نظام الإعلانات

```csharp
public class AdManager
{
    private List<Sprite> bannerSprites = new List<Sprite>();
    private List<Sprite> popupSprites = new List<Sprite>();
    
    private float bannerChangeInterval = 7f;
    private float popupInterval = 20f;
    private float popupDuration = 5f;
}
```

### 3. بيانات الألعاب

```csharp
[System.Serializable]
public class GameData
{
    public string gameName;
    public string sceneName;
    public Sprite gameIcon;
    public int requiredEnergy = 10;
}
```

## نظام الحفظ والتحميل

### PlayerPrefs
يتم استخدام PlayerPrefs لحفظ البيانات محلياً:

```csharp
private void SavePlayerData()
{
    PlayerPrefs.SetInt("Coins", coins);
    PlayerPrefs.SetInt("Energy", energy);
    PlayerPrefs.SetInt("MusicEnabled", musicEnabled ? 1 : 0);
    PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    PlayerPrefs.Save();
}

private void LoadPlayerData()
{
    coins = PlayerPrefs.GetInt("Coins", 0);
    energy = PlayerPrefs.GetInt("Energy", maxEnergy);
    musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
    musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
}
```

## تفاصيل كل لعبة

### 1. Pulse Clicker

**الميكانيكية:**
- 4 أوضاع مختلفة للتحدي
- دائرة هدف تتحرك بطرق مختلفة
- النقر الدقيق على الدائرة

**الكود الرئيسي:**
```csharp
private void HandleInput()
{
    if (Input.GetMouseButtonDown(0))
    {
        Vector2 localPos = GetLocalMousePosition();
        if (Vector2.Distance(localPos, targetPosition) < 50)
        {
            OnCorrectTap();
        }
    }
}
```

**المكافآت:**
- 10 عملات لكل نقرة صحيحة
- مضاعفات عند النقرات المتتالية

### 2. Golden Harvest

**الميكانيكية:**
- زراعة المحاصيل
- انتظار النمو (10 ثوان)
- حصاد وبيع

**نظام النمو:**
```csharp
if (cropPlanted && !cropReady)
{
    cropGrowthTimer += Time.deltaTime;
    float growthProgress = cropGrowthTimer / cropGrowthTime;
    
    if (growthProgress >= 1f)
    {
        cropReady = true;
    }
    
    cropGrowthBar.fillAmount = growthProgress;
}
```

**المكافآت:**
- 25 عملة لكل حصاد ناجح

### 3. Neon Pop

**الميكانيكية:**
- توليد فقاعات عشوائية
- تفجير الفقاعات بالنقر
- نظام Combo

**نظام التوليد:**
```csharp
private void SpawnBubble()
{
    GameObject bubble = Instantiate(bubblePrefab, bubblesContainer);
    RectTransform rect = bubble.GetComponent<RectTransform>();
    
    rect.anchoredPosition = new Vector2(
        Random.Range(-400, 400),
        Random.Range(-600, 600)
    );
    
    bubble.GetComponent<Image>().color = new Color(
        Random.value, Random.value, Random.value
    );
}
```

**المكافآت:**
- 15 عملة × مضاعف Combo

### 4. Cyber Slither

**الميكانيكية:**
- توجيه الثعبان بأزرار الأسهم
- جمع الطعام للنمو
- تجنب الجدران والجسم

**نظام الحركة:**
```csharp
private void MoveSnake()
{
    Vector2Int newHead = snakeBody[0] + direction;
    
    // التحقق من الاصطدام
    if (IsOutOfBounds(newHead) || snakeBody.Contains(newHead))
    {
        EndGame();
        return;
    }
    
    snakeBody.Insert(0, newHead);
    
    if (newHead == foodPosition)
    {
        SpawnFood();
    }
    else
    {
        snakeBody.RemoveAt(snakeBody.Count - 1);
    }
}
```

**المكافآت:**
- 20 عملة لكل طعام

### 5. Block Crush

**الميكانيكية:**
- شبكة 10×10
- ترتيب المكعبات
- تفجير الصفوف والأعمدة الكاملة

**نظام المطابقة:**
```csharp
private void CheckForMatches()
{
    List<Vector2Int> toRemove = new List<Vector2Int>();
    
    // التحقق من الصفوف الكاملة
    for (int y = 0; y < gridHeight; y++)
    {
        bool rowFull = IsRowFull(y);
        if (rowFull)
        {
            AddRowToRemove(y, toRemove);
        }
    }
    
    if (toRemove.Count > 0)
    {
        combo++;
        RemoveBlocks(toRemove);
    }
}
```

**المكافآت:**
- 50 عملة × عدد المكعبات × مضاعل Combo

### 6. Sky Stacker

**الميكانيكية:**
- قطع تسقط من الأعلى
- وضع القطع بدقة
- تقليل العرض مع كل قطعة

**نظام الحساب:**
```csharp
private float CalculateOverlap(float currentPos, float lastPos)
{
    float currentLeft = currentPos - pieceWidth / 2;
    float currentRight = currentPos + pieceWidth / 2;
    float lastLeft = lastPos - pieceWidth / 2;
    float lastRight = lastPos + pieceWidth / 2;
    
    float overlapLeft = Mathf.Max(currentLeft, lastLeft);
    float overlapRight = Mathf.Min(currentRight, lastRight);
    
    return Mathf.Max(0, overlapRight - overlapLeft);
}
```

**المكافآت:**
- 30 عملة لكل قطعة ناجحة

### 7. Dash Escape

**الميكانيكية:**
- جري مستمر
- قفز لتجاوز العقبات
- جمع العملات

**نظام الفيزياء:**
```csharp
private void HandlePlayerMovement()
{
    if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
    {
        isJumping = true;
        playerVelocityY = jumpForce;
    }
    
    if (isJumping)
    {
        playerVelocityY -= gravity * Time.deltaTime;
        playerY += playerVelocityY * Time.deltaTime;
        
        if (playerY <= 0)
        {
            playerY = 0;
            isJumping = false;
        }
    }
}
```

**المكافآت:**
- 10 عملات لكل عملة مجمعة
- 5 عملات لكل متر

### 8. Brain Storm

**الميكانيكية:**
- أسئلة رياضية ومنطقية
- 4 خيارات إجابة
- 120 ثانية للعب

**نظام الأسئلة:**
```csharp
private void GeneratePuzzles()
{
    puzzles.Add(new PuzzleData
    {
        question = "ما هو 5 + 3؟",
        answers = new[] { "7", "8", "9", "10" },
        correctIndex = 1
    });
    // ... المزيد من الأسئلة
}
```

**المكافآت:**
- 50 عملة لكل إجابة صحيحة

### 9. Mind Match

**الميكانيكية:**
- تسلسل ألوان متزايد
- تذكر التسلسل
- إعادة الإدخال بدقة

**نظام التسلسل:**
```csharp
private IEnumerator PlaySequence()
{
    for (int i = 0; i < sequence.Count; i++)
    {
        yield return new WaitForSeconds(delayBetweenMoves);
        
        int colorIndex = sequence[i];
        yield return StartCoroutine(FlashButton(colorIndex));
    }
}
```

**المكافآت:**
- 100 عملة لكل مستوى

### 10. Nitro Race

**الميكانيكية:**
- قيادة سيارة
- تجنب السيارات الأخرى
- جمع العملات

**نظام التحكم:**
```csharp
private void HandlePlayerMovement()
{
    if (Input.GetKey(KeyCode.LeftArrow))
        playerX -= playerSpeed * Time.deltaTime;
    if (Input.GetKey(KeyCode.RightArrow))
        playerX += playerSpeed * Time.deltaTime;
    
    playerX = Mathf.Clamp(playerX, -playerBoundaryX, playerBoundaryX);
}
```

**المكافآت:**
- 15 عملة لكل عملة مجمعة
- 25 عملة لكل سيارة تجاوز

## نظام الصوت والموسيقى

### الموسيقى الخلفية
```csharp
private void Start()
{
    backgroundMusic = GetComponent<AudioSource>();
    if (backgroundMusic != null)
    {
        backgroundMusic.volume = musicVolume;
        backgroundMusic.loop = true;
        if (musicEnabled)
            backgroundMusic.Play();
    }
}
```

### التحكم بالصوت
```csharp
public void SetMusicVolume(float volume)
{
    musicVolume = Mathf.Clamp01(volume);
    if (backgroundMusic != null)
        backgroundMusic.volume = musicVolume;
    SavePlayerData();
}
```

## نظام الانتقال بين المشاهد

### تحميل المشهد
```csharp
public void LoadScene(string sceneName)
{
    SceneManager.LoadScene(sceneName);
}

public void LoadScene(int sceneIndex)
{
    SceneManager.LoadScene(sceneIndex);
}
```

## معايير الأداء

### استهلاك الذاكرة
- GameManager: Singleton (نسخة واحدة فقط)
- كل لعبة: تحرر الموارد عند الخروج
- Object Pooling: للعناصر المتكررة

### معدل الإطارات
- الهدف: 60 FPS على الأجهزة القديمة
- 120 FPS على الأجهزة الحديثة

### استهلاك البطارية
- تقليل استهلاك CPU بتقليل Update calls
- استخدام Coroutines بدلاً من Update

## الأخطاء المحتملة والحلول

### 1. NullReferenceException
**السبب:** عدم تعيين الـ References في Inspector
**الحل:** التحقق من جميع الـ References قبل الاستخدام

```csharp
if (xPlayManager == null)
{
    Debug.LogError("XPlayManager not found!");
    return;
}
```

### 2. Scene Not Found
**السبب:** اسم المشهد غير صحيح
**الحل:** التأكد من أن اسم المشهد يطابق اسم الملف

```csharp
public void LoadScene(string sceneName)
{
    if (string.IsNullOrEmpty(sceneName))
    {
        Debug.LogError("Scene name is empty!");
        return;
    }
    SceneManager.LoadScene(sceneName);
}
```

### 3. PlayerPrefs Overflow
**السبب:** حفظ بيانات كبيرة جداً
**الحل:** استخدام JSON للبيانات المعقدة

## الاختبار والضمان

### اختبار الوحدة (Unit Testing)
```csharp
[Test]
public void TestAddCoins()
{
    XPlayManager.Instance.AddCoins(100);
    Assert.AreEqual(100, XPlayManager.Instance.GetCoins());
}
```

### اختبار التكامل (Integration Testing)
- اختبار الانتقال بين المشاهد
- اختبار نظام الحفظ والتحميل
- اختبار نظام الإعلانات

## الأمان

### حماية البيانات
- تشفير بيانات اللاعب (في الإنتاج)
- التحقق من صحة البيانات المحفوظة

### منع الغش
- التحقق من قيم العملات والطاقة
- تسجيل جميع العمليات

## التوسع المستقبلي

### إضافة ميزات جديدة
1. نظام الإنجازات (Achievements)
2. نظام التصنيفات (Leaderboards)
3. نظام المتجر (Shop)
4. نظام الأصدقاء (Friends)

### تحسينات الأداء
1. استخدام Addressables للأصول
2. تحسين الرسومات
3. تقليل حجم التطبيق

---

**آخر تحديث:** مارس 2026
**الإصدار:** 1.0.0
