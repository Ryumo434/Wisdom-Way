using UnityEngine;
using UnityEngine.SceneManagement;

public class DragMovableObject : MonoBehaviour
{
    [SerializeField] private GameObject PressE;
    [SerializeField] private GameObject NameText;
    [SerializeField] private GameObject hotbar;
    [SerializeField] private GameObject weapon;
    [SerializeField] private Transform movableObject;
    private FixedJoint2D joint;
    private Rigidbody2D rb;
    private PlayerController playerController;

    private bool isPlayerInTrigger;
    private static DragMovableObject activeObject; // Speichert das aktuell aktive Objekt

    void Start()
    {
        if (this.name == "RobertKoch" || this.name == "IsaacNewton" || this.name == "JohnRockefeller")
        {
            NameText.SetActive(false);
        }
        hotbar = GameObject.Find("Active Inventory");
        weapon = GameObject.Find("Active Weapon");
        rb = GetComponent<Rigidbody2D>();
        FreezePositionInXY();

        if (hotbar != null) hotbar.SetActive(true);
        if (weapon != null) weapon.SetActive(true);

        AssignPlayerAndJoint();


        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void AssignPlayerAndJoint()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            playerController = playerObject.GetComponent<PlayerController>();
            joint = playerObject.GetComponent<FixedJoint2D>();

            if (joint != null)
            {
                joint.enabled = false;
            }
            else
            {
                Debug.LogWarning("FixedJoint2D nicht auf dem Spieler gefunden.");
            }
        }
        else
        {
            Debug.LogWarning("Player nicht gefunden.");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignPlayerAndJoint();
    }

    void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (activeObject == null || activeObject == this) // Prüfen, ob kein anderes Objekt aktiv ist
            {
                if (IsPositionFrozen())
                {
                    UnfreezePositionInXY();
                    AttachToPlayer();
                    if (hotbar != null) hotbar.SetActive(false);
                    if (weapon != null) weapon.SetActive(false);
                    playerController.activateDraggingAnimation();
                    activeObject = this; // Setze das aktuelle Objekt als aktiv
                }
                else
                {
                    FreezePositionInXY();
                    DetachFromPlayer();
                    if (hotbar != null) hotbar.SetActive(true);
                    if (weapon != null) weapon.SetActive(true);
                    playerController.deactivateDraggingAnimation();
                    activeObject = null; // Kein Objekt mehr aktiv
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;

            if (this.name == "RobertKoch" || this.name == "IsaacNewton" || this.name == "JohnRockefeller")
            {
                NameText.SetActive(true);
            }

            if (PressE != null) PressE.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;

            if (this.name == "RobertKoch" || this.name == "IsaacNewton" || this.name == "JohnRockefeller")
            {
                NameText.SetActive(false);
            }

            if (PressE != null) PressE.SetActive(false);
        }
    }

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

    private void AttachToPlayer()
    {
        if (joint != null)
        {
            joint.enableCollision = true;
            joint.enabled = true;
            joint.connectedBody = rb;
        }
        else
        {
            Debug.LogWarning("AttachToPlayer: FixedJoint2D ist null.");
        }
    }

    private void DetachFromPlayer()
    {
        if (joint != null)
        {
            joint.enabled = false;
            joint.connectedBody = null;
        }
        else
        {
            Debug.LogWarning("DetachFromPlayer: FixedJoint2D ist null.");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (activeObject == this)
        {
            activeObject = null; // Das aktive Objekt zurücksetzen, falls es zerstört wird
        }
    }
}
