using UnityEngine.UI;
using AxGrid.Base;
using AxGrid.Path;
using UnityEngine;

public class TestRoulette : MonoBehaviourExt
{
    [SerializeField] private VerticalLayoutGroup layoutGroup;
    [SerializeField, Range(0, 100)] private float _speed;
    [SerializeField] private RectTransform stopMarket;
    [SerializeField] private RectTransform targetCell;
    [SerializeField] private float duration;
    [SerializeField] private Vector3 punchDirection;
    [SerializeField] private float duration2;

    private RectTransform _rect;
    private float _yOffset;
    private bool _isStopRequest;

    [OnUpdate]
    private void UpdateThis()
    {
        _rect.anchoredPosition -= Vector2.up * _speed;

        if (_rect.anchoredPosition.y < _yOffset)
        {
            _rect.anchoredPosition = Vector2.zero;

            if (_isStopRequest)
            {
                _isStopRequest = false;
                PerformStop();
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            _isStopRequest = true;
        }
    }

    private void PerformStop()
    {
        _speed = 0;

        var targetPosition = (Vector3)stopMarket.anchoredPosition;
        targetPosition.x = 0;
        targetPosition.y -= targetCell.localPosition.y;

        var startPosition = _rect.localPosition;

        Path = new CPath();

        Path.EasingBackOut(duration, 0, 1f, value => { _rect.localPosition = (targetPosition - startPosition) * value + startPosition; }).EasingLinear(duration2, 0, 1f, value =>
        {
            var multiplier = Mathf.Sin(Mathf.PI * value);
            _rect.localPosition = punchDirection * multiplier + targetPosition;
        });
    }

    [OnStart]
    private void StartThis()
    {
        Debug.Log("StartUpd");
        _rect = GetComponent<RectTransform>();
        _yOffset -= _rect.rect.height - (targetCell.sizeDelta.y * 3 + layoutGroup.spacing * 2);
        Path = new CPath();

    }
}