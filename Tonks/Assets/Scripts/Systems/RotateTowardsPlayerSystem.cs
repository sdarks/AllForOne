using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsPlayerSystem : BaseSystem
{
	public override void SystemUpdate()
	{
		//Components required for system
		List<System.Type> componentTypes = new List<System.Type>();
		componentTypes.Add(typeof(RotateTowardsPlayerComponent));
		componentTypes.Add(typeof(RotateTargetComponent));

		//Get the list of archetypes
		List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);
		
		EntityComponent PlayerEntity = EntityManagementSystem.inst.GetPlayerEntity();
		if(PlayerEntity)
		{
			//Loop through the archetypes
			foreach (Archetype arc in ArchetypesToUpdate)
			{
				//Get the list of components in this archetype
				List<BaseComponent> rotateTargetComponents = arc.Components[arc.ComponentTypeMap[typeof(RotateTargetComponent)]];

				//Loop through all the components this could be burst compiled
				for (int i = 0; i < rotateTargetComponents.Count; i++)
				{
					RotateTargetComponent RTC = (RotateTargetComponent)rotateTargetComponents[i];
					RTC.TargetPosition = PlayerEntity.transform.position;
				}
			}
		}
		

	}
}
