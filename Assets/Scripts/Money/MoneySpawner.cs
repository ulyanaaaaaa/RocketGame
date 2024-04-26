using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MoneyFabrica))]
public class MoneySpawner : MonoBehaviour
{
    [SerializeField] private float _delay;
    private Coroutine _spawnTick;
    private KeabordInput _input;
    private MoneyFabrica _moneyFabrica;

    private void Awake()
    {
        _moneyFabrica = GetComponent<MoneyFabrica>();
        _input = GetComponent<KeabordInput>();
    }

    private void Start()
    {
        _spawnTick = StartCoroutine(SpawnTick());
    }

    private IEnumerator SpawnTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(_delay);
            _moneyFabrica.CreateMoney();
        }
    }
}
