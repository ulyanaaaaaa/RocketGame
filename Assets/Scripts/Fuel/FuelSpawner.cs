using System.Collections;
using UnityEngine;

[RequireComponent(typeof(FuelFabrica))]
public class FuelSpawner : MonoBehaviour
{
    [SerializeField] private float _delay;
    private Coroutine _spawnTick;
    private KeabordInput _input;
    private FuelFabrica _fuelFabrica;

    private void Awake()
    {
        _fuelFabrica = GetComponent<FuelFabrica>();
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
            _fuelFabrica.CreateFuel();
        }
    }
}
