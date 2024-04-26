using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CloudFabrica))]
public class CloudSpawner : MonoBehaviour
{
   [SerializeField] private float _delay;
   private Coroutine _spawnTick;
   private CloudFabrica _cloudFabrica;

   private void Awake()
   {
      _cloudFabrica = GetComponent<CloudFabrica>();
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
         _cloudFabrica.CreateCloud();
         _cloudFabrica.CreateBigCloud();
         _cloudFabrica.CreateSmallCloud();
      }
   }
}
