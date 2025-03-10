using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCoin : MonoBehaviour
{
    public GameObject coinPrefab;
    public float bobbingAmplitude = 0.5f;
    public float bobbingFrequency = 2f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float bobbingOffset = Mathf.Sin(Time.time * bobbingFrequency) * bobbingAmplitude;
        transform.position = startPosition + new Vector3(0, bobbingOffset, 0);
    }
}
