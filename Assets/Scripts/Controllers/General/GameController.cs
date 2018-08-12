using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameController : MonoBehaviour {

    private delegate void DelayedMethod();

    private static List<PlayerController> playerControllers = new List<PlayerController>();
    public static int activePlayer = 0;
    public static int lastPlayer = 0;

    public GameObject playerPrefab;

    public GameObject[] levelPrefabs;

    public static GameObject currentLevelObject;

    public static int Score { get; private set; }
    public static int HighScore { get; private set; }
    public static int Level { get; private set; }
    public static int HighLevel { get; private set; }
    public static bool notExitingLevel = true;

    public static bool IsRunning { get { return stateMachine.currentState == gcRunningState; } }

    public static bool PlayerNotDying { get; set; }

    [SerializeField]
    private AudioMixer mixer;
    public static AudioMixer Mixer { get; private set; }

    private static StateMachine<GameController> stateMachine;

    private static GameController _gameController;
    public static GameController gameController
    {
        get
        {
            if (!_gameController)
                _gameController = FindObjectOfType<GameController>();

            return _gameController;
        }
    }

    private static GCMenuState _gcMenuState;
    public static GCMenuState gcMenuState
    {
        get
        {
            if (_gcMenuState == null)
                _gcMenuState = new GCMenuState();

            return _gcMenuState;
        }
    }

    private static GCRunningState _gcRunningState;
    public static GCRunningState gcRunningState
    {
        get
        {
            if (_gcRunningState == null)
                _gcRunningState = new GCRunningState();

            return _gcRunningState;
        }
    }

    private static GCPausedState _gcPausedState;
    public static GCPausedState gcPausedState
    {
        get
        {
            if (_gcPausedState == null)
                _gcPausedState = new GCPausedState();

            return _gcPausedState;
        }
    }

    private void Awake()
    {
        stateMachine = new StateMachine<GameController>(this);

        stateMachine.ChangeState(gcMenuState);

        Mixer = mixer;

        Score = 0;
        Level = 0;
        HighLevel = GetHighLevel();

        PlayerNotDying = true;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus == false)
            PauseGame(true);
    }

    private void OnApplicationQuit()
    {
        OptionsController.SaveOptions();

        GameOver();
    }

    public static void SwapPlayer(bool swap, float axis)
    {
        if (swap && playerControllers.Count > 1 && notExitingLevel && PlayerNotDying)
        {
            playerControllers[activePlayer].activePlayer = false;
            lastPlayer = activePlayer;
            activePlayer = ((int)(activePlayer + axis)).Modulo(playerControllers.Count);
            playerControllers[activePlayer].activePlayer = true;

            InputController.SetPlayer(playerControllers[activePlayer]);
        }
    }

    public static void StartGame()
    {
        if (stateMachine.currentState != gcRunningState)
        {
            stateMachine.ChangeState(gcRunningState);

            currentLevelObject = Instantiate(gameController.levelPrefabs[Level]);

            HighScore = GetHighScore();
        }
        else
            Debug.Log("Game already started");
    }

    public static void PauseGame(bool pause)
    {
        if (stateMachine.currentState != gcMenuState)
        {
            if (pause)
                stateMachine.ChangeState(gcPausedState);
            else
                stateMachine.ChangeState(gcRunningState);
        }
    }

    public static void EndGame()
    {
        ClearLevel();

        stateMachine.ChangeState(gcMenuState);
    }

    public static void ClearLevel()
    {
        foreach (PlayerController player in playerControllers)
            Destroy(player.gameObject);

        playerControllers.Clear();

        Destroy(currentLevelObject);

        SaveHighScore();
    }

    public static void ResetLevel()
    {
        ClearLevel();

        currentLevelObject = Instantiate(gameController.levelPrefabs[Level]);

        notExitingLevel = true;
    }

    public static void AddScore(int amount)
    {
        Score += amount;

        if (Score > HighScore)
        {
            HighScore = Score;
            SaveHighScore();
        }
    }

    public static int GetHighScore()
    {
        if (PlayerPrefs.HasKey("high-score"))
            return PlayerPrefs.GetInt("high-score");

        return 0;
    }

    public static int GetHighLevel()
    {
        if (PlayerPrefs.HasKey("high-level"))
            return PlayerPrefs.GetInt("high-level");

        return 0;
    }

    public static void SaveHighScore()
    {
        PlayerPrefs.SetInt("high-score", HighScore);
        PlayerPrefs.SetInt("high-level", HighLevel);
        PlayerPrefs.Save();
    }

    public static void NextLevel()
    {
        gameController.StartCoroutine(WaitForNextLevel(ToNextLevel, 3f));
    }

    public static void ToNextLevel()
    {
        ClearLevel();

        Level++;

        if (Level > HighLevel)
            HighLevel = Level;

        currentLevelObject = Instantiate(gameController.levelPrefabs[Level]);

        notExitingLevel = true;

        SaveHighScore();
    }

    public static void GameOver()
    {
        SaveHighScore();

        ClearLevel();
    }

    public static void AddPlayer(PlayerController player)
    {
        if (player.activePlayer)
        {
            InputController.SetPlayer(player);

            activePlayer = playerControllers.Count;
        }

        playerControllers.Add(player);
    }

    public static void OnPlayerDeath(PlayerController player)
    {
        lock (playerControllers)
        {
            playerControllers.Remove(player);

            if (notExitingLevel)
            {
                if (playerControllers.Count > 0)
                {
                    if (player.activePlayer)
                    {
                        activePlayer = lastPlayer.Modulo(playerControllers.Count);

                        playerControllers[activePlayer].activePlayer = true;

                        InputController.SetPlayer(playerControllers[activePlayer]);
                    }
                }
                else
                {
                    gameController.StartCoroutine(WaitForNextLevel(ResetLevel, 1f));
                }
            }
        }

        PlayerNotDying = true;
    }

    private static IEnumerator WaitForNextLevel(DelayedMethod method, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        method();
    }
}

public static class IntExtension {

    public static int Modulo(this int value, int mod)
    {
        return (value % mod + mod) % mod;
    }

}
