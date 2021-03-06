﻿using Assets.Scripts;
using Assets.Scripts.MapObjects;
using Assets.Scripts.Players;
using QuarentineSurvival.Model;
using QuarentineSurvival.Model.Actions;
using QuarentineSurvival.Model.Interface;
using SaavedraCraft.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Interfaces.Transportation;
using SaavedraCraft.Model.Resources;
using SaavedraCraft.Model.Transportation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManangerBehaviour : MonoBehaviour
{
    public Component SinglePlayerComponent;
    public Component SimpleRoadComponent;
    public Component ChestComponent;
    public Component ActionableMediumWithDoor;
    public Component BedComponent;
    public Component BedroomFloorComponent;
    public Component CoinComponent;

    private List<IMovableMediumCollisionAware<Component>> movableMediums = new List<IMovableMediumCollisionAware<Component>>();

    private List<IMovable<Component>> movablesInMap = new List<IMovable<Component>>();
    private ICollisionable<Component> chestCollision;
    private ICargoTransporter<Component> player;

    private Component realInstancePlayer;

    private IConstructionManagerObserver<Component> singleObserver;
    private ITransporterAndWarehouseManager<Component> transporterAndWarehouseManager;

    private StepOnActionable<Component> stepOnActionableChestCamera;
    private StepOnActionable<Component> stepOnActionableBeforeChestCamera;
    private StepOnActionable<Component> stepOnActionableOutsideMainChamber;
    private StepOnActionable<Component> stepOnActionableInside;

    private CamaraController camaraController;

    // Start is called before the first frame update
    void Start()
    {
        transporterAndWarehouseManager = new TransporterAndWarehouseManager<Component>();
        IMovableMediumCollisionAware<Component> medium = getEntityFromTileCoor(0,0);//new SimpleStreet<Component>("Floor", null, 0, 0);
        player = new SinglePlayerComp("Player", SinglePlayerComponent, medium, transporterAndWarehouseManager);
        player.SetDirectionI(+0);
        player.SetDirectionJ(+0);
        player.SetVelocity(0.5f);
        movablesInMap.Add(player);
        NewMovable(movablesInMap[0]);
    }

    // Update is called once per frame
    void Update()
    {
        float timeDelta = Time.deltaTime;
        if (movablesInMap.Count  == 0)
        {
            return;
        }
        IMovable<Component> playerMovable = movablesInMap[0];
        playerMovable.TimeTick(timeDelta); //0.01f
        float[] newPosFloat = MapFactoryBehaviour.turnIJCoordIntoRealVector3(playerMovable.GetCoordI(), playerMovable.GetCoordJ());
        Vector3 newPos = new Vector3(newPosFloat[0], newPosFloat[1], newPosFloat[2]);
        realInstancePlayer.transform.position = newPos;
    }    

    public void AddConstructionManagerObserver(IConstructionManagerObserver<Component> newObserver)
    {
        singleObserver = newObserver;
    }

    public void NewMovableMedium(IMovableMediumCollisionAware<Component> toBeConstructedClone)
    {
        movableMediums.Add(toBeConstructedClone);
        singleObserver.NewBuildCreated(toBeConstructedClone);
        Component newChildComponentMolde = toBeConstructedClone.GetComponentMolde();
        Component newChild = null;
        newChild = Instantiate(newChildComponentMolde, new Vector3(0,0,0), Quaternion.identity);
        toBeConstructedClone.SetComponentInstanciaReal(newChild);
        toBeConstructedClone.SetActive(true);
    }

    public void NewMovable(IMovable<Component> toBeConstructedClone)
    {
        movablesInMap.Add(toBeConstructedClone);
        //singleObserver.NewBuildCreated(toBeConstructedClone);
        Component newChildComponentMolde = toBeConstructedClone.GetComponentMolde();
        Component newChild = null;
        newChild = Instantiate(newChildComponentMolde, new Vector3(0, 0, 0), Quaternion.identity);
        toBeConstructedClone.SetComponentInstanciaReal(newChild);
        toBeConstructedClone.SetActive(true);
        realInstancePlayer = newChild; // Temporal storage, this should the work of the FACTORY to save this...!
    }

    public IMovableMediumCollisionAware<Component> getEntityFromTileCoor(float i, float j)
    {
        if ((Mathf.Abs(i)>=MapFactoryBehaviour.MAX_TITLE_WIDTH/2) || (Mathf.Abs(j) >= MapFactoryBehaviour.MAX_TITLE_WIDTH / 2))
        {
            return null;
        }
        IMovableMediumCollisionAware<Component> newMedium = movableMediums.Find(x => x.GetCoordI() == i && x.GetCoordJ() == j);
        if (newMedium!=null)
        {
            return newMedium;
        }
        if (i == 0 && j == -3)
        {
            newMedium = new ActionCollisionableMediumAware<Component>("PlayerStartMedium" + i + j, CoinComponent, i, j);
            StepOnActionable<Component> coin = new StepOnActionable<Component>("Coin1", CoinComponent, newMedium, transporterAndWarehouseManager);
            coin.SetAutoAction(new AutoAction<Component>(x => {
                IActionExecutor<Component> executor = x;
                ICargo<Component> simpleCargo = new SimpleCargo<Component>();
                IResource resource = new SimpleResource(1, "Coin", 0);
                simpleCargo.addResources(resource, null);
                QurentinePlayerModel<Component> playerToLoadNewCargoFromCollision = executor as QurentinePlayerModel<Component>;
                playerToLoadNewCargoFromCollision.LoadCargo(simpleCargo);
                coin.WasAlreadyTriggered = true;
            }));
        }
        else if (i == 0 && j == -2)
        {
            newMedium = new ActionCollisionableMediumAware<Component>("PlayerStartMedium" + i + j, BedroomFloorComponent, i, j);
            stepOnActionableOutsideMainChamber = new StepOnActionableComp("stepOnActionableOutsideMainChamber", null, newMedium, transporterAndWarehouseManager);
            stepOnActionableOutsideMainChamber.SetWidh(0.9f);
            stepOnActionableOutsideMainChamber.SetHeigh(0.9f);
            stepOnActionableOutsideMainChamber.SetAutoAction(new AutoAction<Component>(x => {
                if (x == player)
                {
                    Debug.Log("Im in the OUTSIDE!");
                    GetComponent<CamaraController>().SwitchToOutside();
                    stepOnActionableOutsideMainChamber.WasAlreadyTriggered = true;
                    stepOnActionableInside.WasAlreadyTriggered = false;
                }
            }));
        }        
        else if (j<=-2)
        {
            newMedium = new ActionCollisionableMediumAware<Component>("PlayerStartMedium" + i + j, SimpleRoadComponent, i, j);
        }
        else if ((i == 0) && (j == 0))
        {
            newMedium = new ActionCollisionableMediumAware<Component>("PlayerStartMedium" + i + j, SimpleRoadComponent, i, j);            
        }
        else if ((i ==1) && (j == 1))
        {
            newMedium = new ActionCollisionableMediumAware<Component>("PlayerStartMedium" + i + j, BedroomFloorComponent, i, j);
        }
        else if ((i==2) && (j==0))
        {
            newMedium = new WarehouseChest<Component>("Cofre" + i + j, ChestComponent, i, j, transporterAndWarehouseManager);            
            ICargo<Component> simpleCargo = new SimpleCargo<Component>();
            IResource resource = new SimpleResource(1, "Encendedor", 0);
            IMovableMedium<Component> destinyOfResources = null; // The resouce has no fixed destination
            simpleCargo.addResources(resource, destinyOfResources);
            ((WarehouseChest<Component>)newMedium).addCargo(simpleCargo);
            //More cargo
            simpleCargo = new SimpleCargo<Component>();
            resource = new SimpleResource(1, "Lavandina", 0);
            destinyOfResources = null; // The resouce has no fixed destination
            simpleCargo.addResources(resource, destinyOfResources);
            ((WarehouseChest<Component>)newMedium).addCargo(simpleCargo);
            //More cargo (Combinable this time)
            IResource resourceToCombine = new CombinableIntoActionableResource<Component>(1, "Sock", "Scissors", "SocketMask", new WearResourceAction<Component>(), 0);
            IResource otherResourceToCombine = new CombinableIntoActionableResource<Component>(1, "Scissors", "Sock", "SocketMask", new WearResourceAction<Component>(), 0);
            simpleCargo = new SimpleCargo<Component>();
            simpleCargo.addResources(resourceToCombine, destinyOfResources);
            ((WarehouseChest<Component>)newMedium).addCargo(simpleCargo);
            simpleCargo = new SimpleCargo<Component>();
            simpleCargo.addResources(otherResourceToCombine, destinyOfResources);
            ((WarehouseChest<Component>)newMedium).addCargo(simpleCargo);
            //----                    
            stepOnActionableChestCamera = new StepOnActionableComp("StepOnMeChestCamera", null, newMedium, transporterAndWarehouseManager);
            stepOnActionableChestCamera.SetWidh(0.9f);
            stepOnActionableChestCamera.SetHeigh(0.9f);
            stepOnActionableChestCamera.SetAutoAction(new AutoAction<Component>(x => {
                if (x == player)
                {
                    GetComponent<CamaraController>().SwitchToChestCamera();
                    stepOnActionableChestCamera.WasAlreadyTriggered = true;
                    stepOnActionableBeforeChestCamera.WasAlreadyTriggered = false;
                }
            }));
            //----
            chestCollision = new SimpleTransporterCollisionable<Component>("Obstaculo", null, newMedium, transporterAndWarehouseManager);
            chestCollision.SetWidh(0.6f);
            chestCollision.SetHeigh(0.25f);
            chestCollision.SetDeltaJ(0.25f);
        }  else if ( ((i == 1) && (j == 0)) || ((i == 0) && (j == 0)))
        {
            newMedium = new ActionCollisionableMediumAware<Component>("Street" + i + j, BedroomFloorComponent, i, j);
            stepOnActionableBeforeChestCamera = new StepOnActionableComp("StepOnMeBeforeChestCamera", null, newMedium, transporterAndWarehouseManager);
            stepOnActionableBeforeChestCamera.SetWidh(0.9f);
            stepOnActionableBeforeChestCamera.SetHeigh(0.9f);
            stepOnActionableBeforeChestCamera.SetAutoAction(new AutoAction<Component>(x => {
                if (x == player)
                {
                    this.GetComponent<CamaraController>().SwitchToMainCamera();
                    stepOnActionableBeforeChestCamera.WasAlreadyTriggered = true;
                    stepOnActionableChestCamera.WasAlreadyTriggered = false;
                }
            }));
        } else if ((i == 0) && (j == -1))
        {
            newMedium = new QuerentineFloor("ActionStreetCollisionableMediumAware" + i + j, ActionableMediumWithDoor, i, j);
            IMovableMediumCollisionAware<Component> pisoActionable = (IMovableMediumCollisionAware<Component>)newMedium;
            SimpleDoorComp realInstanceDoor = new SimpleDoorComp("Puerta", ActionableMediumWithDoor, pisoActionable, transporterAndWarehouseManager);
            pisoActionable.addActionable((IActionable<Component>)realInstanceDoor);
            ICollisionable<Component> paredDerecha = new SimpleTransporterCollisionable<Component>("Obstaculo1", null, pisoActionable, transporterAndWarehouseManager);
            paredDerecha.SetWidh(0.25f);
            paredDerecha.SetHeigh(0.1f);
            //paredDerecha.SetNewIJ(i-0.25f, j+0.5f); //Don't use IJ since they're absolute, deltas are relatives!
            paredDerecha.SetDeltaI(-0.375f);
            ICollisionable<Component> paredIzq = new SimpleTransporterCollisionable<Component>("Obstaculo2", null, pisoActionable, transporterAndWarehouseManager);
            paredIzq.SetWidh(0.25f);
            paredIzq.SetHeigh(0.1f);
            paredIzq.SetDeltaI(+0.375f);
            //--------------------------------------------------------------------------
            stepOnActionableInside = new StepOnActionableComp("stepOnActionableInside", null, newMedium, transporterAndWarehouseManager);
            stepOnActionableInside.SetWidh(0.9f);
            stepOnActionableInside.SetHeigh(0.9f);
            stepOnActionableInside.SetAutoAction(new AutoAction<Component>(x => {
                if (x == player)
                {
                    Debug.Log("Im in the INSIDE!");
                    GetComponent<CamaraController>().SwitchToMainCamera();
                    stepOnActionableOutsideMainChamber.WasAlreadyTriggered = false;
                    stepOnActionableInside.WasAlreadyTriggered = true;
                }
            }));
        }
        else if ((i == 0) && (j == 1))
        {
            newMedium = new QuerentineFloor("Bed" + i + j, BedComponent, i, j);
            ICollisionable<Component> obstaculo = new SimpleTransporterCollisionable<Component>("ObstaculoCama1", null, newMedium, transporterAndWarehouseManager);
            obstaculo.SetWidh(0.8f);
            obstaculo.SetHeigh(0.8f);
        }
        else if ((i == 0) && (j == -2))
        {
            newMedium = new ActionCollisionableMediumAware<Component>("PlayerStartMedium" + i + j, BedroomFloorComponent, i, j);
        }
        else if (j == -3)
        {
            newMedium = new ActionCollisionableMediumAware<Component>("PlayerStartMedium" + i + j, BedroomFloorComponent, i, j);
        }
        else if ((i == 0) && (j == -4))
        {
            newMedium = new ActionCollisionableMediumAware<Component>("PlayerStartMedium" + i + j, BedroomFloorComponent, i, j);
        }
        if (newMedium == null)
        {
            return null;
        }
        movableMediums.Add(newMedium);
        newMedium.SetMovableMediumAtNorth(getEntityFromTileCoor(i, j + 1));//Add North
        newMedium.SetMovableMediumAtSouth(getEntityFromTileCoor(i, j - 1));
        newMedium.SetMovableMediumAtWest(getEntityFromTileCoor(i - 1, j));
        newMedium.SetMovableMediumAtEast(getEntityFromTileCoor(i + 1, j));
        return newMedium;
    }

    public SinglePlayerComp GetPlayer()
    {
        if (movablesInMap.Count == 0)
        {
            return null;
        }
        return (SinglePlayerComp)movablesInMap[0];
    }

    public Component GetRealInstancePlayer()
    {
        return realInstancePlayer;
    }
}
