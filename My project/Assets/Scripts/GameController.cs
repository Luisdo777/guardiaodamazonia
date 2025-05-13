using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController gc;

    public Text lifeText;
    public int lives = 3;

    void Awake()
    {
        if(gc == null)
        {
            gc = this;
            DontDestroyOnLoad(gameObject);
        }        
        else if(gc != this)
        {
            Destroy(gameObject);
        }
        ResetLives(); // Resetar vidas ao iniciar
    }

    public void SetLives(int life)
    {
        lives += life;
        lives = Mathf.Max(0, lives); // Garante que não fique negativo
        RefreshScreen();
    }

    public void ResetLives()
    {
        lives = 3; // Reset para 3 vidas
        RefreshScreen();
    }

    public void RefreshScreen()
    {
        lifeText.text = lives.ToString();
    }
}