using System;
using System.Collections.Generic;
using UnityEngine;

public class MovementMotor : MonoBehaviour
{
    /*
    This class can be used like an interface.
    Inherit from it to define your own movement motor that can control
    the movement of characters, enemies, or other entities.
    */

    // The direction the character wants to move in, in world space.
    // The vector should have a length between 0 and 1.
    [HideInInspector]
    public Vector3 movementDirection;

    // Simpler motors might want to drive movement based on a target purely
    [HideInInspector]
    public Vector3 movementTarget;

    // The direction the character wants to face towards, in world space.
    [HideInInspector]
    public Vector3 facingDirection ;

}

