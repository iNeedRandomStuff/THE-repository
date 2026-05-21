using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCosmonautsDead : MonoBehaviour
{
    public List<GameObject> ListOfCosmonauts;

    [Header("Win signs")]
    public GameObject theMonsterWonSign;

    void Start()
    {
        theMonsterWonSign = GameObject.FindGameObjectWithTag("TheMonsterWon");
        InvokeRepeating(nameof(CheckCosmonauts), 0f, 1.0f);
    }

    void CheckCosmonauts()
    {
        for (int i = 0; i <= ListOfCosmonauts.Count; i++)
        {
            GameObject _cosmonaut = GameObject.FindGameObjectWithTag("Player");

            if (_cosmonaut != null)
            {
                if (!ListOfCosmonauts.Contains(_cosmonaut))
                {
                    print("player added");
                    ListOfCosmonauts.Add(_cosmonaut);
                    _cosmonaut = null;
                }
            }

            foreach (GameObject gameObject in ListOfCosmonauts)
            {
                if(gameObject.activeSelf == true)
                {
                    ListOfCosmonauts.RemoveAt(i);
                }
            }

            
            if (ListOfCosmonauts.Count == 0)
            {
                print("the monster won");
                theMonsterWonSign.transform.localScale = new Vector3(1, 1, 1);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                AllCosmonautsDead thisScript = gameObject.GetComponent<AllCosmonautsDead>();
                thisScript.enabled = false;
            }
        }
    }
}
