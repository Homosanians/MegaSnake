using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Snake.Board
{
	public class MainMenu : MonoBehaviour
	{
		public TMP_InputField[] InputFields;


		public GameObject Credits;
		public GameObject Warning;

		private void Awake()
		{
			for(int i = 0; i < GameManager.WordsCount; i++) 
			{
				string word = $"word{i+1}";
				if (PlayerPrefs.HasKey(word))
				{
					InputFields[i].text = PlayerPrefs.GetString(word);
				}
			}
		}

		public void StartGame()
		{
			int counter = 0;

			for (int i = 0; i < GameManager.WordsCount; i++) 
			{
				var wordName = $"word{i+1}";
				var word = InputFields[i].text.ToUpper();
				PlayerPrefs.SetString(wordName, word);
				if (!string.IsNullOrEmpty(word)) 
				{
					counter++;
				}
			}

			if (counter == 0) 
			{
				Warning.SetActive(true);
				return;
			}
			PlayerPrefs.Save();
			SceneManager.LoadScene(1);
		}
	}
}
