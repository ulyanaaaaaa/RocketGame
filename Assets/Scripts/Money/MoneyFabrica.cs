using UnityEngine;

public class MoneyFabrica : MonoBehaviour
{
    [SerializeField] private Rocket _rocket;

    public void Setup(Rocket rocket)
    {
        _rocket = rocket;
    }
    
    public Money CreateMoney()
    {
        Money money = Resources.Load<Money>("Money");
        Vector3 randomPosition = new Vector3(Random.Range(-22, 14), _rocket.transform.position.y + 30, _rocket.transform.position.z);
        return Instantiate(money, randomPosition, Quaternion.Euler(0, -90f, 90f));
    }
}
