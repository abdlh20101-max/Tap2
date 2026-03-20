using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// BrainStormManager: لعبة عاصفة الدماغ (Brain Storm)
/// حل الألغاز الرياضية والمنطقية لجمع العملات
/// Developed by: Abdullah Al-husini
/// </summary>
public class BrainStormManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float gameDuration = 120f;
    [SerializeField] private int coinsPerPuzzle = 50;

    [Header("UI References")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text timerText;
    [SerializeField] private Text questionText;
    [SerializeField] private Button backButton;

    [Header("Answer Buttons")]
    [SerializeField] private Button[] answerButtons = new Button[4];

    private XPlayManager gameManager;
    private int score = 0;
    private float timeRemaining;
    private bool gameActive = true;
    private int correctAnswerIndex = 0;
    private int puzzleCount = 0;

    private List<PuzzleData> puzzles = new List<PuzzleData>();

    [System.Serializable]
    public class PuzzleData
    {
        public string question;
        public string[] answers;
        public int correctIndex;
    }

    private void Start()
    {
        gameManager = XPlayManager.Instance;
        timeRemaining = gameDuration;

        if (backButton != null)
            backButton.onClick.AddListener(BackToMenu);

        // إنشاء الألغاز
        GeneratePuzzles();

        // عرض اللغز الأول
        ShowNextPuzzle();
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

        UpdateUI();
    }

    private void GeneratePuzzles()
    {
        puzzles.Add(new PuzzleData
        {
            question = "ما هو 5 + 3؟",
            answers = new[] { "7", "8", "9", "10" },
            correctIndex = 1
        });

        puzzles.Add(new PuzzleData
        {
            question = "ما هو 12 - 4؟",
            answers = new[] { "6", "7", "8", "9" },
            correctIndex = 2
        });

        puzzles.Add(new PuzzleData
        {
            question = "ما هو 6 × 7؟",
            answers = new[] { "40", "42", "44", "46" },
            correctIndex = 1
        });

        puzzles.Add(new PuzzleData
        {
            question = "ما هو 100 ÷ 5؟",
            answers = new[] { "18", "19", "20", "21" },
            correctIndex = 2
        });

        puzzles.Add(new PuzzleData
        {
            question = "كم عدد أيام السنة؟",
            answers = new[] { "360", "364", "365", "366" },
            correctIndex = 2
        });

        puzzles.Add(new PuzzleData
        {
            question = "كم عدد أشهر السنة؟",
            answers = new[] { "10", "11", "12", "13" },
            correctIndex = 2
        });

        puzzles.Add(new PuzzleData
        {
            question = "ما هو 2^3؟",
            answers = new[] { "6", "8", "10", "12" },
            correctIndex = 1
        });

        puzzles.Add(new PuzzleData
        {
            question = "ما هو الجذر التربيعي لـ 16؟",
            answers = new[] { "2", "3", "4", "5" },
            correctIndex = 2
        });
    }

    private void ShowNextPuzzle()
    {
        if (puzzleCount >= puzzles.Count)
        {
            // إعادة تعيين الألغاز
            puzzleCount = 0;
        }

        PuzzleData puzzle = puzzles[puzzleCount];
        correctAnswerIndex = puzzle.correctIndex;

        if (questionText != null)
            questionText.text = puzzle.question;

        // إعداد أزرار الإجابات
        for (int i = 0; i < answerButtons.Length && i < puzzle.answers.Length; i++)
        {
            int index = i;
            answerButtons[i].GetComponentInChildren<Text>().text = puzzle.answers[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
        }

        puzzleCount++;
    }

    private void OnAnswerSelected(int selectedIndex)
    {
        if (selectedIndex == correctAnswerIndex)
        {
            // إجابة صحيحة
            score += coinsPerPuzzle;
            gameManager.AddCoins(coinsPerPuzzle);
            Debug.Log("Correct!");
        }
        else
        {
            // إجابة خاطئة
            Debug.Log("Wrong!");
        }

        // عرض اللغز التالي
        ShowNextPuzzle();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"النقاط: {score}";

        if (timerText != null)
            timerText.text = $"الوقت: {Mathf.Max(0, timeRemaining):F1}";
    }

    private void EndGame()
    {
        gameActive = false;
        gameManager.AddCoins(score);
        Debug.Log($"Puzzle Game Over! Final Score: {score}");
        BackToMenu();
    }

    private void BackToMenu()
    {
        gameManager.LoadScene(GameConfig.Scenes.MAIN_MENU);
    }
}
