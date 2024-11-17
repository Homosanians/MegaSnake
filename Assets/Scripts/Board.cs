using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class Board : MonoBehaviour
{
    public Tilemap Tilemap { get; private set; }
    public Piece ActivePiece {  get; private set; }

    [SerializeField]
    private GameObject EndGame;

    public Vector3Int SpawnPosition;

    public TetrominoData[] Tetrominoes;
    public Vector2Int BoardSize = new Vector2Int(10, 20);

    public RectInt Bounds 
    {
        get 
        {
            Vector2Int position = new Vector2Int(-this.BoardSize.x /2, -this.BoardSize.y / 2);
            return new RectInt(position, this.BoardSize);
        }
    }

    private void Awake() 
    {
        Tetrominoes[0].Letters = new string[4];
        if (PlayerPrefs.HasKey("word1"))
        {
            string[] wordArray = PlayerPrefs.GetString("word1").ToCharArray().Select(c => c.ToString()).ToArray();
            for (int i = 0; i < 4; i++) 
            {
                if (i < wordArray.Length)
                {
                    Tetrominoes[0].Letters[i] = wordArray[i];
                }
                else 
                {
                    Tetrominoes[0].Letters[i] = "";
                }

            }
        }
        Tetrominoes[1].Letters = new string[4];
        if (PlayerPrefs.HasKey("word2"))
        {
            string[] wordArray = PlayerPrefs.GetString("word2").ToCharArray().Select(c => c.ToString()).ToArray();
            for (int i = 0; i < 4; i++)
            {
                if (i < wordArray.Length)
                {
                    Tetrominoes[1].Letters[i] = wordArray[i];
                }
                else
                {
                    Tetrominoes[1].Letters[i] = "";
                }

            }
        }
        Tetrominoes[2].Letters = new string[4];
        if (PlayerPrefs.HasKey("word3"))
        {
            string[] wordArray = PlayerPrefs.GetString("word3").ToCharArray().Select(c => c.ToString()).ToArray();
            for (int i = 0; i < 4; i++)
            {
                if (i < wordArray.Length)
                {
                    Tetrominoes[2].Letters[i] = wordArray[i];
                }
                else
                {
                    Tetrominoes[2].Letters[i] = "";
                }

            }
        }
        Tetrominoes[3].Letters = new string[4];
        if (PlayerPrefs.HasKey("word4"))
        {
            string[] wordArray = PlayerPrefs.GetString("word4").ToCharArray().Select(c => c.ToString()).ToArray();
            for (int i = 0; i < 4; i++)
            {
                if (i < wordArray.Length)
                {
                    Tetrominoes[3].Letters[i] = wordArray[i];
                }
                else
                {
                    Tetrominoes[3].Letters[i] = "";
                }

            }
        }
        Tetrominoes[4].Letters = new string[4];
        if (PlayerPrefs.HasKey("word5"))
        {
            string[] wordArray = PlayerPrefs.GetString("word5").ToCharArray().Select(c => c.ToString()).ToArray();
            for (int i = 0; i < 4; i++)
            {
                if (i < wordArray.Length)
                {
                    Tetrominoes[4].Letters[i] = wordArray[i];
                }
                else
                {
                    Tetrominoes[4].Letters[i] = "";
                }

            }
        }
        Tetrominoes[5].Letters = new string[4];
        if (PlayerPrefs.HasKey("word6"))
        {
            string[] wordArray = PlayerPrefs.GetString("word6").ToCharArray().Select(c => c.ToString()).ToArray();
            for (int i = 0; i < 4; i++)
            {
                if (i < wordArray.Length)
                {
                    Tetrominoes[5].Letters[i] = wordArray[i];
                }
                else
                {
                    Tetrominoes[5].Letters[i] = "";
                }

            }
        }
        Tetrominoes[6].Letters = new string[4];
        if (PlayerPrefs.HasKey("word7"))
        {
            string[] wordArray = PlayerPrefs.GetString("word7").ToCharArray().Select(c => c.ToString()).ToArray();
            for (int i = 0; i < 4; i++)
            {
                if (i < wordArray.Length)
                {
                    Tetrominoes[6].Letters[i] = wordArray[i];
                }
                else
                {
                    Tetrominoes[6].Letters[i] = "";
                }

            }
        }

        Tilemap = GetComponentInChildren<Tilemap>();
        ActivePiece = GetComponentInChildren<Piece>();

        for (var i = 0; i < Tetrominoes.Length; i++)
        {
            Tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void SpawnPiece() 
    {
        var random = Random.Range(0, Tetrominoes.Length);
        TetrominoData data = this.Tetrominoes[random];

        ActivePiece.Initialize(this, SpawnPosition, data);

        if (IsValidPosition(ActivePiece, SpawnPosition))
        {
            Set(ActivePiece);
        }
        else 
        {
            GameOver();
        }

        
    }
    private void GameOver() 
    {
        ActivePiece.GameOver();
        EndGame.SetActive(true);
    }

    public void ReturnToWords() 
    {
        SceneManager.LoadScene(0);
    }


    public void Set(Piece piece) 
    {
        for (var i = 0; i < piece.Cells.Length;i++) 
        {
            Vector3Int tilePosition = piece.Cells[i] + piece.Position;
            piece.TetrominoData.Tile.gameObject.GetComponent<TMP_Text>().text = piece.TetrominoData.Letters[i];
            Tilemap.SetTile(tilePosition, piece.TetrominoData.Tile); 
        }
    }

    public void Clear(Piece piece)
    {
        for (var i = 0; i < piece.Cells.Length; i++)
        {
            Vector3Int tilePosition = piece.Cells[i] + piece.Position;
            Tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position) 
    {
        RectInt bounds = Bounds;

        for (int i = 0; i < piece.Cells.Length; i++) 
        {
            Vector3Int tilePosition = piece.Cells[i] + position;

            if (!bounds.Contains((Vector2Int)tilePosition)) 
            {
                return false;
            }

            if (this.Tilemap.HasTile(tilePosition)) 
            {
                return false;
            }
        }

        return true;
    }

    public void ClearLines() 
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;

        while (row < bounds.yMax) 
        {
            if (IsLineFull(row))
            {
                LineClear(row);
            }
            else 
            {
                row++;
            }
        }
    }

    private bool IsLineFull(int row) 
    {
       RectInt bounds = Bounds;
        for (int col = bounds.xMin; col < bounds.xMax; col++) 
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!Tilemap.HasTile(position)) 
            {
                return false; 
            }
        }

        return true;
    }

    private void LineClear(int row) 
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++) 
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            Tilemap.SetTile(position, null);
        }

        while (row < bounds.yMax) 
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++) 
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = Tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                Tilemap.SetTile(position, above);
            }

            row++;
        }
    }
}
