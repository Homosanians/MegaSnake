using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardBootstrap : MonoBehaviour
{
    [SerializeField] private SnakeBoard _board;

    [SerializeField] SpriteRenderer _boardTilesRenderer;
    [SerializeField] SpriteRenderer _borderRenderer;

    [SerializeField] bool _calculateUniformScaleOnAwake = true;
    [SerializeField] Vector2 _uniformScaleFactor;

    private void Awake()
    {
        if (_calculateUniformScaleOnAwake)
            _uniformScaleFactor = new Vector2(
                _borderRenderer.transform.localScale.x / _boardTilesRenderer.size.x,
                _borderRenderer.transform.localScale.y / _boardTilesRenderer.size.y
                );

        _boardTilesRenderer.drawMode = SpriteDrawMode.Tiled;
        _boardTilesRenderer.size = new Vector2(_board.Width, _board.Height);

        _borderRenderer.transform.localScale = new Vector3(
            _uniformScaleFactor.x * _board.Width,
            _uniformScaleFactor.y * _board.Height,
            1);
    }
}
