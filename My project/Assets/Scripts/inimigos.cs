
using UnityEngine;

public class inimigos : MonoBehaviour
{
    public float speed;
    public bool ground = true;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public bool facingRight = true;
    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        ground = Physics2D.Linecast(groundCheck.position, transform.position, groundLayer);
        Debug.Log(ground);

        if(ground == false)
        {
            speed *= -1;
        }

        if(speed > 0 && !facingRight)
        {
            Flip();
        }
        else if(speed < 0 && facingRight)
        {
            Flip();
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
