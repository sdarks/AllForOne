using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputComponent : BaseComponent
{
    public Vector3 PlayerMovementAxis = Vector3.zero;
    public Vector3 MouseWorldPosition = Vector3.zero;
    public bool MouseDown = false;
}
