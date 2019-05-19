using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakSystem : BaseSystem
{
    public override void SystemUpdate()
    {
        List<System.Type> componentTypes = new List<System.Type>();
        componentTypes.Add(typeof(BreakableComponent));
        componentTypes.Add(typeof(DamageableComponent));

        List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);

        foreach (Archetype arc in ArchetypesToUpdate)
        {
            List<BaseComponent> damageables = arc.Components[arc.ComponentTypeMap[typeof(DamageableComponent)]];
            List<BaseComponent> breakables = arc.Components[arc.ComponentTypeMap[typeof(BreakableComponent)]];
            for (int i = 0; i<damageables.Count; i++)
            {
                DamageableComponent dam = (DamageableComponent)damageables[i];
                BreakableComponent brk = (BreakableComponent)breakables[i];
                if (dam.CurrentHP <= 0)
                {
                    TeamComponent team = (TeamComponent)brk.ParentEntity.GetECSComponent<TeamComponent>();
                    //Break here
                    foreach (GameObject GO in brk.PrefabsToBreakInto)
                    {
                        SystemSystem.inst.CreateEntity(GO, null, brk.transform.position, brk.transform.rotation, new Vector3(), new Vector3(), team ? team.TeamID : -1);
                    }
                }
            }
        }
        
    }
}
