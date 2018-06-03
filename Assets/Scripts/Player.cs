using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace com.aqvin.tictactoe
{
	/// <summary>
	/// A data class to keep player's record.
	/// </summary>
	[Serializable]
	public class Player
	{
		public string m_playerName;  

		[NonSerialized]
		public Cell.SelectionStatus m_statusToSet; 

		[NonSerialized]
		public Sprite m_overlaySprite;

		/// <summary>
		/// Useful to determine if the current player is human or not. This is true by default. 
		/// To modify this, override this function in a derived class: See AI.cs.
		/// </summary>
		/// <returns><c>true</c> if current player is human; otherwise, <c>false</c>.</returns>
		public virtual bool IsHuman()
		{
			return true;
		}

		/// <summary>
		/// Useful to clear any data from previous game.
		/// </summary>
		public virtual void Reset()
		{
		}
	} 
}