using Assets.Scripts;
using Assets.Scripts.Players;
using SaavedraCraft.Model.Interfaces;
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

    private List<IMovableMedium<Component>> movableMediums = new List<IMovableMedium<Component>>();

    private List<IMovable<Component>> movablesInMap = new List<IMovable<Component>>();

    private Component realInstancePlayer;
    private IConstructionManagerObserver<Component> singleObserver;

    // Start is called before the first frame update
    void Start()
    {
        IMovableMedium<Component> medium = getEntityFromTileCoor(0,0);//new SimpleStreet<Component>("Floor", null, 0, 0);
        IMovable<Component> player = new SinglePlayerComp("Player", SinglePlayerComponent, medium);
        player.SetDirectionI(+0);
        player.SetDirectionJ(+0);
        player.SetVelocity(0.3f);
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

    public void NewMovableMedium(IMovableMedium<Component> toBeConstructedClone)
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

    public IMovableMedium<Component> getEntityFromTileCoor(float i, float j)
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
        if ((Math.Abs(i)<=1) && (Math.Abs(j) <= 1))
        {
            if (movableMediums.Find(x=>x.GetCoordI() == i && x.GetCoordJ() == j) == null)
            {
                IMovableMedium<Component> newMedium = new SimpleStreet<Component>("Street" + i + j, SimpleRoadComponent, i, j);
                movableMediums.Add(newMedium);
               // if (i == 0 && j == 0)
                {
                    newMedium.SetMovableMediumAtNorth(getEntityFromTileCoor(i, j + 1));//Add North
                    newMedium.SetMovableMediumAtSouth(getEntityFromTileCoor(i, j - 1));
                    newMedium.SetMovableMediumAtWest(getEntityFromTileCoor(i - 1, j));
                    newMedium.SetMovableMediumAtEast(getEntityFromTileCoor(i + 1, j));
                }
            }             
        } else if ((i==2) && (j==0))
        {
            IMovableMedium<Component> newMedium = new SimpleStreet<Component>("Cofre" + i + j, ChestComponent, i, j);
            movableMediums.Add(newMedium);
            {
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
