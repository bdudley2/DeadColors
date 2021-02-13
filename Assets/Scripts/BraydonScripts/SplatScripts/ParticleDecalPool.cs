// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class ParticleDecalData {
//     public Vector3 pos;
//     public float size;
//     public Vector3 rot;
//     public Color color;
// }


// public class ParticleDecalPool : MonoBehaviour
// {
//     public int maxDecals = 100;
//     public float decalSizeMin = .5f;
//     public float decalSizeMax = 1.5f;

//     public ParticleSystem decalParticleSystem;
//     private int pDecalDataIndex;
//     private ParticleDecalData[] particleData; //Decal Array
//     private ParticleSystem.Particle[] particles; //Decal-As-Particle Array

//     // Start is called before the first frame update
//     void Start()
//     {
//         decalParticleSystem = GetComponent<ParticleSystem>();
//         particleData = new ParticleDecalData[maxDecals];
//         for (int i = 0; i < maxDecals; i++) {
//             particleData[i] = new ParticleDecalData();
//         }
//         particles = new ParticleSystem.Particle[maxDecals];
//     }

//     public void ParticleHit(ParticleCollisionEvent pce, Gradient colorGradient) {
//         SetParticleData(pce, colorGradient);
//         DisplayParticles();
//     }

//     void SetParticleData(ParticleCollisionEvent pce, Gradient colorGradient) {
//         pDecalDataIndex = pDecalDataIndex % maxDecals;

//         // store position, rotation, size, and color data from collision event
//         particleData[pDecalDataIndex].pos = pce.intersection;
//         Vector3 pRotationEuler = Quaternion.LookRotation(pce.normal).eulerAngles;
//         pRotationEuler.z = Random.Range(0.0f, 360.0f);
//         particleData[pDecalDataIndex].rot = pRotationEuler;
//         particleData[pDecalDataIndex].size = Random.Range(decalSizeMin, decalSizeMax);
//         particleData[pDecalDataIndex].color = colorGradient.Evaluate(Random.Range(0.0f, 1.0f));

//         pDecalDataIndex++;
//     }

//     void DisplayParticles() {
//         for (int i = 0; i < particleData.Length; i++) {
//             particles[i].position = particleData[i].pos;
//             particles[i].rotation3D = particleData[i].rot;
//             particles[i].startSize = particleData[i].size;
//             particles[i].startColor = particleData[i].color;
//         }
//         decalParticleSystem.SetParticles(particles, particles.Length);
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDecalData 
{
    public Vector3 position;
    public float size;
    public Vector3 rotation;
    public Color color;

}

public class ParticleDecalPool : MonoBehaviour {

    public int maxDecals = 100;
    public float decalSizeMin = .5f;
    public float decalSizeMax = 1.5f;

    private ParticleSystem decalParticleSystem;
    private int particleDecalDataIndex;
    private ParticleDecalData[] particleData;
    private ParticleSystem.Particle[] particles;

    void Start () 
    {
        decalParticleSystem = GetComponent<ParticleSystem> ();
        particles = new ParticleSystem.Particle[maxDecals];
        particleData = new ParticleDecalData[maxDecals];
        for (int i = 0; i < maxDecals; i++) 
        {
            particleData [i] = new ParticleDecalData ();    
        }
    }

    public void ParticleHit(ParticleCollisionEvent particleCollisionEvent, Gradient colorGradient)
    {
        SetParticleData (particleCollisionEvent, colorGradient);
        DisplayParticles ();
    }

    void SetParticleData(ParticleCollisionEvent particleCollisionEvent, Gradient colorGradient)
    {
        if (particleDecalDataIndex >= maxDecals) 
        {
            particleDecalDataIndex = 0;
        }

        particleData [particleDecalDataIndex].position = particleCollisionEvent.intersection;
        Vector3 particleRotationEuler = Quaternion.LookRotation (particleCollisionEvent.normal).eulerAngles;
        particleRotationEuler.z = Random.Range (0, 360);
        particleData [particleDecalDataIndex].rotation = particleRotationEuler;
        particleData [particleDecalDataIndex].size = Random.Range (decalSizeMin, decalSizeMax);
        particleData [particleDecalDataIndex].color = colorGradient.Evaluate (Random.Range (0f, 1f));

        particleDecalDataIndex++;
    }

    void DisplayParticles()
    {
        for (int i = 0; i < particleData.Length; i++) 
        {
            particles [i].position = particleData [i].position;
            particles [i].rotation3D = particleData [i].rotation;
            particles [i].startSize = particleData [i].size;
            particles [i].startColor = particleData [i].color;
        }

        decalParticleSystem.SetParticles (particles, particles.Length);
    }
}