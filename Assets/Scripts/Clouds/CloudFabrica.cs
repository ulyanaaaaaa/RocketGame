using UnityEngine;

public class CloudFabrica : MonoBehaviour
{
    [SerializeField] private Rocket _rocket; 
    
    public void Setup(Rocket rocket)
    {
        _rocket = rocket;
    }
    
    public Cloud CreateCloud()
    {
        Cloud cloud = Resources.Load<Cloud>("Cloud");
        Vector3 randomPosition = new Vector3(Random.Range(-150, 150), _rocket.transform.position.y + 60, Random.Range(100, -2));
        return Instantiate(cloud, randomPosition, Quaternion.identity);
    }
    
    public BigCloud CreateBigCloud()
    {
        BigCloud cloud = Resources.Load<BigCloud>("BigCloud");
        Vector3 randomPosition = new Vector3(Random.Range(-150, 150), _rocket.transform.position.y + 60, Random.Range(100, -2));
        return Instantiate(cloud, randomPosition, Quaternion.identity);
    }
    
    public SmallCloud CreateSmallCloud()
    {
        SmallCloud cloud = Resources.Load<SmallCloud>("SmallCloud");
        Vector3 randomPosition = new Vector3(Random.Range(-150, 150), _rocket.transform.position.y + 60, Random.Range(100, -2));
        return Instantiate(cloud, randomPosition, Quaternion.identity);
    }
}
