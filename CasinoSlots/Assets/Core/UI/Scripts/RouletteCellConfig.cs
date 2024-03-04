using Core.UI.Scripts;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/RouletteCell", fileName = "RouletteCell")]
public class RouletteCellConfig : ScriptableObject, IWindowObject
{
    [SerializeField] private SlotCellStruct[] slotCellStructs;

    public MonoSlotCell GetCell(ECellType cellType)
    {
        var cellInstance = Instantiate(MonoSlotCell);

        var cellStruct = slotCellStructs.First(cell => cell.Type == cellType);
        cellInstance.Initialize(cellStruct);

        return cellInstance;
    }
    
    public string Patch => "Roulette/RouletteCell";
    [field:SerializeField] public MonoSlotCell MonoSlotCell { get; private set; }
}