using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void LoadScenes(string cena)
    {

        // Reseta as vidas ao carregar uma nova cena
        
        SceneManager.LoadScene(cena);
        GameController.gc.ResetLives();

    }

    public void Quit()
    {
        Application.Quit();
    }
}