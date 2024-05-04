using System.Collections;
using UnityEngine;

[RequireComponent(typeof(FuelFabrica))]
public class FuelSpawner : MonoBehaviour, IPause
{
    [SerializeField] private float _delay;
    private Coroutine _spawnTick;
    private PauseService _pauseService;
    private KeabordInput _input;
    private FuelFabrica _fuelFabrica;
    
    public void Setup(PauseService pauseService)
    {
        _pauseService = pauseService;
    }

    private void Awake()
    {
        _fuelFabrica = GetComponent<FuelFabrica>();
        _input = GetComponent<KeabordInput>();
    }

    private void Start()
    {
        _pauseService.AddPause(this);
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

    public void Pause()
    {
        StopCoroutine(_spawnTick);
    }

    public void Resume()
    {
        _spawnTick = StartCoroutine(SpawnTick());
    }
}
