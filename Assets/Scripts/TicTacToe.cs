using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine; 
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace com.aqvin.tictactoe
{
	/// <summary>
	/// This is the main game play class: Player 1 is user and player 2 is AI. 
	/// Responsibilities: Check for victory/ draw, get input from user/AI etc
	/// </summary>
	public class TicTacToe : MonoBehaviour {
	  
		Player m_player1 = null; 
		AI m_player2 = null;

		[SerializeField]
		List<Cell> m_cells; 
	 
		[SerializeField]
		Sprite m_xSprite = null, m_oSprite = null;

		[SerializeField]
		Color m_interactableColor= Color.black, m_disabledColor = Color.black;

		[SerializeField]
		Color m_victoryColor = Color.red;

		Player m_currentPlayer;

		AudioSource m_audioSource;

		[SerializeField]
		AudioClip m_selectSound = null, m_winSound = null, m_loseSound = null, m_drawSound = null;

		void Awake()
		{
			m_audioSource = GetComponent<AudioSource> ();
			m_cells.ForEach (cell => {
				cell.Initialize(m_interactableColor, m_disabledColor);
			});

			m_player1 = new Player ();
			m_player1.m_statusToSet = Cell.SelectionStatus.Player1;
			m_player1.m_overlaySprite = m_xSprite;

			m_player2 = new AI ();
			m_player2.m_statusToSet = Cell.SelectionStatus.Player2;
			m_player2.m_overlaySprite = m_oSprite; 

			m_currentPlayer = m_player2;
		}  

		public void ResetSelection()
		{
			m_cells.ForEach (cell => {
				cell.Reset();
			}); 
		}

		public void StartNewGame()
		{
			m_player1.m_playerName = FlowController.GetPlayerName ();
			m_player1.Reset ();

			m_player2.m_playerName = FlowController.GetAIName ();
			m_player2.Reset ();
			SetNextPlayer ();
			m_gameOver = false;
			m_moveCount = 0;
			ResetSelection ();
		}

		Vector3 m_mousePosition;  
		int m_moveCount = 0;
		bool m_gameOver = false;

		void OnMouseUpAsButton()
		{
			// prevent click detection when UI is on top of this gameobject
			if (EventSystem.current.IsPointerOverGameObject ()) {
				return;
			}
			// ignore input when the game is finished
			if (m_gameOver) {
				return;
			}
			// if current player is human, only then process the input
			if (m_currentPlayer.IsHuman ()) { 
				m_mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				for (int i = 0; i < m_cells.Count; i++) {
					if (m_cells[i].ContainsPoint (m_mousePosition)) {
						if (m_cells[i].Status == Cell.SelectionStatus.Free) {
							HandleSelection (i);
						}
					}
				}  
			} 


		}

		void HandleSelection(int a_cellIndex)
		{
			m_audioSource.PlayOneShot (m_selectSound);
			m_player2.Remove (a_cellIndex);

			m_cells[a_cellIndex].Select (m_currentPlayer.m_overlaySprite);
			m_moveCount++;
			m_cells[a_cellIndex].Status = m_currentPlayer.m_statusToSet; 
			if (CheckCompletion ()) {
				m_gameOver = true;
				if (m_currentPlayer.IsHuman()) {
					m_audioSource.PlayOneShot (m_winSound);
				} else {
					m_audioSource.PlayOneShot (m_loseSound);
				}
				FlowController.SetHudText (m_currentPlayer.m_playerName + " Wins!!");
				FlowController.OnGameOver (); 
				return;	
			}
			if (m_moveCount >= 9) {
				m_audioSource.PlayOneShot (m_drawSound);
				m_gameOver = true;
				FlowController.SetHudText ("Draw!!");
				FlowController.OnGameOver (); 
				return;
			}
			SetNextPlayer ();
		}

		/// <summary>
		/// Check if the current move resulted in a winning sequence.
		/// </summary>
		/// <returns><c>true</c>, if a winning condition is met, <c>false</c> otherwise.</returns>
		bool CheckCompletion()
		{  
			//Check all the rows
			for (int rowIndex = 0; rowIndex < 9; rowIndex+= 3) {
				if (m_cells [rowIndex].Status == m_currentPlayer.m_statusToSet) {
					if (m_cells [rowIndex].Status == m_cells [rowIndex + 1].Status && m_cells [rowIndex].Status == m_cells [rowIndex + 2].Status) {
						m_cells [rowIndex].SetColor (m_victoryColor);
						m_cells [rowIndex + 1].SetColor (m_victoryColor);
						m_cells [rowIndex + 2].SetColor (m_victoryColor);
						return true;
					}
				}
			}

			//Check all the columns
			for (int colIndex = 0; colIndex < 3; colIndex++	) {
				if (m_cells [colIndex].Status == m_currentPlayer.m_statusToSet) {
					if (m_cells [colIndex].Status == m_cells [colIndex + 3].Status && m_cells [colIndex].Status == m_cells [colIndex + 6].Status) {
						m_cells [colIndex].SetColor (m_victoryColor);
						m_cells [colIndex + 3].SetColor (m_victoryColor);
						m_cells [colIndex + 6].SetColor (m_victoryColor);
						return true;
					}
				}
			}

			/* Diagonal check 
			 * x . .
			 * . x .
			 * . . x
			 */
			if (m_cells [0].Status == m_cells [4].Status && m_cells [0].Status == m_cells [8].Status && m_cells [0].Status == m_currentPlayer.m_statusToSet) {
				m_cells [0].SetColor (m_victoryColor);
				m_cells [4].SetColor (m_victoryColor);
				m_cells [8].SetColor (m_victoryColor);
				return true;
			}

			/* Diagonal check 
			 * . . x
			 * . x .
			 * x . .
			 */
			if (m_cells [2].Status == m_cells [4].Status && m_cells [2].Status == m_cells [6].Status && m_cells [2].Status == m_currentPlayer.m_statusToSet) {
				m_cells [2].SetColor (m_victoryColor);
				m_cells [4].SetColor (m_victoryColor);
				m_cells [6].SetColor (m_victoryColor);
				return true;
			}
			// no win detected
			return false;
		}

		// Simple switch to next player
		void SetNextPlayer()
		{
			if (m_currentPlayer == m_player1) {
				m_currentPlayer = m_player2;
			} else {
				m_currentPlayer = m_player1;
			}
			FlowController.SetTurnText(m_currentPlayer.m_playerName);
			if (!m_currentPlayer.IsHuman ()) {
				StartCoroutine (GetAIInputCor ());
			}
		}

		// To simulate thinking, we wait for a few seconds before requesting response from the AI
		IEnumerator GetAIInputCor()
		{
			yield return new WaitForSeconds(UnityEngine.Random.Range(1,3));
			HandleSelection (((AI)m_currentPlayer).SelectOne ());
		}

	}
}