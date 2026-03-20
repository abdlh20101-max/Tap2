# بنية مشروع GamesHub

## نظرة عامة على الهيكل

```
GamesHub/
├── Assets/
│   ├── Scripts/                          # جميع السكريبتات
│   │   ├── GameManager.cs               # مدير اللعبة الرئيسي (Singleton)
│   │   ├── IntroManager.cs              # مدير مشهد البداية
│   │   ├── MainMenuManager.cs           # مدير الواجهة الرئيسية
│   │   ├── AdManager.cs                 # مدير الإعلانات
│   │   ├── BackgroundController.cs      # التحكم بالخلفية 3D
│   │   ├── UIManager.cs                 # مدير واجهة المستخدم
│   │   ├── SettingsManager.cs           # مدير الإعدادات
│   │   ├── GameConfig.cs                # ملف التكوين المركزي
│   │   └── Games/                       # مديري الألعاب
│   │       ├── TapGameManager.cs        # لعبة النقر الصحيح
│   │       ├── FarmGameManager.cs       # لعبة مزرعة الذهب
│   │       ├── BubbleGameManager.cs     # لعبة صائد الفقاعات
│   │       ├── SnakeGameManager.cs      # لعبة ثعبان النيون
│   │       ├── BlockGameManager.cs      # لعبة مكعبات الطاقة
│   │       ├── TowerGameManager.cs      # لعبة برج التوازن
│   │       ├── RunnerGameManager.cs     # لعبة هروب المحترف
│   │       ├── PuzzleGameManager.cs     # لعبة لغز الأرقام
│   │       ├── MemoryGameManager.cs     # لعبة تحدي الذاكرة
│   │       └── RaceGameManager.cs       # لعبة سباق السرعة
│   │
│   ├── Scenes/                          # جميع المشاهد
│   │   ├── Intro.unity                  # مشهد البداية (3 ثوان)
│   │   ├── MainMenu.unity               # الواجهة الرئيسية
│   │   ├── TapGame.unity                # مشهد لعبة النقر
│   │   ├── FarmGame.unity               # مشهد لعبة المزرعة
│   │   ├── BubbleGame.unity             # مشهد لعبة الفقاعات
│   │   ├── SnakeGame.unity              # مشهد لعبة الثعبان
│   │   ├── BlockGame.unity              # مشهد لعبة المكعبات
│   │   ├── TowerGame.unity              # مشهد لعبة البرج
│   │   ├── RunnerGame.unity             # مشهد لعبة الهروب
│   │   ├── PuzzleGame.unity             # مشهد لعبة اللغز
│   │   ├── MemoryGame.unity             # مشهد لعبة الذاكرة
│   │   └── RaceGame.unity               # مشهد لعبة السباق
│   │
│   ├── Prefabs/                         # (سيتم إنشاؤها)
│   │   ├── GameButton.prefab
│   │   ├── Bubble.prefab
│   │   ├── SnakeSegment.prefab
│   │   ├── Block.prefab
│   │   ├── Piece.prefab
│   │   ├── Obstacle.prefab
│   │   └── ...
│   │
│   ├── Audio/                           # (سيتم إضافتها)
│   │   ├── BackgroundMusic.mp3
│   │   ├── TapSound.wav
│   │   ├── CoinSound.wav
│   │   ├── SuccessSound.wav
│   │   └── ...
│   │
│   ├── Sprites/                         # (سيتم إضافتها)
│   │   ├── GameIcons/
│   │   │   ├── TapGame.png
│   │   │   ├── FarmGame.png
│   │   │   └── ...
│   │   ├── UI/
│   │   │   ├── Button.png
│   │   │   ├── Panel.png
│   │   │   └── ...
│   │   └── GameAssets/
│   │       ├── Bubble.png
│   │       ├── Block.png
│   │       └── ...
│   │
│   ├── Materials/                       # (سيتم إضافتها)
│   │   ├── BackgroundMaterial.mat
│   │   ├── BubbleMaterial.mat
│   │   └── ...
│   │
│   └── Plugins/                         # (إن لزم الأمر)
│       └── (مكتبات خارجية)
│
├── Packages/
│   └── manifest.json                    # إدارة حزم Unity
│
├── ProjectSettings/
│   └── (إعدادات المشروع)
│
├── Documentation/                       # (سيتم إنشاؤها)
│   ├── API_REFERENCE.md
│   ├── ARCHITECTURE.md
│   └── ...
│
├── Tests/                               # (سيتم إنشاؤها)
│   ├── GameManagerTests.cs
│   ├── AdManagerTests.cs
│   └── ...
│
├── README.md                            # الدليل الأساسي
├── README_COMPLETE.md                   # الدليل الشامل
├── TECHNICAL_DOCUMENTATION.md           # التوثيق التقني
├── DEVELOPMENT_GUIDE.md                 # دليل التطوير
├── DEPLOYMENT_GUIDE.md                  # دليل النشر
├── CHANGELOG.md                         # سجل التغييرات
├── QUALITY_ASSURANCE.md                 # تقرير الجودة
├── PROJECT_STRUCTURE.md                 # هذا الملف
├── .gitignore                           # ملف تجاهل Git
└── .git/                                # مستودع Git
```

## وصف كل مكون

### Scripts (السكريبتات)

#### المديرين الأساسيين:

**GameManager.cs**
- إدارة حالة اللعبة
- نظام العملات والطاقة
- حفظ واسترجاع البيانات
- إدارة الموسيقى والصوت
- نمط Singleton

**IntroManager.cs**
- عرض مشهد البداية
- انتقال سلس إلى الواجهة الرئيسية
- تأثيرات بصرية

