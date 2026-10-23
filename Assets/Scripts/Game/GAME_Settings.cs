using UnityEngine;

public static class GAME_Settings
{
    const string HighScoreKey = "highScore_";
    public static int GetHighScore(int difficulty)
    {
        return PlayerPrefs.GetInt(HighScoreKey + difficulty, 0);
    }

    public static bool TrySetHighScore(int difficulty, int score)
    {
        if (score <= GetHighScore(difficulty)) return false;

        PlayerPrefs.SetInt(HighScoreKey + difficulty, score);
        PlayerPrefs.Save();
        return true;
    }
    public struct Difficulty
    {
        public string name;
        public int pointsPerSpeedUp;
        public float speedIncrease;
        public float maxSpeed;
        public float trafficSpeedMultiplier;
        public float hazardGapMultiplier;

        public Difficulty(string name, int points, float increase, float max, float trafficSpeed, float hazardGap)
        {
            this.name = name;
            pointsPerSpeedUp = points;
            speedIncrease = increase;
            maxSpeed = max;
            trafficSpeedMultiplier = trafficSpeed;
            hazardGapMultiplier = hazardGap;
        }
    }

    const string CanCollideWithCurbs = "curbCollisions";
    const string HazardsKey = "roadHazards";

    public static bool CurbCollisions
    {
        get => PlayerPrefs.GetInt(CanCollideWithCurbs, 1) == 1;
        set => PlayerPrefs.SetInt(CanCollideWithCurbs, value ? 1 : 0);
    }

    public static bool RoadHazards
    {
        get => PlayerPrefs.GetInt(HazardsKey, 1) == 1;
        set => PlayerPrefs.SetInt(HazardsKey, value ? 1 : 0);
    }

    public static readonly Difficulty[] Difficulties =
    {
        new Difficulty("Facil",       50, 2.5f, 30f, 0.5f, 2.0f),
        new Difficulty("Normal",     30, 5f, 75f, 1.0f, 1.0f),
        new Difficulty("Dificil",       15, 10f, 150f, 1.4f, 0.7f),
        new Difficulty("Imposible", 5,  15f, 300f, 1.8f, 0.5f),
    };

    const string DifficultyKey = "difficulty";
    const string MusicKey = "musicVolume";
    const string SfxKey = "sfxVolume";

    public static int DifficultyIndex
    {
        get => Mathf.Clamp(PlayerPrefs.GetInt(DifficultyKey, 1), 0, Difficulties.Length - 1);  // default: Normal
        set => PlayerPrefs.SetInt(DifficultyKey, value);
    }

    public static Difficulty CurrentDifficulty => Difficulties[DifficultyIndex];

    public static float MusicVolume
    {
        get => PlayerPrefs.GetFloat(MusicKey, 1f);
        set => PlayerPrefs.SetFloat(MusicKey, value);
    }

    public static float SfxVolume
    {
        get => PlayerPrefs.GetFloat(SfxKey, 1f);
        set => PlayerPrefs.SetFloat(SfxKey, value);
    }
}