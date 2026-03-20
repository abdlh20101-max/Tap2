# دليل التطوير - X-Play

Developed by: Abdullah Al-husini

## إعداد بيئة التطوير

### المتطلبات
- **Unity 2022.3.62f3** (إصدار محدد)
- **Visual Studio Code** أو **Visual Studio 2022**
- **Git** للتحكم بالإصدارات
- **.NET Framework 4.7.1** أو أعلى

### خطوات التثبيت

1. **تثبيت Unity Hub**
   ```bash
   # على Windows
   # قم بتحميل Unity Hub من https://unity.com/download
   
   # على macOS
   brew install --cask unity-hub
   
   # على Linux
   # قم بتحميل Unity Hub من https://unity.com/download
   ```

2. **تثبيت Unity 2022.3.62f3**
   ```bash
   # استخدم Unity Hub لتثبيت الإصدار المحدد
   # تأكد من تثبيت Android Build Support و iOS Build Support
   ```

3. **استنساخ المستودع**
   ```bash
   git clone https://github.com/abdlh20101-max/Tap2.git
   cd X-Play
   ```

4. **فتح المشروع في Unity**
   - افتح Unity Hub
   - اختر "Add Project from Disk"
   - حدد مجلد X-Play
   - انتظر حتى يتم تحميل المشروع

## هيكل المشروع

```
X-Play/
├── Assets/
│   ├── Scripts/
│   │   ├── XPlayManager.cs              # مدير المنصة الرئيسي
│   │   ├── IntroManager.cs             # مدير مشهد Intro
│   │   ├── MainMenuManager.cs          # مدير الواجهة الرئيسية
│   │   ├── AdManager.cs                # مدير الإعلانات
│   │   └── Games/
│   │       ├── PulseClickerManager.cs       # Pulse Clicker
│   │       ├── GoldenHarvestManager.cs      # Golden Harvest
│   │       ├── NeonPopManager.cs    # Neon Pop
│   │       ├── CyberSlitherManager.cs     # Cyber Slither
│   │       ├── BlockCrushManager.cs     # Block Crush
│   │       ├── SkyStackerManager.cs     # Sky Stacker
│   │       ├── DashEscapeManager.cs    # Dash Escape
│   │       ├── BrainStormManager.cs    # Brain Storm
│   │       ├── MindMatchManager.cs    # Mind Match
│   │       └── NitroRaceManager.cs      # Nitro Race
│   ├── Scenes/
│   │   ├── Intro.unity
│   │   ├── MainMenu.unity
│   │   ├── TapGame.unity
│   │   ├── FarmGame.unity
│   │   ├── BubbleGame.unity
│   │   ├── SnakeGame.unity
│   │   ├── BlockGame.unity
│   │   ├── TowerGame.unity
│   │   ├── RunnerGame.unity
│   │   ├── PuzzleGame.unity
│   │   ├── MemoryGame.unity
│   │   └── RaceGame.unity
│   ├── Prefabs/
│   │   ├── GameButton.prefab
│   │   ├── Bubble.prefab
│   │   ├── SnakeSegment.prefab
│   │   ├── Block.prefab
│   │   ├── Piece.prefab
│   │   └── ...
│   ├── Audio/
│   │   ├── BackgroundMusic.mp3
│   │   ├── TapSound.wav
│   │   ├── CoinSound.wav
│   │   └── ...
│   ├── Sprites/
│   │   ├── GameIcons/
│   │   ├── UI/
│   │   └── ...
│   ├── Materials/
│   └── Plugins/
├── ProjectSettings/
├── Packages/
│   └── manifest.json
├── README.md
├── TECHNICAL_DOCUMENTATION.md
├── DEVELOPMENT_GUIDE.md
└── .gitignore
```

## معايير الكود

### تسمية المتغيرات
```csharp
// Private fields
private int playerHealth;
private float moveSpeed;

// Public properties
public int Score { get; private set; }

// Constants
private const int MAX_HEALTH = 100;
private const float GRAVITY = 9.81f;

// Enums
public enum GameState { Playing, Paused, GameOver }
```

### تسمية الدوال
```csharp
// الفعل + الاسم
private void OnPlayerDeath()
private bool IsPlayerAlive()
private void UpdatePlayerPosition()
private void HandleInput()
private void CheckCollisions()
```

### التعليقات
```csharp
/// <summary>
/// وصف الدالة بشكل واضح
/// </summary>
/// <param name="amount">وصف المعامل</param>
/// <returns>وصف القيمة المرجعة</returns>
public void AddCoins(int amount)
{
    // تعليق للشرح
    coins += amount;
}
```

### معايير الكود
- استخدام 4 مسافات للمحاذاة (Indentation)
- سطر واحد لكل statement
- استخدام `using` statements في الأعلى
- تجنب الـ Magic Numbers

## سير العمل (Workflow)

### 1. إنشاء فرع جديد
```bash
git checkout -b feature/game-name
```

