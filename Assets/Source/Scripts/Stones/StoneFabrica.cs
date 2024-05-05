using UnityEngine;

public class StoneFabrica : MonoBehaviour
{
    [SerializeField] private Rocket _rocket; 
    
    public void Setup(Rocket rocket)
    {
        _rocket = rocket;
    }

    public Stone CreateStone()
    {
        Stone stone = Resources.Load<Stone>(AssetsPath.StonesPath.Stone);
        Vector3 randomPosition = new Vector3(Random.Range(-150, 150), _rocket.transform.position.y + 60, _rocket.transform.position.z);
        return Instantiate(stone, randomPosition, Quaternion.identity);
    }
    
    public Stone CreateSmallStone()
    {
        Stone stone = Resources.Load<Stone>(AssetsPath.StonesPath.SmallStone);
        Vector3 randomPosition = new Vector3(Random.Range(-150, 150), _rocket.transform.position.y + 60, _rocket.transform.position.z);
        return Instantiate(stone, randomPosition, Quaternion.identity);
    }
    
    public Stone CreateBigStone()
    {
        Stone stone = Resources.Load<Stone>(AssetsPath.StonesPath.BigStone);
        Vector3 randomPosition = new Vector3(Random.Range(-150, 150), _rocket.transform.position.y + 60, _rocket.transform.position.z);
        return Instantiate(stone, randomPosition, Quaternion.identity);
    }
    
    public Stone CreateMiddleStone()
    {
        Stone stone = Resources.Load<Stone>(AssetsPath.StonesPath.MiddleStone);
        Vector3 randomPosition = new Vector3(Random.Range(-150, 150), _rocket.transform.position.y + 60, _rocket.transform.position.z);
        return Instantiate(stone, randomPosition, Quaternion.identity);
    }
    
    public Stone CreateWidthStone()
    {
        Stone stone = Resources.Load<Stone>(AssetsPath.StonesPath.WidthStone);
        Vector3 randomPosition = new Vector3(Random.Range(-150, 150), _rocket.transform.position.y + 60, _rocket.transform.position.z);
        return Instantiate(stone, randomPosition, Quaternion.identity);
    }
}
