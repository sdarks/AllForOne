using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawnerComponent : BaseComponent
{
	public List<GameObject> ThingsToSpawn;
	public float SpawnFrequency;
	public float LastSpawnTime;
	public float SafeDistance;
}
