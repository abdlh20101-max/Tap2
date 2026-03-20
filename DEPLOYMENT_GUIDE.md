# دليل النشر على Google Play Console

## المتطلبات الأساسية

### 1. حساب Google Play Developer
- **الرسوم:** 25 دولار أمريكي (لمرة واحدة)
- **الموقع:** https://play.google.com/console
- **المتطلبات:** حساب Google نشط

### 2. إعدادات Android في Unity
- **Target API Level:** 31 أو أعلى
- **Minimum API Level:** 21 (Android 5.0)
- **Architecture:** ARM64-v8a و ARMv7

### 3. شهادة التوقيع (Keystore)
- ملف `.keystore` للتوقيع الرقمي
- كلمة مرور آمنة
- معلومات صاحب الحساب

## خطوات النشر

### المرحلة 1: إعداد المشروع

#### 1.1 إعدادات البناء (Build Settings)
```
File > Build Settings
- Platform: Android
- Scenes In Build:
  - Intro (Scene 0)
  - MainMenu (Scene 1)
  - PulseClicker (Scene 2)
  - GoldenHarvest (Scene 3)
  - NeonPop (Scene 4)
  - CyberSlither (Scene 5)
  - BlockCrush (Scene 6)
  - SkyStacker (Scene 7)
  - DashEscape (Scene 8)
  - BrainStorm (Scene 9)
  - MindMatch (Scene 10)
  - NitroRace (Scene 11)
```

#### 1.2 إعدادات اللاعب (Player Settings)
```
Edit > Project Settings > Player
- Product Name: X-Play
- Version: 1.0.0
- Bundle Version Code: 1
- Minimum API Level: 21
- Target API Level: 31+
- Graphics APIs: OpenGL ES 3.0
- Scripting Backend: IL2CPP
- API Compatibility Level: .NET Framework
```

#### 1.3 الأيقونة والـ Splash Screen
```
- App Icon: 512x512 PNG
- Splash Screen: 1080x1920 PNG
- Package Name: com.xplay.games
```

### المرحلة 2: إنشاء Keystore

#### 2.1 إنشاء ملف Keystore جديد
```
Edit > Project Settings > Player > Publishing Settings
- Create New Keystore
- Keystore Name: xplay.keystore
- Keystore Password: [كلمة مرور قوية]
- Key Alias: xplay_key
- Key Password: [نفس كلمة المرور]
- Validity (years): 25
```

#### 2.2 ملء معلومات المفتاح
```
- First and Last Name: [اسمك]
- Organizational Unit: [شركتك]
- Organization: [اسم المؤسسة]
- City or Locality: [المدينة]
- State or Province: [الولاية/المقاطعة]
- Country Code: [رمز الدولة - مثل SA للسعودية]
```

### المرحلة 3: بناء التطبيق

#### 3.1 بناء APK
```
File > Build Settings
- Build System: Gradle
- Export Project: OFF (لبناء مباشر)
- Build
- اختر مكان الحفظ
```

#### 3.2 بناء AAB (Android App Bundle)
```
File > Build Settings
- Build System: Gradle
- Build App Bundle (Google Play)
- اختر مكان الحفظ
```

**ملاحظة:** Google Play يفضل AAB بدلاً من APK

### المرحلة 4: رفع على Google Play Console

#### 4.1 إنشاء تطبيق جديد
```
1. اذهب إلى Google Play Console
2. اضغط "Create app"
3. أدخل اسم التطبيق: X-Play
4. اختر الفئة: Games
5. اختر نوع التطبيق: Free
```

#### 4.2 ملء معلومات التطبيق
```
App > App information
- App name: X-Play
- Short description: X-Play: منصة ألعاب عالمية بـ 10 ألعاب متنوعة. Developed by: Abdullah Al-husini
- Full description: وصف كامل للتطبيق
- Category: Games
- Content rating: [حسب الفئة العمرية]
```

#### 4.3 رفع الأيقونة والصور
```
Store listing > Graphics
- App icon: 512x512 PNG
- Feature graphic: 1024x500 PNG
- Screenshots: 4-8 صور (1080x1920)
- Promotional graphic: 180x120 PNG
```

#### 4.4 رفع ملف AAB
```
Release > Production
- Upload AAB file
- Review and release
```

### المرحلة 5: الاختبار

#### 5.1 اختبار الجهاز الفعلي
```
1. قم بتوصيل جهاز Android
2. File > Build And Run
3. اختبر جميع الميزات
4. تحقق من عدم وجود أخطاء
```

