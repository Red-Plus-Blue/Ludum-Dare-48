using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerComponent : MonoBehaviour
{
    public static GameManagerComponent Instance { get; protected set; }

    protected const int LEVEL_SCENE = 0;

    protected bool _exiting;

    private void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += (scene, mode) => _exiting = false;
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public void ExitLevel()
    {
        if(_exiting) { return; }
        _exiting = true;
        Debug.Log("Loading next level");
        SceneManager.LoadScene(LEVEL_SCENE);
    }
}
