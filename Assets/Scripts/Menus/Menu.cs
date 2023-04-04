using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    #region inspector

    [SerializeField] MenuType menuType;

    #endregion

    #region variables

    protected MenuManager menuManager;

    #endregion

    private void Start()
    {
        EnableMenu();
    }

    public virtual void EnableMenu()
    {
        switch (menuType)
        {
            case MenuType.Overlay:
                menuManager = FindObjectOfType<MenuManager>();
                if (!menuManager.stack.Contains(gameObject.name)) menuManager.stack.Push(gameObject.name);
                else while (!menuManager.stack.TryPeek(out string topMenu) || topMenu != gameObject.name) menuManager.stack.Pop();
                return;

            case MenuType.StandAlone:
                menuManager = gameObject.AddComponent<MenuManager>();
                return;
                
            default:
                Debug.LogError($"Menu type of {gameObject.name} is invalid!");
                return;
        }
    }

    public virtual void OpenMenu(string sceneName)
    {
        switch (menuType)
        {
            case MenuType.Overlay:
                SceneManager.UnloadSceneAsync(menuManager.stack.Peek());
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                return;

            case MenuType.StandAlone:
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                return;

            default:
                Debug.LogError($"Menu type of {gameObject.name} is invalid!");
                return;
        }
    }

    public virtual void CloseMenu()
    {
        switch (menuType)
        {
            case MenuType.Overlay:
                SceneManager.UnloadSceneAsync(menuManager.stack.Pop());

                if (menuManager.stack.TryPeek(out string menu)) SceneManager.LoadSceneAsync(menu, LoadSceneMode.Additive);
                else if (FindObjectOfType<GameManager>()) FindObjectOfType<GameManager>().DisablePause();

                return;

            case MenuType.StandAlone:
                Debug.LogWarning("Closing a Stand Alone menu is not supported! Use OpenScene or OpenMenu instead.");
                return;

            default:
                Debug.LogError($"Menu type of {gameObject.name} is invalid!");
                return;
        }
    }

    public virtual void OpenScene(string sceneName)
    {
        switch (menuType)
        {
            case MenuType.Overlay:
                SceneManager.UnloadSceneAsync(menuManager.stack.Peek());
                menuManager.stack.Clear();
                
                SceneManager.LoadScene(sceneName);
                return;
                
            case MenuType.StandAlone:
                SceneManager.LoadScene(sceneName);
                return;

            default:
                Debug.LogError($"Menu type of {gameObject.name} is invalid!");
                return;
        }
    }

    public virtual void RestartLevel()
    {
        switch (menuType)
        {
            case MenuType.Overlay:
                SceneManager.UnloadSceneAsync(menuManager.stack.Peek());
                menuManager.stack.Clear();
                
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                return;

            case MenuType.StandAlone:
                Debug.LogWarning("Restart Level from a Stand Alone menu is not supported!");
                return;

            default:
                Debug.LogError($"Menu type of {gameObject.name} is invalid!");
                return;
        }
    }

    public virtual void QuitGame()
    {
        Application.Quit();
    }
}

public enum MenuType
{
    Overlay,
    StandAlone
}
