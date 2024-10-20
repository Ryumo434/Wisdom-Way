using UnityEngine;
using UnityEngine.SceneManagement; // Für Szenenwechsel-Events

public class DragMovableObject : MonoBehaviour
{
    [SerializeField] private GameObject PressE;
    [SerializeField] private GameObject hotbar;
    [SerializeField] private GameObject weapon;
    [SerializeField] private Transform movableObject;
    private FixedJoint2D joint;
    private Rigidbody2D rb;
    private PlayerController playerController;

    private bool isPlayerInTrigger;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        FreezePositionInXY();

        if (hotbar != null) hotbar.SetActive(true);
        if (weapon != null) weapon.SetActive(true);

        // Den Spieler in der aktuellen Szene finden und den FixedJoint zuweisen
        AssignPlayerAndJoint();

        // Verhindere, dass dieses GameObject beim Szenenwechsel zerstört wird
        DontDestroyOnLoad(gameObject);

        // Abonniere das Event, das aufgerufen wird, wenn eine Szene geladen wird
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Diese Methode weist den Spieler und den Joint zu
    private void AssignPlayerAndJoint()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            playerController = playerObject.GetComponent<PlayerController>();
            joint = playerObject.GetComponent<FixedJoint2D>();

            if (joint != null)
            {
                joint.enabled = false; // Standardmäßig deaktiviert, bis es gebraucht wird
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

    // Wird aufgerufen, wenn eine neue Szene geladen wird
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Weisen den Spieler und den Joint neu zu, nachdem die Szene geladen wurde
        AssignPlayerAndJoint();
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

        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;

            if (hotbar != null) hotbar.SetActive(false);
            if (weapon != null) weapon.SetActive(false);
            if (PressE != null) PressE.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;

            if (hotbar != null) hotbar.SetActive(true);
            if (weapon != null) weapon.SetActive(true);
            if (PressE != null) PressE.SetActive(false);
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
        if (joint != null) // Sicherstellen, dass der Joint existiert
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
        if (joint != null) // Sicherstellen, dass der Joint existiert
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
        // Event abmelden, wenn das GameObject zerstört wird
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}