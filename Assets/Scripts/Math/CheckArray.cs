using UnityEngine;

public class CheckArray : MonoBehaviour
{
    public int[] rightOrder = { 1, 2, 3, 4, 5 };
    public int[] selectedOrder = new int[5];
    public int currentTorchIndex = 0;
    public bool isActive = false;
    public bool CorrectOrder = false; 
}
