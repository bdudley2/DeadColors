using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public bool GodMode = false;

    public float walkSpeed = 4f;
    public float sprintMultiplier = 1.5f;

    public float gravity = -20f;
    public float jumpHeight = 0.6f;
    public Transform groundCheck;
    public float groundDistance = 0.2f;

    bool isGrounded;
    bool isSprinting = false;

    Vector3 velocity;

    //Omar's Additions
    private float healTimer;
    private float damageTimer;
    //public static bool takingDamage = false;
    public static int count = 0; 
    public static GameObject damageGO; //damage Game Object
    public static Image damageImage;
    public static Color c;
    public static AudioSource playerAudio;
    private bool newWave = false; 
    private int waveNumber; 

    // public static float timeCollision = 0.0f;

    void Start()
    {
        HudControl.isGameOver = false;
        healTimer = 0.1f;
        damageTimer = 0.5f;
        if (damageGO == null) {
            damageGO = GameObject.FindWithTag("BloodOnScreen");
        }
        // Instantiate(this, damageGO.transform.position, damageGO.transform.rotation);
        //blood on screen image whose's alpha value is initially 0 (no blood initially)
        damageImage = damageGO.GetComponent<Image>();
        c = damageImage.color;
        c.a = 0;
        damageImage.color = c;
        playerAudio = GetComponent<AudioSource>();
        playerAudio.loop = true; 
        // timeCollision = 0.0f;
        waveNumber = WaveController.Instance.getWaveNum(); 
    }

    // Update is called once per frame
    void Update()
    {
        // timeCollision += Time.deltaTime;

        // Checks if player is on the ground (short raycast down from feet)
        isGrounded = Physics.Raycast(groundCheck.position, -Vector3.up, groundDistance);

        // If on the ground, ~ stop downward movement
        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        // Sprint if key down
        if (Input.GetButtonDown("Sprint") && isGrounded) {
            isSprinting = true;
        } 
        else if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
        }
        float movementSpeed = walkSpeed;
        if (isSprinting) {
            movementSpeed *= sprintMultiplier;
        }

        // Get's degree of WASD, joystick etc. movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Move player according to input
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * movementSpeed * Time.deltaTime);

        // Allow jump if on ground and jump just pressed
        if (Input.GetButtonDown("Jump") && isGrounded && HudControl.gameStarted) {
            // Based on physics formula v = sqrt(2*g*h)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Accelerate player downwards, move according to y += v_y * t
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Updates player's health indicators (blood on screen or not)
        //Debug.Log("count: " + count); 
        if(count > 0){
            // timeCollision = 0.0f;
            c.a += 5;
            if(c.a > 255){
                c.a = 255;
            }
            playerAudio.pitch += 0.01f; 
        }
        else
        {   
            if (playerAudio.pitch != 1){
                playerAudio.pitch -= 0.01f; 
            }
            healTimer -= Time.deltaTime;
            if (healTimer < 0) {
                healTimer = 0.01f;
                PlayerHealthHUD.healthSystemHUD.heal();
                c.a -= (5*Time.deltaTime);
                if(c.a < 0){
                    c.a = 0;
                }
                // Debug.Log(c.a);
                if(waveNumber != WaveController.Instance.getWaveNum())
                {
                    healTimer += 0.02f;
                }
            }
        }

        damageGO.GetComponent<Image>().color = c;
        if(damageImage.color.a == 0 && playerAudio.isPlaying){
            playerAudio.Stop(); 
            playerAudio.pitch = 1f; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "hitByEnemy")
        {
            count++; 
        }
    }

    // This is typically the way damage is dealt to the player though the radius is smaller than the PlayerController.
    private void OnTriggerStay(Collider other)
    {
        if(other.transform.tag == "hitByEnemy")
        {
            // PlayerMovement.timeCollision = 0.0f;
            //takingDamage = true;
            if (!GodMode)
            {
                if(!playerAudio.isPlaying){
                    playerAudio.Play();
                }
                // FIX
                // FIX
                // FIX - use localScale to pick enemy types rather than by speed...
                float enemySpeed = other.GetComponent<NavMeshAgent>().speed;
                if (enemySpeed < 3 || enemySpeed == 5) { //Big enemy
                    PlayerHealthHUD.healthSystemHUD.takeDamage(1.0f);
                } else if (enemySpeed == 3) { //Base enemy
                    PlayerHealthHUD.healthSystemHUD.takeDamage(0.6f);
                } else { //Little enemy
                    PlayerHealthHUD.healthSystemHUD.takeDamage(0.3f);
                }
                if(PlayerHealthHUD.healthSystemHUD.getHealth() <= 0){
                    HudControl.isGameOver = true;
                    // displayGameOverScreen(); // GAME OVER
                    // GameObject.FindWithTag("GameOverHUD").transform.GetChild(0).gameObject.SetActive(true);
                    // Cursor.lockState = CursorLockMode.None;
                    // Cursor.visible = true;
                    // Time.timeScale = 0;                    
                    // #if UNITY_EDITOR
                    // UnityEditor.EditorApplication.isPlaying = false;
                    // #else
                    // Application.Quit();
                    // #endif
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "hitByEnemy")
        {
            // PlayerMovement.timeCollision = 0.0f;
            // if(takingDamage){
            //     c.a = 3;
            // }
            // takingDamage = false;
            if (count > 0)
            {
                c.a = 3;
                count -= 2;
                if (count < 0)
                {
                    count = 0; 
                }
            }
        }
    }

    // private void displayGameOverScreen() {
    //     GameObject.FindWithTag("ControlHUD").transform.GetChild(0).gameObject.SetActive(false);
    //     GameObject.FindWithTag("PauseHUD").transform.GetChild(0).gameObject.SetActive(false);
    //     GameObject.FindWithTag("MainHUD").transform.GetChild(0).gameObject.SetActive(false);
    //     GameObject.FindWithTag("GameOverHUD").transform.GetChild(0).gameObject.SetActive(true);
    //     PauseMenu.isGameOver = true;
    // }
}
