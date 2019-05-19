using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsMouseSystem : BaseSystem
{
    public override void SystemUpdate()
    {
        //Components required for system
        List<System.Type> componentTypes = new List<System.Type>();
        componentTypes.Add(typeof(RotateTowardsMouseComponent));
        componentTypes.Add(typeof(RotateTargetComponent));
        componentTypes.Add(typeof(PlayerInputComponent));

        //Get the list of archetypes
        List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);

        //Loop through the archetypes
        foreach (Archetype arc in ArchetypesToUpdate)
        {
            //Get the list of components in this archetype
            List<BaseComponent> playerInputComponents = arc.Components[arc.ComponentTypeMap[typeof(PlayerInputComponent)]];
            List<BaseComponent> rotateTargetComponents = arc.Components[arc.ComponentTypeMap[typeof(RotateTargetComponent)]];

            //Loop through all the components this could be burst compiled
            for (int i = 0; i < playerInputComponents.Count; i++)
            {
                PlayerInputComponent PIC = (PlayerInputComponent)playerInputComponents[i];
                RotateTargetComponent RTC = (RotateTargetComponent)rotateTargetComponents[i];

                if (PIC.MouseWorldPosition != Vector3.zero)
                {
                    RTC.TargetPosition = PIC.MouseWorldPosition;
                }
                else
                {
                    RTC.TargetPosition = PIC.ParentEntity.transform.position;
                }
            }
        }
        

    }

}
