using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerSystem : BaseSystem
{
    public override void SystemUpdate()
    {
        //Components required for system
        List<System.Type> componentTypes = new List<System.Type>();
        componentTypes.Add(typeof(FollowPlayerComponent));
        componentTypes.Add(typeof(FollowTargetComponent));

        //Get the list of archetypes
        List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);

        int PlayerID = EntityManagementSystem.inst.GetPlayerEntityID();
        //Loop through the archetypes
        foreach (Archetype arc in ArchetypesToUpdate)
        {
            //Get the list of components in this archetype
            List<BaseComponent> followPlayerComponents = arc.Components[arc.ComponentTypeMap[typeof(FollowPlayerComponent)]];
            List<BaseComponent> followTargetComponents = arc.Components[arc.ComponentTypeMap[typeof(FollowTargetComponent)]];

            //Loop through all the components this could be burst compiled
            for (int i = 0; i < followPlayerComponents.Count; i++)
            {
                FollowPlayerComponent FPC = (FollowPlayerComponent)followPlayerComponents[i];
                FollowTargetComponent FTC = (FollowTargetComponent)followTargetComponents[i];
                if (FPC.enabled)
                {
                    FTC.EntityToFollow = PlayerID;
                }
            }
        }
        
    }
}