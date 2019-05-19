using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManagerSystem : BaseSystem
{
	public static float ProjectileTimeoutTime = 3;
	public static int MaxProjectilesOnScreen = 2000;
	public override void SystemUpdate()
	{
		//Components required for system
		List<System.Type> componentTypes = new List<System.Type>();
		componentTypes.Add(typeof(ProjectileComponent));

		//Get the list of archetypes
		List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);
		
		EntityComponent PlayerEntity = EntityManagementSystem.inst.GetPlayerEntity();
		if (PlayerEntity)
		{
			//Loop through the archetypes
			foreach (Archetype arc in ArchetypesToUpdate)
			{
				//Get the list of components in this archetype
				List<BaseComponent> projectileComponents = arc.Components[arc.ComponentTypeMap[typeof(ProjectileComponent)]];

				int projectilesToDestroy = projectileComponents.Count - MaxProjectilesOnScreen;
				//Loop through all the components this could be burst compiled
				for (int i = 0; i < projectileComponents.Count; i++)
				{
					if(projectilesToDestroy > 0)
					{
						SystemSystem.inst.ReturnToPool(projectileComponents[i].transform);
						projectilesToDestroy--;
					}
					else
					{
						ProjectileComponent PC = (ProjectileComponent)projectileComponents[i];
						if (!PC.HasSpawned)
						{
							if (PC.isActiveAndEnabled)
							{
								PC.HasSpawned = true;
								PC.SpawnTime = Time.time;
							}
						}
						else if (!PC.Disabled)
						{
							Rigidbody RBody = PC.GetComponent<Rigidbody>();
							if (RBody)
							{
								if (Time.time > PC.SpawnTime + ProjectileTimeoutTime || RBody.velocity.sqrMagnitude <= 1)
								{
									PC.Disabled = true;
									
									RBody.detectCollisions = false;
									RBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
									RBody.isKinematic = true;
								}
							}
						}
					}
					
					
				}
			}
		}


	}
}
