using UnityEngine;

public class Governador_ProjetilInimigo : MonoBehaviour
{
    [Header("Configurações do Projétil")]
    public float velocidade = 5f;
    public int dano = 20;
    public float tempoVida = 3f;
    public LayerMask layersParaColidir;
    
    private Vector2 direcao;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Destroy(gameObject, tempoVida); // Destrói após tempo de vida
    }

    public void Configurar(Vector2 dir, bool flipX)
    {
        direcao = dir.normalized;
        if (sr != null && flipX)
        {
            sr.flipX = true;
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.velocity = direcao * velocidade;
        }
    }

    void OnTriggerEnter2D(Collider2D colisor)
    {
        // Verifica se colidiu com uma layer que deve causar impacto
        if (layersParaColidir == (layersParaColidir | (1 << colisor.gameObject.layer)))
        {
            // Verifica se é o jogador
            if (colisor.CompareTag("Player"))
            {
                var playerLife = colisor.GetComponent<PlayerLife>();
                if (playerLife != null)
                {
                    playerLife.LoseLife();
                }
            }
            
            // Destrói o projétil após colisão
            Destroy(gameObject);
        }
    }
}