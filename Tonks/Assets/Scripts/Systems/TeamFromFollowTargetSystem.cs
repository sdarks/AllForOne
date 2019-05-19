using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamFromFollowTargetSystem : BaseSystem
{
	public override void SystemUpdate()
	{
		//Components required for system
		List<System.Type> componentTypes = new List<System.Type>();
		componentTypes.Add(typeof(FollowTargetComponent));
		componentTypes.Add(typeof(TeamComponent));
		componentTypes.Add(typeof(TeamFromFollowTargetComponent));

		//Get the list of archetypes
		List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);

		//Loop through the archetypes
		foreach (Archetype arc in ArchetypesToUpdate)
		{
			//Get the list of components in this archetype
			List<BaseComponent> followTargetComponents = arc.Components[arc.ComponentTypeMap[typeof(FollowTargetComponent)]];
			List<BaseComponent> teamComponents = arc.Components[arc.ComponentTypeMap[typeof(TeamComponent)]];

			//Loop through all the components this could be burst compiled
			for (int i = 0; i < followTargetComponents.Count; i++)
			{
				FollowTargetComponent FTC = (FollowTargetComponent)followTargetComponents[i];
				TeamComponent TC = (TeamComponent)teamComponents[i];

				if (FTC.enabled && FTC.EntityToFollow != -1)
				{
					EntityComponent followEntity = EntityManagementSystem.inst.GetEntity(FTC.EntityToFollow);
					if (followEntity)
					{
						TeamComponent followTeam = followEntity.GetECSComponent<TeamComponent>();
						if(followTeam)
						{
							TC.TeamID = followTeam.TeamID;
						}

					}

				}
			}
		}

	}
}
