using UnityEngine;

public class Padre_inimigos : MonoBehaviour
{
    [Header("Configurações Básicas")]
    public float speed;
    public bool ground = true;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public bool facingRight = true;
    
    [Header("Configurações de Ataque")]
    public GameObject projetilPrefab;
    public Transform pontoDeDisparo;
    public float intervaloDeDisparo = 2f;
    public float distanciaParaAtirar = 5f;
    public float velocidadeProjetil = 7f;
    
    private float tempoUltimoDisparo;
    private Transform player;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        tempoUltimoDisparo = Time.time;
    }
    
    void Update()
    {
        // Movimento padrão do inimigo
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        ground = Physics2D.Linecast(groundCheck.position, transform.position, groundLayer);
        
        if(ground == false)
        {
            speed *= -1;
        }
        
        // Verifica direção do sprite
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
            
            if (distanciaParaPlayer <= distanciaParaAtirar && Time.time - tempoUltimoDisparo >= intervaloDeDisparo)
            {
                Atirar();
                tempoUltimoDisparo = Time.time;
            }
        }
    }
    
    void Atirar()
    {
        if (player == null) return;
        
        // Calcula direção para o jogador
        Vector2 direcao = (player.position - pontoDeDisparo.position).normalized;
        
        // Instancia o projétil
        GameObject projetil = Instantiate(projetilPrefab, pontoDeDisparo.position, Quaternion.identity);
        Padre_ProjetilInimigo scriptProjetil = projetil.GetComponent<Padre_ProjetilInimigo>();
        
        if (scriptProjetil != null)
        {
            scriptProjetil.Configurar(direcao);
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
}