using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Jobs;
using UnityEngine;

public class gamejam : MonoBehaviour
{
    public int playerHealth = 100;
    public bool hasShield = false;
    public bool isInvulnerable = false;

    private void Update()
    {
        // Calls immediately for defeat to prevent deeper nesting
        if (playerHealth <= 0)
        {
            Debug.Log("Player is defeated");
            return;
        }

        if (playerHealth > 0 && isInvulnerable)
        {
            Debug.Log("Player is invulnerable. Health remains unchanged.");
            return;
        }

        // Placed the conditions together instead of nesting

        if (playerHealth > 0 && !isInvulnerable && hasShield)
        {
            Debug.Log("Player has a shield. Health is protected");
        }
        else if (playerHealth > 0 && !isInvulnerable && !hasShield)
        {
            Debug.Log("Player has no shield. Health is vulnerable");
            playerHealth -= 10;
        }
    }

}
    