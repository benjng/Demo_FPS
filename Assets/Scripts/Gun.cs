using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private Camera myCam;
    // private Ray ray;

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            Debug.Log("LMB clicked");
            Shoot();
        }
    }

    void Shoot(){
        RaycastHit hit;
        // ray = myCam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(myCam.transform.position, myCam.transform.forward, out hit, range);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(myCam.transform.position, myCam.transform.forward * Mathf.Infinity);
    }
}
