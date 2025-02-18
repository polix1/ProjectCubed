using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "New Player")]
public class PlayerData : ScriptableObject
{
    public float walkingSpeed, sprintSpeed, jumpHeight, health, groundDistance;
}