**MainMenuManager.cs**
- عرض قائمة الألعاب
- إدارة الانتقالات
- عرض البيانات الأساسية

**AdManager.cs**
- إدارة البنر السفلي
- إدارة البوب-أب
- توقيت الإعلانات

#### مديري الواجهة:

**BackgroundController.cs**
- حركة الخلفية 3D
- تغيير اللون
- تغيير الحجم

**UIManager.cs**
- عرض العملات والطاقة
- إدارة الإشعارات
- تحديث الواجهة

**SettingsManager.cs**
- إدارة الموسيقى والصوت
- إعادة تعيين البيانات
- حفظ الإعدادات

**GameConfig.cs**
- ثوابت مركزية
- مكافآت الألعاب
- متطلبات الطاقة
- مفاتيح PlayerPrefs

#### مديري الألعاب:

كل لعبة لها مدير خاص بها يتعامل مع:
- منطق اللعبة
- حساب النقاط
- إدارة الموارد
- الانتقال بين الحالات

### Scenes (المشاهد)

**Intro.unity**
- مشهد البداية
- مدة 3 ثوان
- انتقال سلس

**MainMenu.unity**
- الواجهة الرئيسية
- قائمة الألعاب
- عرض البيانات

**Game Scenes**
- مشهد لكل لعبة
- 10 مشاهد إجمالاً
- بنية موحدة

### Prefabs (النماذج المسبقة)

سيتم إنشاء Prefabs لـ:
- أزرار اللعبة
- العناصر المتحركة
- الأصول البصرية
- الكائنات المتكررة

### Audio (الصوتيات)

- **موسيقى خلفية:** ناعمة وهادئة
- **مؤثرات صوتية:** لكل إجراء
- **أصوات الألعاب:** خاصة بكل لعبة

### Sprites (الصور)

- **أيقونات الألعاب:** 512x512 PNG
- **عناصر الواجهة:** أزرار، لوحات، إلخ
- **أصول الألعاب:** فقاعات، مكعبات، إلخ

### Materials (المواد)

- **مواد الخلفية:** للخلفية 3D
- **مواد الألعاب:** للعناصر المختلفة

## تدفق البيانات

```
GameManager (Singleton)
    ├── PlayerData
    │   ├── Coins
    │   ├── Energy
    │   ├── MusicEnabled
    │   ├── SFXEnabled
    │   └── MusicVolume
    │
    ├── GameState
    │   ├── CurrentScene
    │   ├── LastPlayedGame
    │   └── TotalGamesPlayed
    │
    └── Events
        ├── OnCoinsChanged
        ├── OnEnergyChanged
        └── OnGameStateChanged
```

## تدفق المشاهد

```
Intro (3 ثوان)
    ↓
MainMenu
    ├── TapGame
    ├── FarmGame
    ├── BubbleGame
    ├── SnakeGame
    ├── BlockGame
    ├── TowerGame
    ├── RunnerGame
    ├── PuzzleGame
    ├── MemoryGame
    └── RaceGame
        ↓
    (عودة إلى MainMenu)
```

## معايير التسمية

### المتغيرات:
- `private int playerHealth;` - camelCase للـ private
- `public int Score { get; }` - PascalCase للـ public

### الدوال:
- `private void OnPlayerDeath()` - PascalCase مع فعل
- `public bool IsPlayerAlive()` - PascalCase مع Is/Has/Can

### الفئات:
- `public class GameManager` - PascalCase
- `public class TapGameManager` - PascalCase

### الثوابت:
- `private const int MAX_HEALTH = 100;` - UPPER_CASE

## معايير الملفات

### حجم الملفات:
- **السكريبتات:** 200-500 سطر لكل ملف
- **المشاهد:** 5-10 MB لكل مشهد
- **الصور:** 100 KB - 1 MB لكل صورة
- **الصوتيات:** 500 KB - 5 MB لكل ملف

### تنظيم المجلدات:
- مجلد منفصل لكل نوع من الأصول
- تسمية واضحة ومنطقية
- عدم وضع ملفات في الجذر

## نظام التحكم بالإصدارات (Git)

### فروع:
- `master` - الإصدار الرسمي
- `develop` - التطوير النشط
- `feature/*` - ميزات جديدة

### Commits:
- رسالة واضحة ومختصرة
- ربط بـ Issues عند الحاجة
- تجميع التغييرات المرتبطة

## الاختبار

### اختبارات الوحدة:
- اختبار كل دالة بشكل منفصل
- التحقق من الحالات الخاصة
- التحقق من الأخطاء

### اختبارات التكامل:
- اختبار التفاعل بين المكونات
- اختبار الانتقال بين المشاهد
- اختبار حفظ البيانات

### اختبارات الأداء:
- قياس استهلاك الذاكرة
- قياس معدل الإطارات
- قياس استهلاك البطارية

## الأداء والتحسينات

### استهلاك الموارد:
- **الذاكرة:** < 20 MB
- **المعالج:** < 50% استخدام
- **البطارية:** استهلاك منخفض

### معدل الإطارات:
- **الهدف:** 60 FPS
- **الحد الأدنى:** 30 FPS
- **الأجهزة القديمة:** تحسينات خاصة

## التوسع المستقبلي

### إضافة ألعاب جديدة:
1. إنشاء مشهد جديد
2. إنشاء مدير اللعبة
3. إضافة إلى GameConfig
4. إضافة إلى MainMenu

### إضافة ميزات:
1. تحديث GameManager
2. إضافة UI جديدة
3. إضافة سكريبتات جديدة
4. الاختبار والتوثيق

---

**آخر تحديث:** مارس 2026
**الإصدار:** 1.0.0
