using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealerCleanupSystem : BaseSystem
{
    public override void SystemUpdate()
    {
        //Components required for system
        List<System.Type> componentTypes = new List<System.Type>();
        componentTypes.Add(typeof(DamageDealerComponent));

        //Get the list of archetypes
        List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);

        //Loop through the archetypes
        foreach (Archetype arc in ArchetypesToUpdate)
        {
            //Get the list of components in this archetype
            List<BaseComponent> damageDealerComponents = arc.Components[arc.ComponentTypeMap[typeof(DamageDealerComponent)]];

            //Loop through all the components this could be burst compiled
            for (int i = 0; i < damageDealerComponents.Count; i++)
            {
                DamageDealerComponent DC = (DamageDealerComponent)damageDealerComponents[i];

                DC.LatestCollision = null;
            }
        }

    }
}
