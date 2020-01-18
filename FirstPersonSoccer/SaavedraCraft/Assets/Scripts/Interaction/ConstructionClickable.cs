using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class ConstructionClickable : MonoBehaviour {

    private IConstruction contruction;
    private bool isSelected = false;
    private Component selectedIcon;

    public Component SelectableComponent;

    private ConstructionManager constructionManager;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        if (contruction == null)
        {
            //Debug.Log("OnMouseDown");
        } else
        {            
            if (!IsSelected())
            {
                isSelected = true;
                //Debug.Log("OnMouseDown " + contruction.GetName() + " Coord:" + contruction.GetCoordI() + "x" + contruction.GetCoordJ());
                selectedIcon = Instantiate(SelectableComponent, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z-1), Quaternion.identity);                
                selectedIcon.transform.parent = this.transform;
                constructionManager.ConstructionWasClicked(this);
            }
            else
            {
                this.Deselect();
            }
        }        
    }

    public void SetConstruction(IConstruction aConstruction)
    {
        contruction = aConstruction;
    }

    public IConstruction GetContruction()
    {
        return contruction;
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public void setConstructionManager(ConstructionManager constructionManager)
    {
        this.constructionManager = constructionManager; 
    }

    public void Deselect()
    {
        if (IsSelected())
        {
            Destroy(selectedIcon.gameObject);
        }
        isSelected = false;       
    }
}
