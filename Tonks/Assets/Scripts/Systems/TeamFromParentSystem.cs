using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamFromParentSystem : BaseSystem
{
	public override void SystemUpdate()
	{
		//Components required for system
		List<System.Type> componentTypes = new List<System.Type>();
		componentTypes.Add(typeof(TeamFromParentComponent));
		componentTypes.Add(typeof(TeamComponent));

		//Get the list of archetypes
		List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);

		//Loop through the archetypes
		foreach (Archetype arc in ArchetypesToUpdate)
		{
			//Get the list of components in this archetype
			List<BaseComponent> teamComponents = arc.Components[arc.ComponentTypeMap[typeof(TeamComponent)]];

			//Loop through all the components this could be burst compiled
			for (int i = 0; i < teamComponents.Count; i++)
			{
				TeamComponent TC = (TeamComponent)teamComponents[i];

				if(TC.transform.parent)
				{
					TeamComponent parent = TC.transform.parent.GetComponentInParent<TeamComponent>();
					if (parent)
					{
						TC.TeamID = parent.TeamID;
					}
				}
				
			}
		}

	}
}
