using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveForwardSystem : BaseSystem
{

    public override void SystemUpdate()
    {
        //Components required for system
        List<System.Type> componentTypes = new List<System.Type>();
        componentTypes.Add(typeof(MovementSpeedComponent));
        componentTypes.Add(typeof(MoveForwardsComponent));

        //Get the list of archetypes
        List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);
        
        //Loop through the archetypes
        foreach (Archetype arc in ArchetypesToUpdate)
        {
            bool playerComp = arc.HasComponent(typeof(PlayerComponent));
            //Get the list of components in this archetype
            List<BaseComponent> moveSpeedComponents = arc.Components[arc.ComponentTypeMap[typeof(MovementSpeedComponent)]];
            List<BaseComponent> moveForwardComponents = arc.Components[arc.ComponentTypeMap[typeof(MoveForwardsComponent)]];

            //Loop through all the components this could be burst compiled
            for (int i = 0; i < moveSpeedComponents.Count; i++)
            {
                MovementSpeedComponent MSC = (MovementSpeedComponent)moveSpeedComponents[i];
                MoveForwardsComponent MFC = (MoveForwardsComponent)moveForwardComponents[i];

                if (MFC.Move)
                {
                    
                    MSC.ParentEntity.transform.position = MSC.ParentEntity.transform.position + (MSC.MoveSpeed * Time.deltaTime * MSC.ParentEntity.transform.forward);
                }
            }
        }
        
    }
}
