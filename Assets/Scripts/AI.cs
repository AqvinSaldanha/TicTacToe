using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.aqvin.tictactoe
{
	/// <summary>
	/// This is a derived class from player. 
	/// We use HashSet data structure to determine next move. 
	/// m_combinations contains list of all possible win conditions
	/// We use set operations to determine if next move can result in a win: 
	/// For example assume for Row 1 - 
	/// AI has secured 0 and 1 element. 2 element is yet to be selected
	/// The elements 0 and 1 are stored in m_mySelectedIndex
	/// so when we take an compliment operation(ExceptWith) between set Row 1 {0,1,2} and selected items set {0,1}
	/// we get 2 as the remaining item.
	/// </summary>
	public class AI : Player
	{   
		/// <summary>
		/// A list of the index of the available cells[index from m_cells in TicTacToe class]
		/// 9 is the max possible number of items
		/// </summary>
		List<int> m_availableIndex = new List<int>(9);

		/// <summary>
		/// The list of index of cells selected by AI.
		/// </summary>
		List<int> m_mySelectedIndex = new List<int>(9);

		/// <summary>
		/// The list of index of cells selected by Human.
		/// </summary>
		List<int> m_humanSelectedIndex = new List<int>(9);


		int[][] m_combinations;

		/// <summary>
		/// Predetermined rows and columns for winning condition
		/// </summary>
		readonly int[] ROW1 = new int[] { 0,1,2 };
		readonly int[] ROW2 = new int[] { 3,4,5 };
		readonly int[] ROW3 = new int[] { 6,7,8 };

		readonly int[] COL1 = new int[] { 0,3,6 };
		readonly int[] COL2 = new int[] { 1,4,7 };
		readonly int[] COL3 = new int[] { 2,5,8 };

		readonly int[] DIA1 = new int[] { 0,4,8 };
		readonly int[] DIA2 = new int[] { 2,4,6 };


		public override void Initialize (Cell.SelectionStatus a_selectionStatus, Sprite a_overlaySprite)
		{
			base.Initialize (a_selectionStatus, a_overlaySprite);
			if (m_combinations == null) {
				m_combinations = new int[8][];
				m_combinations [0] = ROW1;
				m_combinations [1] = ROW2;
				m_combinations [2] = ROW3;
				m_combinations [3] = COL1;
				m_combinations [4] = COL2;
				m_combinations [5] = COL3;
				m_combinations [6] = DIA1;
				m_combinations [7] = DIA2;
			}
		}

		/// <summary>
		/// Reset available index list: At the beginning, all the cells are made available.
		/// </summary>
		public override void Reset()
		{ 
			m_availableIndex.Clear ();
			m_mySelectedIndex.Clear ();
			m_humanSelectedIndex.Clear ();
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
			if (!m_mySelectedIndex.Contains (a_index)) {
				m_humanSelectedIndex.Add (a_index);

				string l_temp = "";
				foreach (int i in m_humanSelectedIndex) {

					l_temp += i + " , ";
				}
				Debug.LogError ("Human got: " + l_temp);
			}
			m_availableIndex.Remove (a_index);
		}

		/// <summary>
		/// Pick a cell from the available list.
		/// </summary>
		/// <returns>Index of the cell to be removed.[Index of the cell in m_cells list in the TicTacToe class]</returns>
		public int SelectOne()
		{ 
			int l_newIndex = GetNext ();
			m_mySelectedIndex.Add (m_availableIndex [l_newIndex]);

			return m_availableIndex [l_newIndex];
		}

		HashSet<int> m_compareSet;
		HashSet<int> m_currentSet;
		/// <summary>
		/// This is the main logic of the AI.
		/// </summary>
		/// <returns>The next.</returns>
		int GetNext()
		{
			// this loop will check if game can be won by AI!
			for (int i = 0; i < 8; i++) {
				m_compareSet = new HashSet<int> (m_combinations [i]);
				m_currentSet = new HashSet<int> (m_combinations [i]);

				// keep only selected items in the compare set
				m_compareSet.IntersectWith (m_mySelectedIndex);

				// if 2 items are selected in in the particular row, then check if third can be selected
				if (m_compareSet.Count == 2) {
					// return the third item if not already taken; game win!!
					m_currentSet.ExceptWith (m_compareSet);
					int[] l_arr = new int[1];
					m_currentSet.CopyTo (l_arr);
					int l_toCheck = l_arr [0];
					if (m_availableIndex.Contains (l_toCheck)) {
						return m_availableIndex.IndexOf (l_toCheck);
					}
				} 
			}

			// Flow reached here, means game cannot be won by AI at the moment: Sabotage any chances of player winning!
			for (int i = 0; i < 8; i++) {
				m_compareSet = new HashSet<int> (m_combinations [i]);
				m_currentSet = new HashSet<int> (m_combinations [i]);

				m_compareSet.IntersectWith (m_humanSelectedIndex);

				if (m_compareSet.Count == 2) {
					// return the third item if not already taken; Prevent player from winning!
					m_currentSet.ExceptWith (m_compareSet);
					int[] l_arr = new int[1];
					m_currentSet.CopyTo (l_arr);
					int l_toCheck = l_arr [0];
					if (m_availableIndex.Contains (l_toCheck)) {
						return m_availableIndex.IndexOf (l_toCheck);
					}
				} 
			}

			return UnityEngine.Random.Range (0, m_availableIndex.Count - 1);
		}

		void PrintSet(HashSet<int> a_set)
		{
			string l_temp = "";
			foreach (int m in a_set) {
				l_temp += m;
				l_temp += " , ";
			}
			Debug.LogError ("Set " + l_temp);
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
