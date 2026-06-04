//test test test test test
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class Arrow : NetworkBehaviour
{
    private GameObject arrowObj;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            Arrow arrow = gameObject.GetComponent<Arrow>();
            arrow.enabled = false;
        }
    }

    void Update()
    {
        arrowObj = GameObject.FindGameObjectWithTag("Arrow");
        if(arrowObj == null)
        {
            return;
        }
        arrowObj.SetActive(false);
    }

    /*
    public List<GameObject> listOfArrows;

    private GameObject currentArrow;

    void Update()
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Arrow");

        bool addedNew = false;


        foreach (GameObject _arrow in objectsWithTag)
        {
            if (!listOfArrows.Contains(_arrow))
            {
                listOfArrows.Add(_arrow);
                addedNew = true;
            }
            else
            {
                if (base.IsOwner)
                {
                    owner();
                    print("is monster");
                }
                else
                {
                    notOwner(_arrow);
                    print("is comonaut");
                }
            }
        }

        if (objectsWithTag.Length > 0 && !addedNew)
        {
            // We assume all arrows are now in the list
            Debug.Log("All arrows found, disabling script");
            //this.enabled = false;
        }

    }

    
    void owner()
    {
        print("arrow already on da list");
    }

    [ServerRpc]
    void notOwner(GameObject _obj)
    {
        ServerManager.Despawn(_obj);
    }
    */

}
