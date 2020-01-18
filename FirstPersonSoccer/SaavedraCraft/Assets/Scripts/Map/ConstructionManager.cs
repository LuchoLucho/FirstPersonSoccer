﻿using Assets.Scripts.Camera;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Map;
using Assets.Scripts.Map.Constructions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : MonoBehaviour, ICameraObserver {

    private List<IConstruction> constructionAvailable = new List<IConstruction>();
    private List<IConstruction> constructionInMap = new List<IConstruction>();
    private List<ConstructionClickable> constructionClickables = new List<ConstructionClickable>();

    public Component CasaNS1Molde;
    public Component CalleNS1;
    public Component Campo01;
    public Component Mercado01;

    private IConstructionManagerObserver singleObserver;
    private ICentralResourcesCommunicator centralResourcesCommunicator = new CentralResourcesCommunicator();

    public void AddConstructionManagerObserver(IConstructionManagerObserver newObserver)
    {
        singleObserver = newObserver;
    }

    // Use this for initialization
    void Start ()
    {
        //Available Constructions:
        constructionAvailable.Add(new Assets.Scripts.Casa1("Casa1", CasaNS1Molde, 0, 0, centralResourcesCommunicator));
        constructionAvailable.Add(new Assets.Scripts.Casa1("CalleNS1", CalleNS1, 0, 0, centralResourcesCommunicator));
        constructionAvailable.Add(new CampoTomates("Campo01", Campo01, 0, 0, centralResourcesCommunicator));
        constructionAvailable.Add(new Assets.Scripts.Casa1("Mercado01", Mercado01, 0, 0, centralResourcesCommunicator));
        //----
        constructionInMap.Add(constructionAvailable[0].CloneMe().SetNewIJ(0, -2));
        constructionInMap.Add(new Assets.Scripts.Casa1("Casa2", CasaNS1Molde, 1, -2, centralResourcesCommunicator));
        constructionInMap.Add(new Assets.Scripts.Casa1("Casa3", CasaNS1Molde, 2, -2, centralResourcesCommunicator));
        constructionInMap.Add(new Assets.Scripts.Casa1("Casa4", CasaNS1Molde, 0, 2, centralResourcesCommunicator));
        constructionInMap.Add(constructionAvailable[3].CloneMe().SetNewIJ(-2, -2));//Mercado
        constructionInMap.Add(constructionAvailable[2].CloneMe().SetNewIJ(4, -2));//Campo

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

    public void NewBuild(IConstruction toBeConstructedClone)
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

    public IConstruction getConstructionSelected()
    {
        ConstructionClickable clickeable = constructionClickables.Find(x => x.IsSelected());
        return clickeable.GetContruction();
    }

    // Update is called once per frame
    void Update ()
    {
		foreach (IConstruction currentConstruction in constructionInMap)
        {
            currentConstruction.TimeTick(Time.deltaTime);
        }
	}

    public List<IConstruction> GetAvailableConstructions()
    {
        return constructionAvailable;
    }

    public IConstruction getConstructionFromTileCoor(float i, float j)
    {
        IConstruction contructionToFind = constructionInMap.Find(x => x.GetCoordI() == i && x.GetCoordJ() == j);
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
