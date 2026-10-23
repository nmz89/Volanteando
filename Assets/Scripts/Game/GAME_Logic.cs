using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using TMPro;

public class LogicScript : MonoBehaviour
{
    public int playerScore;
    public Text scoreText;
    public TMP_Text modeText;
    public TMP_Text runEndedMessage;
    public GameObject GameOver;
    public UI_MenuSystem_Options inGameOptions;
    public PLAYER_CarControl player;

    [Header("Score")]
    public float pointsPerSecond = 2f;
    public Text highScoreText;

    int runDifficulty;
    int bestScore;

    [Header("Speed progression (difficulty based)")]
    public int pointsPerSpeedUp = 10;
    public float speedIncrease = 1f;
    public float maxSpeed = 20f;

    [SerializeField] private GameObject Gameplay_PP;
    [SerializeField] private GameObject GameOver_PP;

    [Header("Post-processing transition")]
    [SerializeField] private float transitionDuration = 0.5f;

    private Coroutine volumeTransition;

    public bool gameIsOver { get; private set; }

    float score;
    int nextThreshold;
    AUDIO_Manager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AUDIO_Manager>();

        var d = GAME_Settings.CurrentDifficulty;
        pointsPerSpeedUp = d.pointsPerSpeedUp;
        speedIncrease = d.speedIncrease;
        maxSpeed = d.maxSpeed;

        if (modeText != null)
        {
            modeText.text = UI_Language.Get("diff" + GAME_Settings.DifficultyIndex);
        }

        runDifficulty = GAME_Settings.DifficultyIndex;
        bestScore = GAME_Settings.GetHighScore(runDifficulty);
        RefreshBest();
    }

    void Start()
    {
        Gameplay_PP.SetActive(true);
        GameOver_PP.SetActive(false);

        nextThreshold = pointsPerSpeedUp;
    }

    public void Update()
    {
        if (!gameIsOver)
        {
            score += pointsPerSecond * Time.deltaTime;
            playerScore = Mathf.FloorToInt(score);

            if (playerScore > bestScore)
            {
                bestScore = playerScore;
                RefreshBest();
            }

            scoreText.text = playerScore.ToString();
            CheckSpeedUp(playerScore);
        }
    }

    void CheckSpeedUp(int currentScore)
    {
        while (currentScore >= nextThreshold)
        {
            player.playerSpeed = Mathf.Min(player.playerSpeed + speedIncrease, maxSpeed);
            nextThreshold += pointsPerSpeedUp;
        }
    }

    void RefreshBest()
    {
        if (highScoreText != null)
        {
            highScoreText.text = UI_Language.Get("best") + ": " + bestScore;
        }
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void gameOver()
    {
        if (!EndRun()) return;

        SwitchVolume();
        GameOver.SetActive(true);
        runEndedMessage.text = UI_Language.Get("crashed");
    }
    public void offScreen()
    {
        if (!EndRun()) return;

        SwitchVolume();
        GameOver.SetActive(true);
        runEndedMessage.text = UI_Language.Get("offscreen");
    }

    void SwitchVolume()
    {
        if (volumeTransition != null)
            StopCoroutine(volumeTransition);

        volumeTransition = StartCoroutine(
            TransitionVolumes()
        );
    }

    System.Collections.IEnumerator TransitionVolumes()
    {
        // Get Volume components from GameObjects
        Volume gameplay = Gameplay_PP.GetComponent<Volume>();
        Volume gameOver = GameOver_PP.GetComponent<Volume>();

        Gameplay_PP.SetActive(true);
        GameOver_PP.SetActive(true);

        gameplay.weight = 1f;
        gameOver.weight = 0f;

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / transitionDuration);

            gameplay.weight = Mathf.Lerp(1f, 0f, t);
            gameOver.weight = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        gameplay.weight = 0f;
        gameOver.weight = 1f;

        // Turn off old volume once transition finishes.
        Gameplay_PP.SetActive(false);

        volumeTransition = null;
    }

    bool EndRun()
    {
        if (gameIsOver) return false;
        gameIsOver = true;

        GAME_Settings.TrySetHighScore(runDifficulty, playerScore);

        return true;
    }
}