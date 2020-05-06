using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene("LevelGeometry", LoadSceneMode.Additive);
        SceneManager.LoadScene("Kitchen", LoadSceneMode.Additive);
    }
}
