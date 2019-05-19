using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireComponent : BaseComponent
{
    public bool Firing = false;
    public float Cooldown;
    public float LastFiredTime = 0;
    public GameObject Projectile;
    public float FireForce;
}
