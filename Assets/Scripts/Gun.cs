using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float bulletForce = 30f;
    [SerializeField] private float fireRate = 15f;
    [SerializeField] private Camera myCam;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject impactEffect;
    private float fireInterval = 0f;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= fireInterval) {
            fireInterval = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot(){
        Debug.Log("SHOOTING with old gun");
        RaycastHit hit;

        // if hit
        if (Physics.Raycast(myCam.transform.position, myCam.transform.forward, out hit, range)){
            // avoid hitting self 
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player")) return;

            muzzleFlash.Play();
            // Debug.Log(hit.collider.name);
            Target target = hit.transform.GetComponent<Target>();
            if (target != null){
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null){
                hit.rigidbody.AddForce(-hit.normal * bulletForce);
            }

            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 2f);
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(myCam.transform.position, myCam.transform.forward * range);
    }
}
