using UnityEngine;

public class MorteInimigos : MonoBehaviour
{
    private int vidaAtual = 100;
    private bool morrendo = false;

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
        
        // Desativa componentes que podem interferir
        var collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;
        
        var movimento = GetComponent<inimigos>();
        if (movimento != null) movimento.enabled = false;
        
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 1; // Garante que a gravidade está ativada
        }

        // Rotaciona o inimigo 180 graus no eixo X
        transform.rotation = Quaternion.Euler(180f, 0f, 0f);
        
        // Opcional: adiciona um pequeno impulso para cima
        if (rb != null)
        {
            rb.AddForce(new Vector2(0, 2f), ForceMode2D.Impulse);
        }
        
        // Destroi após um delay
        Destroy(gameObject, 1f);
    }
}