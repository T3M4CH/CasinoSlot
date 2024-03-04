using UnityEngine.UI;
using AxGrid.Base;
using AxGrid.Model;
using AxGrid.Path;
using UnityEngine;
using System;

public class MonoRouletteController : MonoBehaviourExtBind
{
    [SerializeField] private VerticalLayoutGroup layoutGroup;
    [SerializeField] private RectTransform stopMarket;
    [SerializeField] private RectTransform targetCell;
    [SerializeField] private float duration;
    [SerializeField] private Vector3 punchDirection;
    [SerializeField] private float duration2;

    private RectTransform _rect;
    private float _yOffset;
    private bool _isStopRequest;

    [Bind("IsSpinning")]
    private void PerformStartSpinning(bool isSpinning)
    {
        Path = new CPath();
        if (!isSpinning)
        {
            _isStopRequest = true;
            return;
        }

        var startPosition = _rect.anchoredPosition;
        var targetPosition = _rect.anchoredPosition + new Vector2(0, 30);
        Path.EasingBackOut(0.6f, 0, 1f, value => { _rect.anchoredPosition = (targetPosition - startPosition) * value + startPosition; }).Action(() => { Model.EventManager.Invoke(FSMConstants.StartAcceleration); });
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
            });
    }

    [OnStart]
    private void StartThis()
    {
        _rect = GetComponent<RectTransform>();
        _yOffset -= _rect.rect.height - (targetCell.sizeDelta.y * 3 + layoutGroup.spacing * 2);
        Path = new CPath();
    }

    private float Speed => Model.GetFloat("Speed");
}