using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Bestiary/Item")]
public class BestiaryItemData : ScriptableObject
{
    public string itemName;
    [TextArea(5, 10)] // Vytvoří větší pole pro text v Inspectoru
    public string description;
    public Sprite icon; // Obrázek pro detail
}