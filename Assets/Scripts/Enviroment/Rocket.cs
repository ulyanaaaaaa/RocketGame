using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rocket : MonoBehaviour, IPause
{
    public Action OnDie;

    [field: SerializeField] public float MaxFuel { get; private set; }
    [field: SerializeField] public float Fuel { get; private set; }
    public int Wallet { get; private set; }
    public float Speed{ get; private set; }

    [SerializeField] private Transform _patronSpawnPosition;
    private string _id = "rocket";
    private Rigidbody _rigidbody;
    private Patron _patron;
    private KeabordInput _input;
    private Coroutine _fuelTick;
    private MoneyCounter _moneyCounter;
    private FuelCounter _fuelCounter;
    private bool _isPlay;
    private float _tempSpeed;
    private ISaveService _saveService;
    private PauseService _pauseService;
    private SpeedCounter _speedCounter;
    
    public void Setup(KeabordInput input, MoneyCounter moneyCounter, FuelCounter fuelCounter, PauseService pauseService,
        SpeedCounter speedCounter)
    {
        _speedCounter = speedCounter;
        _pauseService = pauseService;
        _input = input;
        _moneyCounter = moneyCounter;
        _fuelCounter = fuelCounter;
    }

    private void Awake()
    {
        _patron = Resources.Load<Patron>("Patron");
    }

    private void Start()
    {
        _pauseService.AddPause(this);
        _saveService = new SaveService();
        
        if (!_saveService.Exists(_id))
        {
            Wallet = 0;
            MaxFuel = 50;
            Speed = 10;
            Save();
        }
        else
            Load(); 
        
        Fuel = MaxFuel;
        _moneyCounter.CurrentMoney(Wallet);
        _fuelCounter.CurrentMaxFuel(MaxFuel);
        _speedCounter.CurrentSpeed(Speed);
        _rigidbody = GetComponent<Rigidbody>();
        _input.OnPlay += StartPlay;
        _input.OnShoot += Shoot;
        OnDie += () => _pauseService.Pause();
    }

    private void Update()
    {
        if (_isPlay)
            _rigidbody.velocity = Vector3.up * Speed;
    }
    
    public void AddSpeed(float speed)
    {
        Speed += speed;
        _speedCounter.CurrentSpeed(Speed);
    }

    public void AddFuel(float fuel)
    {
        MaxFuel += fuel;
        _fuelCounter.CurrentMaxFuel(MaxFuel);
    }

    public bool TrySpend(int amount)
    {
        if (amount < 0)
            throw new ArgumentException("Value must be positive!");

        if (amount > Wallet)
        {
            return false;
        }
        else
        {
            RemoveValue(amount);
            return true;
        }
    }

    private void StartPlay()
    {
        _input.OnLeftClicked += TurnLeft;
        _input.OnRightClicked += TurnRight;
        _isPlay = true;
        _fuelTick = StartCoroutine(FuelTick());
    }

    private void Shoot()
    {
        Instantiate(_patron, _patronSpawnPosition.position, Quaternion.identity).Setup(this);
    }
    
    private void TurnLeft()
    {
        _rigidbody.velocity = (Vector3.left + Vector3.up) * Speed;
    }
    
    private void TurnRight()
    {
        _rigidbody.velocity = (Vector3.right + Vector3.up) * Speed;
    }
    
    private void Save()
    {
        RocketSaveData data = new RocketSaveData();
        data.Wallet = Wallet;
        data.MaxFuel = MaxFuel;
        data.Speed = Speed;
        _saveService.Save(_id, data);
    }

    private void Load()
    {
        _saveService.Load<RocketSaveData>(_id, data =>
        {
            Wallet = data.Wallet;
            MaxFuel = data.MaxFuel;
            Speed = data.Speed;
        });
    }
    
    private void RemoveValue(int amount)
    {
        if (amount < 0)
            throw new ArgumentException("Value must be positive!");
        
        Wallet -= amount;
        _moneyCounter.CurrentMoney(Wallet);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Money money))
        {
            Wallet += money.Resources;
            _moneyCounter.CurrentMoney(Wallet);
            money.gameObject.SetActive(false);
        }
        
        if (collider.gameObject.TryGetComponent(out Fuel fuel))
        {
            Fuel += fuel.Count;
            fuel.gameObject.SetActive(false);
        }
    }

    private IEnumerator FuelTick()
    {
        while (true)
        {
            Fuel--;
            yield return new WaitForSeconds(1);

            if (Fuel == 0)
            {
                Save();
                OnDie?.Invoke();
                break;
            }
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Stone>())
        {
            OnDie?.Invoke();
        }
    }

    public void Pause()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        StopCoroutine(_fuelTick);
    }

    public void Resume()
    {
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _fuelTick = StartCoroutine(FuelTick());
    }
}

public class RocketSaveData
{
    public int Wallet;
    public float MaxFuel;
    public float Speed;
}