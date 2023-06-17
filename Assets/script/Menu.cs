using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Menu : MonoBehaviour
{
    public enum MenuStatus
    {
        None,
        Pause,
        Clear,
        GameOver,
        Continue,
    }

    public MenuStatus menuStatus = MenuStatus.None;

    TextMeshProUGUI title;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.gameObject.SetActive(false);
        title = this.transform.Find("BG").Find("Title").GetComponent<TextMeshProUGUI>();
    }

    public void SetMunu(MenuStatus _MenuStatus)
    {
        menuStatus = _MenuStatus;
        this.gameObject.SetActive(true);

        switch (_MenuStatus)
        {
            case MenuStatus.None:    title.text =  "None"; break;
            case MenuStatus.Pause:   title.text =  "Pause"; break;
            case MenuStatus.Clear:   title.text =  "Clear"; break;
            case MenuStatus.GameOver:title.text =  "GameOver"; break;
            case MenuStatus.Continue: title.text = "None";
            this.gameObject.SetActive(false);
            break;
        }
    }
    public void BtnClose()
    {
        menuStatus = MenuStatus.None;
        GameManager.Instance.ContinueGame();
    }
    public void BtnRestart()
    {
        menuStatus = MenuStatus.None;
        GameManager.Instance.RestartGame();
    }
    public void BtnHome()
    {
    
        GameManager.Instance.MainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
