using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    public bool alive = true;

    void Update()
    {
        GameController.gc.RefreshScreen();
    }

    public void LoseLife()
    {
        if(alive)
        {
            alive = false;
            gameObject.GetComponent<Animator>().SetTrigger("Dead");
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            gameObject.GetComponent<Player>().enabled = false;
            gameObject.GetComponent<Animator>().SetBool("Jump", false);
            GameController.gc.SetLives(-1);

            if(GameController.gc.lives > 0) // Mudado para > 0 em vez de >= 0
            {
                Invoke("LoadScene", 1f);
            }        
            else
            {
                // Reseta as vidas ao voltar para o menu
                GameController.gc.ResetLives();
                SceneManager.LoadScene("MENU");
            }
        }
    }

    void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}