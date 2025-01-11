using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private bool gameOver;
    public bool GameOver
    {
        get { return gameOver; }
        set { gameOver = value; }
    }

    private GameState currentState = GameState.NotStarted;
    public GameState CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    private float score;
    public float Score
    {
        get { return score; }
        set { score = value; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        ManagerRegistry.Register<GameManager>(this);
    }

    private void OnEnable()
    {
        EventManager.UpdateGameState += HandleInput;
    }
    private void OnDisable()
    {
        EventManager.UpdateGameState -= HandleInput;
    }
    private void HandleInput(GameState currentStateOfTheGame)
    {
        switch (currentStateOfTheGame)
        {
            case GameState.Started:
                StartGame();
                break;
            case GameState.Paused:
                PauseGame();
                break;
            case GameState.Running:
                ResumeGame();
                break;
            case GameState.GameOver:
                RestartGame();
                break;
        }
    }
    private void StartGame()
    {
        currentState = GameState.Running;
    }

    private void PauseGame()
    {
        currentState = GameState.Paused;
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        currentState = GameState.Running;
        Time.timeScale = 1f;
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        currentState = GameState.NotStarted;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

}
public enum GameState
{
    NotStarted,
    Started,
    Running,
    Paused,
    GameOver
}