using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCleanupSystem : BaseSystem
{
    public override void SystemUpdate()
    {
        List<System.Type> componentTypes = new List<System.Type>();
        componentTypes.Add(typeof(DamageableComponent));

        List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);

        foreach(Archetype arc in ArchetypesToUpdate)
        {
            List<BaseComponent> damageables = new List<BaseComponent>(arc.Components[arc.ComponentTypeMap[typeof(DamageableComponent)]]);
			int numDamageables = damageables.Count;
			for(int i=0; i < numDamageables; i++)
			{
				DamageableComponent dam = (DamageableComponent)damageables[i];
				if (dam.CurrentHP <= 0)
				{
					TeamComponent team = dam.ParentEntity.GetECSComponent<TeamComponent>();
					if(team && team.TeamID != SystemSystem.PlayerTeamID)
					{
						UIManager.inst.EnemiesKilled++;
					}
					foreach (UnparentIfParentDestroyedComponent child in dam.ParentEntity.transform.GetComponentsInChildren<UnparentIfParentDestroyedComponent>())
					{
						child.transform.parent = null;
					}
					GameObject.Destroy(dam.ParentEntity.gameObject);
				}

			}
        }
    }
}
