using System.Collections;
using UnityEngine;

public class EntryPoint : MonoBehaviour, IPause
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Transform _rocketStartPoint;

    [SerializeField] private RectTransform _fuelShopPosition;
    [SerializeField] private RectTransform _speedShopPosition;
    [SerializeField] private RectTransform _scoreCounterPosition;
    [SerializeField] private RectTransform _fuelBarPosition;
    [SerializeField] private RectTransform _pausePosition;
    [SerializeField] private RectTransform _resumeMenuPosition;
    [SerializeField] private RectTransform _menuPosition;
    [SerializeField] private RectTransform _inscriptionPosition;
    [SerializeField] private RectTransform _translateButtonPosition;
    [SerializeField] private RectTransform _languageMenuPosition;
    [SerializeField] private RectTransform _moneyCounterPosition;
    [SerializeField] private RectTransform _adsItemPosition;
    [SerializeField] private RectTransform _moneyInfoPosition;

    private LanguageMenu _languageMenu;
    private LanguageMenu _languageMenuCreated;
    private TranslateButton _translateButton;
    private TranslateButton _translateButtonCreated;
    private PlayInscription _inscription;
    private PlayInscription _inscriptionCreated;
    private PauseService _pauseService;
    private Pause _pause;
    private Pause _pauseCreated;
    private ResumeMenu _resume;
    private ResumeMenu _resumeCreated;
    private PlayerInput _playerInput;
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
    private FuelBar _fuelBar;
    private FuelBar _fuelBarCreated;
    private ShopSpeedItem _shopSpeedItem;
    private ShopSpeedItem _shopSpeedItemCreated;
    private ShopFuelItem _shopFuelItem;
    private ShopFuelItem _shopFuelItemCreated;
    private MoneyCounter _moneyCounterCreated;
    private FuelCounter _fuelCounterCreated;
    private BestScore _bestScoreCreated;
    private ScoreCounter _scoreCounter;
    private ScoreCounter _scoreCounterCreated;
    private SpeedCounter _speedCounterCreated;
    private InvisibleWall _invisibleWall;
    private InvisibleWall _invisibleLeftWallCreated;
    private InvisibleWall _invisibleRightWallCreated;
    private Coroutine _createWallTick;
    private Menu _menu;
    private Menu _menuCreated;
    private FailWindow _failWindow;
    private FailWindow _failWindowCreated;
    private Translator _translator;
    private MoneyCounter _moneyCounter;
    private AdsItem _adsItem;
    private AdsItem _adsItemCreated;
    private MoneyInfo _moneyInfo;
    private MoneyInfo _moneyInfoCreated;

    private void Awake()
    {
        _pauseService = GetComponent<PauseService>();
        _playerInput = GetComponent<PlayerInput>();
        _translator = GetComponent<Translator>();
        _moneyInfo = Resources.Load<MoneyInfo>(AssetsPath.UiPath.MoneyInfo);
        _adsItem = Resources.Load<AdsItem>(AssetsPath.ShopPath.ItemAds);
        _moneyCounter = Resources.Load<MoneyCounter>(AssetsPath.MoneyPath.MoneyCounter);
        _languageMenu = Resources.Load<LanguageMenu>(AssetsPath.UiPath.LanguageMenu);
        _translateButton = Resources.Load<TranslateButton>(AssetsPath.UiPath.TranslateButton);
        _menu = Resources.Load<Menu>(AssetsPath.UiPath.Menu);
        _inscription = Resources.Load<PlayInscription>(AssetsPath.UiPath.Inscription);
        _rocket = Resources.Load<Rocket>(AssetsPath.RocketPath.Rocket);
        _fuelBar = Resources.Load<FuelBar>(AssetsPath.FuelPath.FuelBar);
        _pause = Resources.Load<Pause>(AssetsPath.UiPath.Pause);
        _resume = Resources.Load<ResumeMenu>(AssetsPath.UiPath.ResumeMenu);
        _failWindow = Resources.Load<FailWindow>(AssetsPath.UiPath.FailWindow); 
        _moneyFabrica = Resources.Load<MoneyFabrica>(AssetsPath.MoneyPath.MoneyFabrica);
        _cloudFabrica = Resources.Load<CloudFabrica>(AssetsPath.CloudsPath.CloudFabrica);
        _fuelFabrica = Resources.Load<FuelFabrica>(AssetsPath.FuelPath.FuelFabrica);
        _shopSpeedItem = Resources.Load<ShopSpeedItem>(AssetsPath.ShopPath.ItemSpeed);
        _shopFuelItem = Resources.Load<ShopFuelItem>(AssetsPath.ShopPath.ItemFuel);
        _stoneFabrica = Resources.Load<StoneFabrica>(AssetsPath.StonesPath.StoneFabrica);
        _invisibleWall = Resources.Load<InvisibleWall>(AssetsPath.EnvironmentPath.InvisibleWall);
        _scoreCounter = Resources.Load<ScoreCounter>(AssetsPath.UiPath.ScoreCounter);
        CreateRocket();
        CreateUI();
        _pauseService.AddPause(this);
    }

    private void Start()
    {
        _playerInput.OnPlay += CreateSpawners;
        _playerInput.OnPlay += CreateScoreCounter;
        _playerInput.OnPlay += DisableShop;
        _playerInput.OnPlay += FuelBarCreated;
        _playerInput.OnPlay += CreateInvisibleWall;
        _playerInput.OnPlay += DisableMenu;
        _playerInput.OnPlay += CreatePause;
        _playerInput.OnPlay += CreateMoneyCounter;
        _playerInput.OnPlay += CreateMoneyInfo;
    }
    
    public void Pause()
    {
        StopCoroutine(_createWallTick);
    }

    public void Resume()
    {
        _createWallTick = StartCoroutine(CreateWallTick());
    }

    private void CreateUI()
    {
       CreateFailWindow();
       CreateMoneyCounter();
       CreateMenu();
       CreateInscription();
       CreateTranslateButton();
       CreateShopFuelItem();
       CreateSpeedFuelItem();
       CreateAdsItem();
    }

    private void CreateMoneyInfo()
    {
        _moneyInfoCreated = Instantiate(_moneyInfo,
            _moneyInfoPosition.GetComponent<RectTransform>().position,
            Quaternion.identity,
            _canvas.transform);
        _moneyInfoCreated.transform.position = _moneyInfoPosition.GetComponent<RectTransform>().position;
        
        TextTranslator[] textTranslators = _moneyInfoCreated.GetComponentsInChildren<TextTranslator>();
        foreach (TextTranslator textTranslator in textTranslators)
        {
            textTranslator.Setup(_translator);
        }
    }

    private void CreateAdsItem()
    {
        _adsItemCreated = Instantiate(_adsItem, 
            _adsItemPosition.GetComponent<RectTransform>().position,
            Quaternion.identity,
            _canvas.transform);
        _adsItemCreated.transform.position = _adsItemPosition.GetComponent<RectTransform>().position;
        
        _adsItemCreated.GetComponent<AdsItem>().Setup(_rocketCreated);
        
        TextTranslator[] textTranslators = _adsItemCreated.GetComponentsInChildren<TextTranslator>();
        foreach (TextTranslator textTranslator in textTranslators)
        {
            textTranslator.Setup(_translator);
        }
    }

    private void CreateTranslateButton()
    {
        _translateButtonCreated = Instantiate(_translateButton,
            _translateButtonPosition.GetComponent<RectTransform>().position,
            Quaternion.identity,
            _canvas.transform);
        _translateButtonCreated.transform.position = _translateButtonPosition.GetComponent<RectTransform>().position;
        _translateButtonCreated.OnClick += CreateLanguageMenu;
    }

    private void CreateLanguageMenu()
    {
        _languageMenuCreated = Instantiate(_languageMenu,
            _languageMenuPosition.GetComponent<RectTransform>().position,
            Quaternion.identity,
            _canvas.transform);
        _languageMenuCreated.OnExit += () 
            => Destroy(_languageMenuCreated.gameObject);
        _languageMenuCreated.transform.position = _languageMenuPosition.GetComponent<RectTransform>().position;
        _languageMenuCreated.Setup(_translator);
    }

    private void CreateInscription()
    {
        _inscriptionCreated = Instantiate(_inscription,
            _inscriptionPosition.GetComponent<RectTransform>().position,
            Quaternion.identity,
            _canvas.transform);
        _inscriptionCreated.GetComponent<TextTranslator>().Setup(_translator);
        _inscriptionCreated.transform.position = _inscriptionPosition.GetComponent<RectTransform>().position;
    }

    private void CreateMoneyCounter()
    {
        _moneyCounterCreated = Instantiate(_moneyCounter,
            _moneyCounterPosition.GetComponent<RectTransform>().position,
            Quaternion.identity,
            _canvas.transform);
        _moneyCounterCreated.Setup(_rocketCreated);
        _moneyCounterCreated.GetComponent<TextTranslator>().Setup(_translator);
        _moneyCounterCreated.transform.position = _moneyCounterPosition.GetComponent<RectTransform>().position;
    }

    private void CreateMenu()
    {
        _menuCreated = Instantiate(_menu,
            _menuPosition.GetComponent<RectTransform>().position,
            Quaternion.identity,
            _canvas.transform);
        _menuCreated.transform.position = _menuPosition.GetComponent<RectTransform>().position;
        _menuCreated.GetComponentInChildren<SpeedCounter>().Setup(_rocketCreated);
        _menuCreated.GetComponentInChildren<BestScore>().Setup(_rocketCreated);
        _menuCreated.GetComponentInChildren<FuelCounter>().Setup(_rocketCreated);
        _rocketCreated.Setup(_playerInput, _moneyCounterCreated,
            _menuCreated.GetComponentInChildren<FuelCounter>(),
            _pauseService,
            _menuCreated.GetComponentInChildren<SpeedCounter>());

        TextTranslator[] textTranslators = _menuCreated.GetComponentsInChildren<TextTranslator>();
        foreach (TextTranslator textTranslator in textTranslators)
        {
            textTranslator.Setup(_translator);
        }
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
        
        TextTranslator[] textTranslators = _resumeCreated.GetComponentsInChildren<TextTranslator>();
        foreach (TextTranslator textTranslator in textTranslators)
        {
            textTranslator.Setup(_translator);
        }
    }
    
    private void CreateFailWindow()
    {
        _failWindowCreated = Instantiate(_failWindow, 
            _failWindow.GetComponent<RectTransform>().localPosition, 
            Quaternion.identity, 
            _canvas.transform);
        _failWindowCreated.GetComponent<RectTransform>().localPosition = Vector3.zero;
        _failWindowCreated.Setup(_rocketCreated);
        _failWindowCreated.GetComponentInChildren<AdsButton>().Setup(_rocketCreated, _pauseService);
        
        TextTranslator[] textTranslators = _failWindowCreated.GetComponentsInChildren<TextTranslator>();
        foreach (TextTranslator text in textTranslators)
        {
            text.Setup(_translator);
        }
    }
    

    private void CreateScoreCounter()
    {
        _scoreCounterCreated = Instantiate(_scoreCounter,
            _scoreCounterPosition.GetComponent<RectTransform>().position,
            Quaternion.identity,
            _canvas.transform);
        _scoreCounterCreated.Setup(_rocketCreated);
        _scoreCounterCreated.GetComponent<TextTranslator>().Setup(_translator);
        _scoreCounterCreated.transform.position = _scoreCounterPosition.GetComponent<RectTransform>().position;
    }

    private void CreateSpeedFuelItem()
    {
        _shopSpeedItemCreated = Instantiate(_shopSpeedItem, 
            _speedShopPosition.GetComponent<RectTransform>().position, 
            Quaternion.identity, 
            _canvas.transform);
        _shopSpeedItemCreated.GetComponent<TextTranslator>().Setup(_translator);
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
        _shopFuelItemCreated.GetComponent<TextTranslator>().Setup(_translator);
        _shopFuelItemCreated.Setup(_rocketCreated);
        _shopFuelItemCreated.GetComponent<ShopFuelItemViewer>().Setup(_rocketCreated);
        _shopFuelItemCreated.transform.position = _fuelShopPosition.GetComponent<RectTransform>().position;
    }
    
    private void CreateInvisibleWall()
    {
        _createWallTick = StartCoroutine(CreateWallTick());
    }

    private void CreateRocket()
    {
        _rocketCreated = Instantiate(_rocket, _rocketStartPoint.position, Quaternion.identity);
        _playerInput.Setup(_rocketCreated);
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
        _adsItemCreated.gameObject.SetActive(false);
    }

    private void DisableMenu()
    {
        _menuCreated.gameObject.SetActive(false);
        _inscriptionCreated.gameObject.SetActive(false);
        _translateButtonCreated.gameObject.SetActive(false);
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
}
