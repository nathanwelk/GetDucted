using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(KeyInventory))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    [Range(1,1000)]
    private float maxHp = 100f;

    [System.NonSerialized]
    public float hp;

    [SerializeField]
    [Range(0.1f,10f)]
    private float speed = 1f;

    [SerializeField]
    [Range(1f, 3f)]
    private float sprintSpeed = 1.2f;

    [SerializeField]
    [Range(0.01f, 0.5f)]
    private float rotationSpeed = 0.1f;

    [SerializeField]
    private TMP_Text hpText;

    [SerializeField]
    private SpriteRenderer sprite;

    private float timeDamageTakenAt = -999;

    [SerializeField]
    private float timeToFlashDamage = 0.25f;

    [SerializeField]
    private RespawnScreen respawnScreen;

    AudioSource audioSource;
    [SerializeField] AudioClip runningSound;
    [SerializeField] float audioDelay = 0.1f;
    float lastAudioPlay;

    public bool disableInteraction = false;

    void playAudio() {
        float playerVelocity = gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;

        if(playerVelocity > 0) {
            if(Time.time - lastAudioPlay > audioDelay / playerVelocity) {
                audioSource.clip = runningSound;
                audioSource.Play();
                lastAudioPlay = Time.time;
            }
        }
    }

    void Start() {
        audioSource = gameObject.GetComponent<AudioSource>();

        hp = maxHp;

        if (hpText == null)
            Debug.LogError("Missing text mesh for hp on player");

        if (!respawnScreen) {
            respawnScreen = GameObject.Find("RespawnScreen").GetComponent<RespawnScreen>();
        }
        Debug.Log(respawnScreen.gameObject.name);
        respawnScreen.gameObject.SetActive(false);

        Camera cam = new Camera();
    }

    void Update(){
        if(!disableInteraction) {
            Movement();
            Rotation();
        }

        playAudio();
        CheckHp();
        GuiUpdate();
        CheckDamageAnimation();
    }

    private void CheckDamageAnimation() {
        if(Time.time - timeDamageTakenAt <= timeToFlashDamage) { //Play anim
            float per = (Time.time - timeDamageTakenAt) / timeToFlashDamage;
            sprite.color = new Color(1, per, per);
        } else { //No anim playing
            sprite.color = Color.white;
        }
    }

    public void toggleInteractions(bool disable) {
        disableInteraction = disable;
    }

    private void Rotation() {
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();

        float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0f,0f, 
            Mathf.LerpAngle(transform.rotation.eulerAngles.z, rotZ, rotationSpeed)));
        transform.rotation = rotation;
        
    }

    public void TakeDamage(float dmg) {
        hp -= dmg;
        hp = Mathf.Clamp(hp, 0, maxHp);
        timeDamageTakenAt = Time.time;
        CheckHp();
    }


    private void Movement() {
        float runSpeed = speed;

        if(Input.GetAxis("Sprint") > 0)
            runSpeed *= sprintSpeed;
        

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical) * runSpeed;
        Vector2 velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
        
        velocity = Vector2.Lerp(Vector2.zero, velocity, 0.2f);

        velocity += direction; 

        gameObject.GetComponent<Rigidbody2D>().velocity = velocity;
    }

    private void CheckHp() {
        if(hp <= 0) {
            respawnScreen.gameObject.SetActive(true);
            disableInteraction = true;
            speed = 0;
        }
    }

    private void GuiUpdate() {
        hpText.text = "HP: " + hp + " / " + maxHp;
    }
}
