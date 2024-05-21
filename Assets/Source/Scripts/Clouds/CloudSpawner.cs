using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CloudFabrica))]
public class CloudSpawner : MonoBehaviour, IPause
{
   [SerializeField] private float _delay;
   private Coroutine _spawnTick;
   private PauseService _pauseService;
   private CloudFabrica _cloudFabrica;

   public void Setup(PauseService pauseService)
   {
      _pauseService = pauseService;
   }

   private void Awake()
   {
      _cloudFabrica = GetComponent<CloudFabrica>();
   }

   private void Start()
   {
      _pauseService.AddPause(this);
      _spawnTick = StartCoroutine(SpawnTick());
   }
   
   public void Pause()
   {
      StopCoroutine(_spawnTick);
   }

   public void Resume()
   { 
      _spawnTick = StartCoroutine(SpawnTick());
   }

   private IEnumerator SpawnTick()
   {
      while (true)
      {
         yield return new WaitForSeconds(_delay);
         _cloudFabrica.CreateCloud();
         _cloudFabrica.CreateBigCloud();
         _cloudFabrica.CreateSmallCloud();
      }
   }
}
