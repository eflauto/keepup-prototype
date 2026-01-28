using UnityEngine;

public class DartManager : MonoBehaviour
{
    public static bool hasHitDartBoard = false;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("DartBoard"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            Destroy(rb);
            hasHitDartBoard = true;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
