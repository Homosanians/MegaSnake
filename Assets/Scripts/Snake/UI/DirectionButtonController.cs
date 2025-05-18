using UnityEngine;
using UnityEngine.UI;

public class DirectionButtonController : MonoBehaviour
{
    [SerializeField] private Button _buttonLeft;
    [SerializeField] private Button _buttonRight;
    
    private PlayerSnakeController _playerController;
    
    private void Start()
    {
        FindPlayerController();
        SetupButtons();
    }
    
    private void FindPlayerController()
    {
        // Find the player snake controller in the scene
        _playerController = FindObjectOfType<PlayerSnakeController>();
        
        if (_playerController == null)
        {
            Debug.LogError("Player Snake Controller not found in scene!");
        }
    }
    
    private void SetupButtons()
    {
        if (_buttonLeft != null && _playerController != null)
        {
            _buttonLeft.onClick.AddListener(() => _playerController.InputButtonLeft());
        }
        
        if (_buttonRight != null && _playerController != null)
        {
            _buttonRight.onClick.AddListener(() => _playerController.InputButtonRight());
        }
    }
} 