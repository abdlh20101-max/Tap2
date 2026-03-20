using UnityEngine;

/// <summary>
/// GameConfig: ملف تكوين اللعبة المركزي
/// يحتوي على جميع الثوابت والإعدادات
/// </summary>
public static class GameConfig
{
    // الإصدار
    public const string GAME_VERSION = "1.0.0";
    public const string GAME_NAME = "X-Play";

    // الشاشة
    public const int SCREEN_WIDTH = 1080;
    public const int SCREEN_HEIGHT = 1920;

    // العملات والطاقة
    public const int INITIAL_COINS = 0;
    public const int INITIAL_ENERGY = 100;
    public const int MAX_ENERGY = 100;
    public const float ENERGY_RECHARGE_RATE = 1f; // طاقة واحدة كل ثانية

    // الإعلانات
    public const float BANNER_CHANGE_INTERVAL = 7f;
    public const float POPUP_INTERVAL = 20f;
    public const float POPUP_DURATION = 5f;

    // الموسيقى والصوت
    public const float DEFAULT_MUSIC_VOLUME = 0.7f;
    public const float DEFAULT_SFX_VOLUME = 0.8f;

    // الألعاب
    public const int TOTAL_GAMES = 10;

    // مكافآت الألعاب
    public static class GameRewards
    {
        public const int PULSE_CLICKER_REWARD = 10;
        public const int GOLDEN_HARVEST_REWARD = 25;
        public const int NEON_POP_REWARD = 15;
        public const int CYBER_SLITHER_REWARD = 20;
        public const int BLOCK_CRUSH_REWARD = 50;
        public const int SKY_STACKER_REWARD = 30;
        public const int DASH_ESCAPE_REWARD = 10;
        public const int BRAIN_STORM_REWARD = 50;
        public const int MIND_MATCH_REWARD = 100;
        public const int NITRO_RACE_REWARD = 15;
    }

    // متطلبات الطاقة للألعاب
    public static class EnergyRequirements
    {
        public const int PULSE_CLICKER = 10;
        public const int GOLDEN_HARVEST = 15;
        public const int NEON_POP = 12;
        public const int CYBER_SLITHER = 10;
        public const int BLOCK_CRUSH = 15;
        public const int SKY_STACKER = 10;
        public const int DASH_ESCAPE = 12;
        public const int BRAIN_STORM = 10;
        public const int MIND_MATCH = 10;
        public const int NITRO_RACE = 15;
    }

    // مدة الألعاب
    public static class GameDurations
    {
        public const float PULSE_CLICKER = 60f;
        public const float GOLDEN_HARVEST = 120f;
        public const float NEON_POP = 60f;
        public const float CYBER_SLITHER = 0f; // حتى الموت
        public const float BLOCK_CRUSH = 120f;
        public const float SKY_STACKER = 0f; // حتى الخطأ
        public const float DASH_ESCAPE = 0f; // حتى الاصطدام
        public const float BRAIN_STORM = 120f;
        public const float MIND_MATCH = 0f; // حتى الخطأ
        public const float NITRO_RACE = 0f; // حتى الاصطدام
    }

    // الألوان
    public static class Colors
    {
        public static Color PRIMARY = new Color(0.2f, 0.8f, 1f); // Cyan
        public static Color SECONDARY = new Color(1f, 0.2f, 0.8f); // Magenta
        public static Color SUCCESS = Color.green;
        public static Color ERROR = Color.red;
        public static Color WARNING = Color.yellow;
    }

    // المشاهد
    public static class Scenes
    {
        public const string INTRO = "Intro";
        public const string MAIN_MENU = "MainMenu";
        public const string PULSE_CLICKER = "PulseClicker";
        public const string GOLDEN_HARVEST = "GoldenHarvest";
        public const string NEON_POP = "NeonPop";
        public const string CYBER_SLITHER = "CyberSlither";
        public const string BLOCK_CRUSH = "BlockCrush";
        public const string SKY_STACKER = "SkyStacker";
        public const string DASH_ESCAPE = "DashEscape";
        public const string BRAIN_STORM = "BrainStorm";
        public const string MIND_MATCH = "MindMatch";
        public const string NITRO_RACE = "NitroRace";
    }

    // المفاتيح للـ PlayerPrefs
    public static class PlayerPrefsKeys
    {
        public const string COINS = "Coins";
        public const string ENERGY = "Energy";
        public const string MUSIC_ENABLED = "MusicEnabled";
        public const string SFX_ENABLED = "SoundEffectsEnabled";
        public const string MUSIC_VOLUME = "MusicVolume";
        public const string SFX_VOLUME = "SFXVolume";
        public const string LAST_PLAYED_GAME = "LastPlayedGame";
        public const string TOTAL_GAMES_PLAYED = "TotalGamesPlayed";
    }

    // الإحصائيات
    public static class Statistics
    {
        public const string TOTAL_COINS_EARNED = "TotalCoinsEarned";
        public const string TOTAL_GAMES_COMPLETED = "TotalGamesCompleted";
        public const string HIGHEST_SCORE = "HighestScore";
        public const string PLAYTIME = "PlayTime";
    }

    // الحصول على مكافأة اللعبة
    public static int GetGameReward(int gameIndex)
    {
        return gameIndex switch
        {
            0 => GameRewards.PULSE_CLICKER_REWARD,
            1 => GameRewards.GOLDEN_HARVEST_REWARD,
            2 => GameRewards.NEON_POP_REWARD,
            3 => GameRewards.CYBER_SLITHER_REWARD,
            4 => GameRewards.BLOCK_CRUSH_REWARD,
            5 => GameRewards.SKY_STACKER_REWARD,
            6 => GameRewards.DASH_ESCAPE_REWARD,
            7 => GameRewards.BRAIN_STORM_REWARD,
            8 => GameRewards.MIND_MATCH_REWARD,
            9 => GameRewards.NITRO_RACE_REWARD,
            _ => 0
        };
    }

    // الحصول على متطلبات الطاقة للعبة
    public static int GetEnergyRequirement(int gameIndex)
    {
        return gameIndex switch
        {
            0 => EnergyRequirements.PULSE_CLICKER,
            1 => EnergyRequirements.GOLDEN_HARVEST,
            2 => EnergyRequirements.NEON_POP,
            3 => EnergyRequirements.CYBER_SLITHER,
            4 => EnergyRequirements.BLOCK_CRUSH,
            5 => EnergyRequirements.SKY_STACKER,
            6 => EnergyRequirements.DASH_ESCAPE,
            7 => EnergyRequirements.BRAIN_STORM,
            8 => EnergyRequirements.MIND_MATCH,
            9 => EnergyRequirements.NITRO_RACE,
            _ => 10
        };
    }

    // الحصول على مدة اللعبة
    public static float GetGameDuration(int gameIndex)
    {
        return gameIndex switch
        {
            0 => GameDurations.PULSE_CLICKER,
            1 => GameDurations.GOLDEN_HARVEST,
            2 => GameDurations.NEON_POP,
            3 => GameDurations.CYBER_SLITHER,
            4 => GameDurations.BLOCK_CRUSH,
            5 => GameDurations.SKY_STACKER,
            6 => GameDurations.DASH_ESCAPE,
            7 => GameDurations.BRAIN_STORM,
            8 => GameDurations.MIND_MATCH,
            9 => GameDurations.NITRO_RACE,
            _ => 60f
        };
    }
}
