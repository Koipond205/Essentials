using UnityEngine;
using System.Collections.Generic;

public class PlayerPowerUpController : MonoBehaviour
{
    public List<PowerupData> ownedPowerups;
    public PowerupData activePowerup;
    private int activePowerupIndex = -1;
    private GroundPoundAbility groundPoundAbility;
    private DashAbility dashAbility;
    private WebSwingAbility webSwingAbility;
    private PickupAbility pickupAbility;

    void Start()
    {
        groundPoundAbility = GetComponent<GroundPoundAbility>();
        dashAbility = GetComponent<DashAbility>();
        webSwingAbility = GetComponent<WebSwingAbility>();
        pickupAbility = GetComponent<PickupAbility>();

        // For demonstration, let's add some powerups.
        // You would normally add these when the player picks them up.
        // Assuming you have created these assets in your project.
        // ownedPowerups.Add(Resources.Load<PowerupData>("Powerup_GroundPound"));
        // ownedPowerups.Add(Resources.Load<PowerupData>("Powerup_SpeedBoost"));
        
        if (ownedPowerups.Count > 0)
        {
            activePowerupIndex = 0;
            activePowerup = ownedPowerups[activePowerupIndex];
        }
    }

    void Update()
    {
        // Check if the 'E' key is held down for swapping
        if (Input.GetKey(KeyCode.E))
        {
            HandlePowerupSwap();
        }
        else
        {
            HandleAbilityUse();
        }
    }

    void HandlePowerupSwap()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && ownedPowerups.Count > 1)
        {
            activePowerupIndex = (activePowerupIndex + 1) % ownedPowerups.Count;
            activePowerup = ownedPowerups[activePowerupIndex];
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && ownedPowerups.Count > 1)
        {
            activePowerupIndex = (activePowerupIndex - 1 + ownedPowerups.Count) % ownedPowerups.Count;
            activePowerup = ownedPowerups[activePowerupIndex];
        }
    }

    void HandleAbilityUse()
    {
        if (activePowerup != null && activePowerup.canGroundPound)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                groundPoundAbility.Activate();
            }
        }
        if (activePowerup != null && activePowerup.canDash)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                dashAbility.Activate();
            }
        }
        if (activePowerup != null && activePowerup.canWebSwing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                webSwingAbility.ShootWeb();
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                webSwingAbility.StopWeb();
            }
        }
        if (activePowerup != null && activePowerup.canPickup)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                pickupAbility.Activate();
            }
        }
    }
    
}
