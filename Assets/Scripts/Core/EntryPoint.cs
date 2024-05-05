using System.Collections;
using UnityEngine;

public class EntryPoint : MonoBehaviour, IPause
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Transform _rocketStartPoint;

    [SerializeField] private RectTransform _fuelShopPosition;
    [SerializeField] private RectTransform _speedShopPosition;
    [SerializeField] private RectTransform _bestScorePosition;
    [SerializeField] private RectTransform _moneyCounterPosition;
    [SerializeField] private RectTransform _maxFuelPosition;
    [SerializeField] private RectTransform _scoreCounterPosition;
    [SerializeField] private RectTransform _fuelCounterPosition;
    [SerializeField] private RectTransform _fuelBarPosition;
    [SerializeField] private RectTransform _pausePosition;
    [SerializeField] private RectTransform _resumeMenuPosition;
    [SerializeField] private RectTransform _speedCounterPosition;

    private PauseService _pauseService;
    private Pause _pause;
    private Pause _pauseCreated;
    private ResumeMenu _resume;
    private ResumeMenu _resumeCreated;
    private KeabordInput _input;
    private StoneFabrica _stoneFabrica;
    private StoneFabrica _stoneFabricaCreated;
    private MoneyFabrica _moneyFabrica;
    private MoneyFabrica _moneyFabricaCreated;
    private FuelFabrica _fuelFabrica;
    private FuelFabrica _fuelFabricaCreated;
    private CloudFabrica _cloudFabrica;
    private CloudFabrica _cloudFabricaCreated;
    private Rocket _rocket;
    private Rocket _rocketCreated;
    private FailWindow _failWindow;
    private FailWindow _failWindowCreated;
    private FuelBar _fuelBar;
    private FuelBar _fuelBarCreated;
    private ShopSpeedItem _shopSpeedItem;
    private ShopSpeedItem _shopSpeedItemCreated;
    private ShopFuelItem _shopFuelItem;
    private ShopFuelItem _shopFuelItemCreated;
    private MoneyCounter _moneyCounter;
    private MoneyCounter _moneyCounterCreated;
    private FuelCounter _fuelCounter;
    private FuelCounter _fuelCounterCreated;
    private BestScore _bestScore;
    private BestScore _bestScoreCreated;
    private ScoreCounter _scoreCounter;
    private ScoreCounter _scoreCounterCreated;
    private SpeedCounter _speedCounter;
    private SpeedCounter _speedCounterCreated;
    private InvisibleWall _invisibleWall;
    private InvisibleWall _invisibleLeftWallCreated;
    private InvisibleWall _invisibleRightWallCreated;
    private Coroutine _createWallTick;

    private void Awake()
    {
        _pauseService = GetComponent<PauseService>();
        _input = GetComponent<KeabordInput>();
        _rocket = Resources.Load<Rocket>("Rocket");
        _speedCounter = Resources.Load<SpeedCounter>("SpeedCounter");
        _failWindow = Resources.Load<FailWindow>("FailWindow"); 
        _fuelBar = Resources.Load<FuelBar>("FuelBar");
        _pause = Resources.Load<Pause>("Pause");
        _resume = Resources.Load<ResumeMenu>("ResumeMenu");
        _moneyFabrica = Resources.Load<MoneyFabrica>("MoneyFabrica");
        _cloudFabrica = Resources.Load<CloudFabrica>("CloudFabrica");
        _fuelFabrica = Resources.Load<FuelFabrica>("FuelFabrica");
        _shopSpeedItem = Resources.Load<ShopSpeedItem>("ItemSpeed");
        _shopFuelItem = Resources.Load<ShopFuelItem>("ItemFuel");
        _stoneFabrica = Resources.Load<StoneFabrica>("StoneFabrica");
        _moneyCounter = Resources.Load<MoneyCounter>("MoneyCounter");
        _invisibleWall = Resources.Load<InvisibleWall>("InvisibleWall");
        _fuelCounter = Resources.Load<FuelCounter>("FuelCounter");
        _bestScore = Resources.Load<BestScore>("BestScore");
        _scoreCounter = Resources.Load<ScoreCounter>("ScoreCounter");
        CreateMoneyCounter();
        CreateFuelCounter();
        CreateRocket();
        CreateUI();
        _pauseService.AddPause(this);
        _input.OnPlay += CreateSpawners;
        _input.OnPlay += CreateScoreCounter;
        _input.OnPlay += DisableShop;
        _input.OnPlay += FuelBarCreated;
        _input.OnPlay += CreateInvisibleWall;
        _input.OnPlay += DisableFuel;
        _input.OnPlay += DisableBestScoreUI;
        _input.OnPlay += CreatePause;
    }

    private void CreateUI()
    {
       CreateFailWindow();
       CreateShopFuelItem();
       CreateSpeedFuelItem();
       CreateBestScore();
       CreateSpeedCounter();
    }

    private void CreateSpeedCounter()
    {
        _speedCounterCreated = Instantiate(_speedCounter,
            _speedCounterPosition.GetComponent<RectTransform>().position,
            Quaternion.identity,
            _canvas.transform);
        _rocketCreated.Setup(_input, _moneyCounterCreated, _fuelCounterCreated, _pauseService, _speedCounterCreated);
        _speedCounterCreated.transform.position = _speedCounterPosition.GetComponent<RectTransform>().position;
        _input.OnPlay += () => Destroy(_speedCounterCreated.gameObject);
    }

    private void CreatePause()
    {
        _pauseCreated = Instantiate(_pause,
            _pausePosition.GetComponent<RectTransform>().position,
            Quaternion.identity,
            _canvas.transform);
        _pauseCreated.Setup(GetComponent<PauseService>());
        _pauseCreated.transform.position = _pausePosition.GetComponent<RectTransform>().position;
        _pauseCreated.OnPause += CreateResume;
        _pauseCreated.OnPause += () => Destroy(_pauseCreated.gameObject);
    }

    private void CreateResume()
    {
        _resumeCreated = Instantiate(_resume,
            _resumeMenuPosition.GetComponent<RectTransform>().position, 
            Quaternion.identity,
            _canvas.transform);
        _resumeCreated.transform.position = _resumeMenuPosition.GetComponent<RectTransform>().position;
        _resumeCreated.GetComponentInChildren<Resume>().Setup(_pauseService);
        _resumeCreated.GetComponentInChildren<Resume>().OnResume += CreatePause;
        _resumeCreated.GetComponentInChildren<Resume>().OnResume += () => Destroy(_resumeCreated.gameObject);
    }

    private void CreateBestScore()
    {
        _bestScoreCreated = Instantiate(_bestScore,
            _bestScorePosition.GetComponent<RectTransform>().position,
            Quaternion.identity,
            _canvas.transform);
        _bestScoreCreated.Setup(_rocketCreated);
        _bestScoreCreated.transform.position = _bestScorePosition.GetComponent<RectTransform>().position;
    }

    private void CreateScoreCounter()
    {
        _scoreCounterCreated = Instantiate(_scoreCounter,
            _scoreCounterPosition.GetComponent<RectTransform>().position,
            Quaternion.identity,
            _canvas.transform);
        _scoreCounterCreated.Setup(_rocketCreated);
        _scoreCounterCreated.transform.position = _scoreCounterPosition.GetComponent<RectTransform>().position;
    }

    private void CreateFailWindow()
    {
        _failWindowCreated = Instantiate(_failWindow, 
            _failWindow.GetComponent<RectTransform>().localPosition, 
            Quaternion.identity, 
            _canvas.transform);
        _failWindowCreated.GetComponent<RectTransform>().localPosition = Vector3.zero;
        _failWindowCreated.Setup(_rocketCreated);
    }

    private void CreateSpeedFuelItem()
    {
        _shopSpeedItemCreated = Instantiate(_shopSpeedItem, 
            _speedShopPosition.GetComponent<RectTransform>().position, 
            Quaternion.identity, 
            _canvas.transform);
        _shopSpeedItemCreated.transform.position = _speedShopPosition.GetComponent<RectTransform>().position;
        _shopSpeedItemCreated.Setup(_rocketCreated);
        _shopSpeedItemCreated.GetComponent<ShopSpeedItemViewer>().Setup(_rocketCreated);
    }

    private void CreateShopFuelItem()
    {
        _shopFuelItemCreated = Instantiate(_shopFuelItem, 
            _fuelShopPosition.GetComponent<RectTransform>().position, 
            Quaternion.identity, 
            _canvas.transform);
        _shopFuelItemCreated.Setup(_rocketCreated);
        _shopFuelItemCreated.GetComponent<ShopFuelItemViewer>().Setup(_rocketCreated);
        _shopFuelItemCreated.transform.position = _fuelShopPosition.GetComponent<RectTransform>().position;
    }

    private void CreateFuelCounter()
    {
        _fuelCounterCreated = Instantiate(_fuelCounter,
            _fuelCounterPosition.GetComponent<RectTransform>().position, 
            Quaternion.identity, 
            _canvas.transform);
        _fuelCounterCreated.transform.position = _fuelCounterPosition.GetComponent<RectTransform>().position;
    }

    private void CreateMoneyCounter()
    {
        _moneyCounterCreated = Instantiate(_moneyCounter,
            _moneyCounterPosition.GetComponent<RectTransform>().position, 
            Quaternion.identity, 
            _canvas.transform);
        _moneyCounterCreated.transform.position = _moneyCounterPosition.GetComponent<RectTransform>().position;
    }

    private void CreateInvisibleWall()
    {
        _createWallTick = StartCoroutine(CreateWallTick());
    }

    private void CreateRocket()
    {
        _rocketCreated = Instantiate(_rocket, _rocketStartPoint.position, Quaternion.identity);
        _input.Setup(_rocketCreated);
    }

    private void CreateSpawners()
    {
        _stoneFabricaCreated = Instantiate(_stoneFabrica);
        _stoneFabricaCreated.Setup(_rocketCreated);
        _moneyFabricaCreated = Instantiate(_moneyFabrica);
        _moneyFabricaCreated.Setup(_rocketCreated);
        _cloudFabricaCreated = Instantiate(_cloudFabrica);
        _cloudFabricaCreated.Setup(_rocketCreated);
        _fuelFabricaCreated = Instantiate(_fuelFabrica);
        _fuelFabricaCreated.Setup(_rocketCreated);
        _cloudFabricaCreated.GetComponent<CloudSpawner>().Setup(_pauseService);
        _fuelFabricaCreated.GetComponent<FuelSpawner>().Setup(_pauseService);
        _moneyFabricaCreated.GetComponent<MoneySpawner>().Setup(_pauseService);
        _stoneFabricaCreated.GetComponent<StoneSpawner>().Setup(_pauseService);
    }
    
    private void FuelBarCreated()
    {
        _fuelBarCreated = Instantiate(_fuelBar, 
            _fuelBarPosition.GetComponent<RectTransform>().position, 
            Quaternion.identity, 
            null);
        _fuelBarCreated.transform.SetParent(_canvas.transform, false);
        _fuelBarCreated.Setup(_rocketCreated, _pauseService);
        _fuelBarCreated.transform.position = _fuelBarPosition.GetComponent<RectTransform>().position;
    }

    private void DisableShop()
    {
        _shopSpeedItemCreated.gameObject.SetActive(false);
        _shopFuelItemCreated.gameObject.SetActive(false);
    }

    private void DisableFuel()
    {
        _fuelCounterCreated.gameObject.SetActive(false);
    }
    
    private void DisableBestScoreUI()
    {
        _bestScoreCreated.gameObject.SetActive(false);
    }
    
    private IEnumerator CreateWallTick()
    {
        while (true)
        {
            _invisibleLeftWallCreated = Instantiate(_invisibleWall,
                new Vector3(-40, _rocketCreated.transform.position.y, _rocketCreated.transform.position.z), 
                Quaternion.identity, 
                _canvas.transform);
            _invisibleRightWallCreated = Instantiate(_invisibleWall,
                new Vector3(70, _rocketCreated.transform.position.y, _rocketCreated.transform.position.z), 
                Quaternion.identity, 
                _canvas.transform);
            yield return new WaitForSeconds(1);
        }
    }

    public void Pause()
    {
        StopCoroutine(_createWallTick);
    }

    public void Resume()
    {
        _createWallTick = StartCoroutine(CreateWallTick());
    }
}
