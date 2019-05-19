using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeLeaderSystem : BaseSystem
{
	public override void SystemUpdate()
	{
		List<System.Type> componentTypes = new List<System.Type>();
		componentTypes.Add(typeof(EnemyLeaderComponent));

		//Get the list of archetypes
		List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);

		//Loop through the archetypes
		foreach (Archetype arc in ArchetypesToUpdate)
		{
			//Get the list of components in this archetype
			List<BaseComponent> enemyLeaderComponents = arc.Components[arc.ComponentTypeMap[typeof(EnemyLeaderComponent)]];


			//Loop through all the components this could be burst compiled
			for (int i = 0; i < enemyLeaderComponents.Count; i++)
			{
				EnemyLeaderComponent leader = (EnemyLeaderComponent)enemyLeaderComponents[i];

				List<System.Type> componentTypes2 = new List<System.Type>();
				componentTypes2.Add(typeof(EnemyLeaderComponent));

				//Get the list of archetypes
				List<Archetype> ArchetypesToUpdate2 = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes2);

				//Loop through the archetypes
				foreach (Archetype arc2 in ArchetypesToUpdate2)
				{
					//Get the list of components in this archetype
					List<BaseComponent> enemyLeaderComponents2 = arc2.Components[arc2.ComponentTypeMap[typeof(EnemyLeaderComponent)]];


					//Loop through all the components this could be burst compiled
					for (int j = 0; j < enemyLeaderComponents2.Count; j++)
					{
						EnemyLeaderComponent leader2 = (EnemyLeaderComponent)enemyLeaderComponents2[j];
						if(leader != leader2 && leader.Replace && leader2.Replace && Vector3.Distance(leader.transform.position, leader2.transform.position) < 20)
						{
							leader2.Replace = false;
							leader2.ParentEntity.GetECSComponent<DamageableComponent>().CurrentHP = 0;
						}
					}
				}
				
			}
		}
		
	}
}
