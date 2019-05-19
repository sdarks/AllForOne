using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSystem : BaseSystem
{
	private float lowPitch = 0.75f;
	private float highPitch = 1.5f;
    public override void SystemUpdate()
    {
        //Components required for system
        List<System.Type> componentTypes = new List<System.Type>();
        componentTypes.Add(typeof(FireComponent));
        componentTypes.Add(typeof(TeamComponent));

        //Get the list of archetypes
        List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);

        //Loop through the archetypes
        foreach (Archetype arc in ArchetypesToUpdate)
        {
            //Get the list of components in this archetype
            List<BaseComponent> fireComponents = arc.Components[arc.ComponentTypeMap[typeof(FireComponent)]];
            List<BaseComponent> teamComponents = arc.Components[arc.ComponentTypeMap[typeof(TeamComponent)]];

            //Loop through all the components this could be burst compiled
            for (int i = 0; i < fireComponents.Count; i++)
            {
                FireComponent FC = (FireComponent)fireComponents[i];
                TeamComponent TC = (TeamComponent)teamComponents[i];
                if (FC.Firing)
                {
                    if (Time.time >= FC.LastFiredTime + FC.Cooldown)
                    {
                        FC.LastFiredTime = Time.time;

						if(TC.TeamID == SystemSystem.PlayerTeamID)
						{
							UIManager.inst.ProjectilesFired++;
						}
						
						AudioSource audio = FC.GetComponent<AudioSource>();
						if(audio)
						{
							audio.pitch = Random.Range(lowPitch, highPitch);
							audio.PlayOneShot(SystemSystem.inst.ProjectileSound, 1);
						}
						SystemSystem.inst.CreateProjectile(null, FC.ParentEntity.transform.position, FC.ParentEntity.transform.rotation, Vector3.zero, FC.FireForce * FC.ParentEntity.transform.forward, TC.TeamID);
						//SystemSystem.inst.CreateEntity(FC.Projectile, null, FC.ParentEntity.transform.position, FC.ParentEntity.transform.rotation, Vector3.zero, FC.FireForce * FC.ParentEntity.transform.forward, TC.TeamID);
					}
                    FC.Firing = false;
                }
            }
        }

        
    }
}
