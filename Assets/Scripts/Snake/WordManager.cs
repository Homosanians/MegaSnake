using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WordManager : MonoBehaviourSingleton<WordManager>
{
    private List<string> _words = new List<string>();
    private int _currentWordIndex = 0;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        // Reset the word index when enabled
        _currentWordIndex = 0;
    }
    
    public void Initialize()
    {
        LoadWordsFromPlayerPrefs();
    }

    private void LoadWordsFromPlayerPrefs()
    {
        _words.Clear();
        
        for (int i = 1; i <= GameManager.WordsCount; i++)
        {
            string key = $"word{i}";
            if (PlayerPrefs.HasKey(key))
            {
                string word = PlayerPrefs.GetString(key);
                if (!string.IsNullOrEmpty(word))
                {
                    _words.Add(word);
                }
            }
        }
        
        if (_words.Count == 0)
        {
            _words.Add("СЛОВО");
        }
        
        // Reset the current word index
        _currentWordIndex = 0;
    }

    public string GetCurrentWord()
    {
        if (_currentWordIndex < _words.Count)
        {
            return _words[_currentWordIndex];
        }
        return string.Empty;
    }

    public void MoveToNextWord()
    {
        _currentWordIndex++;
        
        if (_currentWordIndex >= _words.Count)
        {
            ShowVictoryScreen();
        }
    }
    
    public char[] GetCurrentWordAsCharArray()
    {
        string currentWord = GetCurrentWord();
        return currentWord.ToCharArray();
    }
    
    private void ShowVictoryScreen()
    {
        GameManager gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.ShowVictoryScreen();
        }
        else
        {
            Debug.LogError("GameManager not found when trying to show victory screen");
        }
    }
} 