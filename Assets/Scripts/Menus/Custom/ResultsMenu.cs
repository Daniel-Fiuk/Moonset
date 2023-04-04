using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.SceneManagement;

public class ResultsMenu : Menu
{
    #region inspector

    [SerializeField] TMP_Text finalTime, deathCount;

    #endregion

    public override void EnableMenu()
    {
        base.EnableMenu();

        Debug.Log(PlayerPrefs.GetString("FinalTime") + " : " + PlayerPrefs.GetInt("DeathCount"));

        finalTime.SetText("Final Time: " + PlayerPrefs.GetString("FinalTime"));
        deathCount.SetText("Total Deaths: " + PlayerPrefs.GetInt("DeathCount").ToString());
    }

    public void OpenNextLevel()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        OpenScene(gameManager.nextScene);
    }
}
