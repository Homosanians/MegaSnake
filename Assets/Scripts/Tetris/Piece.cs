using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Piece : MonoBehaviour
{
    public Board Board { get; private set; }
    public TetrominoData TetrominoData { get; private set; }
    public Vector3Int[] Cells { get; private set; }
    public Vector3Int Position { get; private set; }
    public int RotationIndex { get; private set; }

    private PieceAction PieceAction = PieceAction.None;

    public float StepDelay = 1f;
    public float LockDelay = 0.5f;

    private float stepTime;
    private float lockTime;

    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        Board = board;
        TetrominoData = data;
        Position = position;
        RotationIndex = 0;
        stepTime = Time.time + StepDelay;
        lockTime = 0f;

        if (Cells == null)
        {
            Cells = new Vector3Int[TetrominoData.Cells.Length];
        }

        for (var i = 0; i < TetrominoData.Cells.Length; i++)
        {
            Cells[i] = ((Vector3Int)TetrominoData.Cells[i]);
        }
    }

    public bool Move(Vector2Int translation)
    {
        var newPosition = this.Position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = Board.IsValidPosition(this, newPosition);

        if (valid)
        {
            Position = newPosition;
            lockTime = 0f;
        }

        return valid;
    }

    private void Update()
    {
        Board.Clear(this);
        if (Cells.Length > 0)
        {
            switch (PieceAction)
            {
                case PieceAction.Drop:
                    HardDrop();
                    break;
                case PieceAction.MoveLeft:
                    Move(Vector2Int.left);
                    break;
                case PieceAction.MoveRight:
                    Move(Vector2Int.right);
                    break;
                case PieceAction.RotateLeft:
                    Rotate(-1);
                    break;
                case PieceAction.RotateRight:
                    Rotate(1);
                    break;
            }
            PieceAction = PieceAction.None;
            lockTime += Time.deltaTime;
            if (Time.time > stepTime)
            {
                Step();
            }
            Board.Set(this);
        }
    }

    public void GameOver() 
    {
        Cells = new Vector3Int[0];
    }

    private void Step() 
    {
        stepTime = Time.time + StepDelay;
        bool canMove = Move(Vector2Int.down);

        if (lockTime >= LockDelay || !canMove) 
        {
            Lock();
        }
    }

    private void Lock() 
    {
        Board.Set(this);
        Board.ClearLines();
        Board.SpawnPiece();
    }

    private void HardDrop()
    {
        while (Move(Vector2Int.down))
        {
            continue;
        }
        Lock();
    }

    private void Rotate(int direction)
    {

        int originalRotation = RotationIndex;
        RotationIndex = Wrap(RotationIndex + direction, 0, 4);

        ApplyRotationMatrix(direction);
        if (!TestWallKicks(RotationIndex, direction)) 
        {
            RotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }


    }

    private void ApplyRotationMatrix(int direction)
    {
        for (int i = 0; i < Cells.Length; i++)
        {
            Vector3 cell = Cells[i];

            int x, y;

            switch (TetrominoData.TetrominoType)
            {
                case TetrominoType.I:
                case TetrominoType.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }

            Cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKicksIndex = GetWallKicksIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < TetrominoData.WallKicks.GetLength(1); i++) 
        {
            Vector2Int translation = TetrominoData.WallKicks[wallKicksIndex, i];
            if (Move(translation)) 
            {
                return true;
            }
        }
        return false;
    }

    private int GetWallKicksIndex(int rotationIndex, int rotationDirection) 
    {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0) 
        {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, TetrominoData.WallKicks.GetLength(0));
    }

    private int Wrap(int input, int min, int max) 
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else 
        {
            return min + (input - min) % (max - min);
        }
    }

    public void OnClickLeft() 
    {
        if (PieceAction == PieceAction.None)
        {
            PieceAction = PieceAction.MoveLeft;
        }
    }

    public void OnClickRight()
    {
        if (PieceAction == PieceAction.None)
        {
            PieceAction = PieceAction.MoveRight;
        }
    }

    public void OnClickDrop() 
    {
        if (PieceAction == PieceAction.None) 
        {
            PieceAction = PieceAction.Drop;
        }
    }

    public void OnClickRotateLeft() 
    {
        if (PieceAction == PieceAction.None)
        {
            PieceAction = PieceAction.RotateLeft;
        }
    }

    public void OnClickRotateRight()
    {
        if (PieceAction == PieceAction.None)
        {
            PieceAction = PieceAction.RotateRight;
        }
    }

    public void OnClickBackButton() 
    {
        SceneManager.LoadScene(0);
    }
}
