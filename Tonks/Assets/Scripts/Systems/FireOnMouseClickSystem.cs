using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOnMouseClickSystem : BaseSystem
{
    public override void SystemUpdate()
    {
        //Components required for system
        List<System.Type> componentTypes = new List<System.Type>();
        componentTypes.Add(typeof(FireComponent));
        componentTypes.Add(typeof(FireOnMouseClickComponent));
        componentTypes.Add(typeof(PlayerInputComponent));

        //Get the list of archetypes
        List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);

        //Loop through the archetypes
        foreach (Archetype arc in ArchetypesToUpdate)
        {
            //Get the list of components in this archetype
            List<BaseComponent> fireComponents = arc.Components[arc.ComponentTypeMap[typeof(FireComponent)]];
            List<BaseComponent> playerInputComponents = arc.Components[arc.ComponentTypeMap[typeof(PlayerInputComponent)]];

            //Loop through all the components this could be burst compiled
            for (int i = 0; i < fireComponents.Count; i++)
            {
                FireComponent FC = (FireComponent)fireComponents[i];
                PlayerInputComponent PIC = (PlayerInputComponent)playerInputComponents[i];
                if (PIC.MouseDown)
                {
                    FC.Firing = true;
                }
            }
        }

    }
}
