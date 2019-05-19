using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplacePlayerSystem : BaseSystem
{
	public Vector3 LastPlayerPosition;

	public override void SystemUpdate()
	{

		EntityComponent player = EntityManagementSystem.inst.GetPlayerEntity();

		if(player)
		{
			LastPlayerPosition = player.transform.position;
		}
		else
		{
			float closestPosition = float.MaxValue;
			bool foundReplacement = false;
			int replacementEntityID = -1;

			//Components required for system
			List<System.Type> componentTypes = new List<System.Type>();
			componentTypes.Add(typeof(TeamComponent));
			componentTypes.Add(typeof(PotentialPlayerComponent));

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

					if(TC.TeamID == SystemSystem.PlayerTeamID)
					{
						float distance = Vector3.Distance(TC.transform.position, LastPlayerPosition);
						if (distance < closestPosition)
						{
							foundReplacement = true;
							replacementEntityID = TC.ParentEntity.ID;
							closestPosition = distance;
						}
					}
					
				}
			}


			if(foundReplacement)
			{
				GameObject newPlayer = UnityEngine.GameObject.Instantiate(SystemSystem.inst.PlayerPrefab, null);
				EntityComponent replacementEntity = EntityManagementSystem.inst.GetEntity(replacementEntityID);
				newPlayer.transform.position = replacementEntity.transform.position;
				newPlayer.transform.rotation = replacementEntity.transform.rotation;

				EntityComponent newPlayerEntity = newPlayer.GetComponent<EntityComponent>();
				DamageableComponent newPlayerDamageable = newPlayerEntity.GetECSComponent<DamageableComponent>();
				DamageableComponent replacementDamageable = replacementEntity.GetECSComponent<DamageableComponent>();
				newPlayerDamageable.MaximumHP = replacementDamageable.MaximumHP;
				newPlayerDamageable.CurrentHP = replacementDamageable.CurrentHP;

				UnityEngine.GameObject.Destroy(replacementEntity.gameObject);
			}
		}
		
	}
}
