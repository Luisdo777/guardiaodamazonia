using UnityEngine;
using UnityEngine.SceneManagement;

public class Governador_MorteInimigos : MonoBehaviour
{
    private int vidaAtual = 200;
    private bool morrendo = false;
    
    [Header("Configuração de Fase")]
    public float delayTransicao = 2f; // Tempo antes de mudar de cena

    public void DanoNoInimigo(int dano)
    {
        if (morrendo) return;
        
        vidaAtual -= dano;

        if(vidaAtual <= 0)
        {
            MorrerDeCabecaParaBaixo();
        }
    }

    void MorrerDeCabecaParaBaixo()
    {
        morrendo = true;
        
        // Desativa componentes
        var collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;
        
        var movimento = GetComponent<Padre_inimigos>();
        if (movimento != null) movimento.enabled = false;
        
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 1;
        }

        // Animação de morte
        transform.rotation = Quaternion.Euler(180f, 0f, 0f);
        
        if (rb != null)
        {
            rb.AddForce(new Vector2(0, 2f), ForceMode2D.Impulse);
        }
        
        // Chama a transição de cena após o delay
        Invoke("MudarParaProximaCena", delayTransicao);
        
        // Destroi o inimigo
        Destroy(gameObject, delayTransicao);
    }

    void MudarParaProximaCena()
    {
        int proximaCenaIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        // Verifica se existe uma próxima cena
        if (proximaCenaIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(proximaCenaIndex);
        }
        else
        {
            Debug.LogWarning("Não há mais cenas disponíveis! Voltando para o menu principal...");
            // Opcional: Carregar menu principal ou primeira cena
            SceneManager.LoadScene(0);
        }
    }
}