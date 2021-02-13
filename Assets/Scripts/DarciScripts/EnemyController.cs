using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent enemy;
    public GameObject bar;
    public GameObject bar_1;
    public GameObject bar_2;


    private HealthSystem healthSystem;
    private HealthSystem healthSystem_1;

    private HealthSystem healthSystem_2;

    private GameObject player;
    
    // Omar Additions (4/27/20)
    public GameObject damagedBarTemplate;
    public GameObject damagedBarTemplate_1;
    public GameObject damagedBarTemplate_2;

    public GameObject HealthBar;
    public GameObject HealthBar_1;
    public GameObject HealthBar_2;

    public AudioClip hitmarkerSound;
    private AudioSource playerAudio;
    private float fullHealthBarXPosition = 5f; //the x position of the right side of the health bar (used for the animation effect)
    private float fullHealthBarXPosition_1 = 5f; //the x position of the right side of the health bar (used for the animation effect)
    private float fullHealthBarXPosition_2 = 5f; //the x position of the right side of the health bar (used for the animation effect)

    private SpriteRenderer SR_Bar; //for changing the color of the health bar
    private SpriteRenderer SR_Bar_1; //for changing the color of the health bar
    private SpriteRenderer SR_Bar_2; //for changing the color of the health bar
    private int BigEnemyDamageAmount = 45; //only need to adjust these values
    private int BaseEnemyDamageAmount = 25; //only need to adjust these values
    private float enemyHealthMax; 

    private int SmallEnemyDamageAmount = 10; //only need to adjust these values
    private int damagePoints = 10; //only need to adjust these values
    private int killPoints = 150; //only need to adjust these values

    // Darci/Braydon Additions (4/25/20)  
    private string enemyTag;
    private float enemySpeed;
    private float enemyAcceleration;
    private float enemyAngular;


    private bool isDead = false;

    // Darci (4/28)
    private Color redHealthbarColor;
    private Color darkRedHealthbarColor;
    private Color greenHealthbarColor;
    private Color darkGreenHealthbarColor;

    private Color blueHealthbarColor;
    private Color darkBlueHealthbarColor;


    private ColorEnum enemyColorEnum;

    private void Awake() 
    {
        playerAudio = GetComponent<AudioSource>();
        SR_Bar = bar.transform.GetComponentInChildren<SpriteRenderer>();
        SR_Bar_1 = bar_1.transform.GetComponentInChildren<SpriteRenderer>();
        SR_Bar_2 = bar_2.transform.GetComponentInChildren<SpriteRenderer>();


        redHealthbarColor = new Color(1.0f, 0, 0, 1.0f);
        darkRedHealthbarColor = new Color(0.3f, 0.1f, 0.1f, 1.0f);
        greenHealthbarColor = new Color(0, 1.0f, 0, 1.0f);
        darkGreenHealthbarColor = new Color(0.1f, 0.3f, 0.1f, 1.0f);
        blueHealthbarColor = new Color(0, 0, 1.0f, 1.0f);
        darkBlueHealthbarColor = new Color(0.1f, 0.1f, 0.3f, 1.0f);
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // setting up healthsystem for each enemy type based on speed
        enemySpeed = GetComponent<NavMeshAgent>().speed;
        enemyAcceleration = GetComponent<NavMeshAgent>().acceleration;
        enemyAngular = GetComponent<NavMeshAgent>().angularSpeed;
        if (WaveController.Instance.getWaveNum() != 10) {
            if (enemySpeed < 3) {
                healthSystem = new HealthSystem(2600); // Big enemy health
                enemyHealthMax = 2600; 
            } else if (enemySpeed > 3) {
                healthSystem = new HealthSystem(350); // Little enemy health
                enemyHealthMax = 350; 
            } else {
                healthSystem = new HealthSystem(1100); // Base enemy health
                enemyHealthMax = 1100; 
            }
            bar.transform.localScale = new Vector3(healthSystem.getHealthPercent(), 1);
        } else {
            healthSystem = new HealthSystem(10000); // Boss enemy health red
            healthSystem_1 = new HealthSystem(10000); // Boss enemy health green
            healthSystem_2 = new HealthSystem(10000); // Boss enemy health blue
            enemyHealthMax = 30000;
            bar.transform.localScale = new Vector3(healthSystem.getHealthPercent(), 1);
            bar_1.transform.localScale = new Vector3(healthSystem_1.getHealthPercent(), 1);
            bar_2.transform.localScale = new Vector3(healthSystem_2.getHealthPercent(), 1);
        }
    }


    // Update is called once per frame
    void Update()
    {
        // update health bar
        bar.transform.localScale = new Vector3(healthSystem.getHealthPercent(), 1);
        if (WaveController.Instance.getWaveNum() == 10) {
            bar_1.transform.localScale = new Vector3(healthSystem_1.getHealthPercent(), 1);
            bar_2.transform.localScale = new Vector3(healthSystem_2.getHealthPercent(), 1);
        }
        if (enemy != null && player != null) enemy.SetDestination(player.transform.position);

        if (Vector3.Distance(player.transform.position, this.transform.position) > 28.0f) {
            GetComponent<NavMeshAgent>().speed = 12;
            GetComponent<NavMeshAgent>().acceleration = 20;
            GetComponent<NavMeshAgent>().angularSpeed = 500;
        } else if (healthSystem.getHealth() <= 1500 && enemyHealthMax == 2500) {
            GetComponent<NavMeshAgent>().speed = 5;
            GetComponent<NavMeshAgent>().acceleration = 20;
            GetComponent<NavMeshAgent>().angularSpeed = 200;
        } else {
            GetComponent<NavMeshAgent>().speed = enemySpeed;
            GetComponent<NavMeshAgent>().acceleration = enemyAcceleration;
            GetComponent<NavMeshAgent>().angularSpeed = enemyAngular;
        }
        // if (PlayerMovement.takingDamage == false) {
        //     PlayerMovement.playerAudio.Stop();
        //     Color c = PlayerMovement.damageImage.color;
        //     c.a = 0;
        //     PlayerMovement.damageImage.color = c;
        // }
    }

    public void updateEnemyColorEnum(ColorEnum colorEnum) {
        // Update the enemy's private color enum
        enemyColorEnum = colorEnum;

        // Update the color of the health bar
        SpriteRenderer healthBarSprite = this.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<SpriteRenderer>();
        SpriteRenderer healthBarSprite_1;
        SpriteRenderer healthBarSprite_2;
        if (WaveController.Instance.getWaveNum() == 10) {
            healthBarSprite_1 = this.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<SpriteRenderer>();
            healthBarSprite_2 = this.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<SpriteRenderer>();
            healthBarSprite.color = redHealthbarColor;
            healthBarSprite_1.color = greenHealthbarColor;
            healthBarSprite_2.color = blueHealthbarColor;
        } else {
            if (enemyColorEnum == ColorEnum.Red)
            {
                // Debug.Log("Changing healthbar color" +  colorEnum);
                healthBarSprite.color = redHealthbarColor;
            }
            else if (enemyColorEnum == ColorEnum.Green)
            {
                // Debug.Log("Changing healthbar color" + colorEnum);
                healthBarSprite.color = greenHealthbarColor;
            }
            else if (enemyColorEnum == ColorEnum.Blue)
            {
                // Debug.Log("Changing healthbar color" + colorEnum);
                healthBarSprite.color = blueHealthbarColor;
            }
            else
            {
                Debug.Log("Unknown ColorEnum");
            }
        }
    }

    // public ColorEnum getEnemyColorEnum(ColorEnum colorEnum) {
    //     return enemyColorEnum;
    // }

    //Trigger for Enemy Getting Hit By Bullets 
    private void OnTriggerEnter(Collider other)
    {
        // PlayerMovement.timeCollision = 0.0f;

        SpriteRenderer healthBarSprite = this.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<SpriteRenderer>();
        if(other.transform.tag == "pelletBullet" && other != null)
        {
            // Check that pellet is the correct color. Can ignore if not.
            ColorEnum pelletColorEnum = other.gameObject.GetComponent<PelletController>().getPelletColorEnum();
            // Debug.Log("Pellet color is number: " + (int) pelletColorEnum);
            if (enemyColorEnum == pelletColorEnum && WaveController.Instance.getWaveNum() != 10)
            {
                healthSystem.takeDamage(BaseEnemyDamageAmount);
                PointSystem.Instance.addPoints(damagePoints);
                if (healthSystem.getHealth() <= 1500 && enemySpeed < 3 && WaveController.Instance.getWaveNum() != 10) {
                    // change speed and color for big enemy when health drops to far
                    GetComponent<NavMeshAgent>().speed = 5;
                    GetComponent<NavMeshAgent>().acceleration = 20;
                    GetComponent<NavMeshAgent>().angularSpeed = 200;
                    if (enemyColorEnum == ColorEnum.Red) healthBarSprite.color = darkRedHealthbarColor;
                    else if (enemyColorEnum == ColorEnum.Green) healthBarSprite.color = darkGreenHealthbarColor;
                    else if (enemyColorEnum == ColorEnum.Blue) healthBarSprite.color = darkBlueHealthbarColor;
                    else Debug.Log("Unknown ColorEnum");
                }
                if (healthSystem.getHealth() <= 0)
                {
                    // Debug.Log("Agent ID:  " + GetComponent<NavMeshAgent>().agentTypeID);
                    // Prevents adding too many points if two bullets entered on same frame at time of death.
                    if (!isDead)
                    {
                        if (PlayerMovement.count > 0)
                        {
                            PlayerMovement.c.a = 3;
                            PlayerMovement.count -= 2;
                            if (PlayerMovement.count < 0)
                            {
                                PlayerMovement.count = 0; 
                            }
                        }
                        Destroy(this.gameObject);
                        isDead = true;
                        PointSystem.Instance.addPoints(killPoints);
                        //PlayerMovement.takingDamage = false; 
                    }
                }
                else
                {
                    GameObject damagedBar = Instantiate(damagedBarTemplate, HealthBar.transform);
                    damagedBar.gameObject.SetActive(true);
                    damagedBar.transform.rotation = Camera.main.transform.rotation;
                    fullHealthBarXPosition -= ((BaseEnemyDamageAmount / enemyHealthMax) * 10f); //10f is due to the size of the health bar (goes from transform positon -5 ot 5)
                    damagedBar.transform.localPosition = new Vector3(fullHealthBarXPosition, 0, 0);
                    damagedBar.transform.localScale = new Vector3(BaseEnemyDamageAmount / enemyHealthMax, 1, 1);
                    damagedBar.gameObject.AddComponent<HealthBarDamageAnimation>();
                    playerAudio.PlayOneShot(hitmarkerSound, 1.0f);
                }

            // ******* Special 10th wave boss conditions ********
            } else if (WaveController.Instance.getWaveNum() == 10) {
                if ((int) pelletColorEnum == 0 && healthSystem.getHealth() > 0) { //red
                    healthSystem.takeDamage(BaseEnemyDamageAmount);
                    PointSystem.Instance.addPoints(damagePoints);
                } else if ((int) pelletColorEnum == 1 && healthSystem_1.getHealth() > 0) { //green
                    healthSystem_1.takeDamage(BaseEnemyDamageAmount);
                    PointSystem.Instance.addPoints(damagePoints);
                } else if ((int) pelletColorEnum == 2 && healthSystem_2.getHealth() > 0) { //blue
                    healthSystem_2.takeDamage(BaseEnemyDamageAmount);
                    PointSystem.Instance.addPoints(damagePoints);
                }
                Debug.Log("health1: " + healthSystem.getHealth());
                Debug.Log("health2: " + healthSystem_1.getHealth());
                Debug.Log("health3: " + healthSystem_2.getHealth());
                if (healthSystem.getHealth() <= 0 && healthSystem_1.getHealth() <= 0 && healthSystem_2.getHealth() <= 0) {
                    if (!isDead) {
                        if (PlayerMovement.count > 0)
                        {
                            PlayerMovement.c.a = 3;
                            PlayerMovement.count -= 2;
                            if (PlayerMovement.count < 0)
                            {
                                PlayerMovement.count = 0; 
                            }
                        }
                        Destroy(this.gameObject);
                        isDead = true;
                        PointSystem.Instance.addPoints(killPoints);
                        //PlayerMovement.takingDamage = false;
                    }
                } else {
                    if ((int) pelletColorEnum == 0 && healthSystem.getHealth() > 0) {
                        GameObject damagedBar = Instantiate(damagedBarTemplate, HealthBar.transform);
                        damagedBar.gameObject.SetActive(true);
                        damagedBar.transform.rotation = Camera.main.transform.rotation;
                        fullHealthBarXPosition -= ((BaseEnemyDamageAmount / (enemyHealthMax / 3)) * 10f); //10f is due to the size of the health bar (goes from transform positon -5 ot 5)
                        damagedBar.transform.localPosition = new Vector3(fullHealthBarXPosition, 0, 0);
                        damagedBar.transform.localScale = new Vector3(BaseEnemyDamageAmount / enemyHealthMax, 1, 1);
                        damagedBar.gameObject.AddComponent<HealthBarDamageAnimation>();
                        playerAudio.PlayOneShot(hitmarkerSound, 1.0f); 
                    }
                    if ((int) pelletColorEnum == 1 && healthSystem_1.getHealth() > 0) {
                        GameObject damagedBar = Instantiate(damagedBarTemplate, HealthBar_1.transform);
                        damagedBar.gameObject.SetActive(true);
                        damagedBar.transform.rotation = Camera.main.transform.rotation;
                        fullHealthBarXPosition_1 -= ((BaseEnemyDamageAmount / (enemyHealthMax / 3)) * 10f); //10f is due to the size of the health bar (goes from transform positon -5 ot 5)
                        damagedBar.transform.localPosition = new Vector3(fullHealthBarXPosition_1, 0, 0);
                        damagedBar.transform.localScale = new Vector3(BaseEnemyDamageAmount / (enemyHealthMax / 3), 1, 1);
                        damagedBar.gameObject.AddComponent<HealthBarDamageAnimation>();
                        playerAudio.PlayOneShot(hitmarkerSound, 1.0f); 
                    }
                    if ((int) pelletColorEnum == 2 && healthSystem_2.getHealth() > 0) {
                        GameObject damagedBar = Instantiate(damagedBarTemplate, HealthBar_2.transform);
                        damagedBar.gameObject.SetActive(true);
                        damagedBar.transform.rotation = Camera.main.transform.rotation;
                        fullHealthBarXPosition_2 -= ((BaseEnemyDamageAmount / (enemyHealthMax / 3)) * 10f); //10f is due to the size of the health bar (goes from transform positon -5 ot 5)
                        damagedBar.transform.localPosition = new Vector3(fullHealthBarXPosition_2, 0, 0);
                        damagedBar.transform.localScale = new Vector3(BaseEnemyDamageAmount / (enemyHealthMax / 3), 1, 1);
                        damagedBar.gameObject.AddComponent<HealthBarDamageAnimation>();
                        playerAudio.PlayOneShot(hitmarkerSound, 1.0f); 
                    }
                }
            }
        }
    }
}
