using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetSystem : BaseSystem
{
    public override void SystemUpdate()
    {
        //Components required for system
        List<System.Type> componentTypes = new List<System.Type>();
        componentTypes.Add(typeof(FollowTargetComponent));
        componentTypes.Add(typeof(RotateTargetComponent));
        componentTypes.Add(typeof(MoveForwardsComponent));

        //Get the list of archetypes
        List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);

        //Loop through the archetypes
        foreach (Archetype arc in ArchetypesToUpdate)
        {
            //Get the list of components in this archetype
            List<BaseComponent> followTargetComponents = arc.Components[arc.ComponentTypeMap[typeof(FollowTargetComponent)]];
            List<BaseComponent> rotateTargetComponents = arc.Components[arc.ComponentTypeMap[typeof(RotateTargetComponent)]];
            List<BaseComponent> moveForwardComponents = arc.Components[arc.ComponentTypeMap[typeof(MoveForwardsComponent)]];

            //Loop through all the components this could be burst compiled
            for (int i = 0; i < followTargetComponents.Count; i++)
            {
                FollowTargetComponent FTC = (FollowTargetComponent)followTargetComponents[i];
                RotateTargetComponent RTC = (RotateTargetComponent)rotateTargetComponents[i];
                MoveForwardsComponent MFC = (MoveForwardsComponent)moveForwardComponents[i];

                if (FTC.enabled && FTC.EntityToFollow != -1)
                {
					EntityComponent followEntity = EntityManagementSystem.inst.GetEntity(FTC.EntityToFollow);
					if(followEntity)
					{
						if(FTC.FollowAtDistance)
						{
							RaycastHit rayHit;
							if (Physics.Raycast(FTC.transform.position, followEntity.transform.position - FTC.transform.position, out rayHit, Mathf.Infinity))
							{
								if(rayHit.distance <= FTC.FollowDistance)
								{
									RTC.TargetPosition = followEntity.transform.position;
									MFC.Move = false;
								}
								else
								{
									RTC.TargetPosition = rayHit.point - (rayHit.point - FTC.transform.position).normalized * FTC.FollowDistance;
									MFC.Move = true;
								}
							}
							else
							{
								RTC.TargetPosition = followEntity.transform.position;
								MFC.Move = true;
							}
						}
						else
						{
							RTC.TargetPosition = followEntity.transform.position;
							MFC.Move = true;
						}
						

					}

				}
            }
        }

    }
}
