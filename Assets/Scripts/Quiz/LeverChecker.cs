using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LeverChecker : MonoBehaviour
{
    public int[] leverPositioning = { 0, 0, 0, 0 };
    public int[] rightLeverPositioning = { 1, 0, 0, 1 };
    [SerializeField] GameObject bridge;
    [SerializeField] GameObject brokenBridge;
    [SerializeField] GameObject barrier;

    void Start()
    {
        bridge.SetActive(false);
        brokenBridge.SetActive(true);
        barrier.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // Verwende SequenceEqual um die Inhalte der Arrays zu vergleichen
        if (leverPositioning.SequenceEqual(rightLeverPositioning))
        {
            bridge.SetActive(true);
            brokenBridge.SetActive(false);
            barrier.SetActive(false);
        }
    }
}