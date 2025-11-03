using UnityEngine;

[CreateAssetMenu(fileName = "NewPowerup", menuName = "Inventory/Powerup")]
public class PowerupData : ScriptableObject
{
    public string powerupName;
    public Sprite icon;
    public bool canGroundPound;
    public bool canDash;
    public bool canWebSwing;
    public bool canPickup;
    public bool canMine;
}