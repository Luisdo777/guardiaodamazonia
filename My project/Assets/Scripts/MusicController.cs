using UnityEngine;

public class MusicController : MonoBehaviour
{
    private static MusicController mC;
    private void Awake()
    {
        if(mC == null)
        {
            mC = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(mC != this)
        {
            Destroy(gameObject);
        }
    }
}
