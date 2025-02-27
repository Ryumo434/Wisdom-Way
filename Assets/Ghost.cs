using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Animator ghostAnim;
    [SerializeField] private GameObject Trigger;
    [SerializeField] private GameObject visualCue;

    private void Awake()
    {
        ghostAnim = GetComponent<Animator>();
        Trigger =  transform.GetChild(1).gameObject;
        visualCue = transform.GetChild(0).gameObject;

        if (Trigger == null)
        {
            Debug.Log("Auf Trigger konnte nicht zugegriffen werden");
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
            visualCue.SetActive(false);
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
