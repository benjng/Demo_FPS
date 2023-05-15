using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Pick which GunScriptableObject will use
[DisallowMultipleComponent]
public class PlayerGunSelector : MonoBehaviour
{
    [SerializeField] private GunType Gun;
    [SerializeField] private Transform GunParent;
    [SerializeField] private List<GunScriptableObject> Guns;
    // [SerializeField] private PlayerIK InverseKinematics;

    [Space]
    [Header("Runtime Filled")]
    public GunScriptableObject ActiveGun;

    private void Start() {
        GunScriptableObject gun = Guns.Find(gun => gun.Type == Gun); // Find the gun from Guns by its type

        if (gun == null){
            Debug.LogError($"No GunScriptableObject found for GunType: {gun}");
            return;
        }

        ActiveGun = gun;
        gun.Spawn(GunParent, this);

        // some IK logics
        // ...
    }
}
