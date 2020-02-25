using Assets.Scripts.Camera;
using Assets.Scripts.Map.Constructions;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : MonoBehaviour, ICameraObserver {

    private List<IConstruction<Component>> constructionAvailable = new List<IConstruction<Component>>();
    private List<IConstruction<Component>> constructionInMap = new List<IConstruction<Component>>();
    private List<ConstructionClickable> constructionClickables = new List<ConstructionClickable>();

    public Component CasaNS1Molde;
    public Component CalleNS1;
    public Component Campo01;
    public Component Mercado01;
    public Component CentralMarketComponent;
    public Component CasaWorker01;

    private IConstructionManagerObserver<Component> singleObserver;    

    public void AddConstructionManagerObserver(IConstructionManagerObserver<Component> newObserver)
    {
        singleObserver = newObserver;
    }

    public ICentralMarket<Component> GetCentralMarket()
    {
        CentralMarketBehaviour centralMarketBehaviour = CentralMarketComponent.GetComponent<CentralMarketBehaviour>();
        ICentralMarket<Component> centralMarket = centralMarketBehaviour.GetCentralMarket();
        return centralMarket;
    }

    // Use this for initialization
    void Start ()
    {
        //Available Constructions:
        constructionAvailable.Add(new Assets.Scripts.Casa1("Casa1", CasaNS1Molde, 0, 0, GetCentralMarket()));
        constructionAvailable.Add(new Assets.Scripts.Calle("CalleNS1", CalleNS1, 0, 0));
        constructionAvailable.Add(new CampoTomatesMono("Campo01", Campo01, 0, 0, GetCentralMarket()));
        constructionAvailable.Add(new Assets.Scripts.Casa1("Mercado01", Mercado01, 0, 0, GetCentralMarket()));
        constructionAvailable.Add(new CasaWorkerComponent("CasaWorker01",CasaWorker01,0,0,GetCentralMarket()));
        //----
        constructionInMap.Add(constructionAvailable[0].CloneMe().SetNewIJ(0, -2));
        constructionInMap.Add(new Assets.Scripts.Casa1("Casa2", CasaNS1Molde, 1, -2, GetCentralMarket()));
        constructionInMap.Add(new Assets.Scripts.Casa1("Casa4", CasaNS1Molde, 0, 2, GetCentralMarket()));
        constructionInMap.Add(constructionAvailable[3].CloneMe().SetNewIJ(2, -2));//Mercado
        constructionInMap.Add(constructionAvailable[4].CloneMe().SetNewIJ(4, -2));//CasaWorker01
        constructionInMap.Add(constructionAvailable[2].CloneMe().SetNewIJ(5, -2));//CampoTomates


        constructionInMap.Add(constructionAvailable[1].CloneMe().SetNewIJ(-4, -1));
        constructionInMap.Add(constructionAvailable[1].CloneMe().SetNewIJ(-3, -1));
        constructionInMap.Add(constructionAvailable[1].CloneMe().SetNewIJ(-2, -1));
        constructionInMap.Add(constructionAvailable[1].CloneMe().SetNewIJ(-1, -1));
        constructionInMap.Add(constructionAvailable[1].CloneMe().SetNewIJ(0, -1));
        constructionInMap.Add(constructionAvailable[1].CloneMe().SetNewIJ(3, -1));
        constructionInMap.Add(constructionAvailable[1].CloneMe().SetNewIJ(1, -1));
        constructionInMap.Add(constructionAvailable[1].CloneMe().SetNewIJ(2, -1));
        constructionInMap.Add(constructionAvailable[1].CloneMe().SetNewIJ(3, -1));
        constructionInMap.Add(constructionAvailable[1].CloneMe().SetNewIJ(4, -1));

        constructionInMap.Add(constructionAvailable[0].CloneMe().SetNewIJ(0, 0));
        constructionInMap.Add(constructionAvailable[0].CloneMe().SetNewIJ(1, 0));
        constructionInMap.Add(constructionAvailable[0].CloneMe().SetNewIJ(1, 0));

        constructionInMap.Add(constructionAvailable[1].CloneMe().SetNewIJ(-1, 1));
        constructionInMap.Add(constructionAvailable[1].CloneMe().SetNewIJ(-2, 1));
        constructionInMap.Add(constructionAvailable[1].CloneMe().SetNewIJ(-3, 1));
        constructionInMap.Add(constructionAvailable[1].CloneMe().SetNewIJ(-4, 1));

        constructionInMap.Add(constructionAvailable[1].CloneMe().SetNewIJ(0, 1));
        constructionInMap.Add(constructionAvailable[1].CloneMe().SetNewIJ(1, 1));
        constructionInMap.Add(constructionAvailable[1].CloneMe().SetNewIJ(2, 1));
        constructionInMap.Add(constructionAvailable[1].CloneMe().SetNewIJ(3, 1));
    }

    public void ConstructionWasClicked(ConstructionClickable constructionClickable)
    {
        List<ConstructionClickable> otherClickedConstructions = constructionClickables.FindAll(x =>  (x != constructionClickable) && x.IsSelected() );
        foreach (ConstructionClickable otherClicked in otherClickedConstructions)
        {
            otherClicked.Deselect();
        }
    }

    public bool isThereAConstructionSelected()
    {
        foreach (ConstructionClickable constructionClickable in constructionClickables)
        {
            if ((constructionClickable!=null) &&(constructionClickable.IsSelected()))
            {
                return true;
            }
        }
        return false;
    }

    public void SetConstructionClickeables(List<ConstructionClickable> consClickable)
    {
        constructionClickables.AddRange(consClickable);
    }

    public void NewBuild(IConstruction<Component> toBeConstructedClone)
    {
        constructionInMap.Add(toBeConstructedClone);
        singleObserver.NewBuildCreated(toBeConstructedClone);
    }

    public void AddNewConstructionClickeable(ConstructionClickable consClickable)
    {
        constructionClickables.Add(consClickable);
        consClickable.setConstructionManager(this);
    }

    public List<ConstructionClickable> GetConstructionClickeables()
    {
        return constructionClickables;
    }

    public IConstruction<Component> getConstructionSelected()
    {
        ConstructionClickable clickeable = constructionClickables.Find(x => x.IsSelected());
        return clickeable.GetContruction();
    }

    // Update is called once per frame
    void Update ()
    {
		foreach (IConstruction<Component> currentConstruction in constructionInMap)
        {
            currentConstruction.TimeTick(Time.deltaTime);
        }
	}

    public List<IConstruction<Component>> GetAvailableConstructions()
    {
        return constructionAvailable;
    }

    public IConstruction<Component> getConstructionFromTileCoor(float i, float j)
    {
        IConstruction<Component> contructionToFind = constructionInMap.Find(x => x.GetCoordI() == i && x.GetCoordJ() == j);
        if (contructionToFind == null)
        {
            return null;
        }
        return contructionToFind;
    }

    public void NotifyCameraNewPos()
    {
        throw new NotImplementedException();
    }
}
