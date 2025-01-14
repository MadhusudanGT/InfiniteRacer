using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private Button _playBtn;

    private void OnEnable()
    {
        _playBtn.onClick.AddListener(StartTheGame);
    }

    private void OnDisable()
    {
        _playBtn.onClick.RemoveListener(StartTheGame);
    }

    void StartTheGame()
    {
        EventManager.UpdateGameState?.Invoke(GameState.Running);
    }
}
