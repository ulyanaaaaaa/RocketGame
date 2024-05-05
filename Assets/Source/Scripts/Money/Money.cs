using UnityEngine;

public class Money : MonoBehaviour
{
    public int Resources;

    private void Start()
    {
        Resources = (int)transform.position.y;
    }
}
