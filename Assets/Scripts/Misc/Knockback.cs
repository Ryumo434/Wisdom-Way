using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public bool GettingKnockedBack {  get; private set; }

    //SerialzedField sorgt dafür dass man die Variable im Inspector sehen kann auch wenn sie auf Privat gestellt ist. Sie bleibt dabei trotzdem private
    //.2f = 0.2f
    [SerializeField] private float knockBackTime = .2f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetknockedBack(Transform damageSource, float knockBackThrust)
    {
        GettingKnockedBack = true;
        //.normalized sorgt dafür dass der Vektor auf die Länge 1 angepasst wird aber dennoch in diesselbe Richtung zeigt. Wird verwendet wenn die Richtung des Vektors gebraucht wird aber nicht seine grösse
        Vector2 difference = (transform.position - damageSource.position).normalized * knockBackThrust * rb.mass;
    
        rb.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }


    //IEnumerator ist ein Rückgabetyp für Coroutines. Coroutines sind spezielle Methoden, die das Spielgeschehen für eine bestimmte Zeit pausieren können, ohne die gesamte Spielausführung zu blockieren.
    private IEnumerator KnockRoutine()
    {
        //yield: Das Schlüsselwort yield pausiert die Coroutine an dieser Stelle und wartet, bis die angegebene Bedingung erfüllt ist, bevor sie fortgesetzt wird.
        yield return new WaitForSeconds(knockBackTime);
        //rb.velocity gibt die aktuelle Bewegungsrichtung und Geschwindigkeit des Objekts in Unity an.
        rb.velocity = Vector2.zero;
        GettingKnockedBack = false;
    }
}
