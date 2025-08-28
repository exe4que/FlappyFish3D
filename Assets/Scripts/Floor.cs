using UnityEngine;

public class Floor : MonoBehaviour
{
    public Transform Fishy;
    private float _zOffset;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _zOffset = transform.position.z - Fishy.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, Fishy.position.z + _zOffset);
    }
}
