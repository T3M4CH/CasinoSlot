using Core.UI.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class MonoSlotCell : MonoBehaviour
{
    [SerializeField] private Image image;
    
    private SlotCellStruct _cellStruct;

    public void Initialize(SlotCellStruct cellStruct)
    {
        _cellStruct = cellStruct;
        image.sprite = _cellStruct.Sprites[0];
    }
}
