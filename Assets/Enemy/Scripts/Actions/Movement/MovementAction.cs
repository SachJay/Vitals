using System.Collections;
using UnityEngine;

public abstract class MovementAction : EnemyAction
{
    [SerializeField] protected Enemy enemy = null;
    [SerializeField, Tooltip("The multiplayer for the speed, during this action")] protected float speedMultiplayer = 2;
}
