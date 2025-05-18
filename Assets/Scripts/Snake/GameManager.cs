using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [SerializeField] private GameObject _victoryPanel;
    [SerializeField] private GameObject _gameOverPanel;

    public static int WordsCount { get; } = 10;
    
    private bool _isGameOver = false;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        // Reset game state
        _isGameOver = false;
    }
    
    private void Start()
    {
        if (_victoryPanel != null)
        {
            _victoryPanel.SetActive(false);
        }
        
        if (_gameOverPanel != null)
        {
            _gameOverPanel.SetActive(false);
        }
    }
    
    public void ShowVictoryScreen()
    {
        if (_isGameOver) return;
        
        _isGameOver = true;
        
        if (_victoryPanel != null)
        {
            _victoryPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Victory panel not assigned in GameManager");
        }
    }
    
    public void ShowGameOverScreen()
    {
        if (_isGameOver) return;
        
        _isGameOver = true;
        
        if (_gameOverPanel != null)
        {
            _gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Game over panel not assigned in GameManager");
        }
    }
    
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("WordsScene");
    }
    
    public void RestartGame()
    {
        // Reset all singletons before reloading the scene
        MonoBehaviourSingleton<GameManager>.Reset();
        MonoBehaviourSingleton<WordManager>.Reset();
        MonoBehaviourSingleton<SnakeOrchestrator>.Reset();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
} 