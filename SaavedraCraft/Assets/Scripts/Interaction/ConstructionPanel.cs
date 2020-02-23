using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionPanel : MonoBehaviour {

    public Texture btnTexture;
    private bool isContructionListDesplegable = false;
    private IConstruction<Component> toBeConstructedClone;
    public Vector2 scrollPosition = Vector2.zero;

    private Component toBeConstructedGhostComponent = null;

    void OnGUI()
    {
        ConstructionManager constructionManager = getConstructionManager();
        ICentralMarket<Component> centralMarket = constructionManager.GetCentralMarket();

        foreach (Transaction<Component> currentTransanction in centralMarket.GetTransactions())
        {
            Debug.Log(currentTransanction.ToString());
        }

        GUI.skin.button.fontSize = 18;
        GUI.skin.label.fontSize = 18;
        if (GUI.Button(new Rect(20, 30, 150, 45), "Construir!"))
        {
            isContructionListDesplegable = !isContructionListDesplegable;
        }
        if (isContructionListDesplegable)
        {
            int n = constructionManager.GetAvailableConstructions().Count;
            GUI.Box(new Rect(20, 80, 360, 320), btnTexture);
            GUI.BeginGroup(new Rect(30, 80, 350, 280));
            scrollPosition = GUI.BeginScrollView(new Rect(5, 5, 310, 275), scrollPosition, new Rect(0, 0, 220, n * 80), false, true);
            int i = 0;
            foreach (IConstruction<Component> currentConstruction in constructionManager.GetAvailableConstructions())
            {
                GUI.BeginGroup(new Rect(5, 80 * i, 310, 80));
                GUI.Label(new Rect(5, 35, 130, 70), currentConstruction.GetName());
                if (GUI.Button(new Rect(150, 30, 100, 45), "Select"))
                {
                    toBeConstructedClone = currentConstruction.CloneMe();
                    destroyConsutructionGhost();
                    float[] coord = ConstructionFactory.turnRealVector3CoordIntoIJ(Camera.main.transform.position.x, Camera.main.transform.position.y);
                    coord[0] = Mathf.Round(coord[0]);
                    coord[1] = Mathf.Round(coord[1]);
                    Vector3 ijRoundCameraPos = new Vector3(coord[0], coord[1], -5);
                    toBeConstructedClone.SetNewIJ(Mathf.RoundToInt(ijRoundCameraPos.x), Mathf.RoundToInt(ijRoundCameraPos.y));
                    coord = ConstructionFactory.turnIJCoordIntoRealVector3(ijRoundCameraPos.x, ijRoundCameraPos.y);
                    Vector3 realCoordFromIJ = new Vector3(coord[0], coord[1], -5);
                    toBeConstructedGhostComponent = Instantiate(toBeConstructedClone.GetComponentMolde(), realCoordFromIJ, Quaternion.identity);
                }
                GUI.EndGroup();
                i++;
            }
            GUI.EndScrollView();
            GUI.EndGroup();
        }
        if (toBeConstructedClone != null)
        {
            int initialX = Screen.width * 2 / 3;
            GUI.Box(new Rect(initialX, 80, 150, 300), btnTexture);
            GUI.BeginGroup(new Rect(initialX + 10, 85, 130, 290));
            GUI.Label(new Rect(5, 5, 130, 30), toBeConstructedClone.GetName() + " " + toBeConstructedClone.GetCoordI() + "x" + toBeConstructedClone.GetCoordJ());
            if (GUI.Button(new Rect(2, 35, 120, 40), "Confirmar"))
            {
                constructionManager.NewBuild(toBeConstructedClone);
            }

            if (GUI.Button(new Rect(2, 85, 120, 95), "Cancelar"))
            {
                destroyConsutructionGhost();
                toBeConstructedClone = null;
                isContructionListDesplegable = false;
            }
            GUI.EndGroup();
        }
        if (constructionManager.isThereAConstructionSelected())
        {
            IConstruction<Component> constructionSelected = constructionManager.getConstructionSelected();
            int initialX = Screen.width * 2 / 3;
            int initialY = Screen.height * 2 / 3;
            GUI.Box(new Rect(initialX, initialY, 200, 150), btnTexture);
            GUI.BeginGroup(new Rect(initialX + 10, initialY + 10, 190, 320));
            GUI.skin.label.fontSize = 12;
            GUI.Label(new Rect(5, 5, 190, 90), "Info: " + constructionSelected.GetConstructionInfo());

            GUI.EndGroup();
        }
    }

    private ConstructionManager getConstructionManager()
    {
        return gameObject.GetComponent<ConstructionManager>();
    }

    private void destroyConsutructionGhost()
    {
        if (toBeConstructedGhostComponent != null)
        {
            Destroy(toBeConstructedGhostComponent.gameObject);
        }
    }

    // Use this for initialization
    void Start () { }
	
	// Update is called once per frame
	void Update () {
		if (toBeConstructedClone !=null)
        {
            //toBeConstructedComponent.
        }
	}
}
