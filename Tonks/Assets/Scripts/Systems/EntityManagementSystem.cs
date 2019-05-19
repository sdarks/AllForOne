using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityManagementSystem : BaseSystem
{
    public List<EntityComponent> Entities = new List<EntityComponent>();
    public static EntityManagementSystem inst;
    //public List<int> EntityIDsForSystemUpdate = new List<int>();
    public Dictionary<int, int> EntityMap = new Dictionary<int, int>();
    public List<Archetype> Archetypes = new List<Archetype>();
    public int EntityIDCounter = 0;
    public EntityManagementSystem()
    {
        if (inst != null)
        {
            Debug.LogError("TWO ENTITY MANAGEMENT SYSTEMS??!?!?");
        }
        else
        {
            inst = this;
        }
    }
    /*
    public List<int> GetIDsForUpdate<T>() where T: BaseComponent
    {
        EntityIDsForSystemUpdate.Clear();
        for (int i=0; i<Entities.Count; i++)
        {
            if (Entities[i].ComponentTypes.Contains(typeof(T)))
            {
                EntityIDsForSystemUpdate.Add(Entities[i].ID);
            }
        }
        return EntityIDsForSystemUpdate;
    }
    public List<int> GetIDsForUpdate<T,T2>() where T : BaseComponent where T2 : BaseComponent
    {
        EntityIDsForSystemUpdate.Clear();
        for (int i = 0; i < Entities.Count; i++)
        {
            if (Entities[i].ComponentTypes.Contains(typeof(T)) && Entities[i].ComponentTypes.Contains(typeof(T2)))
            {
                EntityIDsForSystemUpdate.Add(Entities[i].ID);
            }
        }
        return EntityIDsForSystemUpdate;
    }
    public List<int> GetIDsForUpdate<T, T2, T3>() where T : BaseComponent where T2 : BaseComponent where T3 : BaseComponent
    {
        EntityIDsForSystemUpdate.Clear();
        for (int i = 0; i < Entities.Count; i++)
        {
            if (Entities[i].ComponentTypes.Contains(typeof(T)) && Entities[i].ComponentTypes.Contains(typeof(T2)) && Entities[i].ComponentTypes.Contains(typeof(T3)))
            {
                EntityIDsForSystemUpdate.Add(Entities[i].ID);
            }
        }
        return EntityIDsForSystemUpdate;
    }*/

    public List<Archetype> GetArchetypesForUpdate(List<System.Type> Components)
    {
        List<Archetype> arcs = new List<Archetype>();
        foreach(Archetype arc in Archetypes)
        {
            if (arc.MatchesComponentTypes(Components))
            {
                arcs.Add(arc);
            }
             
        }

        return arcs;
    }

    public EntityComponent GetPlayerEntity()
    {
		List<System.Type> playerComp = new List<System.Type>();
		playerComp.Add(typeof(PlayerComponent));

		foreach (Archetype arc in Archetypes)
		{
			if (arc.MatchesComponentTypes(playerComp))
			{
				List<BaseComponent> bcl = arc.Components[arc.ComponentTypeMap[typeof(PlayerComponent)]];
				if(bcl.Count>0)
				{
					PlayerComponent returnComp = (PlayerComponent)bcl[0];
					return returnComp.ParentEntity;
				}
			}

		}
		
        return null;
    }

    public int GetPlayerEntityID()
    {
		List<System.Type> playerComp = new List<System.Type>();
		playerComp.Add(typeof(PlayerComponent));

		foreach (Archetype arc in Archetypes)
		{
			if (arc.MatchesComponentTypes(playerComp))
			{
				List<BaseComponent> bcl = arc.Components[arc.ComponentTypeMap[typeof(PlayerComponent)]];
				if (bcl.Count > 0)
				{
					PlayerComponent returnComp = (PlayerComponent)bcl[0];
					return returnComp.ParentEntity.ID;
				}
			}

		}

		return -1;
	}
    
    public EntityComponent GetEntity(int EntityID)
    {
		if(EntityMap.ContainsKey(EntityID))
			return Entities[EntityMap[EntityID]];

		return null;
    }

    void InitEntity(EntityComponent entity)
    {
        entity.Components.Clear();
		entity.ComponentTypes.Clear();
        foreach(BaseComponent c in entity.GetComponents<BaseComponent>())
        {
            entity.Components.Add(c);
            entity.ComponentTypes.Add(c.GetType());
            c.ParentEntity = entity;
        }
        Entities.Add(entity);
        entity.ID = EntityIDCounter++;
        EntityMap.Add(entity.ID, Entities.Count - 1);

      

        bool hasMatch=false;
        foreach(Archetype arc in Archetypes)
        {
            if(arc.ArchetypeMatchEntity(entity))
            {
                hasMatch = true;
                arc.AddEntity(entity);
                entity.CurArchetype = arc;
                break;
            }
        }

        if(!hasMatch)
        {
            Archetype arc = new Archetype();
            arc.InitArchetype(entity);
            arc.AddEntity(entity);
            entity.CurArchetype = arc;
            Archetypes.Add(arc);
        }
    }

    public void RegisterWithEntityManager(EntityComponent entity)
    {
        InitEntity(entity); 
    }

    public void UnRegisterWithEntityManager(EntityComponent entity)
    {
        Archetype arc = entity.CurArchetype;
        entity.CurArchetype.RemoveEntity(entity);
        Entities.Remove(entity);

        EntityMap.Clear();

        for(int i=0; i<Entities.Count; i++)
        {
            EntityMap.Add(Entities[i].ID, i);
        }
    }

    public override void SystemUpdate()
    {
        
    }
}
