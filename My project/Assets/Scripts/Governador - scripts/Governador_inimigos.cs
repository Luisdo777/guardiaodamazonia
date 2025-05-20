using UnityEngine;

public class Governador_inimigos : MonoBehaviour
{
    [Header("Configurações Básicas")]
    public float speed;
    public Transform groundCheck;
    public Transform frontCheck;
    public LayerMask groundLayer;
    public bool facingRight = true;
    public float checkRadius = 0.2f; // Raio para detecção de bordas
    
    [Header("Configurações de Ataque")]
    public GameObject projetilPrefab;
    public Transform pontoDeDisparo;
    public float intervaloDeDisparo = 2f;
    public float distanciaMinimaParaAtirar = 5f;
    
    private float tempoUltimoDisparo;
    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        tempoUltimoDisparo = Time.time;
        rb = GetComponent<Rigidbody2D>();
        
        // Garante que o inimigo não vai cair
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void Update()
    {
        // Verifica se há chão na frente e abaixo do inimigo
        bool hasGroundAhead = Physics2D.OverlapCircle(frontCheck.position, checkRadius, groundLayer);
        bool hasGroundBelow = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // Se não houver chão à frente ou abaixo, vira
        if (!hasGroundAhead || !hasGroundBelow)
        {
            speed *= -1;
        }

        // Movimentação
        rb.velocity = new Vector2(speed, rb.velocity.y);

        // Verificação de flip
        if(speed > 0 && !facingRight)
        {
            Flip();
        }
        else if(speed < 0 && facingRight)
        {
            Flip();
        }
        
        // Lógica de disparo
        if (player != null && projetilPrefab != null && pontoDeDisparo != null)
        {
            float distanciaParaPlayer = Vector2.Distance(transform.position, player.position);
            
            if (distanciaParaPlayer <= distanciaMinimaParaAtirar && 
                Time.time - tempoUltimoDisparo >= intervaloDeDisparo)
            {
                Atirar();
                tempoUltimoDisparo = Time.time;
            }
        }
    }
    
    void Atirar()
    {
        if (projetilPrefab == null || pontoDeDisparo == null) return;
        
        GameObject projetil = Instantiate(projetilPrefab, pontoDeDisparo.position, Quaternion.identity);
        Governador_ProjetilInimigo scriptProjetil = projetil.GetComponent<Governador_ProjetilInimigo>();
        
        if (scriptProjetil != null)
        {
            Vector2 direcaoDisparo;
            
            if (player != null)
            {
                direcaoDisparo = (player.position - pontoDeDisparo.position).normalized;
            }
            else
            {
                direcaoDisparo = facingRight ? Vector2.right : Vector2.left;
            }
            
            scriptProjetil.Configurar(direcaoDisparo, !facingRight);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerLife>().LoseLife();
        }
    }

    // Desenha gizmos para visualização no editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
        
        if (frontCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(frontCheck.position, checkRadius);
        }
    }
}