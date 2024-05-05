using UnityEngine;

public class FuelFabrica : MonoBehaviour
{
    [SerializeField] private Rocket _rocket;

    public void Setup(Rocket rocket)
    {
        _rocket = rocket;
    }
    
    public Fuel CreateFuel()
    {
        Fuel fuel = Resources.Load<Fuel>(AssetsPath.FuelPath.Fuel);
        Vector3 randomPosition = new Vector3(Random.Range(-22, 14), _rocket.transform.position.y + 30, _rocket.transform.position.z);
        return Instantiate(fuel, randomPosition, Quaternion.Euler(0, -90f, 90f));
    }
}
