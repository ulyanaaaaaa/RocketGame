using System.Collections;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Transform _rocketStartPoint;
    private KeabordInput _input;
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
    private InvisibleWall _invisibleWall;
    private InvisibleWall _invisibleLeftWallCreated;
    private InvisibleWall _invisibleRightWallCreated;
    private Coroutine _createWallTick;

    private void Awake()
    {
        _input = GetComponent<KeabordInput>();
        _rocket = Resources.Load<Rocket>("Rocket");
        _failWindow = Resources.Load<FailWindow>("FailWindow"); 
        _fuelBar = Resources.Load<FuelBar>("FuelBar"); 
        _moneyFabrica = Resources.Load<MoneyFabrica>("MoneyFabrica");
        _cloudFabrica = Resources.Load<CloudFabrica>("CloudFabrica");
        _fuelFabrica = Resources.Load<FuelFabrica>("FuelFabrica");
        _shopSpeedItem = Resources.Load<ShopSpeedItem>("ItemSpeed");
        _shopFuelItem = Resources.Load<ShopFuelItem>("ItemFuel");
        _moneyCounter = Resources.Load<MoneyCounter>("MoneyCounter");
        _invisibleWall = Resources.Load<InvisibleWall>("InvisibleWall");
        _fuelCounter = Resources.Load<FuelCounter>("FuelCounter");
        CreateMoneyCounter();
        CreateFuelCounter();
        CreateRocket();
        CreateUI();
        _input.OnPlay += CreateSpawners;
        _input.OnPlay += DisableShop;
        _input.OnPlay += FuelBarCreated;
        _input.OnPlay += CreateInvisibleWall;
        _input.OnPlay += DisableFuel;
    }

    private void CreateUI()
    {
       CreateFailWindow();
       CreateShopFuelItem();
       CreateSpeedFuelItem();
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
            _shopSpeedItem.GetComponent<RectTransform>().localPosition, 
            Quaternion.identity, 
            _canvas.transform);
        _shopSpeedItemCreated.Setup(_rocketCreated);
        _shopSpeedItemCreated.GetComponent<ShopSpeedItemViewer>().Setup(_rocketCreated);
        _shopSpeedItemCreated.GetComponent<RectTransform>().localPosition = 
            _shopSpeedItem.GetComponent<RectTransform>().localPosition;
    }

    private void CreateShopFuelItem()
    {
        _shopFuelItemCreated = Instantiate(_shopFuelItem, 
            _shopFuelItem.GetComponent<RectTransform>().localPosition, 
            Quaternion.identity, 
            _canvas.transform);
        _shopFuelItemCreated.Setup(_rocketCreated);
        _shopFuelItemCreated.GetComponent<ShopFuelItemViewer>().Setup(_rocketCreated);
        _shopFuelItemCreated.GetComponent<RectTransform>().localPosition = 
            _shopFuelItem.GetComponent<RectTransform>().localPosition;
    }

    private void CreateFuelCounter()
    {
        _fuelCounterCreated = Instantiate(_fuelCounter,
            _fuelCounter.GetComponent<RectTransform>().localPosition, 
            Quaternion.identity, 
            _canvas.transform);
        _fuelCounterCreated.GetComponent<RectTransform>().localPosition = 
            _fuelCounter.GetComponent<RectTransform>().localPosition;
    }

    private void CreateMoneyCounter()
    {
        _moneyCounterCreated = Instantiate(_moneyCounter,
            _moneyCounter.GetComponent<RectTransform>().localPosition, 
            Quaternion.identity, 
            _canvas.transform);
        _moneyCounterCreated.GetComponent<RectTransform>().localPosition = 
            _moneyCounter.GetComponent<RectTransform>().localPosition;
    }

    private void CreateInvisibleWall()
    {
        _createWallTick = StartCoroutine(CreateWallTick());
    }

    private void CreateRocket()
    {
        _rocketCreated = Instantiate(_rocket, _rocketStartPoint.position, Quaternion.identity);
        _rocketCreated.Setup(_input, _moneyCounterCreated, _fuelCounterCreated);
    }

    private void CreateSpawners()
    {
        _moneyFabricaCreated = Instantiate(_moneyFabrica);
        _moneyFabricaCreated.Setup(_rocketCreated);
        _cloudFabricaCreated = Instantiate(_cloudFabrica);
        _cloudFabricaCreated.Setup(_rocketCreated);
        _fuelFabricaCreated = Instantiate(_fuelFabrica);
        _fuelFabricaCreated.Setup(_rocketCreated);
    }
    
    private void FuelBarCreated()
    {
        _fuelBarCreated = Instantiate(_fuelBar, 
            _failWindow.GetComponent<RectTransform>().localPosition, 
            Quaternion.identity, 
            _canvas.transform);
        _fuelBarCreated.Setup(_rocketCreated);
        _fuelBarCreated.GetComponent<RectTransform>().localPosition = 
            _fuelBar.GetComponent<RectTransform>().localPosition;

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
