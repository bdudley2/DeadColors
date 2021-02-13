using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Can cast red = 0, green = 1, blue = 2
public enum ColorEnum { Red, Green, Blue }

// public class ParticleDecalData {
//     public Vector3 pos;
//     public float size;
//     public Vector3 rot;
//     public Color color;
// }


public class PelletController : MonoBehaviour
{

    public Material redPelletMaterial;
    public Material bluePelletMaterial;
    public Material greenPelletMaterial;

    private ColorEnum pelletColorEnum = ColorEnum.Red;

    public GameObject splatObject;
    // public Material splatMat;
    // public ParticleSystem.Particle splatDecal;

    // public ParticleSystem decalParticleSystem;
    // private int pDecalDataIndex;
    // private ParticleDecalData[] particleData; //Decal Array
    // private ParticleSystem.Particle[] particles; //Decal-As-Particle Array

    // Start is called before the first frame update
    void Awake()
    {
    }

    void Start() {
        // splatObject = new GameObject("splat(Clone)");
        // splatObject.AddComponent<Renderer>();
        // if (splatObject.GetComponent<Renderer>() != null) splatObject.GetComponent<Renderer>().material = splatMat;

        // splatObject.AddComponent<ParticleSystem>();
        // splatObject.GetComponent<ParticleSystem>().Particle = splatDecal;
    }

    // void Start()
    // {
    //     decalParticleSystem = GetComponent<ParticleSystem>();
    //     particleData = new ParticleDecalData[maxDecals];
    //     for (int i = 0; i < maxDecals; i++) {
    //         particleData[i] = new ParticleDecalData();
    //     }
    //     particles = new ParticleSystem.Particle[maxDecals];
    // }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updatePelletColorEnum(ColorEnum colorEnum)
    {
        // Debug.Log("pellet asked to change color: " + colorEnum);
        // Update the private color variable
        pelletColorEnum = colorEnum;

        // Update the pellet material
        if (pelletColorEnum == ColorEnum.Red)
        {
            // Debug.Log("setting to " + pelletColorEnum);
            this.GetComponent<MeshRenderer>().material = redPelletMaterial;
            // Material m_Material = GetComponent<Renderer>().material;
            // m_Material.SetColor("_TintColor", Color.red);
            // splatObject.GetComponent<Renderer>().material = m_Material;
            // splatObject.GetComponent<Renderer>().material.SetColor("_TintColor", Color.red);
        }
        else if (pelletColorEnum == ColorEnum.Green)
        {
            // Debug.Log("setting to " + pelletColorEnum);
            this.GetComponent<MeshRenderer>().material = greenPelletMaterial;
            // Material m_Material = GetComponent<Renderer>().material;
            // m_Material.color = Color.green;
            // splatObject.GetComponent<Renderer>().material.color = Color.green;
        }
        else if (pelletColorEnum == ColorEnum.Blue)
        {
            // Debug.Log("setting to " + pelletColorEnum);
            this.GetComponent<MeshRenderer>().material = bluePelletMaterial;
            // Material m_Material = GetComponent<Renderer>().material;
            // m_Material.color = Color.blue;
            // splatObject.GetComponent<Renderer>().material.color = Color.blue;
        }
        else
        {
            Debug.Log("Unknown ColorEnum");
        }
    }

    public void updateSplatColor(GameObject go) {
        if (pelletColorEnum == ColorEnum.Red)
        {
            go.GetComponent<Renderer>().material.SetColor("_TintColor", Color.red);
        }
        else if (pelletColorEnum == ColorEnum.Green)
        {
            go.GetComponent<Renderer>().material.SetColor("_TintColor", Color.green);
        }
        else if (pelletColorEnum == ColorEnum.Blue)
        {
            go.GetComponent<Renderer>().material.SetColor("_TintColor", Color.blue);
        }
        else
        {
            Debug.Log("Unknown ColorEnum");
        }
    }

    public ColorEnum getPelletColorEnum()
    {
        return pelletColorEnum;
    }

    //Destroys bullet on collision and adds decal to scene
    void OnCollisionEnter(Collision other) {
        // if (other.transform.name != "pellet(Clone)") {
        if (other.collider.gameObject.layer == 10) {
            Debug.Log("Collision with: " + other.transform.name);
            GetComponent<MeshRenderer>().enabled = false;
            var norm = other.contacts[0].point - transform.position;
            var q = Quaternion.FromToRotation(Vector3.up, norm);
            var go = Instantiate(splatObject, transform.position, q);
            updateSplatColor(go);
            // splatObject.transform.rotation = Quaternion.Euler(other.contacts[0].normal);
            Debug.Log("Rotation is at: " + other.contacts[0].normal.x + ", " + other.contacts[0].normal.y + ", " + other.contacts[0].normal.z);
            Debug.Log("Splat Rotation is at: " + splatObject.transform.rotation.x + ", " + splatObject.transform.rotation.y + ", " + splatObject.transform.rotation.z);
            Destroy(this.gameObject);
        }
    }

    // void ParticleHit(ParticleCollisionEvent pce, Gradient colorGradient) {
    //     // pDecalDataIndex = pDecalDataIndex % maxDecals;

    //     // store position, rotation, size, and color data from collision event
    //     // particleData[pDecalDataIndex].pos = pce.intersection;
    //     Vector3 pRotationEuler = Quaternion.LookRotation(pce.normal).eulerAngles;
    //     pRotationEuler.z = Random.Range(0.0f, 360.0f);
    //     particleData[pDecalDataIndex].rot = pRotationEuler;
    //     particleData[pDecalDataIndex].size = Random.Range(decalSizeMin, decalSizeMax);
    //     particleData[pDecalDataIndex].color = colorGradient.Evaluate(Random.Range(0.0f, 1.0f));

    //     pDecalDataIndex++;

    //     // Display Particles
    //     for (int i = 0; i < particleData.Length; i++) {
    //         particles[i].position = particleData[i].pos;
    //         particles[i].rotation3D = particleData[i].rot;
    //         particles[i].startSize = particleData[i].size;
    //         particles[i].startColor = particleData[i].color;
    //     }
    //     decalParticleSystem.SetParticles(particles, particles.Length);
    // }
}