### 2. العمل على الميزة
```bash
# قم بالتعديلات
# اختبر الكود
# تأكد من عدم وجود أخطاء
```

### 3. Commit التغييرات
```bash
git add .
git commit -m "Add: description of changes"
```

### 4. Push إلى المستودع
```bash
git push origin feature/game-name
```

### 5. إنشاء Pull Request
- اذهب إلى GitHub
- اختر "New Pull Request"
- اختر الفرع الخاص بك
- أضف وصف التغييرات
- انتظر المراجعة والموافقة

## الاختبار

### اختبار اليد (Manual Testing)
1. افتح المشروع في Unity
2. اضغط Play
3. اختبر كل لعبة
4. تحقق من:
   - عدم وجود أخطاء في Console
   - صحة الحسابات
   - سلاسة الحركة
   - استجابة الـ UI

### اختبار الأداء
```csharp
// استخدم Profiler في Unity
// Window > Analysis > Profiler
// تحقق من:
// - CPU Usage
// - Memory Usage
// - GPU Usage
```

## بناء التطبيق

### بناء لـ Android
1. اذهب إلى File > Build Settings
2. اختر Android
3. تأكد من تعيين الـ Scenes الصحيحة
4. اضغط Build
5. حدد مكان الحفظ

### بناء لـ iOS
1. اذهب إلى File > Build Settings
2. اختر iOS
3. تأكد من تعيين الـ Scenes الصحيحة
4. اضغط Build
5. افتح Xcode وقم بالبناء النهائي

### بناء لـ WebGL
1. اذهب إلى File > Build Settings
2. اختر WebGL
3. اضغط Build
4. افتح index.html في المتصفح

## الأخطاء الشائعة وحلولها

### 1. "Scene 'SceneName' couldn't be loaded"
**السبب:** اسم المشهد غير صحيح أو المشهد لم يتم إضافته إلى Build Settings
**الحل:**
```bash
# تأكد من أن المشهد موجود في:
# File > Build Settings > Scenes In Build
```

### 2. "NullReferenceException"
**السبب:** محاولة الوصول إلى object غير موجود
**الحل:**
```csharp
// تحقق من القيمة قبل الاستخدام
if (xPlayManager != null)
{
    gameManager.AddCoins(100);
}
```

### 3. "The object of type 'GameObject' has been destroyed"
**السبب:** محاولة الوصول إلى object تم حذفه
**الحل:**
```csharp
// تحقق من وجود الـ Object
if (gameObject != null)
{
    // استخدم الـ Object
}
```

## نصائح التطوير

### 1. استخدم Debug.Log
```csharp
Debug.Log("Normal log");
Debug.LogWarning("Warning message");
Debug.LogError("Error message");
```

### 2. استخدم Gizmos للتصحيح
```csharp
private void OnDrawGizmos()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, 1f);
}
```

### 3. استخدم Coroutines للعمليات المعقدة
```csharp
StartCoroutine(PlayAnimation());

private IEnumerator PlayAnimation()
{
    yield return new WaitForSeconds(1f);
    // قم بشيء
}
```

### 4. استخدم Events للتواصل بين الـ Objects
```csharp
public event System.Action<int> OnCoinsChanged;

public void AddCoins(int amount)
{
    coins += amount;
    OnCoinsChanged?.Invoke(coins);
}
```

## الأداء والتحسينات

### 1. استخدم Object Pooling
```csharp
public class BulletPool : MonoBehaviour
{
    private Queue<GameObject> pool = new Queue<GameObject>();
    
    public GameObject GetBullet()
    {
        if (pool.Count > 0)
            return pool.Dequeue();
        return Instantiate(bulletPrefab);
    }
    
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        pool.Enqueue(bullet);
    }
}
```

### 2. تقليل استدعاءات Update
```csharp
// بدلاً من:
private void Update()
{
    // عمليات معقدة
}

// استخدم:
private float timer = 0f;
private void Update()
{
    timer += Time.deltaTime;
    if (timer >= 0.1f)
    {
        // عمليات معقدة
        timer = 0f;
    }
}
```

### 3. استخدم Caching
```csharp
private Rigidbody rb;

private void Start()
{
    rb = GetComponent<Rigidbody>(); // تخزين الـ Reference
}

private void Update()
{
    rb.velocity = new Vector3(moveSpeed, 0, 0); // استخدام الـ Cached Reference
}
```

## الموارد المفيدة

- [Unity Documentation](https://docs.unity.com/)
- [Unity Learn](https://learn.unity.com/)
- [C# Documentation](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [GitHub Guides](https://guides.github.com/)

## الدعم والمساعدة

للإبلاغ عن المشاكل أو طلب المساعدة:
1. افتح Issue على GitHub
2. اشرح المشكلة بالتفصيل
3. أرفق لقطات شاشة أو رسائل الخطأ

---

**آخر تحديث:** مارس 2026
