using UnityEngine;

public class Stone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Patron>())
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
