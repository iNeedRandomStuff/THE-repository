using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    public PCmovement pcMovement;
    private bool isInPauseMenu;

    public void DisablePauseMenu()
    {
        pauseMenu.SetActive(false);
        isInPauseMenu = false;
        OffCursor(true);
    }

    void Update()
    {
        /*
        if(pcMovement == null)
        {
            GameObject _monster = GameObject.FindGameObjectWithTag("monster");
            GameObject _player = GameObject.FindGameObjectWithTag("VRplayer");
            if (_monster != null)
            {
                if(_monster.GetComponent<PCmovement>().enabled == true)
                {
                    pcMovement = _monster.GetComponent<PCmovement>();
                }
            }
            if(_player != null)
            {
                if (_player.GetComponent<PCmovement>().enabled == true)
                {
                    pcMovement = _player.GetComponent<PCmovement>();
                }
            }
        }
        */
        if (pcMovement == null)
        {
            pcMovement = PCmovement.Local;
        }
    
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isInPauseMenu == true)
            {
                pauseMenu.SetActive(false);
                isInPauseMenu = false;
                OffCursor(true);
            }
            else
            {
                isInPauseMenu = true;
                pauseMenu.SetActive(true);
                OffCursor(false);
            }
        }
    }

    void OffCursor(bool _curLockState)
    {
        if (_curLockState == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            settingsMenu.SetActive(false);
            pcMovement.canMove = true;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            pcMovement.canMove = false;
        }
    }

    public void Continue()
    {
        OffCursor(true);
        isInPauseMenu = false;
    }
}
