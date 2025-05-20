using UnityEngine;

public class Lenhador_AtaquePlayer : MonoBehaviour
{
    private bool atacando;
    public Animator animator;
    public Transform ataquePoint;
    public float ataqueRanger = 0.5f;
    public LayerMask enemyLayers;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        atacando = Input.GetButtonDown("Fire2");

        if(atacando)
        {
            Ataque();
        }
    }

    void Ataque()
    {
        // Ativa a animação de ataque
        animator.SetTrigger("Ataque");
        
        // Ajusta a posição do ponto de ataque baseado na direção
        Vector2 direcaoAtaque = sr.flipX ? Vector2.left : Vector2.right;
        Vector2 posicaoAtaque = (Vector2)transform.position + direcaoAtaque * Mathf.Abs(ataquePoint.localPosition.x);
        
        // Detecta inimigos no alcance
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(posicaoAtaque, ataqueRanger, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Lenhador_MorteInimigos>().DanoNoInimigo(100);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (ataquePoint != null)
        {
            Vector2 direcaoAtaque = sr != null && sr.flipX ? Vector2.left : Vector2.right;
            Vector2 posicaoAtaque = (Vector2)transform.position + direcaoAtaque * Mathf.Abs(ataquePoint.localPosition.x);
            Gizmos.DrawWireSphere(posicaoAtaque, ataqueRanger);
        }
    }
}