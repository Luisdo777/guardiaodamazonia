using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Padre_MorteInimigos : MonoBehaviour
{
    [Header("Configurações de Vida")]
    [SerializeField] private int vidaMaxima = 1000;
    private int vidaAtual;
    private bool morrendo = false;

    [Header("Configuração de Fase")]
    [Tooltip("Tempo antes de mudar de cena após a morte")]
    public float delayTransicao = 2f;

    [Header("Invulnerabilidade")]
    [Tooltip("Tempo de invencibilidade após levar dano")]
    public float tempoInvencivel = 1f;
    [SerializeField] private Color corDano = new Color(1f, 0.5f, 0.5f, 0.7f);
    private bool isInvencivel = false;
    private SpriteRenderer spriteRenderer;
    private Color corOriginal;

    [Header("Configurações de Morte")]
    [SerializeField] private float forcaMorte = 2f;
    [SerializeField] private float rotacaoMorte = 180f;

    // Componentes de referência
    private Collider2D colisor;
    private Rigidbody2D rb;
    private Padre_inimigos movimentoScript;

    void Awake()
    {
        vidaAtual = vidaMaxima;
        spriteRenderer = GetComponent<SpriteRenderer>();
        colisor = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        movimentoScript = GetComponent<Padre_inimigos>();
        
        if (spriteRenderer != null)
        {
            corOriginal = spriteRenderer.color;
        }
    }

    public void DanoNoInimigo(int dano)
    {
        if (morrendo || isInvencivel) return;

        vidaAtual = Mathf.Max(0, vidaAtual - dano);

        if (vidaAtual <= 0)
        {
            MorrerDeCabecaParaBaixo();
        }
        else
        {
            StartCoroutine(AtivarInvencibilidade());
        }
    }

    private IEnumerator AtivarInvencibilidade()
    {
        if (isInvencivel) yield break;

        isInvencivel = true;
        float tempoDecorrido = 0f;
        float intervaloPiscada = 0.1f;

        while (tempoDecorrido < tempoInvencivel)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = corDano;
                yield return new WaitForSeconds(intervaloPiscada);
                
                spriteRenderer.color = corOriginal;
                yield return new WaitForSeconds(intervaloPiscada);
            }
            
            tempoDecorrido += intervaloPiscada * 2;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = corOriginal;
        }
        
        isInvencivel = false;
    }

    void MorrerDeCabecaParaBaixo()
    {
        if (morrendo) return;
        morrendo = true;

        // Cancela qualquer invencibilidade ativa
        StopAllCoroutines();
        isInvencivel = false;

        // Desativa componentes
        if (colisor != null) colisor.enabled = false;
        if (movimentoScript != null) movimentoScript.enabled = false;

        // Configura física da morte
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 1;
            rb.AddForce(Vector2.up * forcaMorte, ForceMode2D.Impulse);
        }

        // Animação de morte
        transform.rotation = Quaternion.Euler(rotacaoMorte, 0f, 0f);

        // Restaura cor original
        if (spriteRenderer != null)
        {
            spriteRenderer.color = corOriginal;
        }

        // Agenda destruição e transição de cena
        Invoke(nameof(MudarParaProximaCena), delayTransicao);
        Destroy(gameObject, delayTransicao);
    }

    void MudarParaProximaCena()
    {
        int cenaAtual = SceneManager.GetActiveScene().buildIndex;
        int proximaCena = cenaAtual + 1;

        if (proximaCena < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(proximaCena);
        }
        else
        {
            // Volta para o menu principal ou cena de créditos
            SceneManager.LoadScene(0);
        }
    }

    // Método útil para resetar o inimigo (opcional)
    public void ResetarInimigo()
    {
        vidaAtual = vidaMaxima;
        morrendo = false;
        isInvencivel = false;
        
        if (colisor != null) colisor.enabled = true;
        if (movimentoScript != null) movimentoScript.enabled = true;
        if (rb != null) rb.gravityScale = 0;
        
        transform.rotation = Quaternion.identity;
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = corOriginal;
        }
        
        StopAllCoroutines();
    }
}