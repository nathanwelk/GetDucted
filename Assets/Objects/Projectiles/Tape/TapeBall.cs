using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeBall : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;
    private Rigidbody2D rb;

    [SerializeField]
    private float slowdownEffect = 0.2f;

    [SerializeField]
    private float maxLifetime = 2f;

    private float birthtime;

    public GameObject player;
    public Vector2 direction = Vector2.zero;

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * speed, ForceMode2D.Impulse);
        birthtime = Time.time;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Robot")) {
            collision.gameObject.GetComponent<RobotBrain>().AddSlow(slowdownEffect);
            Destroy(gameObject);    
        }
        
    }


    void Update(){
        if(Time.time - birthtime >= maxLifetime) {
            Destroy(gameObject);
        }
    }
}
