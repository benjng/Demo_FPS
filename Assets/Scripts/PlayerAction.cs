using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    [SerializeField] private PlayerGunSelector GunSelector;
    private void Update(){
        if (Input.GetButton("Fire1") && GunSelector.ActiveGun != null)
            GunSelector.ActiveGun.Shoot();
    }
}
