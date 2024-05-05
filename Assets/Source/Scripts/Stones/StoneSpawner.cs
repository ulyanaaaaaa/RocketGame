using System.Collections;
using UnityEngine;

[RequireComponent(typeof(StoneFabrica))]
public class StoneSpawner : MonoBehaviour, IPause
{
    [SerializeField] private float _delay;
    private Coroutine _spawnTick;
    private PauseService _pauseService;
    private StoneFabrica _stoneFabrica;

    public void Setup(PauseService pauseService)
    {
        _pauseService = pauseService;
    }

    private void Awake()
    {
        _stoneFabrica = GetComponent<StoneFabrica>();
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
            _stoneFabrica.CreateStone();
            _stoneFabrica.CreateBigStone();
            _stoneFabrica.CreateSmallStone();
            _stoneFabrica.CreateWidthStone();
            _stoneFabrica.CreateMiddleStone();
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
