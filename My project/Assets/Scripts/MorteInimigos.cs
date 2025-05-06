using UnityEngine;

public class MorteInimigos : MonoBehaviour
{
    private int vidaAtual = 100;

    public void DanoNoInimigo(int dano)
    {
        vidaAtual -= dano;

        if(vidaAtual <= 0)
        {
                Destroy(this.gameObject);
        }
    }
}
