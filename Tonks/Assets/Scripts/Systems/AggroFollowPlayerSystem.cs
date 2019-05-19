using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroFollowPlayerSystem : BaseSystem
{
    public override void SystemUpdate()
    {
        List<System.Type> componentTypes = new List<System.Type>();
        componentTypes.Add(typeof(FollowPlayerComponent));
        componentTypes.Add(typeof(AggroRangeComponent));

        List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);

        EntityComponent PlayerEntity = EntityManagementSystem.inst.GetPlayerEntity();

        foreach (Archetype arc in ArchetypesToUpdate)
        {
            List<BaseComponent> followPlayerComponents = arc.Components[arc.ComponentTypeMap[typeof(FollowPlayerComponent)]];
            List<BaseComponent> aggroRangeComponents = arc.Components[arc.ComponentTypeMap[typeof(AggroRangeComponent)]];
            for (int i = 0; i < followPlayerComponents.Count; i++)
            {
                FollowPlayerComponent FPC = (FollowPlayerComponent)followPlayerComponents[i];
                AggroRangeComponent ARC = (AggroRangeComponent)aggroRangeComponents[i];
                
                if (PlayerEntity)
                {
                    Transform PlayerTransform = PlayerEntity.transform;
                    if (PlayerTransform && (PlayerTransform.position - FPC.ParentEntity.transform.position).magnitude <= ARC.Radius)
                    {
                        FPC.enabled = true;
                    }
                    else
                    {
                        FPC.enabled = false;
                    }
                }
            }
        }

    }
}
