using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// PulseClickerManager: لعبة النقر السريع (Pulse Clicker)
/// 4 أوضاع: السنتر، الدائرة المتحركة، التصاعد، تجاوز العقبات
/// Developed by: Abdullah Al-husini
/// </summary>
public class PulseClickerManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private int gameMode = 0; // 0: Center, 1: Moving Circle, 2: Ascending, 3: Obstacles
    [SerializeField] private float gameDuration = 60f;
    [SerializeField] private int coinsPerTap = 10;

    [Header("UI References")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text timerText;
    [SerializeField] private Button backButton;

    [Header("Game Objects")]
    [SerializeField] private Image targetCircle;
    [SerializeField] private RectTransform gameArea;

    private XPlayManager gameManager;
    private int score = 0;
    private float timeRemaining;
    private bool gameActive = true;
    private float circleSpeed = 100f;
    private Vector2 circleDirection = Vector2.right;

    private void Start()
    {
        gameManager = XPlayManager.Instance;
        timeRemaining = gameDuration;

        if (backButton != null)
            backButton.onClick.AddListener(BackToMenu);

        // إعداد اللعبة حسب الوضع
        SetupGameMode();
    }

    private void Update()
    {
        if (!gameActive)
            return;

        // تحديث الوقت
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            EndGame();
            return;
        }

        // تحديث عرض الوقت
        if (timerText != null)
            timerText.text = $"الوقت: {Mathf.Max(0, timeRemaining):F1}";

        // تحديث حركة الدائرة حسب الوضع
        UpdateGameMode();

        // معالجة الـ Input
        HandleInput();
    }

    private void SetupGameMode()
    {
        switch (gameMode)
        {
            case 0: // Center
                targetCircle.rectTransform.anchoredPosition = Vector2.zero;
                break;
            case 1: // Moving Circle
                circleSpeed = 150f;
                break;
            case 2: // Ascending
                circleSpeed = 200f;
                break;
            case 3: // Obstacles
                circleSpeed = 120f;
                break;
        }
    }

    private void UpdateGameMode()
    {
        switch (gameMode)
        {
            case 0: // Center - لا حركة
                break;

            case 1: // Moving Circle - تحرك يمين ويسار
                MoveCircleHorizontally();
                break;

            case 2: // Ascending - تحرك لأعلى بسرعة متزايدة
                MoveCircleAscending();
                break;

            case 3: // Obstacles - تحرك عشوائي
                MoveCircleRandomly();
                break;
        }
    }

    private void MoveCircleHorizontally()
    {
        RectTransform rect = targetCircle.rectTransform;
        rect.anchoredPosition += new Vector2(circleDirection.x * circleSpeed * Time.deltaTime, 0);

        // عكس الاتجاه عند الوصول للحافة
        if (Mathf.Abs(rect.anchoredPosition.x) > 300)
            circleDirection.x *= -1;
    }

    private void MoveCircleAscending()
    {
        RectTransform rect = targetCircle.rectTransform;
        rect.anchoredPosition += new Vector2(0, circleSpeed * Time.deltaTime);

        // إعادة تعيين عند الوصول للأعلى
        if (rect.anchoredPosition.y > 500)
        {
            rect.anchoredPosition = new Vector2(Random.Range(-200, 200), -400);
            circleSpeed += 20f; // زيادة السرعة
        }
    }

    private void MoveCircleRandomly()
    {
        RectTransform rect = targetCircle.rectTransform;
        rect.anchoredPosition += new Vector2(
            Random.Range(-1f, 1f) * circleSpeed * Time.deltaTime,
            Random.Range(-1f, 1f) * circleSpeed * Time.deltaTime
        );

        // الحفاظ على الدائرة داخل منطقة اللعب
        rect.anchoredPosition = new Vector2(
            Mathf.Clamp(rect.anchoredPosition.x, -400, 400),
            Mathf.Clamp(rect.anchoredPosition.y, -600, 600)
        );
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                gameArea,
                mousePos,
                null,
                out Vector2 localPos
            );

            // التحقق من الضغط على الدائرة
            if (Vector2.Distance(localPos, targetCircle.rectTransform.anchoredPosition) < 50)
            {
                OnCorrectTap();
            }
            else
            {
                OnWrongTap();
            }
        }
    }

    private void OnCorrectTap()
    {
        score++;
        gameManager.AddCoins(coinsPerTap);

        if (scoreText != null)
            scoreText.text = $"النقرات: {score}";

        // تأثير بصري (يمكن إضافة animation هنا)
        StartCoroutine(FlashCircle());
    }

    private void OnWrongTap()
    {
        // تأثير خطأ (يمكن إضافة animation هنا)
        Debug.Log("Wrong tap!");
    }

    private IEnumerator FlashCircle()
    {
        Color originalColor = targetCircle.color;
        targetCircle.color = Color.green;
        yield return new WaitForSeconds(0.1f);
        targetCircle.color = originalColor;
    }

    private void EndGame()
    {
        gameActive = false;
        gameManager.AddCoins(score * coinsPerTap);
        Debug.Log($"Game Over! Final Score: {score}");
        StartCoroutine(ShowGameOverAndReturn());
    }

    private IEnumerator ShowGameOverAndReturn()
    {
        yield return new WaitForSeconds(2f);
        BackToMenu();
    }

    private void BackToMenu()
    {
        gameManager.LoadScene(GameConfig.Scenes.MAIN_MENU);
    }
}
