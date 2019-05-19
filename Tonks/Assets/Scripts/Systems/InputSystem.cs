using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : BaseSystem
{
    public override void SystemUpdate()
    {
        //Player movement
        Vector3 playerAxis = Vector3.zero;
        //y = rotation
        //z = forward
        //x = strafe
        if (Input.GetKey(KeyCode.W))
        {
            playerAxis.z++;
        }
        if (Input.GetKey(KeyCode.A))
        {
            playerAxis.x--;
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerAxis.z--;
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerAxis.x++;
        }
        playerAxis.Normalize();

        //Mouse position in world
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Transform objectHit = null;
        Vector3 mousePos = Vector3.zero;
        if (Physics.Raycast(ray, out hit))
        {
            objectHit = hit.transform;
            mousePos = hit.point;
        }

        //Mouse click
        bool mouseClick = Input.GetMouseButton(0);

        //Mini system in system

        //Components required for system
        List<System.Type> componentTypes = new List<System.Type>();
        componentTypes.Add(typeof(PlayerInputComponent));

        //Get the list of archetypes
        List<Archetype> ArchetypesToUpdate = EntityManagementSystem.inst.GetArchetypesForUpdate(componentTypes);

        //Loop through the archetypes
        foreach (Archetype arc in ArchetypesToUpdate)
        {
            //Get the list of components in this archetype
            List<BaseComponent> playerInputComponents = arc.Components[arc.ComponentTypeMap[typeof(PlayerInputComponent)]];

            //Loop through all the components this could be burst compiled
            for (int i = 0; i < playerInputComponents.Count; i++)
            {
                PlayerInputComponent PIC = (PlayerInputComponent)playerInputComponents[i];

                PIC.PlayerMovementAxis = playerAxis;
                PIC.MouseWorldPosition = mousePos;
                PIC.MouseDown = mouseClick;
            }
        }
        
    }
}