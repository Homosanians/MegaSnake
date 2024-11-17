using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_InputField InputField1;
    public TMP_InputField InputField2;
    public TMP_InputField InputField3;
    public TMP_InputField InputField4;
    public TMP_InputField InputField5;
    public TMP_InputField InputField6;
    public TMP_InputField InputField7;

    public GameObject Credits;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("word1")) 
        {
            InputField1.text = PlayerPrefs.GetString("word1");
        }
        if (PlayerPrefs.HasKey("word2"))
        {
            InputField2.text = PlayerPrefs.GetString("word2");
        }
        if (PlayerPrefs.HasKey("word3"))
        {
            InputField3.text = PlayerPrefs.GetString("word3");
        }
        if (PlayerPrefs.HasKey("word4"))
        {
            InputField4.text = PlayerPrefs.GetString("word4");
        }
        if (PlayerPrefs.HasKey("word5"))
        {
            InputField5.text = PlayerPrefs.GetString("word5");
        }
        if (PlayerPrefs.HasKey("word6"))
        {
            InputField6.text = PlayerPrefs.GetString("word6");
        }
        if (PlayerPrefs.HasKey("word7"))
        {
            InputField7.text = PlayerPrefs.GetString("word7");
        }
    }

    public void OpenCredits() 
    {
        Credits.SetActive(true);
    }

    public void CloseCredits() 
    {
        Credits.SetActive(false);
    }

    public void StartGame() 
    {
        PlayerPrefs.SetString("word1", InputField1.text.ToUpper());
        PlayerPrefs.SetString("word2", InputField2.text.ToUpper());
        PlayerPrefs.SetString("word3", InputField3.text.ToUpper());
        PlayerPrefs.SetString("word4", InputField4.text.ToUpper());
        PlayerPrefs.SetString("word5", InputField5.text.ToUpper());
        PlayerPrefs.SetString("word6", InputField6.text.ToUpper());
        PlayerPrefs.SetString("word7", InputField7.text.ToUpper());
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }
}
