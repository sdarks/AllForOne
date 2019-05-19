using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollisionSystem : BaseSystem
{
    public override void SystemUpdate()
    {
        //Components required for system
        List<System.Type> componentTypes = new List<System.Type>();
        componentTypes.Add(typeof(DamageDealerComponent));
        componentTypes.Add(typeof(TeamComponent));

        //Get the list of archetypes
        List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);

        //Loop through the archetypes
        foreach (Archetype arc in ArchetypesToUpdate)
        {
            //Get the list of components in this archetype
            List<BaseComponent> damageDealerComponents = arc.Components[arc.ComponentTypeMap[typeof(DamageDealerComponent)]];
            List<BaseComponent> teamComponents = arc.Components[arc.ComponentTypeMap[typeof(TeamComponent)]];

            //Loop through all the components this could be burst compiled
            for (int i = 0; i < damageDealerComponents.Count; i++)
            {
                DamageDealerComponent DC = (DamageDealerComponent)damageDealerComponents[i];
                TeamComponent TC = (TeamComponent)teamComponents[i];

                if (DC.LatestCollision != null)
                {
                    if (DC.LatestCollision.VelocityMagnitude > DC.VelocityMagnitudeToCauseDamage)
                    {
						EntityComponent ent = EntityManagementSystem.inst.GetEntity(DC.LatestCollision.TargetID);

						if(ent)
						{
							TeamComponent teamTarget = ent.GetECSComponent<TeamComponent>();
							if (teamTarget && TC.TeamID != teamTarget.TeamID)
							{
								if(teamTarget.TeamID!= SystemSystem.PlayerTeamID)
								{
									UIManager.inst.DamageDone += (int)DC.Damage;
								}
								else
								{
									UIManager.inst.DamageTaken += (int)DC.Damage;
								}
								SystemSystem.inst.DealDamage(teamTarget.ParentEntity.ID, (int)(DC.Damage), DC.LatestCollision.FirstContactPoint);
								DC.Active = false;
								DC.Enabled = false;
							}
						}
						
                    }
                }
            }
        }

    }
}
