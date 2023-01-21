using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterMovementBase : MonoBehaviour
{
    public abstract void Move(Vector2 move, bool jump);
    public abstract void AddJumpForce(float multiplier);
    public abstract void Freeze();
    public abstract void PlayDieAnimation();
    public abstract Vector2 ActionToSpeed(InputController.Action action);
}
