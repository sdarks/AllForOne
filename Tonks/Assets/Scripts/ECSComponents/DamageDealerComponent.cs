using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealerComponent : BaseComponent
{
    public float Damage;
    public bool Active = true;
    public float VelocityMagnitudeToCauseDamage = 5.0f;
    public CustomCollisionData LatestCollision = null;

    public class CustomCollisionData
    {
        public int TargetID;
        public float VelocityMagnitude;
        public Vector3 FirstContactPoint;
    }
    //Need this non ECS to handle collision callbacks unfortunately
    void OnCollisionEnter(Collision collision)
    {

        DamageDealerComponent DC = this;
        TeamComponent TC = this.ParentEntity.GetECSComponent<TeamComponent>();

        if (collision.contacts.Length > 0)
        {
            DamageableComponent damageTarget = collision.transform.GetComponentInParent<DamageableComponent>();
            if (damageTarget)
            {
                LatestCollision = new CustomCollisionData();
                LatestCollision.TargetID = damageTarget.ParentEntity.ID;
                LatestCollision.VelocityMagnitude = collision.relativeVelocity.magnitude;
                LatestCollision.FirstContactPoint = collision.contacts[0].point;
               
            }
        }
    }
}
