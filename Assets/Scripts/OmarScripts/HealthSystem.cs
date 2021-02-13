using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//code derived by YouTube Tutorial by CodeMonkey
//https://www.youtube.com/watch?v=0T5ei9jN63M
public class HealthSystem {
    private float health;
    private int healthMax;

    public HealthSystem(int health) {
        this.health = health;
        healthMax = health;
    }

    public float getHealth(){
        return health;
    }

    public float getHealthPercent(){
        return (float) health / healthMax;
    }

    public void takeDamage(float damageAmount){
        health -= damageAmount;
        if (health < 0){
            health = 0;
        }
    }

    public void heal(){
        health += 1 + (PointSystem.Instance.multiplier / 15.0f);
        if (health > 100){
            health = 100;
        }
    }
}
