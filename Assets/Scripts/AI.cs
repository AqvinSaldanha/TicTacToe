using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.aqvin.tictactoe
{
	/// <summary>
	/// This is a derived class from player. 
	/// Logic: 
	/// Keep the list of all cells
	/// whenever a cell is selected in the game, remove it from this list
	/// When requested for input, randomly select from one of the available cells and send it to the game.
	/// </summary>
	public class AI : Player
	{   
		/// <summary>
		/// A list of the index of the available cells[index from m_cells in TicTacToe class]
		/// 9 is the max possible number of items
		/// </summary>
		List<int> m_availableIndex = new List<int>(9);

		/// <summary>
		/// Reset available index list: At the beginning, all the cells are made available.
		/// </summary>
		public override void Reset()
		{
			m_availableIndex.Clear ();
			for (int i = 0; i < 9; i++) {
				m_availableIndex.Add (i);
			}

		}

		/// <summary>
		/// Remove the specified cell index: either taken by user or AI itself
		/// </summary>
		/// <param name="a_index">Index of the removed cell</param>
		public void Remove(int a_index)
		{
			m_availableIndex.Remove (a_index);
		}

		/// <summary>
		/// Pick a cell from the available list.
		/// </summary>
		/// <returns>Index of the cell to be removed.[Index of the cell in m_cells list in the TicTacToe class]</returns>
		public int SelectOne()
		{ 
			// At the moment, we select a random index from the available list.
			return m_availableIndex[UnityEngine.Random.Range (0, m_availableIndex.Count - 1)];
		}

		/// <summary>
		/// Overridden from Player class. This returns false by default.
		/// </summary> 
		public override bool IsHuman()
		{
			return false;
		}
	}
}
