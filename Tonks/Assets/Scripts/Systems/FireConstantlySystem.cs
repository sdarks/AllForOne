using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireConstantlySystem : BaseSystem
{
   
    public override void SystemUpdate()
    {
        //Components required for system
        List<System.Type> componentTypes = new List<System.Type>();
        componentTypes.Add(typeof(FireComponent));
        componentTypes.Add(typeof(FireConstantlyComponent));
		componentTypes.Add(typeof(AggroRangeComponent));

        //Get the list of archetypes
        List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);

		EntityComponent PlayerEntity = EntityManagementSystem.inst.GetPlayerEntity();
		//Loop through the archetypes
		foreach (Archetype arc in ArchetypesToUpdate)
        {
            //Get the list of components in this archetype
            List<BaseComponent> fireComponents = arc.Components[arc.ComponentTypeMap[typeof(FireComponent)]];
			List<BaseComponent> aggroComponents = arc.Components[arc.ComponentTypeMap[typeof(AggroRangeComponent)]];

			//Loop through all the components this could be burst compiled
			for (int i = 0; i < fireComponents.Count; i++)
            {
                FireComponent FC = (FireComponent)fireComponents[i];
				AggroRangeComponent ARC = (AggroRangeComponent)aggroComponents[i];

				if (PlayerEntity)
				{
					Transform PlayerTransform = PlayerEntity.transform;
					if (PlayerTransform && (PlayerTransform.position - FC.transform.position).magnitude <= ARC.Radius)
					{
						FC.Firing = true;
					}
					else
					{
						FC.Firing = false;
					}
				}

				
            }
        }
    }
                
}
