using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RobotBrain : MonoBehaviour {

    [SerializeField]
    private float timeBeforeSpeedRegain = 2f;

    [SerializeField]
    private float speedRegen = 1f;

    /// <summary>
    /// 0 -> 1. 1 being 100% movement reduction 0 being no movement reduction
    /// </summary>
    public float slowdownMod = 0f;

    private float oldMaxSpeed = 0f;

    private float lastHit = 0f;

    [SerializeField]
    private bool globalSearchRadius = false;

    [SerializeField]
    private float searchRadius = 3f;

    [Header("Tape Animation")]
    [SerializeField]
    private GameObject TapeAnimation;

    [SerializeField]
    private float maxScale = 1.5f;

    [Header("Damage to player")]
    [SerializeField]
    private float damageToPlayer = 15f;

    [SerializeField]
    private float attackTimeDelay = 0.25f;

    [SerializeField] AudioClip walkAudio;
    [SerializeField] AudioSource walkSource;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hitSound;
    [SerializeField] float audioDelay;
    float lastAudioPlay;
    Vector3 lastLocation;

    private float timeSinceLastAttack = 0f;

    private AIPath pathing;

    private GameObject player;

    private bool canAttack = true;

    private SpriteRenderer sr;

    AIPath path;

    void Start() {
        if(!(path = gameObject.GetComponent<AIPath>())) 
            Debug.LogError("Failed to find pathing script on this robot");

        sr = TapeAnimation.GetComponent<SpriteRenderer>();
        pathing = GetComponent<AIPath>();

        player = GameObject.Find("Player");

        oldMaxSpeed = path.maxSpeed;
    }

    void Update() {
        slowdownMod = Mathf.Clamp(slowdownMod, 0, 1);
        path.maxSpeed = oldMaxSpeed - (oldMaxSpeed * slowdownMod );

        if(slowdownMod > 0 && Time.time - lastHit >= timeBeforeSpeedRegain) {
            slowdownMod -= speedRegen * Time.deltaTime;
            slowdownMod = Mathf.Clamp(slowdownMod, 0, 1);
        }

        float val =  maxScale * slowdownMod;
        TapeAnimation.transform.localScale = new Vector3(val, val, val);

        if(slowdownMod >= 1) { //If locked down
            canAttack = false;
            Vector3 pos = TapeAnimation.transform.localPosition;
            pos.z = -1;
            TapeAnimation.transform.localPosition = pos;
        } else { //if not locked down
            canAttack = true;
            Vector3 pos = TapeAnimation.transform.localPosition;
            pos.z = 0;
            TapeAnimation.transform.localPosition = pos;
        }

        playAudio();

        CheckForPlayerInRadius();
    }

    void playAudio() {
        if (transform.position == lastLocation) return;

        lastLocation = transform.position;
 
        if (Time.time - lastAudioPlay > audioDelay) {
            walkSource.clip = walkAudio;
            walkSource.Play();
            lastAudioPlay = Time.time;
        }
    }

    private void CheckForPlayerInRadius() {
        if (Vector2.Distance(transform.position, player.transform.position) <= searchRadius || globalSearchRadius) {
            pathing.destination = player.transform.position;
        } else {
            pathing.destination = pathing.gameObject.transform.position;
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        GameObject obj = collision.gameObject;

        if (obj.CompareTag("Player")) {

            GameObject player = collision.gameObject.transform.root.gameObject;
            Debug.Log("Hit a player");
            AttackObject(player);
        }

    }

    private void AttackObject(GameObject obj) {

        PlayerController pc;

        if((pc = obj.GetComponent<PlayerController>()) && canAttack && Time.time - timeSinceLastAttack >= attackTimeDelay) {
            pc.TakeDamage(damageToPlayer);

            if(pc.hp > 0) {

                audioSource.clip = hitSound;
                audioSource.Play();
            }

            timeSinceLastAttack = Time.time;
        }
    }

    public void AddSlow(float amount) {
        slowdownMod += amount;
        lastHit = Time.time;
    }

    public void OnDrawGizmosSelected() {
        if(!globalSearchRadius)
            Gizmos.DrawWireSphere(transform.position, searchRadius);

    }


}
