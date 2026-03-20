using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// SkyStackerManager: لعبة برج التوازن (Sky Stacker)
/// بناء برج من قطع متحركة مع زيادة الصعوبة
/// Developed by: Abdullah Al-husini
/// </summary>
public class SkyStackerManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float pieceDropSpeed = 5f;
    [SerializeField] private float pieceWidth = 100f;
    [SerializeField] private int coinsPerBlock = 30;

    [Header("UI References")]
    [SerializeField] private Text heightText;
    [SerializeField] private Text coinsText;
    [SerializeField] private Button backButton;

    [Header("Game Objects")]
    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private Transform towerContainer;
    [SerializeField] private RectTransform gameArea;

    private XPlayManager gameManager;
    private int towerHeight = 0;
    private int earnedCoins = 0;
    private bool gameActive = true;
    private float currentPieceX = 0f;
    private float pieceDirection = 1f;
    private float pieceSpeed = 200f;

    private GameObject currentPiece;
    private List<GameObject> towerPieces = new List<GameObject>();
    private float lastPieceX = 0f;

    private void Start()
    {
        gameManager = XPlayManager.Instance;

        if (backButton != null)
            backButton.onClick.AddListener(BackToMenu);

        // إنشاء القطعة الأولى
        SpawnNewPiece();
    }

    private void Update()
    {
        if (!gameActive)
            return;

        // حركة القطعة الحالية
        if (currentPiece != null)
        {
            currentPieceX += pieceDirection * pieceSpeed * Time.deltaTime;

            // عكس الاتجاه عند الوصول للحافة
            if (Mathf.Abs(currentPieceX) > 300)
                pieceDirection *= -1;

            RectTransform rectTransform = currentPiece.GetComponent<RectTransform>();
            if (rectTransform != null)
                rectTransform.anchoredPosition = new Vector2(currentPieceX, 0);
        }

        // معالجة الـ Input
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            DropPiece();
        }

        UpdateUI();
    }

    private void SpawnNewPiece()
    {
        if (piecePrefab == null || towerContainer == null)
            return;

        currentPiece = Instantiate(piecePrefab, towerContainer);
        currentPieceX = 0f;
        pieceDirection = 1f;

        // زيادة السرعة مع كل قطعة
        pieceSpeed += 20f;

        RectTransform rectTransform = currentPiece.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = new Vector2(0, 400 - towerHeight * 50);
            rectTransform.sizeDelta = new Vector2(pieceWidth, 50);
        }
    }

    private void DropPiece()
    {
        if (currentPiece == null)
            return;

        RectTransform currentRect = currentPiece.GetComponent<RectTransform>();
        if (currentRect == null)
            return;

        // حساب التقاطع مع القطعة السابقة
        float overlapAmount = CalculateOverlap(currentPieceX, lastPieceX);

        if (overlapAmount <= 0)
        {
            // لا يوجد تقاطع - نهاية اللعبة
            EndGame();
            return;
        }

        // تقليل عرض القطعة بناءً على التقاطع
        pieceWidth = Mathf.Max(20, pieceWidth - (pieceWidth - overlapAmount));
        currentRect.sizeDelta = new Vector2(pieceWidth, 50);

        // حفظ موضع القطعة الحالية
        lastPieceX = currentPieceX;

        // إضافة النقاط
        towerHeight++;
        earnedCoins += coinsPerBlock;
        gameManager.AddCoins(coinsPerBlock);

        // إنشاء قطعة جديدة
        SpawnNewPiece();
    }

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

    private void UpdateUI()
    {
        if (heightText != null)
            heightText.text = $"الارتفاع: {towerHeight}";

        if (coinsText != null)
            coinsText.text = $"💰 {earnedCoins}";
    }

    private void EndGame()
    {
        gameActive = false;
        gameManager.AddCoins(earnedCoins);
        Debug.Log($"Tower Game Over! Height: {towerHeight}");
        BackToMenu();
    }

    private void BackToMenu()
    {
        gameManager.LoadScene(GameConfig.Scenes.MAIN_MENU);
    }
}
