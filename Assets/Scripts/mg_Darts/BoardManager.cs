using UnityEngine;

public class BoardManager : MonoBehaviour
{   
    public static bool hasBeenHit = false;
    private Vector3 _startPosition;
    
    void Start()
    {
        _startPosition = transform.position;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Dart"))
        {
            hasBeenHit = true;
        }
    }
    void Update()
    {
        if (!hasBeenHit) 
        {
            float rebound = Mathf.PingPong(Time.time * 2f, 2.0f);
            transform.position = _startPosition + new Vector3(rebound, 0, 0);
        }
    }
}
