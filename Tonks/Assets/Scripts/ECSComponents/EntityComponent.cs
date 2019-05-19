using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityComponent : BaseComponent
{
    public int ID;
    public List<BaseComponent> Components = new List<BaseComponent>();
    public List<System.Type> ComponentTypes = new List<System.Type>();
    public Archetype CurArchetype;
	public bool Registered = false;
    public void OnEnable()
    {
		if(!Registered)
		{
			EntityManagementSystem.inst.RegisterWithEntityManager(this);
			Registered = true;
		}
    }

    public void OnDisable()
    {
		if (Registered)
		{
			EntityManagementSystem.inst.UnRegisterWithEntityManager(this);
			Registered = false;
		}
    }

    public T GetECSComponent<T>() where T: BaseComponent
    {
        foreach( BaseComponent c in Components)
        {
            if(c as T != null)
            {
                return c as T;
            }
        }
        return null;
    }
}
