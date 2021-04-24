using UnityEngine;

public class ExitComponent : MonoBehaviour
{
    protected bool _playerCanExit;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && _playerCanExit)
        {
            FindObjectOfType<GameManagerComponent>().ExitLevel();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.GetComponentInParent<PlayerControllerComponent>()) { return; }
        _playerCanExit = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.GetComponentInParent<PlayerControllerComponent>()) { return; }
        _playerCanExit = false;
    }
}
