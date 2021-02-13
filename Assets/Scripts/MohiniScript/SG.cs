using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG : MonoBehaviour
{
    public static SG Instance { get; private set; }

    // Number of bullets fired on each click
    public int pelletCount;

    // The bullets
    public GameObject pellet;

    // Velocity of each bullet
    public float pelletFireVel = 500;
    
    // The bullets exit from here
    public Transform BarrelExit;

    List<Quaternion> pellets;

    // The delay in seconds from time of mouse click to firing
    public float timeDelayInSeconds;


    // Size of circular spread of bullets
    public float coneSize;

    //-----------------------------------------------
    // Object Pool stuff commented out - officially added 4/27/20

    // // The kind of gameObject we want to pool (in this case: pellet)
    // public GameObject objectToPool;

    // // Number of pellet gameObjects in the pool
    // public int amountToPool;

    //-----------------------------------------------

    // Audio Clips added week of 4/15/20
    public AudioClip fireSound;
    private AudioSource playerAudio;
    //-----------------------------------------------
    // Braydon added on 4/27/20 to allow us to hold down button with a nice stream of rapid fire shots
    private float timeDelta;
    private float timeDelta2;
    public float timeBetweenShots = 0.02f;
    public float minTimeBetweenBulletSounds = 0.05f;


    // Variables needed to track and change current paint color
    public Material redGunMaterial;
    public Material blueGunMaterial;
    public Material greenGunMaterial;
    public Material plainGunMaterial;

    // Enumerator type to keep track of current ammo color.
    private ColorEnum gunColorEnum;

    public bool changeShaftMaterial = true;
    public bool changeHandleMaterial = false;


    // Initialize the number of bullets before game starts
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerAudio = GetComponent<AudioSource>();
        pellets = new List<Quaternion>(pelletCount);
        //pooledObjects = new List<GameObject>(amountToPool);

        // Initalize the pellets
        for (int i = 0; i < pelletCount; i++) {
          pellets.Add(Quaternion.Euler(Vector3.zero));
          
          //float xSpread = Random.Range(-1,1);
          //float ySpread = Random.Range(-1,1);
          //float zSpread = Random.Range(-1, 1);
          //normalize the spread vector to keep it conical
          //Vector3 spread = new Vector3(xSpread, ySpread, zSpread).normalized * coneSize;
          //Quaternion rotation = Quaternion.Euler(spread) * BarrelExit.rotation;
          //GameObject obj = (GameObject)Instantiate(objectToPool, BarrelExit.position, rotation);
          //obj.SetActive(false);
          //pooledObjects.Add(obj);
        }

        // Set the gun and ammo to be initially red
        updateGunColorEnum(ColorEnum.Red);
    }

    public ColorEnum getGunColorEnum() {
        return gunColorEnum;
    }

    public void updateGunColorEnum(ColorEnum colorEnum) {
        // Update the gun's private color enum
        gunColorEnum = colorEnum;

        Material handleMaterial = plainGunMaterial;
        Material shaftMaterial = plainGunMaterial;

        // Update the handle/shaft color of gun as specified by public booleans.
        if (gunColorEnum == ColorEnum.Red)
        {
            if (changeShaftMaterial) { shaftMaterial = redGunMaterial; }
            if (changeHandleMaterial) { handleMaterial = redGunMaterial; }
        }
        else if (gunColorEnum == ColorEnum.Green)
        {
            if (changeShaftMaterial) { shaftMaterial = greenGunMaterial; }
            if (changeHandleMaterial) { handleMaterial = greenGunMaterial; }
        }
        else if (gunColorEnum == ColorEnum.Blue)
        {
            if (changeShaftMaterial) { shaftMaterial = blueGunMaterial; }
            if (changeHandleMaterial) { handleMaterial = blueGunMaterial; }
        }
        else
        {
            Debug.Log("Unknown ColorEnum");
        }

        // allows each gun piece to match the material that corresponds to the correct color
        this.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material = handleMaterial;  // Front handle
        this.transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().material = shaftMaterial;  // Shaft
        this.transform.GetChild(1).GetChild(2).GetComponent<MeshRenderer>().material = handleMaterial;  // Back handle
    }


    void Update()
    {
        timeDelta += Time.deltaTime;
        timeDelta2 += Time.deltaTime;
        // Mouse Left click to fire pellets
        if(Input.GetButton("Fire1") && timeDelta > timeBetweenShots && Time.deltaTime != 0) {

            if (timeDelta2 > minTimeBetweenBulletSounds) {
                playerAudio.PlayOneShot(fireSound, 1.0f);
                timeDelta2 = 0;
            }
            // Shoot bullets "timeDelayInSeconds" seconds after pressing the Left Key
            StartCoroutine(ShootAfterTime(timeDelayInSeconds));
            timeDelta = 0;
        }

        // Let user change color of gun/ammo with keypress (if not paused)
        if (Time.deltaTime != 0) {
            // If Q is pressed, go to previous color. If E is pressed, go to next color.
            // Cycle through RGB, update the material and enum via setGunColorEnum
            if (Input.GetKeyDown(KeyCode.Q))
            {
                // Going back one color equivalent to going forward 2 colors
                ColorEnum newGunColorEnum = (ColorEnum)(((int)gunColorEnum + 2) % 3);
                updateGunColorEnum(newGunColorEnum);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                ColorEnum newGunColorEnum = (ColorEnum)(((int)gunColorEnum + 1) % 3);
                updateGunColorEnum(newGunColorEnum);
            }

            // If number 1, 2, 3 pressed, change color to that (minus 1)
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                ColorEnum newGunColorEnum = (ColorEnum)(1 - 1);
                updateGunColorEnum(newGunColorEnum);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                ColorEnum newGunColorEnum = (ColorEnum)(2 - 1);
                updateGunColorEnum(newGunColorEnum);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                ColorEnum newGunColorEnum = (ColorEnum)(3 - 1);
                updateGunColorEnum(newGunColorEnum);
            }
        }
        

    }

    IEnumerator ShootAfterTime(float time) {

        // Wait for some time
        yield return new WaitForSeconds(time);

        // Fire bullets in random direction given by spreadAngle
        for(int i = 0; i < pelletCount; i++) {

            // The bullet spread should have a conical shape
            float xSpread = Random.Range(-1.0f, 1.0f);
            float ySpread = Random.Range(-1.0f, 1.0f);
            float zSpread = Random.Range(-1.0f, 1.0f);

            // normalize the spread vector to keep it conical
            Vector3 spread = new Vector3(xSpread, ySpread, zSpread).normalized * coneSize;
            // Debug.Log("Spread:  " + spread.x + ", " + spread.y + ", " + spread.z);
            Quaternion rotation = Quaternion.Euler(spread) * BarrelExit.rotation;
            // Debug.Log("Rotation: " + rotation);


            // Instantiate the pellet prefab
            GameObject p = Instantiate(pellet, BarrelExit.position, rotation);
            //GameObject p = GetPooledObject();
            //if (p != null) {
            //  p.SetActive(true);
            //  p.GetComponent<Rigidbody>().AddForce(p.transform.forward * pelletFireVel);
            //}
            // Update the rotation of the pellet
            //p.transform.rotation = Quaternion.RotateTowards(p.transform.rotation, pellets[i], spreadAngle);
            
            
            // Fire pellet with velocity pelletFireVel (changed from 'right' to 'forward')
            p.GetComponent<Rigidbody>().AddForce(p.transform.right * pelletFireVel);
            Destroy(p, 1.0f); // temporary way to delete pellets

            // Ask pellet to change it's color
            p.GetComponent<PelletController>().updatePelletColorEnum(gunColorEnum);
        }
    }

    // public GameObject GetPooledObject() {
    //   for (int i = 0; i < pooledObjects.Count; i++) {
    //     if (!pooledObjects[i].activeInHierarchy) {
    //         return pooledObjects[i];
    //     }
    //   }

    //   return null;
    // }
}


// possible water spray particle effect??
//https://forum.unity.com/threads/water-gun-water-stream.194098/