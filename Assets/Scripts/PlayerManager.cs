using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public float reloadSpeed = 10f;
    
    private int ammo;


    private void Awake()
    {
        ammo = 0;        
    }


    // Public functions
    public void Shoot()
    {
        if (ammo > 0)
        {
            // Create bullet prefab
            --ammo;
        }
    }
}
