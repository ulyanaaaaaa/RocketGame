using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MoneyFabrica))]
public class MoneySpawner : MonoBehaviour, IPause
{
    [SerializeField] private float _delay;
    private Coroutine _spawnTick;
    private KeabordInput _input;
    private PauseService _pauseService;
    private MoneyFabrica _moneyFabrica;
    
    public void Setup(PauseService pauseService)
    {
        _pauseService = pauseService;
    }

    private void Awake()
    {
        _moneyFabrica = GetComponent<MoneyFabrica>();
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
            _moneyFabrica.CreateMoney();
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
