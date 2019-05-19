using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawnerSystem : BaseSystem
{
	public override void SystemUpdate()
	{
		//Components required for system
		List<System.Type> componentTypes = new List<System.Type>();
		componentTypes.Add(typeof(EntitySpawnerComponent));

		//Get the list of archetypes
		List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);

		EntityComponent PlayerEntity = EntityManagementSystem.inst.GetPlayerEntity();
		if (PlayerEntity)
		{
			//Loop through the archetypes
			foreach (Archetype arc in ArchetypesToUpdate)
			{
				//Get the list of components in this archetype
				List<BaseComponent> spawnerComponents = arc.Components[arc.ComponentTypeMap[typeof(EntitySpawnerComponent)]];

				//Loop through all the components this could be burst compiled
				for (int i = 0; i < spawnerComponents.Count; i++)
				{
					EntitySpawnerComponent ESC = (EntitySpawnerComponent)spawnerComponents[i];
					if (Time.time >= ESC.LastSpawnTime + ESC.SpawnFrequency)
					{
						ESC.LastSpawnTime = Time.time;
						int rand = Random.Range(0, ESC.ThingsToSpawn.Count - 1);

						GameObject objToSpawn = ESC.ThingsToSpawn[rand];
						bool spawning = true;
						while (spawning)
						{
							Vector2 direction = Random.insideUnitCircle.normalized;
							direction = direction * ESC.SafeDistance;
							Vector3 spawnLocation = Vector3.zero;
							spawnLocation = new Vector3(PlayerEntity.transform.position.x + direction.x, PlayerEntity.transform.position.y, PlayerEntity.transform.position.z + direction.y);
							Vector3 res = FindFreeSpaceToSpawnGameObject(objToSpawn, spawnLocation, 20, 20);
							if (res != Vector3.zero)
							{
								GameObject.Instantiate(objToSpawn, res, Quaternion.identity, null);
								spawning = false;
							}
						}
					}

				}
			}
		}


	}


	Vector3 FindFreeSpaceToSpawnGameObject( GameObject obj, Vector3 centre, float searchWidth, float searchHeight )
	{
		Vector3 bounds = obj.GetComponentInChildren<Renderer>().bounds.extents;
		float radius = Mathf.Max(bounds.x, bounds.z);
		float heightToSearch = bounds.y / 2;
		Vector3 pointToSearch = new Vector3(Random.Range(centre.x - (searchWidth / 2), centre.x + (searchWidth / 2)), heightToSearch, Random.Range(centre.z - (searchHeight / 2), centre.z + (searchHeight / 2)));

		bool foundSpace = FreeSpaceAtPoint(pointToSearch, radius);

		if (foundSpace)
		{
			return pointToSearch;
		}

		int panic = 0;
		while (!foundSpace && panic < 100)
		{
			panic++;
			pointToSearch = new Vector3(Random.Range(centre.x - (searchWidth / 2), centre.x + (searchWidth / 2)), heightToSearch, Random.Range(centre.z - (searchHeight / 2), centre.z + (searchHeight / 2)));
			foundSpace = FreeSpaceAtPoint(pointToSearch, radius);
			if (foundSpace)
			{
				return pointToSearch;
			}
		}
		Debug.LogError("Failed to find free space to spawn game object");
		return Vector3.zero;
	}

	bool FreeSpaceAtPoint( Vector3 point, float radius )
	{
		//Ignore ground
		int mask = ~((1 << LayerMask.NameToLayer("Ground")));

		bool hit = Physics.CheckSphere(point, radius, mask);

		Debug.DrawLine(new Vector3(point.x - radius, point.y, point.z), new Vector3(point.x + radius, point.y, point.z), hit ? Color.red : Color.green, 10, false);
		Debug.DrawLine(new Vector3(point.x, point.y, point.z - radius), new Vector3(point.x, point.y, point.z + radius), hit ? Color.red : Color.green, 10, false);
		return !hit;
	}

}
