using UnityEngine;

public class GravityPull : MonoBehaviour
{
    public float gravityStrength = 50.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null )
        {
            Vector3 gravityDirection = transform.position - rb.position;
            float distance = gravityDirection.magnitude;
            rb.AddForce(gravityDirection.normalized * gravityStrength / distance);
        }
    }
}
