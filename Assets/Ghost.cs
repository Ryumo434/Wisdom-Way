using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Animator ghostAnim;
    [SerializeField] private GameObject Trigger;

    private void Awake()
    {
        ghostAnim = GetComponent<Animator>();
        Trigger =  transform.GetChild(1).gameObject;

        if (Trigger == null)
        {
            Debug.Log("Auf VisualCue konnte nicht zugegriffen werden");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.Instance.ghostDisappear)
        {
            Trigger.SetActive(false);
            Dissappear();
        }
    }

    void Dissappear()
    {
        ghostAnim.Play("Disappear");
    }

    //wird in der disappear Animation verwendet
    void DestroyGhost()
    {
        Destroy(this.gameObject);
    }
}
