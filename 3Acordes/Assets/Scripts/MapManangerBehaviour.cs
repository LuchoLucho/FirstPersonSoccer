using Assets.Scripts;
using Assets.Scripts.Player;
using QuarentineSurvival.Model;
using QuarentineSurvival.Model.Actions;
using QuarentineSurvival.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Interfaces.Transportation;
using SaavedraCraft.Model.Transportation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManangerBehaviour : MonoBehaviour
{
    public Component SinglePlayerComponent;
    public Component SimpleRoadComponent;

    private ICargoTransporter<Component> player;
    private Component realInstancePlayer;

    private List<IMovableMediumCollisionAware<Component>> movableMediums = new List<IMovableMediumCollisionAware<Component>>();
    private ITransporterAndWarehouseManager<Component> transporterAndWarehouseManager;
    private List<IMovable<Component>> movablesInMap = new List<IMovable<Component>>();
    private IConstructionManagerObserver<Component> singleObserver;

    private float playerInitSpawnCoordI = 0f;
    private float playerInitSpawnCoordJ = 0f;
    private List<StepOnActionableComp> stepOnActionablesFloorList = new List<StepOnActionableComp>();

    // Start is called before the first frame update
    void Start()
    {
        transporterAndWarehouseManager = new TransporterAndWarehouseManager<Component>();
        IMovableMediumCollisionAware<Component> medium = getEntityFromTileCoor(playerInitSpawnCoordI, playerInitSpawnCoordJ);//new SimpleStreet<Component>("Floor", null, 0, 0);
        player = new SinglePlayerComp("Player", SinglePlayerComponent, medium, transporterAndWarehouseManager);
        player.SetDirectionI(+0);
        player.SetDirectionJ(+0);
        player.SetVelocity(0.5f);
        movablesInMap.Add(player);
        NewMovable(movablesInMap[0]);
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
        if ((Mathf.Abs(i) >= MapFactoryBehaviour.MAX_TITLE_WIDTH) || (Mathf.Abs(j) >= MapFactoryBehaviour.MAX_TITLE_HEIGH))
        {
            return null;
        }
        if (/*(Mathf.Abs(i) > 3) ||*/ (Mathf.Abs(j) > 2))
        {
            return null;
        }
        if (((int) Mathf.Round(i)) % 3 == 0 && j == 1.0f)
        {
            return null;
        }
        IMovableMediumCollisionAware<Component> newMedium = movableMediums.Find(x => x.GetCoordI() == i && x.GetCoordJ() == j);
        if (newMedium != null)
        {
            return newMedium;
        }
        newMedium = new ActionCollisionableMediumAware<Component>("Street" + i + j, SimpleRoadComponent, i, j);
        if (i != playerInitSpawnCoordI || j != playerInitSpawnCoordJ) //The action on the same floor that the player Spawn generates a weird BUG!
        {            
            StepOnActionableComp stepOnFloorAction = new StepOnActionableComp("StepOnMeBeforeChestCamera", null, newMedium, transporterAndWarehouseManager);
            stepOnActionablesFloorList.Add(stepOnFloorAction);
            stepOnFloorAction.SetWidh(0.9f);
            stepOnFloorAction.SetHeigh(0.9f);
            stepOnFloorAction.SetAutoAction(new AutoAction<Component>(x => {
                if (x == player)
                {
                    Debug.Log("TEST: ACTION! " + stepOnFloorAction.ToString());
                    Vector3 playerRealCoord = MapFactoryBehaviour.turnIJCoordIntoRealVector3(player);
                    GetComponent<CameraController>().MoveCameraTo(/*stepOnFloorAction*/playerRealCoord);
                    //stepOnActionableOutsideMainChamber.WasAlreadyTriggered = false;
                    //stepOnActionableInside.WasAlreadyTriggered = true;
                    killTriggerForFloorActions();
                    stepOnFloorAction.WasAlreadyTriggered = true;

                    /*this.GetComponent<CamaraController>().SwitchToMainCamera();
                    stepOnActionableBeforeChestCamera.WasAlreadyTriggered = true;
                    stepOnActionableChestCamera.WasAlreadyTriggered = false;*/
                }
                else
                {
                    Debug.Log("Test ACTION! BUT not was from PLAyER!?");
                }
                
            }));
        }
        /*else
        {
            newMedium = new ActionCollisionableMediumAware<Component>("PlayerStartMedium" + i + j, SimpleRoadComponent, i, j);
        }*/
        movableMediums.Add(newMedium);
        newMedium.SetMovableMediumAtNorth(getEntityFromTileCoor(i, j + 1));//Add North
        newMedium.SetMovableMediumAtSouth(getEntityFromTileCoor(i, j - 1));
        newMedium.SetMovableMediumAtWest(getEntityFromTileCoor(i - 1, j));
        newMedium.SetMovableMediumAtEast(getEntityFromTileCoor(i + 1, j));
        return newMedium;
    }

    private void killTriggerForFloorActions()
    {
        stepOnActionablesFloorList.ForEach(x => x.WasAlreadyTriggered = false); ;
    }

    // Update is called once per frame
    void Update()
    {
        float timeDelta = Time.deltaTime;
        if (movablesInMap.Count == 0)
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

    public Component GetRealInstancePlayer()
    {
        return realInstancePlayer;
    }

    public SinglePlayerComp GetPlayer()
    {
        if (movablesInMap.Count == 0)
        {
            return null;
        }
        return (SinglePlayerComp)movablesInMap[0];
    }
}
