using UnityEngine;

public class Patron : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Rocket _rocket;
    
    public Patron Setup(Rocket rocket)
    {
        _rocket = rocket;
        return this;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        _rigidbody.velocity = Vector3.up * (_rocket.Speed + 0.5f * _rocket.Speed);
    }
}
