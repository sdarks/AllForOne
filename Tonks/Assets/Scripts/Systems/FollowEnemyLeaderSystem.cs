using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemyLeaderSystem : BaseSystem
{
	public override void SystemUpdate()
	{
		//Components required for system
		List<System.Type> componentTypes = new List<System.Type>();
		componentTypes.Add(typeof(FollowEnemyLeaderComponent));
		componentTypes.Add(typeof(FollowTargetComponent));

		//Get the list of archetypes
		List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);
		
		//Loop through the archetypes
		foreach (Archetype arc in ArchetypesToUpdate)
		{
			//Get the list of components in this archetype
			List<BaseComponent> followTargetComponents = arc.Components[arc.ComponentTypeMap[typeof(FollowTargetComponent)]];

			//Loop through all the components this could be burst compiled
			for (int i = 0; i < followTargetComponents.Count; i++)
			{
				FollowTargetComponent FTC = (FollowTargetComponent)followTargetComponents[i];

				List<System.Type> componentTypes2 = new List<System.Type>();
				componentTypes2.Add(typeof(EnemyLeaderComponent));

				//Get the list of archetypes
				List<Archetype> ArchetypesToUpdate2 = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes2);

				float closestLeaderDistance = float.MaxValue;
				bool foundLeader = false;
				EnemyLeaderComponent closestLeader=null;

				//Loop through the archetypes
				foreach (Archetype arc2 in ArchetypesToUpdate2)
				{
					//Get the list of components in this archetype
					List<BaseComponent> enemyLeaderComponents = arc2.Components[arc2.ComponentTypeMap[typeof(EnemyLeaderComponent)]];


					//Loop through all the components this could be burst compiled
					for (int j = 0; j < enemyLeaderComponents.Count; j++)
					{
						EnemyLeaderComponent leader = (EnemyLeaderComponent)enemyLeaderComponents[j];
						float distance = Vector3.Distance(leader.transform.position, FTC.transform.position);
						if ( distance < closestLeaderDistance)
						{
							foundLeader = true;
							closestLeader = leader;
							closestLeaderDistance = distance;
						}
					}
				}

				if(foundLeader)
					FTC.EntityToFollow = closestLeader.ParentEntity.ID;
			}
		}

	}
}
