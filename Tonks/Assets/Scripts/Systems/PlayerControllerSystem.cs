using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerSystem : BaseSystem
{
    public override void SystemUpdate()
    {

        //Components required for system
        List<System.Type> componentTypes = new List<System.Type>();
        componentTypes.Add(typeof(PlayerInputComponent));
        componentTypes.Add(typeof(RotateTargetComponent));
        componentTypes.Add(typeof(MoveForwardsComponent));

        //Get the list of archetypes
        List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);

        //Loop through the archetypes
        foreach (Archetype arc in ArchetypesToUpdate)
        {
            //Get the list of components in this archetype
            List<BaseComponent> playerInputComponents = arc.Components[arc.ComponentTypeMap[typeof(PlayerInputComponent)]];
            List<BaseComponent> rotateTargetComponents = arc.Components[arc.ComponentTypeMap[typeof(RotateTargetComponent)]];
            List<BaseComponent> moveForwardComponents = arc.Components[arc.ComponentTypeMap[typeof(MoveForwardsComponent)]];

            //Loop through all the components this could be burst compiled
            for (int i = 0; i < playerInputComponents.Count; i++)
            {
                PlayerInputComponent PIC = (PlayerInputComponent)playerInputComponents[i];
                RotateTargetComponent RTC = (RotateTargetComponent)rotateTargetComponents[i];
                MoveForwardsComponent MFC = (MoveForwardsComponent)moveForwardComponents[i];

                if ((PIC.PlayerMovementAxis.x != 0 || PIC.PlayerMovementAxis.z != 0))
                {
                    Vector3 newPos = PIC.ParentEntity.transform.position;
                    //Move forwards
                    newPos.z += PIC.PlayerMovementAxis.z;
                    newPos.x += PIC.PlayerMovementAxis.x;
                    MFC.Move = true;
                    if (RTC != null)
                        RTC.TargetPosition = newPos;
                }
                else
                {
                    MFC.Move = false;
                    if (RTC != null)
                        RTC.TargetPosition = PIC.ParentEntity.transform.position;
                }
            }
        }
        
    }
}
