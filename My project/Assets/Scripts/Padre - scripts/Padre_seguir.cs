using UnityEngine;

public class InimigoTerrestre : MonoBehaviour
{
    [Header("Configurações de Perseguição")]
    public Transform player;
    public float velocidade = 3f;
    public float distanciaParada = 1f;

    [Header("Configurações do Chão")]
    public LayerMask camadaChao;
    public float raioDetecaoChao = 0.2f;
    public Transform pontoVerificacaoChao;

    private Rigidbody2D rb;
    private bool noChao;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Encontra o jogador automaticamente se não atribuído
        if (player == null)
        {
            GameObject jogador = GameObject.FindGameObjectWithTag("Player");
            if (jogador != null) player = jogador.transform;
        }
    }

    void Update()
    {
        // Verifica se está no chão
        noChao = Physics2D.OverlapCircle(pontoVerificacaoChao.position, raioDetecaoChao, camadaChao);
        
        // Atualiza a direção do sprite
        VirarSprite();
    }

    void FixedUpdate()
    {
        if (player == null || !noChao) return;

        float distancia = Vector2.Distance(transform.position, player.position);
        
        if (distancia > distanciaParada)
        {
            // Move em direção ao jogador (apenas no eixo X para evitar subir obstáculos)
            Vector2 direcao = new Vector2(player.position.x - transform.position.x, 0).normalized;
            rb.velocity = new Vector2(direcao.x * velocidade, rb.velocity.y);
        }
        else
        {
            // Para de se mover quando está perto o suficiente
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void VirarSprite()
    {
        if (player == null) return;
        
        // Vira o sprite na direção do jogador
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    // Visualização do detector de chão no editor
    void OnDrawGizmosSelected()
    {
        if (pontoVerificacaoChao != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(pontoVerificacaoChao.position, raioDetecaoChao);
        }
    }
}