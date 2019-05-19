using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsTargetSystem : BaseSystem
{
    public override void SystemUpdate()
    {

        //Components required for system
        List<System.Type> componentTypes = new List<System.Type>();
        componentTypes.Add(typeof(RotateSpeedComponent));
        componentTypes.Add(typeof(RotateTargetComponent));

        //Get the list of archetypes
        List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);

        //Loop through the archetypes
        foreach (Archetype arc in ArchetypesToUpdate)
        {
            bool playerComp = arc.HasComponent(typeof(PlayerComponent));
            //Get the list of components in this archetype
            List<BaseComponent> rotateSpeedComponents = arc.Components[arc.ComponentTypeMap[typeof(RotateSpeedComponent)]];
            List<BaseComponent> rotateTargetComponents = arc.Components[arc.ComponentTypeMap[typeof(RotateTargetComponent)]];

            //Loop through all the components this could be burst compiled
            for (int i = 0; i < rotateSpeedComponents.Count; i++)
            {
                RotateSpeedComponent RSC = (RotateSpeedComponent)rotateSpeedComponents[i];
                RotateTargetComponent RTC = (RotateTargetComponent)rotateTargetComponents[i];
                Transform T = RSC.ParentEntity.transform;
               

                Vector3 newPos = RTC.TargetPosition;
                Vector3 oldPos = T.position;

                if (newPos != oldPos)
                {
                    T.rotation = Quaternion.RotateTowards(T.rotation, Quaternion.LookRotation((newPos - oldPos).normalized), RSC.RotateSpeed  * Time.deltaTime);
                    T.eulerAngles = new Vector3(0, T.eulerAngles.y, 0);
                }
            }
        }

    }
}
