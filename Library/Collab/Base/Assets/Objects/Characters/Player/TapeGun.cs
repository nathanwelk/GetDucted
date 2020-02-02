using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TapeGun : MonoBehaviour{

    [SerializeField]
    private float ammo = 0f;

    [SerializeField]
    private float maxAmmo = 100f;

    [SerializeField]
    private float ammoCost = 1f;

    [SerializeField]
    private TMP_Text ammoText;

    [SerializeField]
    private GameObject tapeBallPrefab;

    [SerializeField]
    private GameObject tapeBallTether;


    [SerializeField]
    private float tapeBallCooldown = 0.25f;

    private bool interactionsValid = true;

    private TapeTether_Player activeTapeTether_Player;

    /// <summary>
    /// The last time (Time.timeInSeconds) since a shot was fired
    /// </summary>
    private float lastShotTime = 0f;

    void Start(){
        if (ammoText == null || tapeBallPrefab == null)
            Debug.LogError("You need to assign the variables in inspector for this tape gun");
    }

    void Update(){
        if(interactionsValid)
            CheckInput();

        UpdateGUI();
    }

    private void CheckInput() {
        if (Input.GetButton("Fire1")) {
            ShootTapeGlob();
        }else if (Input.GetButton("Fire2")) {
            ShootTapeTetherPlayer();
        }
    }
    private void ShootTapeTetherPlayer() {
        if (!( ammo >= ammoCost*2 ) || Time.time - lastShotTime < tapeBallCooldown) //Not enough ammo or CD bad
            return;

        if(activeTapeTether_Player != null) {

            Destroy(activeTapeTether_Player.gameObject);
            activeTapeTether_Player = null;
            lastShotTime = Time.time+1f; //For CD
            return;
        }

        GameObject tapeball = Instantiate(tapeBallTether);
        tapeball.transform.position = gameObject.transform.position + ( gameObject.transform.right / 2 );
        tapeball.transform.rotation = gameObject.transform.rotation; // Face the way im facing
        tapeball.GetComponent<TapeTether_Player>().direction = gameObject.transform.right;
        activeTapeTether_Player = tapeball.GetComponent<TapeTether_Player>();

        lastShotTime = Time.time;
        ammo -= ammoCost*2;
    }

    private void ShootTapeGlob() {
        if (!( ammo >= ammoCost ) || Time.time - lastShotTime < tapeBallCooldown || activeTapeTether_Player) //Not enough ammo or CD bad
            return;

        GameObject tapeball = Instantiate(tapeBallPrefab);
        tapeball.transform.position = gameObject.transform.position + ( gameObject.transform.right/2 );
        tapeball.transform.rotation = gameObject.transform.rotation; // Face the way im facing
        tapeball.GetComponent<TapeBall>().direction = gameObject.transform.right;

        lastShotTime = Time.time;
        ammo -= ammoCost;
    }

    private void UpdateGUI() {
        ammoText.text = "Ammo: " + ammo + " / " + maxAmmo;
    }

    public void toggleInteractions(bool enabled) {
        interactionsValid = enabled;
    }

    public void AddAmmo(float amt) {
        ammo += amt;
        if (ammo > maxAmmo) {
            ammo = maxAmmo;
        }
    }
}
