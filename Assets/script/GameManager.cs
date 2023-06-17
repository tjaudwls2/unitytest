using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public GameObject GameOverUI;
    public bool isPause = false;
    public bool isLive = true;
   // public Menu Menu;
   public enum GameStatus
    {
        None,
        Pause,
        Clear,
        GameOver,
        Continue,
    }

    private void Start()
    {
        GameOverUI=GameObject.Find("Canvas").transform.GetChild(2).gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPause)
                PauseGame();
            else
                ContinueGame();
        }
       
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isPause = true;

        //Menu.SetMenu(Menu.MenuStatus.Pause);

    }
    public void ContinueGame()
    {
        Time.timeScale = 1;
        isPause = false;
        //Menu.SetMenu(Menu.MenuStatus.ContinueGame);
    }
  
    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1;
        isPause = false;
        GameOverUI.SetActive(false);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Main");
        Time.timeScale = 1;

    }

}
