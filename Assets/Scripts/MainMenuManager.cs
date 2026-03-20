using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// MainMenuManager: مدير الواجهة الرئيسية لمنصة X-Play
/// يدير: عرض الألعاب، نظام العملات والطاقة، الإعدادات، ونظام التقدم.
/// Developed by: Abdullah Al-husini
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Text coinsText;
    [SerializeField] private Text energyText;
    [SerializeField] private Button settingsButton;
    [SerializeField] private GameObject settingsPanel;

    [Header("Games Grid")]
    [SerializeField] private Transform gamesContainer;
    [SerializeField] private GameObject gameButtonPrefab;

    [Header("Game Data")]
    [SerializeField] private List<GameData> games = new List<GameData>();

    private XPlayManager gameManager;
    private bool settingsPanelOpen = false;

    [System.Serializable]
    public class GameData
    {
        public string gameName;
        public string sceneName;
        public Sprite gameIcon;
        public int requiredEnergy = 10;
    }

    private void Start()
    {
        gameManager = XPlayManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("XPlayManager not found!");
            return;
        }

        // إعداد الأزرار
        if (settingsButton != null)
            settingsButton.onClick.AddListener(ToggleSettings);

        // إنشاء قائمة الألعاب
        InitializeGames();

        // تحديث الـ UI
        UpdateUIDisplay();

        // إخفاء لوحة الإعدادات في البداية
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    private void Update()
    {
        // تحديث عرض العملات والطاقة كل إطار
        UpdateUIDisplay();
    }

    private void InitializeGames()
    {
        if (games.Count == 0)
            CreateDefaultGames();

        if (gamesContainer == null)
            return;

        // حذف الأزرار القديمة
        foreach (Transform child in gamesContainer)
            Destroy(child.gameObject);

        // إنشاء أزرار للألعاب
        foreach (GameData game in games)
        {
            GameObject buttonObj = Instantiate(gameButtonPrefab, gamesContainer);
            Button button = buttonObj.GetComponent<Button>();
            Image icon = buttonObj.GetComponentInChildren<Image>();
            Text label = buttonObj.GetComponentInChildren<Text>();

            if (icon != null && game.gameIcon != null)
                icon.sprite = game.gameIcon;

            if (label != null)
                label.text = game.gameName;

            if (button != null)
            {
                string sceneName = game.sceneName;
                int requiredEnergy = game.requiredEnergy;
                button.onClick.AddListener(() => PlayGame(sceneName, requiredEnergy));
            }
        }
    }

    private void CreateDefaultGames()
    {
        // الألعاب الـ 10 بالأسماء الجديدة من GameConfig
        games.Add(new GameData { gameName = "Pulse Clicker", sceneName = GameConfig.Scenes.PULSE_CLICKER, requiredEnergy = GameConfig.EnergyRequirements.PULSE_CLICKER });
        games.Add(new GameData { gameName = "Golden Harvest", sceneName = GameConfig.Scenes.GOLDEN_HARVEST, requiredEnergy = GameConfig.EnergyRequirements.GOLDEN_HARVEST });
        games.Add(new GameData { gameName = "Neon Pop", sceneName = GameConfig.Scenes.NEON_POP, requiredEnergy = GameConfig.EnergyRequirements.NEON_POP });
        games.Add(new GameData { gameName = "Cyber Slither", sceneName = GameConfig.Scenes.CYBER_SLITHER, requiredEnergy = GameConfig.EnergyRequirements.CYBER_SLITHER });
        games.Add(new GameData { gameName = "Block Crush", sceneName = GameConfig.Scenes.BLOCK_CRUSH, requiredEnergy = GameConfig.EnergyRequirements.BLOCK_CRUSH });
        games.Add(new GameData { gameName = "Sky Stacker", sceneName = GameConfig.Scenes.SKY_STACKER, requiredEnergy = GameConfig.EnergyRequirements.SKY_STACKER });
        games.Add(new GameData { gameName = "Dash Escape", sceneName = GameConfig.Scenes.DASH_ESCAPE, requiredEnergy = GameConfig.EnergyRequirements.DASH_ESCAPE });
        games.Add(new GameData { gameName = "Brain Storm", sceneName = GameConfig.Scenes.BRAIN_STORM, requiredEnergy = GameConfig.EnergyRequirements.BRAIN_STORM });
        games.Add(new GameData { gameName = "Mind Match", sceneName = GameConfig.Scenes.MIND_MATCH, requiredEnergy = GameConfig.EnergyRequirements.MIND_MATCH });
        games.Add(new GameData { gameName = "Nitro Race", sceneName = GameConfig.Scenes.NITRO_RACE, requiredEnergy = GameConfig.EnergyRequirements.NITRO_RACE });
    }

    private void PlayGame(string sceneName, int requiredEnergy)
    {
        if (gameManager.ConsumeEnergy(requiredEnergy))
        {
            gameManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning($"Not enough energy to play {sceneName}!");
            // يمكن إضافة رسالة خطأ للاعب هنا
        }
    }

    private void UpdateUIDisplay()
    {
        if (gameManager == null)
            return;

        if (coinsText != null)
            coinsText.text = $"💰 {gameManager.GetCoins()}";

        if (energyText != null)
            energyText.text = $"⚡ {gameManager.GetEnergy()} / {gameManager.GetMaxEnergy()}";
    }

    private void ToggleSettings()
    {
        settingsPanelOpen = !settingsPanelOpen;
        if (settingsPanel != null)
            settingsPanel.SetActive(settingsPanelOpen);
    }

    public void AddGame(GameData game)
    {
        games.Add(game);
        InitializeGames();
    }
}
