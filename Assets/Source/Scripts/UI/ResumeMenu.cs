using UnityEngine;
using UnityEngine.SceneManagement;

public class ResumeMenu : MonoBehaviour
{
    public void Restart() => SceneManager.LoadScene("SampleScene");
}
