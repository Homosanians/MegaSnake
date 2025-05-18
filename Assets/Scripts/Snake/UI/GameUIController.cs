using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIController : MonoBehaviour
{
    [Header("Victory Panel")]
    [SerializeField] private GameObject _victoryPanel;
    [SerializeField] private Button _victoryRestartButton;
    [SerializeField] private Button _victoryMainMenuButton;
    [SerializeField] private TextMeshProUGUI _victoryText;
    
    [Header("Game Over Panel")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private Button _gameOverRestartButton;
    [SerializeField] private Button _gameOverMainMenuButton;
    [SerializeField] private TextMeshProUGUI _gameOverText;
    
    private GameManager _gameManager;
    
    private void Start()
    {
        _gameManager = GameManager.Instance;
        
        if (_gameManager == null)
        {
            Debug.LogError("GameManager not found!");
            return;
        }
        
        SetupPanels();
        SetupButtons();
    }
    
    private void SetupPanels()
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
    
    private void SetupButtons()
    {
        // Victory panel buttons
        if (_victoryRestartButton != null)
        {
            _victoryRestartButton.onClick.AddListener(() => _gameManager.RestartGame());
        }
        
        if (_victoryMainMenuButton != null)
        {
            _victoryMainMenuButton.onClick.AddListener(() => _gameManager.ReturnToMainMenu());
        }
        
        // Game over panel buttons
        if (_gameOverRestartButton != null)
        {
            _gameOverRestartButton.onClick.AddListener(() => _gameManager.RestartGame());
        }
        
        if (_gameOverMainMenuButton != null)
        {
            _gameOverMainMenuButton.onClick.AddListener(() => _gameManager.ReturnToMainMenu());
        }
    }
} 