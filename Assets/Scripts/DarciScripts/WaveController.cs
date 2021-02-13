using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Used to shuffle wave mob list
public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rnd = new System.Random();
        for (var i = 0; i < list.Count; i++)
            list.Swap(i, rnd.Next(i, list.Count));
    }

    public static void Swap<T>(this IList<T> list, int i, int j)
    {
        var temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }
}

public class WaveController : MonoBehaviour
{

    public static WaveController Instance { get; private set; }
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // unless we want to keep track of zombies from before.
        } else {
            Destroy(gameObject);
        }
    }
    // Spawns k*i on wave i of this mob
    public float smallMult = 3.5f;
    public float mediumMult = 1.8f;
    public float largeMult = 0.5f;

    // The maximum number of enemies in a wave. (Could increase with wave number?)
    public int minSpawnCap = 3;

    private int waveSpawnCap = 3;

    // Increase the max number of mobs at a time as the waves progress?
    public float spawnCapMult = 2f;

    // The prefabs that we spawn mobs from.
    public GameObject smallPrefab;
    public GameObject mediumPrefab;
    public GameObject largePrefab;
    public GameObject bossPrefab;

    // Parent of all enemy respawn locations (see hierarchy, feel free to move/add to them)
    public GameObject enemyRespawns;

    // Current wave number. Could use to change mob health (here or other scripts).
    private int waveNum = 0;
    private int waveSize = 0;
    private int waveKills = 0;
    private int totalPastWaveKills = 0;
    private bool goToNextWave = false;
    // List of enemies that have yet to be spawned this wave
    private List<GameObject> waveRemainingSpawns = new List<GameObject>();

    
    // Start is called before the first frame update
    void Start()
    {
        spawnCapMult = Math.Min(spawnCapMult, mediumMult + largeMult + smallMult);
    }

    // Update is called once per frame
    void Update()
    {
        // Added development-only option to instantly progress rounds
        #if UNITY_EDITOR
            // Press N to kill a random enemy
            if(Input.GetKeyDown(KeyCode.N) && Time.deltaTime != 0) {
                Destroy(this.transform.GetChild(0).gameObject);
            }
            // Press M to progress to next Wave instantly
            if(Input.GetKeyDown(KeyCode.M) && Time.deltaTime != 0) {
                goToNextWave = true;
            }
        #endif

        spawnNext();
    }

    public int getWaveNum() {
        return waveNum;
    }

    public int getWaveSize() {
        return waveSize;
    }

    public int getWaveKills() {
        return waveKills;
    }

    public int getTotalKills() {
        return totalPastWaveKills + waveKills;
    }

    public void spawnNext() {
        // If this wave is over (all enemies dead and no more left in wave), prepare for the next
        if (waveRemainingSpawns.Count == 0 && this.transform.childCount == 0)
        {
            goToNextWave = false;
            if (waveNum < 11) waveNum++;
            if (waveNum >= 11) {
                HudControl.gameWon = true;
            } else {
                totalPastWaveKills += waveSize;

                int numSmall = 0;
                int numMedium = 0;
                int numLarge = 0;
                // Create a shuffled list of enemies for the next wave
                // Note: ith wave always has same number/proportion of mobs, just in a random order
                if (waveNum == 10) {
                    waveRemainingSpawns.Add(bossPrefab);
                } else {
                    numSmall = Mathf.RoundToInt((waveNum * smallMult) - (waveNum/5));
                    numMedium = Mathf.RoundToInt((waveNum* mediumMult) - (waveNum/8));
                    numLarge = Mathf.RoundToInt((waveNum * largeMult) - (waveNum/3));
                    if (numMedium < 1) numMedium = 2;
                    for (int i = 0; i < numSmall; i++)
                    {
                        waveRemainingSpawns.Add(smallPrefab);
                    }
                    for (int i = 0; i < numMedium; i++)
                    {
                        waveRemainingSpawns.Add(mediumPrefab);
                    }
                    for (int i = 0; i < numLarge; i++)
                    {
                        waveRemainingSpawns.Add(largePrefab);
                    }

                }
                waveRemainingSpawns.Shuffle();
                waveSize = waveRemainingSpawns.Count;
                waveSpawnCap = minSpawnCap + Mathf.RoundToInt(waveNum * spawnCapMult);
                Debug.Log("WAVE " + waveNum + " (" + numSmall + " small, " + numMedium + " medium, " + numLarge + " large, " + waveSize + " total) (" + waveSpawnCap + " spawncap)");
            }
        } else if (goToNextWave == true) {
            Destroy(this.transform.GetChild(0).gameObject);
        }

        // Ensures last zombies hurry to the player
        if (this.transform.childCount == 1) {
            this.transform.GetChild(0).gameObject.GetComponent<NavMeshAgent>().speed = 5;
            this.transform.GetChild(0).gameObject.GetComponent<NavMeshAgent>().angularSpeed = 500;
            this.transform.GetChild(0).gameObject.GetComponent<NavMeshAgent>().acceleration = 25;
        }

        // spawns whole wave at once or 14 at most
        int spawnCap = waveSize;
        if (waveSize > 14) spawnCap = 14;

        // Keep spawning enemies until spawn cap is reached, or wave has no more mobs to spawn
        while (this.transform.childCount < spawnCap && waveRemainingSpawns.Count > 0) {
            // Get the next enemy that is supposed to be spawned this wave
            GameObject nextSpawn = waveRemainingSpawns[0];
            waveRemainingSpawns.RemoveAt(0);

            // Choose a random respawn location from the Enemy Respawn Points' children
            int randomSpawnPtIdx = UnityEngine.Random.Range(0, enemyRespawns.transform.childCount);
            Transform enemyRespawn = enemyRespawns.transform.GetChild(randomSpawnPtIdx).transform;

            // Offset the spawn location so enemies spawn on top of the ground
            Vector3 respawnPosition = enemyRespawn.position;
            float enemyHeight = nextSpawn.transform.localScale.y;
            float enemySpawnOffset = enemyHeight / 2.0f;
            respawnPosition.y += enemySpawnOffset;

            // Spawn zombie. Make it a child of the Wave Enemies object
            var newEnemy = Instantiate(nextSpawn, respawnPosition, Quaternion.identity);
            newEnemy.transform.parent = this.transform;

            // Ask the zombie to update it's color to a random one that we chose.
            int randomColorIdx = 0;
            if (waveNum != 10) {
                // RGB starting on wave 5
                if (waveNum >= 5)
                {
                    randomColorIdx = UnityEngine.Random.Range(0, 3);
                }
                // RG starting on wave 3
                else if (waveNum >= 3)
                {
                    randomColorIdx = UnityEngine.Random.Range(0, 2);
                }
                // R starting on wave 1
                else
                {
                    randomColorIdx = 0;
                }
            }
            newEnemy.GetComponent<EnemyController>().updateEnemyColorEnum((ColorEnum) randomColorIdx);
        }

        // Number of kills is total wave size minus those that are currenly alive and those that have yet to be spawned
        waveKills = waveSize - this.transform.childCount - waveRemainingSpawns.Count;
    }
}
