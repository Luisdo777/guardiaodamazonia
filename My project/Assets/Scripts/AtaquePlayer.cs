using UnityEngine;

public class AtaquePlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool atacando;
    public Animator animator;
    public Transform ataquePoint;
    public float ataqueRanger = 0.5f;
    public LayerMask enemyLayers;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        atacando = Input.GetButtonDown("Fire2");

        if(atacando == true)
        {
            Ataque();
        }

        void Ataque()
        {
            // vai ser a animação de ataque;
            animator.SetTrigger("Ataque");

            // vai ser o ranger de ataque de acertar o nosso inimigo;
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(ataquePoint.position, ataqueRanger, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<MorteInimigos>().DanoNoInimigo(100);
            }

            // vai ser o dano no inimigo;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(ataquePoint.position, ataqueRanger);
    }
}
