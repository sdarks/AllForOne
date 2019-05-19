using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceEnemyLeaderSystem : BaseSystem
{
	public override void SystemUpdate()
	{
		List<System.Type> componentTypes = new List<System.Type>();
		componentTypes.Add(typeof(TeamComponent));
		componentTypes.Add(typeof(EnemyLeaderComponent));
		componentTypes.Add(typeof(DamageableComponent));

		//Get the list of archetypes
		List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);
		//Loop through the archetypes
		foreach (Archetype arc in ArchetypesToUpdate)
		{
			//Get the list of components in this archetype
			List<BaseComponent> leaderComponents = arc.Components[arc.ComponentTypeMap[typeof(EnemyLeaderComponent)]];
			List<BaseComponent> damComponents = arc.Components[arc.ComponentTypeMap[typeof(DamageableComponent)]];

			//Loop through all the components this could be burst compiled
			for (int i = 0; i < leaderComponents.Count; i++)
			{
				EnemyLeaderComponent enemyLeader = (EnemyLeaderComponent)leaderComponents[i];
				DamageableComponent enemyLeaderDam = (DamageableComponent)damComponents[i];

				Vector3 lastEnemyPosition = enemyLeader.transform.position;

				if(enemyLeaderDam.CurrentHP <= 0 && enemyLeader.Replace)
				{
					float closestPosition = float.MaxValue;
					bool foundReplacement = false;
					int replacementEntityID = -1;

					//Components required for system
					List<System.Type> componentTypes2 = new List<System.Type>();
					componentTypes2.Add(typeof(TeamComponent));
					componentTypes2.Add(typeof(EnemyFollowerComponent));
					componentTypes2.Add(typeof(DamageableComponent));

					//Get the list of archetypes
					List<Archetype> ArchetypesToUpdate2 = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes2);

					//Loop through the archetypes
					foreach (Archetype arc2 in ArchetypesToUpdate2)
					{
						//Get the list of components in this archetype
						List<BaseComponent> teamComponents = arc2.Components[arc2.ComponentTypeMap[typeof(TeamComponent)]];
						List<BaseComponent> damagableComponents = arc2.Components[arc2.ComponentTypeMap[typeof(DamageableComponent)]];
						//Loop through all the components this could be burst compiled
						for (int j = 0; j < teamComponents.Count; j++)
						{
							TeamComponent TC = (TeamComponent)teamComponents[j];
							DamageableComponent DC = (DamageableComponent)damagableComponents[j];
							float distance = Vector3.Distance(TC.transform.position, lastEnemyPosition);
							if (DC.CurrentHP > 0 && distance < closestPosition && distance < 10)
							{
								foundReplacement = true;
								replacementEntityID = TC.ParentEntity.ID;
								closestPosition = distance;
							}

						}
					}


					if (foundReplacement)
					{
						GameObject newLeader = UnityEngine.GameObject.Instantiate(SystemSystem.inst.EnemyLeaderPrefab, null);
						EntityComponent replacementEntity = EntityManagementSystem.inst.GetEntity(replacementEntityID);
						newLeader.transform.position = replacementEntity.transform.position;
						newLeader.transform.rotation = replacementEntity.transform.rotation;

						EntityComponent newPlayerEntity = newLeader.GetComponent<EntityComponent>();
						DamageableComponent newPlayerDamageable = newPlayerEntity.GetECSComponent<DamageableComponent>();
						DamageableComponent replacementDamageable = replacementEntity.GetECSComponent<DamageableComponent>();
						newPlayerDamageable.MaximumHP = replacementDamageable.MaximumHP;
						newPlayerDamageable.CurrentHP = replacementDamageable.CurrentHP;

						UnityEngine.GameObject.Destroy(replacementEntity.gameObject);
					}
				}
					
			}
			
		}

		
	}
}
