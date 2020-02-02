using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeTether_Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;
    private Rigidbody2D rb;

    private LineRenderer lr;

    public GameObject player;
    public Vector2 direction = Vector2.zero;

    private GameObject barrel;

    private Collision2D tetherCollision;
    private float tetherMaxLength = 0f;

    [SerializeField]
    private float tetherPullStrength = 10f;

    [SerializeField]
    private float maxLife = 1f;

    private float birthtime;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");

        foreach(Transform t in player.GetComponentInChildren<Transform>()) {
            if (t.CompareTag("barrel"))
                barrel = t.gameObject;
        }

        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * speed, ForceMode2D.Impulse);

        lr = GetComponent<LineRenderer>();

        birthtime = Time.time;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (tetherCollision != null)
            return;

        IsTetherAble tetherInfo = collision.gameObject.GetComponent<IsTetherAble>();

        if (tetherInfo == null)
            return;

        tetherCollision = collision;
        tetherMaxLength = Vector3.Distance(player.transform.position, tetherCollision.contacts[0].point);

        transform.position = tetherCollision.contacts[0].point;

        transform.SetParent(collision.gameObject.transform);
        Destroy(rb);
        Destroy(GetComponent<SpriteRenderer>());
        Destroy(GetComponent<Collider2D>());
        
    }

    // Update is called once per frame
    void Update(){
        
        lr.SetPositions(new Vector3[2] { transform.position, barrel.transform.position });

        if (tetherCollision != null) {
            if (Vector3.Distance(player.transform.position, transform.position) > tetherMaxLength) {
                Debug.Log("Pulling");

                float scaledTetherStrength = tetherPullStrength * Mathf.Abs(tetherMaxLength - Vector3.Distance(player.transform.position, transform.position)) / tetherMaxLength + 1;
                scaledTetherStrength = Mathf.Clamp(scaledTetherStrength, 0, 8000);

                if (tetherCollision.gameObject.GetComponent<Rigidbody2D>()) {
                    tetherCollision.gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(
                        ( player.transform.position - transform.position ).normalized * Mathf.Pow(2, scaledTetherStrength) * Time.deltaTime, transform.position);
                }

            }
            
        }else if (Time.time - birthtime >= maxLife) { //Destroy self if alive too long
            Destroy(gameObject);
        }

    }
}
