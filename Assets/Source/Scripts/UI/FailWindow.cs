using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FailWindow : MonoBehaviour
{
    [SerializeField] private GameObject _window;
    [SerializeField] private Button _restart;
    
    public void Setup(Rocket rocket)
    {
        rocket.OnDie += Open;
    }

    private void Awake()
    {
        _restart.onClick.AddListener(Restart);
        _window.SetActive(false);
    }
    
    public void Restart() => SceneManager.LoadScene("SampleScene");

    private void Open()
    {
        _window.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
