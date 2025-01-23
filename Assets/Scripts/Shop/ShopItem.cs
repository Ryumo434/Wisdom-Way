using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public string Name; // Name des Items
    public int Price; // Preis des Items
    public Sprite Icon; // Icon des Items
    public string Description; // Beschreibung des Items
    public int Strength; // St�rke (f�r Waffen)
    public string Effect; // Effekt (f�r Tr�nke)
}
