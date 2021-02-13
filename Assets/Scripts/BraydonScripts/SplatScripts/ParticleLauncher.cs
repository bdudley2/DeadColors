// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class ParticleLauncher : MonoBehaviour
// {
//     public ParticleSystem pLauncher;
//     public ParticleSystem splatterParticles;
//     public Gradient pColorGradient;
//     public ParticleDecalPool splatDecalPool;

//     List<ParticleCollisionEvent> collisionEvents;

//     // Start is called before the first frame update
//     void Start()
//     {
//         particleLauncher = GetComponent<ParticleSystem>();
//         collisionEvents = new List<ParticleCollisionEvent>();
//     }

//     void OnParticleCollision(GameObject other) {
//         ParticlePhysicsExtensions.GetCollisionEvents(pLauncher, other, collisionEvents);
//         for (int i = 0; i < collisionEvents.Count; i++) {
//             splatDecalPool.ParticleHit(collisionEvents[i], pColorGradient);
//             EmitAtLocation(collisionEvents[i]);
//         }
//     }

//     void EmitAtLocation(ParticleCollisionEvent pce) {
//         splatterParticles.transform.position = pce.intersection;
//         splatterParticles.transform.rotation = Quaternion.LookRotation(pce.normal);
//         ParticleSystem.MainModule psMain = splatterParticles.main;
//         psMain.startColor = pColorGradient.Evaluate(Random.Range(0.0f, 1.0f));
//         splatterParticles.Emit(1);
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (Input.GetButton("Fire1")) {
//             ParticleSystem.MainModule psMain = pLauncher.main;
//             psMain.startColor = pColorGradient.Evaluate(Random.Range(0.0f, 1.0f));
//             pLauncher.Emit(1);
//         }
//     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLauncher : MonoBehaviour {

    public ParticleSystem particleLauncher;
    public ParticleSystem splatterParticles;
    public Gradient particleColorGradient;
    public ParticleDecalPool splatDecalPool;

    public GameObject barrel;

    List<ParticleCollisionEvent> collisionEvents;

    void Start () 
    {
        particleLauncher = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent> ();
    }

    void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents (particleLauncher, other, collisionEvents);

        for (int i = 0; i < collisionEvents.Count; i++) 
        {
            splatDecalPool.ParticleHit (collisionEvents [i], particleColorGradient);
            EmitAtLocation (collisionEvents[i]);
        }

    }

    void EmitAtLocation(ParticleCollisionEvent particleCollisionEvent)
    {
        splatterParticles.transform.position = particleCollisionEvent.intersection;
        splatterParticles.transform.rotation = Quaternion.LookRotation (particleCollisionEvent.normal);
        ParticleSystem.MainModule psMain = splatterParticles.main;
        psMain.startColor = particleColorGradient.Evaluate (Random.Range (0f, 1f));

        splatterParticles.Emit (1);
    }

    void Update () 
    {
        if (Input.GetButton ("Fire1")) 
        {
            ParticleSystem.MainModule psMain = particleLauncher.main;
            psMain.startColor = particleColorGradient.Evaluate (Random.Range (0f, 1f));
            particleLauncher.transform.position = barrel.transform.position;
            particleLauncher.transform.rotation = barrel.transform.rotation * Quaternion.Euler(0.0f, 90.0f, 0.0f);
            particleLauncher.Emit (1);
        }

    }
}