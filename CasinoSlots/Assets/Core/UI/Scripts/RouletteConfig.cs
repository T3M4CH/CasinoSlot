using UnityEngine;

[CreateAssetMenu(menuName = "Config/RouletteConfig", fileName = "RouletteConfig")]
public class RouletteConfig : ScriptableObject, IWindowObject
{
    [field: SerializeField] public ECellType[] Cells { get; private set; }

    public string Patch => "Roulette/RouletteConfig";
}