using UnityEngine;

public class Padre_ProjetilInimigo : MonoBehaviour
{
    [Header("Configurações do Projétil")]
    public float velocidade = 10f;
    public int dano = 20;
    public float tempoVida = 3f;
    public LayerMask layersParaColidir;
    
    private Vector2 direcao;
    private Rigidbody2D rb;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, tempoVida);
    }
    
    public void Configurar(Vector2 novaDirecao)
    {
        direcao = novaDirecao.normalized;
        
        // Rotaciona o projétil na direção do movimento
        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angulo, Vector3.forward);
        
        // Inicia o movimento
        if (rb != null)
        {
            rb.velocity = direcao * velocidade;
        }
    }
    
    // No script Padre_ProjetilInimigo.cs, modifique o OnTriggerEnter2D:
    void OnTriggerEnter2D(Collider2D colisao)
    {
        if (colisao.CompareTag("Player"))
        {
            var playerLife = colisao.GetComponent<PlayerLife>();
            if (playerLife != null)
            {
                // Se o método não aceitar parâmetros, chame sem o dano
                playerLife.LoseLife(); // Chama o método original sem parâmetro
            
                // Ou, se quiser aplicar múltiplos chamados para dano maior:
                // for (int i = 0; i < dano; i++) {
                //     playerLife.LoseLife();
                // }
            }
            Destroy(gameObject);
        }
        else if (((1 << colisao.gameObject.layer) & layersParaColidir) != 0)
        {
            Destroy(gameObject);
        }
    }

    /*
    void OnTriggerEnter2D(Collider2D colisao)
    {
        // Verifica se colidiu com algo que deve causar dano
        if (colisao.CompareTag("Player"))
        {
            var playerLife = colisao.GetComponent<PlayerLife>();
            if (playerLife != null)
            {
                playerLife.LoseLife(dano);
            }
            Destroy(gameObject);
        }
        // Destrói se atingir terreno ou outros objetos
        else if (((1 << colisao.gameObject.layer) & layersParaColidir) != 0)
        {
            Destroy(gameObject);
        }
    }
    */
}