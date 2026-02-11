using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody rb;
    public float linearDamping = 2.0f;
    public float angularDamping = 2.0f;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearDamping = linearDamping;
        rb.angularDamping = angularDamping;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Fling(Vector3 direction, float forceStrength)
    {
        rb.AddForce(direction * forceStrength, ForceMode.Impulse);
    }
}
