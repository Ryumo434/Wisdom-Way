using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public bool GettingKnockedBack {  get; private set; }

    //SerialzedField sorgt daf�r dass man die Variable im Inspector sehen kann auch wenn sie auf Privat gestellt ist. Sie bleibt dabei trotzdem private
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
        //.normalized sorgt daf�r dass der Vektor auf die L�nge 1 angepasst wird aber dennoch in diesselbe Richtung zeigt. Wird verwendet wenn die Richtung des Vektors gebraucht wird aber nicht seine gr�sse
        Vector2 difference = (transform.position - damageSource.position).normalized * knockBackThrust * rb.mass;
    
        rb.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }


    //IEnumerator ist ein R�ckgabetyp f�r Coroutines. Coroutines sind spezielle Methoden, die das Spielgeschehen f�r eine bestimmte Zeit pausieren k�nnen, ohne die gesamte Spielausf�hrung zu blockieren.
    private IEnumerator KnockRoutine()
    {
        //yield: Das Schl�sselwort yield pausiert die Coroutine an dieser Stelle und wartet, bis die angegebene Bedingung erf�llt ist, bevor sie fortgesetzt wird.
        yield return new WaitForSeconds(knockBackTime);
        //rb.velocity gibt die aktuelle Bewegungsrichtung und Geschwindigkeit des Objekts in Unity an.
        rb.velocity = Vector2.zero;
        GettingKnockedBack = false;
    }
}
