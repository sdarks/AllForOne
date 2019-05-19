using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archetype
{
    public List<List<BaseComponent>> Components;
    public List<System.Type> ComponentTypes;
    public Dictionary<int, int> EntityMap;
    public Dictionary<System.Type, int> ComponentTypeMap;
    public int NumEntities;

    public bool ArchetypeMatchEntity(EntityComponent entity)
    {
        //Have to have same number of components
        if(ComponentTypes.Count != entity.ComponentTypes.Count)
        {
            return false;
        }

        foreach(System.Type compType in entity.ComponentTypes)
        {

            if(!ComponentTypes.Contains(compType))
            {
                //We found a component we don't have so can't be this archetype
                return false;
            }
        }

        return true;
    }

    public void InitArchetype(EntityComponent sampleEntity)
    {
        Components = new List<List<BaseComponent>>();
        ComponentTypes = new List<System.Type>();
        EntityMap = new Dictionary<int, int>();
        ComponentTypeMap = new Dictionary<System.Type, int>();
        NumEntities = 0;


        foreach(System.Type componentType in sampleEntity.ComponentTypes)
        {
            ComponentTypes.Add(componentType);
            Components.Add(new List<BaseComponent>());
            ComponentTypeMap.Add(componentType, Components.Count-1);
        }
        
    }

    public void AddEntity(EntityComponent entity)
    {
        EntityMap.Add(entity.ID, (NumEntities++));

        foreach (BaseComponent comp in entity.Components)
        {
            System.Type compType = comp.GetType();
            Components[ComponentTypeMap[compType]].Add(comp);
        }
    }

    public void RemoveEntity(EntityComponent entity)
    {
        if(EntityMap.ContainsKey(entity.ID))
        {
            foreach (BaseComponent comp in entity.Components)
            {
                System.Type compType = comp.GetType();
                Components[ComponentTypeMap[compType]].Remove(comp);
            }

            EntityMap.Remove(entity.ID);
            NumEntities--;
        }
        else
        {
            Debug.LogError("Tried to remove an entity from an archetype that knows nothing about it");
        }
        entity.CurArchetype = null;
    }

    public bool MatchesComponentTypes(List<System.Type> CompTypes)
    {
        
        foreach (System.Type compType in CompTypes)
        {

            if (!ComponentTypes.Contains(compType))
            {
                //We found a component we don't have so can't be this archetype
                return false;
            }
        }
        return true;
    }

    public bool HasComponent(System.Type ComponentType)
    {
        return ComponentTypes.Contains(ComponentType);
    }


}
