using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class GhostAnim : MonoBehaviour
{

    Animator ghostAnimator;
    // Start is called before the first frame update

    private void Awake()
    {
        ghostAnimator = GetComponent<Animator>();
    }
    void Start()
    {
        if (ghostAnimator.SetBool(isDisappeared)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestroyGhost()
    {
        Destroy(gameObject);
    }
}
