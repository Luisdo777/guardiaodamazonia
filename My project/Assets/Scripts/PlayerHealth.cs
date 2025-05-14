using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private bool isAlive = true;
    
    public void TakeDamage()
    {
        if (isAlive)
        {
            isAlive = false;
            Die();
        }
    }
    
    private void Die()
    {
        // Desativa o jogador (opcional - pode ser uma animação)
        gameObject.SetActive(false);
        
        // Chama o GameManager para reiniciar a fase
        GameManager.instance.PlayerDied();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Exemplo: morre ao tocar em inimigos
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }
}