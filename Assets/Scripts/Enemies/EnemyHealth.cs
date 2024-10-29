using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    public int currentHealth = 3;
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private float knockBackThrust = 15f;

    [SerializeField] private FloatingHealthbar healthbar;
    //[SerializeField] private Slider healthbar;

    //private int currentHealth;
    private Knockback knockback;
    private Flash flash;
    private BossAI bossAI = null;
    private Animator bossAnimator;
    public bool bossIsDead= false;
    private CapsuleCollider2D bossCollider;
    private GameObject bossHealthbar;
    private GameObject bossName;
    private Slider bossHealthbarSlider;
    private GameObject attackCollider;

    private GameObject Player;
    private PlayerHealth playerHealth;
    

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
        bossAI = GetComponent<BossAI>();
        attackCollider = GameObject.Find("Attack Collider");
        
        if (bossAI != null)
        {
            

            bossAI = GetComponent<BossAI>();
            bossAnimator = GetComponent<Animator>();
            

            bossCollider = GetComponent<CapsuleCollider2D>();

            bossHealthbar = GameObject.Find("BossHealthbar");
            bossHealthbarSlider = bossHealthbar.GetComponent<Slider>();

            bossName = GameObject.Find("BossName (TMP)");
        }
        
        Player = GameObject.Find("Player");
        playerHealth = Player.GetComponent<PlayerHealth>();




    }
    
    private void Start()
    {
        currentHealth = startingHealth;
        //healthbar.value = currentHealth;
        // healthbar.UpdateHealthBar(currentHealth, startingHealth);
        
    }

    public void TakeDamage(int damage)
    {   // ist dasselbe wie "currentHealth = currentHealth - damage;"
        currentHealth -= damage;
        healthbar.UpdateHealthBar(currentHealth, startingHealth);

        knockback.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRestoreMatTime());
        DetectDeath();
    }

    private void Update()
    {
        if(playerHealth.isDead) {bossHealthbarSlider.value = startingHealth ; }
    }

    public void DetectDeath()
    {
        //if (bossIsDead) return;


        if (currentHealth <= 0 && bossAI == null)
        {
            //partikel(deathVFXPrefab ein Gameobject) wird an der position(transform.Position) des Enemy instanziiert ohne dass eine rotation stattfindet(Quaternion.identity)
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (currentHealth <= 0 && bossAI != null)
        {
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);

            bossIsDead = true;
            bossAnimator.SetBool("isMoving", false);
            bossAnimator.SetBool("Attack1", false);
            bossAnimator.SetBool("isDead", true);
            
            bossHealthbar.SetActive(false);
            bossName.SetActive(false);
            attackCollider.SetActive(false);





        }
    }
        private IEnumerator DisableAnimatorAfterDeath()
        {
            // Warte, bis die Todesanimation vollständig abgespielt ist.
            yield return new WaitForSeconds(bossAnimator.GetCurrentAnimatorStateInfo(0).length);

            // Animator deaktivieren, damit der Boss im letzten Frame der Animation bleibt
            bossAnimator.enabled = false;

            // Alternativ: Wenn der Boss zerstört werden soll, nach der Animation
            // Destroy(gameObject, 2f);
        }
    
    //AnimEvent
    void DestroyAnimation()
    {
        bossAnimator.enabled = false;
        bossCollider.enabled = false;
        


    }

    

    

}