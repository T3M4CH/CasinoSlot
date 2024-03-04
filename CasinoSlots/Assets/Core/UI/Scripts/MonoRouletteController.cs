using UnityEngine.UI;
using AxGrid.Base;
using AxGrid.Model;
using AxGrid.Path;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class MonoRouletteController : MonoBehaviourExtBind
{
    [SerializeField] private VerticalLayoutGroup layoutGroup;
    [SerializeField] private RectTransform stopMarket;
    [SerializeField] private RectTransform targetCell;
    [SerializeField] private float duration;
    [SerializeField] private Vector3 punchDirection;
    [SerializeField] private float duration2;

    private float _yOffset;
    private bool _isStopRequest;
    private RectTransform _rect;
    private List<MonoSlotCell> _cells = new();
    private RouletteConfig _rouletteConfig;
    private RouletteCellConfig _cellsConfig;

    [Bind("IsSpinning")]
    private void PerformStartSpinning(bool isSpinning)
    {
        Path = new CPath();
        if (!isSpinning)
        {
            _isStopRequest = true;
            return;
        }

        FindAppropriateTarget();
        var startPosition = _rect.anchoredPosition;
        var targetPosition = _rect.anchoredPosition + new Vector2(0, 30);
        Path.EasingBackOut(0.6f, 0, 1f, value => { _rect.anchoredPosition = (targetPosition - startPosition) * value + startPosition; }).Action(() => { Model.EventManager.Invoke(FSMConstants.StartAcceleration); });
    }

    private void FindAppropriateTarget()
    {
        var targetType = Model.Get("ECellType", ECellType.Hand);
        var cell = _cells.FirstOrDefault(cell => cell.Type == targetType);

        if (cell)
        {
            targetCell = cell.RectTransform;
        }
        else
        {
            throw new Exception($"{targetType} doesn't contains in roulette");
        }
    }

    [OnUpdate]
    private void PerformUpdate()
    {
        _rect.anchoredPosition -= Vector2.up * Speed;

        if (_rect.anchoredPosition.y < _yOffset)
        {
            _rect.anchoredPosition = Vector2.zero;

            if (_isStopRequest)
            {
                _isStopRequest = false;
                PerformStop();
            }
        }
    }

    private void PerformStop()
    {
        Model.Set("Speed", 0);

        var targetPosition = (Vector3)stopMarket.anchoredPosition;
        targetPosition.x = 0;
        targetPosition.y -= targetCell.localPosition.y;

        var startPosition = _rect.localPosition;

        Path = new CPath();

        Path.EasingBackOut(duration, 0, 1f, value => { _rect.localPosition = (targetPosition - startPosition) * value + startPosition; })
            .EasingLinear(duration2, 0, 1f, value =>
            {
                var multiplier = Mathf.Sin(Mathf.PI * value);
                _rect.localPosition = punchDirection * multiplier + targetPosition;
            }).Action(() => Model.EventManager.Invoke("CompleteRoll"));
    }

    [OnStart]
    private void StartThis()
    {
        SpawnCells();
        _yOffset -= _rect.rect.height - (targetCell.sizeDelta.y * 3 + layoutGroup.spacing * 2);
        Path = new CPath();
    }

    private void SpawnCells()
    {
        _rect = GetComponent<RectTransform>();

        _rouletteConfig = Resources.Load<RouletteConfig>("RouletteConfig");
        _cellsConfig = Resources.Load<RouletteCellConfig>("RouletteCell");

        var cells = _rouletteConfig.Cells;
        var cellLenght = cells.Length;

        var sizeDelta = _rect.sizeDelta;
        var cellDelta = _cellsConfig.MonoSlotCell.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.y = cellDelta.y * cellLenght + layoutGroup.spacing * (cellLenght - 1);
        _rect.sizeDelta = sizeDelta;

        for (var i = 0; i < cells.Length; i++)
        {
            var type = cells[i];
            var cell = _cellsConfig.GetCell(type);
            cell.transform.SetParent(_rect);

            if (i == 0 || i == cellLenght - 1) continue;

            _cells.Add(cell);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(_rect);
    }

    private float Speed => Model.GetFloat("Speed");
}