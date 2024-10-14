using UnityEngine;
using UnityEngine.SceneManagement; // Für Szenenwechsel-Events

public class DragMovableObject : MonoBehaviour
{
    [SerializeField] private GameObject PressE;
    [SerializeField] private GameObject hotbar;
    [SerializeField] private GameObject weapon;
    [SerializeField] private Transform player;
    [SerializeField] private Transform movableObject;
    [SerializeField] private FixedJoint2D joint;
    private Rigidbody2D rb;
    [SerializeField] private PlayerController playerController;

    private bool isPlayerInTrigger;

    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        FreezePositionInXY();

        if (hotbar != null) hotbar.SetActive(true);
        if (weapon != null) weapon.SetActive(true);

        if (joint != null)
        {
            joint.enabled = false;
        }

        // Verhindere, dass dieses GameObject beim Szenenwechsel zerstört wird
        DontDestroyOnLoad(gameObject);

        // Abonniere das Event, das aufgerufen wird, wenn eine Szene geladen wird
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Wird aufgerufen, wenn eine neue Szene geladen wird
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Finde den Spieler und den Joint in der neuen Szene
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            playerController = playerObject.GetComponent<PlayerController>();

            // Versuche den FixedJoint2D zu finden
            joint = playerObject.GetComponent<FixedJoint2D>();

            // Falls der Joint nicht existiert, fügen wir ihn hinzu
            if (joint == null)
            {
                Debug.LogWarning("FixedJoint2D nicht gefunden auf dem Spieler, füge Joint hinzu.");
                joint = playerObject.AddComponent<FixedJoint2D>();
                joint.enabled = false; // Deaktiviere ihn erstmal
            }
        }
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

            if (hotbar != null) hotbar.SetActive(false);
            if (weapon != null) weapon.SetActive(false);
            if (PressE != null) PressE.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("trigger exit");
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
        if (joint != null) // Sicherstellen, dass der Joint nach Szenenwechsel existiert
        {
            joint.enableCollision = true;
            joint.enabled = true;
            joint.connectedBody = rb;
        }
    }

    private void DetachFromPlayer()
    {
        if (joint != null) // Sicherstellen, dass der Joint nach Szenenwechsel existiert
        {
            joint.enabled = false;
            joint.connectedBody = null;
        }
    }

    private void OnDestroy()
    {
        // Event abmelden, wenn das GameObject zerstört wird
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}