using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_rocket : MonoBehaviour
{
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

    // Enumerator type to keep track of current ammo color.
    private ColorEnum gunColorEnum;

    // Adding extra rocket variables need for rocket projectile
    private GameObject rocket;
    private Vector3 lastRocketPos;



    // Initialize the number of bullets before game starts
    void Awake()
    {
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

        if (timeBetweenShots < timeDelayInSeconds) {
            timeBetweenShots = timeDelayInSeconds + 0.05f;
        }
    }

    public void updateGunColorEnum(ColorEnum colorEnum) {
        // Update the gun's private color enum
        gunColorEnum = colorEnum;

        // Update the material of the gun
        if (gunColorEnum == ColorEnum.Red)
        {
            this.GetComponent<MeshRenderer>().material = redGunMaterial;
        }
        else if (gunColorEnum == ColorEnum.Green)
        {
            this.GetComponent<MeshRenderer>().material = greenGunMaterial;
        }
        else if (gunColorEnum == ColorEnum.Blue)
        {
            this.GetComponent<MeshRenderer>().material = blueGunMaterial;
        }
        else
        {
            Debug.Log("Unknown ColorEnum");
        }
    }


    void Update()
    {
        timeDelta += Time.deltaTime;
        timeDelta2 += Time.deltaTime;
        // Mouse Left click to fire pellets
        if(Input.GetButton("Fire1") && timeDelta > timeBetweenShots && Time.deltaTime != 0) {
            if (rocket == null) rocket = Instantiate(pellet, BarrelExit.position, BarrelExit.rotation);
            rocket.GetComponent<Rigidbody>().AddForce(rocket.transform.right * 400.0f);
            rocket.GetComponent<Rigidbody>().useGravity = false;
            // Destroy(rocket, timeDelayInSeconds + 0.5f); // temporary way to delete pellets (destroy immediately after explosion happens)

            // Ask pellet to change it's color
            // p.GetComponent<PelletController>().updatePelletColorEnum(gunColorEnum);
            
            // Shoot bullets "timeDelayInSeconds" seconds after pressing the Left Key
            StartCoroutine(ShootAfterTime(timeDelayInSeconds));
            timeDelta = 0;
        }
        if (rocket != null) lastRocketPos = rocket.transform.position;
        if (timeDelta >= timeDelayInSeconds && rocket != null) {
            rocket.SetActive(false); //make invisible when explodes
            Destroy(rocket, 1.0f); // Destroy 1 second later after it has successfully been used
        }
        
        // If Q is pressed, go to previous color. If E is pressed, go to next color.
        // Cycle through RGB, update the material and enum via setGunColorEnum
        if (Input.GetKeyDown(KeyCode.Q)) {
            // Going back one color equivalent to going forward 2 colors
            ColorEnum newGunColorEnum = (ColorEnum)(((int)gunColorEnum + 2) % 3);
            updateGunColorEnum(newGunColorEnum);
        } else if (Input.GetKeyDown(KeyCode.E)) {
            ColorEnum newGunColorEnum = (ColorEnum)(((int)gunColorEnum + 1) % 3);
            updateGunColorEnum(newGunColorEnum);
        }
    }

    IEnumerator ShootAfterTime(float time) {

        // Wait for some time
        yield return new WaitForSeconds(time);

        // explosion noise --> need to change sound here, and add firing rocket sound
        if (timeDelta2 > minTimeBetweenBulletSounds) {
            playerAudio.PlayOneShot(fireSound, 1.0f);
            timeDelta2 = 0;
        }

        // Fire bullets in random direction given by spreadAngle (at Rocket's position)
        for(int i = 0; i < pelletCount; i++) {

            // The bullet spread should have a conical shape
            float xSpread = Random.Range(-180.0f, 180.0f);
            float ySpread = Random.Range(-180.0f, 180.0f);
            float zSpread = Random.Range(-180.0f, 180.0f);

            // normalize the spread vector to keep it conical
            // Vector3 spread = new Vector3(xSpread, ySpread, zSpread).normalized * coneSize;
            // // Debug.Log("Spread:  " + spread.x + ", " + spread.y + ", " + spread.z);
            // Quaternion rotation = Quaternion.Euler(spread);
            // // Debug.Log("Rotation: " + rotation);


            // Instantiate the pellet prefab
            GameObject p = Instantiate(pellet, lastRocketPos, Quaternion.Euler(new Vector3(xSpread, ySpread, zSpread)));


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