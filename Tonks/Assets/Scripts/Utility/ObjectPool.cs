using System;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectPool
{
	const int kDefaultPoolSize = 5;

	static Transform s_PoolParent = null;
	static Dictionary<int, List<Component>> s_Pools = new Dictionary<int, List<Component>>();
	static Dictionary<int, List<Component>> s_PoolReverseLookup = new Dictionary<int, List<Component>>();

	// Extension methods
	public static List<Component> CreatePool( this Component prefab, int size )
	{
		if (s_PoolParent == null)
		{
			GameObject poolParentGO = new GameObject("PoolContainer");
			poolParentGO.SetActive(false);
			s_PoolParent = poolParentGO.transform;
		}

		List<Component> pool = GetPool(prefab);
		if (pool == null)
		{
			pool = new List<Component>(size);
			s_Pools.Add(prefab.GetInstanceID(), pool);

			AddItemsToPool(pool, prefab, size);
		}
		else
		{
			Debug.LogError("Already have pool list for prefab " + prefab.name);
		}

		return pool;
	}

	public static Component GetFromPool( this Component prefab )
	{
		List<Component> pool = GetPool(prefab);
		if (pool == null)
		{
			pool = CreatePool(prefab, kDefaultPoolSize);
		}

		if (pool.Count == 0)
		{
			// Add new items
			AddItemsToPool(pool, prefab, 1000);
		}

		int index = pool.Count - 1;
		Component instance = pool[index];

		pool.RemoveAt(index);
		s_PoolReverseLookup.Add(instance.GetInstanceID(), pool);

		return instance;
	}

	public static void ReturnToPool( this Component instance )
	{
		List<Component> pool;
		if (s_PoolReverseLookup.TryGetValue(instance.GetInstanceID(), out pool))
		{
			s_PoolReverseLookup.Remove(instance.GetInstanceID());
			pool.Add(instance);

			instance.transform.SetParent(s_PoolParent, false);
		}
		else
		{
			Debug.LogError("Returning Item to pool that was not taken?? " + instance.name);
		}
	}

	// Internal
	static List<Component> GetPool( Component prefab )
	{
		List<Component> pool = null;
		s_Pools.TryGetValue(prefab.GetInstanceID(), out pool);

		return pool;
	}

	static void AddItemsToPool( List<Component> pool, Component prefab, int count )
	{
		for (int i = 0; i < count; i++)
		{
			Component instance = GameObject.Instantiate(prefab);
			instance.transform.SetParent(s_PoolParent);

			pool.Add(instance);
		}
	}
}