using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Snake.Models
{
	public class LetterTile
	{
		public SnakeBoard Board { get; private set; }
		public int Order { get; private set; }
		public Vector2Int Position { get; private set; }
		public string Letter { get; private set; }
		public Tile CustomTile { get; private set; }

		public LetterTile(SnakeBoard board, int order, Vector2Int position, string letter, Tile customTile)
		{
			Board = board;
			Order = order;
			Position = position;
			Letter = letter;
			CustomTile = customTile;
		}
	}
}
