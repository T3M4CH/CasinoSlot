using UnityEngine;
using System;

namespace Core.UI.Scripts
{
    [Serializable]
    public class SlotCellStruct
    {
        [field: SerializeField] public ECellType Type { get; private set; }
        [field: SerializeField] public Sprite[] Sprites { get; private set; }
    }
}