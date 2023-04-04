using Cinemachine;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //contains components
    #region components

    #endregion

    //contains values visable in the inspector
    #region inspector

    [SerializeField] public string nextScene;
    [SerializeField] public bool openTutorial;

    [Space]

    [SerializeField] public float minLevelYKillLevel = -1000, maxLevelYKillLevel = 1000;

    #endregion

    //contains values not visable in the inspector
    #region variables

    [HideInInspector] public Canvas canvas;
    [HideInInspector] public MenuManager menuManager;

    [HideInInspector] public bool isPaused;

    [HideInInspector] public PlayerController player;
    [HideInInspector] public GameObject virtualCamera;

    [HideInInspector] public int deathCount;

    #endregion

    private void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        menuManager = gameObject.AddComponent<MenuManager>();
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>().gameObject;
        player = FindObjectOfType<PlayerController>();

        Time.timeScale = 1;
        virtualCamera.SetActive(true);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        if (openTutorial)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused) DisablePause();
        else EnablePause();
    }

    public void EnablePause()
    {
        if (!player || isPaused) return;

        isPaused = true;

        virtualCamera.SetActive(false);
        
        SceneManager.LoadScene("Pause Menu", LoadSceneMode.Additive);

        PlayerControlsEnabled(false);

        StopCoroutine(LerpSongTimeScale(true));
        StartCoroutine(LerpSongTimeScale(false));

        player.astraTimeManipulation.TimeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        player.StopAllCoroutines();
        Time.timeScale = 0;
    }

    public void DisablePause()
    {
        if (!player || !isPaused) return;

        isPaused = false;

        virtualCamera.SetActive(true);

        PlayerControlsEnabled(true);

        StopCoroutine(LerpSongTimeScale(false));
        StartCoroutine(LerpSongTimeScale(true));

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        player.StopAllCoroutines();
        Time.timeScale = 1;

        if (menuManager.stack.Count > 0)
        {
            SceneManager.UnloadSceneAsync(menuManager.stack.Peek());
            menuManager.stack.Clear();
        }
    }
    private IEnumerator LerpSongTimeScale(bool active)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.unscaledDeltaTime * 2;
            player.astraTimeManipulation.TimeScale = Mathf.Lerp(active ? 0 : 1, active ? 1 : 0, t);
            yield return new WaitForEndOfFrame();
        }
    }

    public void CompleteLevel()
    {
        PlayerPrefs.SetString("FinalTime", FindObjectOfType<LevelTimer>().timer.text);
        PlayerPrefs.SetInt("DeathCount", deathCount);
        SceneManager.LoadSceneAsync("Results Menu", LoadSceneMode.Additive);
        
        PlayerControlsEnabled(false);
        isPaused = true;
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void PlayerControlsEnabled(bool enabled)
    {
        if (enabled)
        {
            player.movement.Enable();
            player.look.Enable();
            player.jump.Enable();
            player.sprint.Enable();
            player.slowMotionAim.Enable();
            player.grapple.Enable();
        }
        else
        {
            player.movement.Disable();
            player.look.Disable();
            player.jump.Disable();
            player.sprint.Disable();
            player.slowMotionAim.Disable();
            player.grapple.Disable();
        }
    }
}
