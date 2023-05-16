using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
public class GunScriptableObject : ScriptableObject
{
    // public ImpactType ImpactType;
    public GunType Type;
    public string Name;
    public GameObject ModelPrefab;
    public Vector3 SpawnPoint;
    public Vector3 SpawnRotation;
    public float damage = 1f;
    public float range = 100f;
    public float bulletForce = 50f;
    public ShootConfigurationScriptableObject ShootConfig;
    public TrailConfigScriptableObject TrailConfig;

    private ParticleSystem ShootSystem;
    private MonoBehaviour ActiveMonoBehaviour;
    private GameObject Model;
    private float LastShootTime;
    private ObjectPool<TrailRenderer> TrailPool;

    public void Shoot(){
        if (Time.time > ShootConfig.FireRate + LastShootTime){
            LastShootTime = Time.time;
            ShootSystem.Play();

            // Spread the shoot
            Vector3 shootDirection = ShootSystem.transform.forward 
                + new Vector3(
                    Random.Range(
                        -ShootConfig.Spread.x,
                        ShootConfig.Spread.x
                    ),
                    Random.Range(
                        -ShootConfig.Spread.y,
                        ShootConfig.Spread.y
                    ),
                    Random.Range(
                        -ShootConfig.Spread.z,
                        ShootConfig.Spread.z
                    )
                );
            shootDirection.Normalize();

            // if target hit (shoots from the particle effect towards shootDirection)
            if (Physics.Raycast(ShootSystem.transform.position, shootDirection, out RaycastHit hit, range, ShootConfig.HitMask)){

                Target target = hit.transform.GetComponent<Target>();
                if (target != null){
                    target.TakeDamage(damage);
                    if (hit.rigidbody != null){
                        hit.rigidbody.AddForce(-hit.normal * bulletForce);
                    }
                }

                ActiveMonoBehaviour.StartCoroutine(
                    PlayTrail( // Show the trail
                        ShootSystem.transform.position,
                        hit.point, // the impact point position
                        hit // the target hit
                    )
                );
            } else { // if no target is hit
                ActiveMonoBehaviour.StartCoroutine(
                    PlayTrail( // Show the trail
                        ShootSystem.transform.position,
                        ShootSystem.transform.position + (shootDirection * TrailConfig.MissDistance),
                        new RaycastHit()
                    )
                );
            }
        }
    }

    public void Spawn(Transform Parent, MonoBehaviour ActiveMonoBehaviour){
        this.ActiveMonoBehaviour = ActiveMonoBehaviour;
        LastShootTime = 0; // reset in build
        TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);

        Model = Instantiate(ModelPrefab);
        Model.transform.SetParent(Parent, false);
        Model.transform.localPosition = SpawnPoint;
        Model.transform.localRotation = Quaternion.Euler(SpawnRotation);

        ShootSystem = Model.GetComponentInChildren<ParticleSystem>();
    }

    // Trail rendering
    private IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit Hit){
        TrailRenderer instance = TrailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = StartPoint;
        yield return null; // avoid position carry-over from last frame if reused
        
        instance.emitting = true;

        // Move the TrailRenderer instance
        float distance = Vector3.Distance(StartPoint, EndPoint);
        float remainingDistance = distance;

        // Loop (Move) till it reaches Endpoint using Lerp
        while(remainingDistance > 0){
            instance.transform.position = Vector3.Lerp(
                StartPoint,
                EndPoint,
                Mathf.Clamp01(1 - (remainingDistance / distance))
            );
            remainingDistance -= TrailConfig.SimulationSpeed * Time.deltaTime;
            yield return null;
        }

        // Finish
        instance.transform.position = EndPoint; // Make sure it reaches EndPoint

        // Visuals on impact
        // if (Hit.collider != null){
            // SurfaceManager.Instance.HandleImpact{
            //     Hit.transform.gameObject,
            //     EndPoint,
            //     Hit.normal,
            //     ImpactType,
            //     0
            // };
        // }

        yield return new WaitForSeconds(TrailConfig.Duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        TrailPool.Release(instance);
    }

    // Create a trail instance
    private TrailRenderer CreateTrail(){
        GameObject instance = new GameObject("BulletTrail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = TrailConfig.Color;
        trail.material = TrailConfig.Material;
        trail.widthCurve = TrailConfig.WidthCurve;
        trail.time = TrailConfig.Duration;
        trail.minVertexDistance = TrailConfig.MinVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }
}
