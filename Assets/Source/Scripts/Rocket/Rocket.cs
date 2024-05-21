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
    [field:SerializeField] public float Speed{ get; private set; }

    [SerializeField] private Transform _patronSpawnPosition;
    private string _id = "rocket";
    private Rigidbody _rigidbody;
    private Patron _patron;
    private PlayerInput _playerInput;
    private Coroutine _fuelTick;
    private MoneyCounter _moneyCounter;
    private FuelCounter _fuelCounter;
    private ISaveService _saveService;
    private PauseService _pauseService;
    private SpeedCounter _speedCounter;
    private Touch touch;
    private bool _isPlay;
    private float _tempSpeed;
    private bool _canShoot = true;
    private bool _isPause;
    
    public void Setup(PlayerInput playerInput, MoneyCounter moneyCounter, FuelCounter fuelCounter, 
        PauseService pauseService, SpeedCounter speedCounter)
    {
        _speedCounter = speedCounter;
        _pauseService = pauseService;
        _playerInput = playerInput;
        _moneyCounter = moneyCounter;
        _fuelCounter = fuelCounter;
    }

    private void Awake()
    {
        _patron = Resources.Load<Patron>(AssetsPath.RocketPath.Patron);
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _pauseService.AddPause(this);
        _saveService = new SaveService();
        
        if (!_saveService.Exists(_id))
        {
            Wallet = 0;
            MaxFuel = 10;
            Speed = 10;
            Save();
        }
        else
            Load(); 
        
        Fuel = MaxFuel;
        _moneyCounter.CurrentMoney();
        _fuelCounter.CurrentMaxFuel();
        _speedCounter.CurrentSpeed();
        
        _playerInput.OnPlay += StartPlay;
        _playerInput.OnShoot += Shoot;
        _playerInput.OnLeftClicked += TurnLeft;
        _playerInput.OnRightClicked += TurnRight;
    }

    private void OnEnable()
    {
        OnDie += Save;
        OnDie += () => _pauseService.Pause();
    }

    private void Update()
    {
        if (!_isPlay)
            return;

        if (_isPause)
            return;

        _rigidbody.velocity = Vector3.up * Speed;
        
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 touchDeltaPosition = touch.deltaPosition;
                transform.Translate(touchDeltaPosition.x, Time.deltaTime/100, 0, 0);
            }
        }
    }

    public void AddSpeed(float speed)
    {
        Speed += speed;
        _speedCounter.CurrentSpeed();
    }

    public void AddFuel(float fuel)
    {
        MaxFuel += fuel;
        _fuelCounter.CurrentMaxFuel();
    }

    public void AddMoney(int count)
    {
        Wallet += count;
        _moneyCounter.CurrentMoney();
        _isPlay = false;
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

    public void Continue()
    {
        transform.position = new Vector3(0, transform.position.y + 3, 0);
        transform.rotation = Quaternion.identity;
        _pauseService.Resume();
        _isPlay = true;
        _fuelTick = StartCoroutine(FuelTick());
    }

    private void StartPlay()
    {
        _isPlay = true;
        _fuelTick = StartCoroutine(FuelTick());
    }

    private void Shoot()
    {
        if (_canShoot && _isPlay)
        {
            Instantiate(_patron, _patronSpawnPosition.position, Quaternion.identity).Setup(this);
            StartCoroutine(ShootTick());
        }
    }

    private IEnumerator ShootTick()
    {
        _canShoot = false;
        yield return new WaitForSeconds(0.3f);
        _canShoot = true;
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
        _moneyCounter.CurrentMoney();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Money money))
        {
            Wallet += money.Resources;
            _moneyCounter.CurrentMoney();
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
    
    private void OnDisable()
    {
        OnDie -= Save;
        _playerInput.OnPlay -= StartPlay;
        _playerInput.OnShoot -= Shoot;
        _playerInput.OnLeftClicked -= TurnLeft;
        _playerInput.OnRightClicked -= TurnRight;
        OnDie -= () => _pauseService.Pause();
    }

    public void Pause()
    {
        _isPause = true;
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        StopCoroutine(_fuelTick);
    }

    public void Resume()
    {
        _isPause = false;
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