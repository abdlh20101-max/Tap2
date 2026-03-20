using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// MindMatchManager: لعبة تحدي الذاكرة (Mind Match)
/// تذكر تسلسل الألوان والأرقام
/// Developed by: Abdullah Al-husini
/// </summary>
public class MindMatchManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private int initialSequenceLength = 3;
    [SerializeField] private float delayBetweenMoves = 0.5f;
    [SerializeField] private int coinsPerLevel = 100;

    [Header("UI References")]
    [SerializeField] private Text levelText;
    [SerializeField] private Text coinsText;
    [SerializeField] private Button backButton;

    [Header("Game Objects")]
    [SerializeField] private Button[] colorButtons = new Button[4];
    [SerializeField] private Color[] buttonColors = new Color[4];

    private XPlayManager gameManager;
    private List<int> sequence = new List<int>();
    private List<int> playerSequence = new List<int>();
    private int level = 0;
    private int earnedCoins = 0;
    private bool gameActive = true;
    private bool waitingForPlayerInput = false;

    private void Update()
    {
        if (!gameActive)
        {
            return;
        }

        UpdateUI();
    }

    private void Start()
    {
        gameManager = XPlayManager.Instance;

        if (backButton != null)
            backButton.onClick.AddListener(BackToMenu);

        // إعداد أزرار الألوان
        for (int i = 0; i < colorButtons.Length; i++)
        {
            int index = i;
            colorButtons[i].onClick.AddListener(() => OnColorButtonPressed(index));
        }

        // بدء اللعبة
        StartCoroutine(StartNewLevel());
    }

    private IEnumerator StartNewLevel()
    {
        level++;
        sequence.Add(Random.Range(0, 4));
        playerSequence.Clear();

        yield return new WaitForSeconds(1f);

        // عرض التسلسل
        yield return StartCoroutine(PlaySequence());

        // انتظار إدخال اللاعب
        waitingForPlayerInput = true;
    }

    private IEnumerator PlaySequence()
    {
        for (int i = 0; i < sequence.Count; i++)
        {
            yield return new WaitForSeconds(delayBetweenMoves);
            
            int colorIndex = sequence[i];
            yield return StartCoroutine(FlashButton(colorIndex));
        }
    }

    private IEnumerator FlashButton(int colorIndex)
    {
        Button button = colorButtons[colorIndex];
        Image buttonImage = button.GetComponent<Image>();
        Color originalColor = buttonImage.color;

        // تفتيح الزر
        buttonImage.color = Color.white;
        yield return new WaitForSeconds(0.3f);

        // إعادة اللون الأصلي
        buttonImage.color = originalColor;
        yield return new WaitForSeconds(0.2f);
    }

    private void OnColorButtonPressed(int colorIndex)
    {
        if (!waitingForPlayerInput || !gameActive)
            return;

        playerSequence.Add(colorIndex);

        // تأثير بصري
        StartCoroutine(FlashButton(colorIndex));

        // التحقق من الإجابة
        if (playerSequence[playerSequence.Count - 1] != sequence[playerSequence.Count - 1])
        {
            // إجابة خاطئة
            EndGame();
            return;
        }

        // التحقق من إكمال التسلسل
        if (playerSequence.Count == sequence.Count)
        {
            // اكتمل التسلسل بنجاح
            earnedCoins += coinsPerLevel;
            gameManager.AddCoins(coinsPerLevel);
            waitingForPlayerInput = false;

            StartCoroutine(StartNewLevel());
        }
    }

    private void UpdateUI()
    {
        if (levelText != null)
            levelText.text = $"المستوى: {level}";

        if (coinsText != null)
            coinsText.text = $"💰 {earnedCoins}";
    }

    private void EndGame()
    {
        gameActive = false;
        waitingForPlayerInput = false;
        gameManager.AddCoins(earnedCoins);
        Debug.Log($"Memory Game Over! Level: {level}, Coins: {earnedCoins}");
        BackToMenu();
    }

    private void BackToMenu()
    {
        gameManager.LoadScene(GameConfig.Scenes.MAIN_MENU);
    }
}
