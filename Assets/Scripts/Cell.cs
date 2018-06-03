using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace com.aqvin.tictactoe
{
	/// <summary>
	/// This class represents a single cell. It has 3 possible states: Free(not selected), Player1(Selected by player 1), Player2(Selected by player 2)
	/// It also has reference to sprite renderer and overlay spriterenderer: Overlay renderer is used for marking 'X' or 'O'
	/// </summary>
	[Serializable]
	public class Cell
	{
		public enum SelectionStatus {Free=0, Player1, Player2}

		private SelectionStatus m_status = SelectionStatus.Free;
		public SelectionStatus Status {
			get{ return m_status; }
			set{
				m_status = value;
			}
		} 

		public GameObject m_gameObject;
		SpriteRenderer m_spriteRenderer;
		SpriteRenderer m_overLaySpriteRenderer;

		Color m_interactableColor, m_disabledColor;

		/// <summary>
		/// We will use this to check mouse click or touch
		/// </summary>
		Bounds m_bounds;

		public void Initialize( Color a_interactableColor, Color a_disabledColor)
		{
			m_spriteRenderer = m_gameObject.GetComponent<SpriteRenderer> ();
			m_bounds = m_spriteRenderer.bounds;
			m_overLaySpriteRenderer = m_gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<SpriteRenderer> ();
			m_disabledColor = a_disabledColor;
			m_interactableColor = a_interactableColor;
			Reset ();
		}

		/// <summary>
		/// Check if the point is within the spriterenderer's bounds
		/// </summary>
		/// <returns><c>true</c>, if point was is within bounds, <c>false</c> otherwise.</returns>
		/// <param name="a_point">A point.</param>
		public bool ContainsPoint(Vector3 a_point)
		{ 
			return m_bounds.Contains (new Vector3(a_point.x, a_point.y, m_bounds.center.z));
		}

		public void Reset()
		{
			m_status = SelectionStatus.Free;
			m_overLaySpriteRenderer.gameObject.SetActive (false);
			SetColor(m_interactableColor);
		}

		/// <summary>
		/// Call this function to mark the cell as selected
		/// </summary>
		/// <param name="a_overlaySprite">Sprite with 'X' or 'O' marked in it</param>
		public void Select ( Sprite a_overlaySprite)
		{
			m_overLaySpriteRenderer.gameObject.SetActive (true);
			m_overLaySpriteRenderer.sprite = a_overlaySprite;
			SetColor(m_disabledColor);
		}

		public void SetColor(Color a_color)
		{
			m_spriteRenderer.color = a_color;
		}
	}
}