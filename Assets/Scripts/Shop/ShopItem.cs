using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public string Name;      // Name des Items
    public int Price;        // Preis des Items
    public Sprite Icon;      // Icon des Items
    public string Description;
    public int Strength;
    public string Effect;
    public WeaponInfo weaponInfo; // <--- So könnte man das direkt verknüpfen
}
