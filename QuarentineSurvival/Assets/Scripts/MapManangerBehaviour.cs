using Assets.Scripts;
using Assets.Scripts.MapObjects;
using Assets.Scripts.Players;
using QuarentineSurvival.Model;
using QuarentineSurvival.Model.Interface;
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

    private List<IMovableMediumCollisionAware<Component>> movableMediums = new List<IMovableMediumCollisionAware<Component>>();

    private List<IMovable<Component>> movablesInMap = new List<IMovable<Component>>();

    private Component realInstancePlayer;
    private SimpleDoor<Component> realInstanceDoor;

    public SimpleDoor<Component> GetSingleDoor()
    {
        return realInstanceDoor;
    }

    private IConstructionManagerObserver<Component> singleObserver;
    private ITransporterAndWarehouseManager<Component> transporterAndWarehouseManager;

    public Camera mainCamera;
    public Camera chestCamera;

    // Start is called before the first frame update
    void Start()
    {
        transporterAndWarehouseManager = new TransporterAndWarehouseManager<Component>();
        IMovableMediumCollisionAware<Component> medium = getEntityFromTileCoor(0,0);//new SimpleStreet<Component>("Floor", null, 0, 0);
        ICargoTransporter<Component> player = new SinglePlayerComp("Player", SinglePlayerComponent, medium, transporterAndWarehouseManager);
        player.SetDirectionI(+0);
        player.SetDirectionJ(+0);
        player.SetVelocity(0.3f);
        movablesInMap.Add(player);
        NewMovable(movablesInMap[0]);
        mainCamera.enabled = true;
        chestCamera.enabled = false;
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
        /*IObject<Component> contructionToFind = constructionInMap.Find(x => x.GetCoordI() == i && x.GetCoordJ() == j);
        if (contructionToFind == null)
        {
            return null;
        }
        return contructionToFind;*/
        /*if (i == 0 && j == 0)
        {
            return entitiesInMap[0];
        }*/
        if ((i==2) && (j==0))
        {
            WarehouseChest<Component> newMedium = new WarehouseChest<Component>("Cofre" + i + j, ChestComponent, i, j, transporterAndWarehouseManager);
            movableMediums.Add(newMedium);
            {
                newMedium.SetMovableMediumAtNorth(getEntityFromTileCoor(i, j + 1));//Add North
                newMedium.SetMovableMediumAtSouth(getEntityFromTileCoor(i, j - 1));
                newMedium.SetMovableMediumAtWest(getEntityFromTileCoor(i - 1, j));
                newMedium.SetMovableMediumAtEast(getEntityFromTileCoor(i + 1, j));
            }
            ICargo<Component> simpleCargo = new SimpleCargo<Component>();
            IResource resource = new SimpleResource(1, "Encendedor", 0);
            IMovableMedium<Component> destinyOfResources = null; // The resouce has no fixed destination
            simpleCargo.addResources(resource, destinyOfResources);
            newMedium.addCargo(simpleCargo);
            //More cargo
            simpleCargo = new SimpleCargo<Component>();
            resource = new SimpleResource(1, "Lavandina", 0);
            destinyOfResources = null; // The resouce has no fixed destination
            simpleCargo.addResources(resource, destinyOfResources);
            //----
            newMedium.addCargo(simpleCargo);
            //----
            newMedium.OnMovableArrivedAlsoDo(medium => 
            {
                mainCamera.enabled = false;
                chestCamera.enabled = true;
            }
            );
        }  else if ((Math.Abs(i) <= 1) && (Math.Abs(j) <= 1))
        {
            if (movableMediums.Find(x => x.GetCoordI() == i && x.GetCoordJ() == j) == null)
            {
                IMovableMediumCollisionAware<Component> newMedium = new ActionCollisionableMediumAware<Component>("Street" + i + j, SimpleRoadComponent, i, j);
                if ((i == 1) && (j == 0))
                {
                    newMedium.OnMovableArrivedAlsoDo(medium =>
                    {
                        mainCamera.enabled = true; 
                        chestCamera.enabled = false;
                    });
                }
                if ((i == 0) && (j == -1))
                {
                    newMedium = new QuerentineFloor("ActionStreetCollisionableMediumAware" + i+j, ActionableMediumWithDoor, i,j);
                    IMovableMediumCollisionAware<Component> pisoActionable = (IMovableMediumCollisionAware<Component>)newMedium;
                    realInstanceDoor = new SimpleDoorComp("Puerta", null, pisoActionable, transporterAndWarehouseManager);
                    pisoActionable.addActionable((IActionable<Component>)realInstanceDoor);
                }
                movableMediums.Add(newMedium);
                newMedium.SetMovableMediumAtNorth(getEntityFromTileCoor(i, j + 1));//Add North
                newMedium.SetMovableMediumAtSouth(getEntityFromTileCoor(i, j - 1));
                newMedium.SetMovableMediumAtWest(getEntityFromTileCoor(i - 1, j));
                newMedium.SetMovableMediumAtEast(getEntityFromTileCoor(i + 1, j));
            }
        }        
        return movableMediums.Find(x => x.GetCoordI() == i && x.GetCoordJ() == j);
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
