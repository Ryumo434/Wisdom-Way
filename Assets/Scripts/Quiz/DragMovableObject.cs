using UnityEngine;

public class DragMovableObject : MonoBehaviour
{
    [SerializeField] private GameObject PressE;
    [SerializeField] private GameObject hotbar;
    [SerializeField] private GameObject weapon;
    [SerializeField] private Transform player;
    [SerializeField] private Transform movableObject;
    [SerializeField] private FixedJoint2D joint;
    private Rigidbody2D rb;
    PlayerController playerController;

    private bool isPlayerInTrigger;
    

    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        FreezePositionInXY();
        hotbar.SetActive(true);
        weapon.SetActive(true);

        joint.enabled = false;
    }

    void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (IsPositionFrozen())
            {
                UnfreezePositionInXY();
                AttachToPlayer();
                playerController.activateDraggingAnimation();
            }
            else
            {
                FreezePositionInXY();
                DetachFromPlayer();
                playerController.deactivateDraggingAnimation();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trigger enter");
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            hotbar.SetActive(false);
            weapon.SetActive(false);
            PressE.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("trigger exit");
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            hotbar.SetActive(true);
            weapon.SetActive(true);
            PressE.SetActive(false);
        }
    }

    // Methoden zum Freezen und Unfreezen
    void FreezePositionInXY()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }

    void UnfreezePositionInXY()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private bool IsPositionFrozen()
    {
        return (rb.constraints & RigidbodyConstraints2D.FreezePositionX) != 0 &&
               (rb.constraints & RigidbodyConstraints2D.FreezePositionY) != 0;
    }

    // Methoden zum Verbinden und Trennen des Joints
    private void AttachToPlayer()
    {
        joint.enableCollision = true;
        joint.enabled = true;
        joint.connectedBody = rb;
    }

    private void DetachFromPlayer()
    {
        
        joint.enabled = false;
        joint.connectedBody = null;
    }
}