#### 5.2 اختبار الأداء
```
- استخدم Android Profiler
- تحقق من استهلاك الذاكرة
- تحقق من استهلاك البطارية
- تحقق من سرعة التحميل
```

#### 5.3 اختبار الإعلانات
```
- اختبر عرض الإعلانات
- اختبر نقر الإعلانات
- اختبر البوب-أب
```

### المرحلة 6: المراجعة والموافقة

#### 6.1 سياسة الخصوصية
```
- أضف رابط سياسة الخصوصية
- تأكد من الامتثال لـ GDPR
- اشرح استخدام البيانات
```

#### 6.2 تصنيف المحتوى
```
- أكمل نموذج تصنيف المحتوى
- اختر الفئة العمرية المناسبة
- اختر الفئات المناسبة
```

#### 6.3 الموافقة على الشروط
```
- وافق على شروط Google Play
- وافق على سياسات المحتوى
- وافق على سياسات الإعلانات
```

### المرحلة 7: الإطلاق

#### 7.1 اختيار الدول
```
Release > Manage releases
- اختر الدول المستهدفة
- اختر نسبة الإطلاق (مثل 10% أولاً)
- راقب الأخطاء والتقييمات
```

#### 7.2 الإطلاق التدريجي
```
- ابدأ بـ 10% من المستخدمين
- راقب الأخطاء والتقييمات
- زيادة النسبة تدريجياً
- إطلاق كامل بعد 7 أيام
```

## معايير Google Play

### 1. سياسات المحتوى
- لا يوجد محتوى عنيف أو مسيء
- لا يوجد محتوى جنسي
- لا يوجد محتوى تمييزي
- لا يوجد محتوى مخادع

### 2. سياسات الأداء
- لا تعطل التطبيق
- لا توجد أخطاء متكررة
- لا توجد عمليات بطيئة
- استهلاك معقول للموارد

### 3. سياسات الأمان
- لا توجد ثغرات أمنية
- حماية بيانات المستخدم
- عدم الوصول غير المصرح به
- عدم جمع البيانات الشخصية

### 4. سياسات الإعلانات
- إعلانات غير مزعجة
- إعلانات واضحة ومميزة
- عدم الإعلان عن محتوى ممنوع
- عدم خداع المستخدمين

## استكشاف الأخطاء

### خطأ: "Your app contains code that is not compliant"
**الحل:** تأكد من عدم استخدام APIs محظورة

### خطأ: "Crash or ANR"
**الحل:** اختبر التطبيق على أجهزة مختلفة

### خطأ: "Insufficient permissions"
**الحل:** أضف الأذونات المطلوبة في AndroidManifest.xml

### خطأ: "App not optimized for tablets"
**الحل:** اختبر على أجهزة لوحية وحسّن الواجهة

## بعد الإطلاق

### 1. مراقبة الأداء
```
Analytics > User Acquisition
- عدد التحميلات
- معدل الاحتفاظ
- معدل الإلغاء
```

### 2. قراءة التقييمات
```
Reviews
- اقرأ تقييمات المستخدمين
- رد على الملاحظات
- أصلح الأخطاء المذكورة
```

### 3. إصدار تحديثات
```
Release > Manage releases
- أضف تحديثات جديدة
- أصلح الأخطاء
- أضف ميزات جديدة
```

## نصائح للنجاح

1. **اختبر جيداً قبل الإطلاق**
   - اختبر على أجهزة مختلفة
   - اختبر على إصدارات Android مختلفة
   - اختبر على أحجام شاشات مختلفة

2. **اكتب وصفاً جيداً**
   - اشرح الميزات بوضوح
   - استخدم لغة جذابة
   - أضف لقطات شاشة جيدة

3. **اختر أيقونة جذابة**
   - اجعلها بسيطة وواضحة
   - استخدم ألوان جذابة
   - تأكد من وضوحها في أحجام صغيرة

4. **راقب التقييمات**
   - رد على جميع التقييمات
   - أصلح الأخطاء بسرعة
   - استمع لملاحظات المستخدمين

5. **حدّث التطبيق بانتظام**
   - أضف ميزات جديدة
   - أصلح الأخطاء
   - حسّن الأداء

## الموارد المفيدة

- [Google Play Console Help](https://support.google.com/googleplay/android-developer)
- [Android App Bundle Documentation](https://developer.android.com/guide/app-bundle)
- [Google Play Policies](https://play.google.com/about/developer-content-policy/)
- [Android Development Guide](https://developer.android.com/guide)

---

**آخر تحديث:** مارس 2026
