using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemSystem;

public class DealDamageSystem : BaseSystem
{
   
    public List<DamageEvent> DamageEvents = new List<DamageEvent>();
    public void DealDamage(DamageEvent e)
    {
        DamageEvents.Add(e);
    }
    public override void SystemUpdate()
    {

        foreach(DamageEvent DE in DamageEvents)
        {
            EntityComponent Entity = EntityManagementSystem.inst.GetEntity(DE.EntityID);
            DamageableComponent DC = Entity.GetECSComponent<DamageableComponent>();
            if (DC != null)
            {
                DC.CurrentHP -= DE.Damage;
            }
            else
            {
                Debug.LogError("Tried to damage an entity with no DamageableComponent");
            }
        }

        DamageEvents.Clear();
    }
}
