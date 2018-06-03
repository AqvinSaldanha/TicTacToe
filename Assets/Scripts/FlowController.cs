using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.aqvin.tictactoe
{
	/// <summary>
	/// Manages the UI flow.
	/// </summary>
	public class FlowController : MonoBehaviour {
		[SerializeField]
		Text m_playerName;

		[SerializeField]
		TicTacToe m_mainGame = null;

		[SerializeField]
		GameObject m_menuUI;

		[SerializeField]
		GameObject m_gameOverUI;

		[SerializeField]
		string m_title;

		[SerializeField]
		string m_playersTurnText = "'s turn...";

		[SerializeField]
		string m_aiName = "AI";

		[SerializeField]
		string m_defaultPlayerName = "Player 1";


		[SerializeField]
		Text m_hudText;

		private static FlowController Instance;

		void Awake()
		{
			if (Instance == null) {
				Instance = this;
			}
			OnMenuPressed ();
		}

		public void OnPlayPressed()
		{
			Instance.m_gameOverUI.gameObject.SetActive (false);	
			m_menuUI.gameObject.SetActive (false); 
			m_mainGame.StartNewGame ();
		}

		public void OnRetryPressed()
		{ 
			Instance.m_gameOverUI.gameObject.SetActive (false);	
			m_menuUI.gameObject.SetActive (false); 
			m_mainGame.StartNewGame ();
		}

		public void OnMenuPressed()
		{ 
			Instance.m_gameOverUI.gameObject.SetActive (false);	
			Instance.m_menuUI.gameObject.SetActive (true);	
			m_mainGame.ResetSelection ();
			SetHudText(m_title);
		}

		public void OnClosePress()
		{
			#if UNITY_EDITOR
				Debug.LogError ("Close won't work in the editor mode.");
			#endif
			Application.Quit ();

		}

		public static void OnGameOver()
		{
			Instance.m_gameOverUI.gameObject.SetActive (true);	
		}

		public static string GetPlayerName()
		{
			if (string.IsNullOrEmpty (Instance.m_playerName.text)) {
				return Instance.m_defaultPlayerName;
			}
			return Instance.m_playerName.text;
		}

		public static string GetAIName()
		{
			return Instance.m_aiName;
		}

		public static void SetTurnText (string a_curPlayerName)
		{
			SetHudText (a_curPlayerName + Instance.m_playersTurnText);
		} 

		public static void SetHudText (string a_text)
		{
			Instance.m_hudText.text = a_text;
		} 



	}
}