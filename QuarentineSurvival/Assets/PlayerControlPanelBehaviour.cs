using Assets.Scripts.Players;
using QuarentineSurvival.Model.Interface;
using SaavedraCraft.Model.Interfaces.Transportation;
using SaavedraCraft.Model.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using QuarentineSurvival.Model;
using SaavedraCraft.Model.Interfaces;

public class PlayerControlPanelBehaviour : MonoBehaviour
{
    public const int TOTAL_ANIMATION_TIME = 2;
    public const int CHEST_ITEMS_INTERFACE = 2;
    public const float PLAYER_VELOCITY = 0.45f;

    public Texture chestTexture;

    private float currentPlayerDirection = 90.0f;//UP

    private MapManangerBehaviour constructionManager;
    private SinglePlayerComp player;
    private Component realInstancePlayer;

    private float animationTime = 0;
    public Camera currentCamera;
    public Component marker;
    private Component currentMarker;

    private bool areValuesInitiated = false;
    private bool isChestAvailableToOpen = false;
    private bool isChestOpen = false;
    private IWarehouse<Component> currentWarehouse = null;
    private bool inventoryOpen = false;

    // Start is called before the first frame update
    void Start()
    {        
    }

    private void InitCustomPlayerActionsAndProperties()
    {
        Action<IWarehouse<Component>> actionOnParkingSpaceAvailable = x => 
        {
            //x.parkTransporter(player);
            string logMe = "PlayerControlPanelBehaviour InitCustomPlayerActionsAndProperties CHEST to be open!";
            logMe += "\r\n";
            logMe += "Let see what is inside! =>";
            logMe += "\r\n";
            if (x.ShowAllCargo().Count > 0)
            {
                x.ShowAllCargo().ForEach(y =>
                {
                    logMe += y.ToString();
                    logMe += "\r\n";
                });
                isChestAvailableToOpen = true;
            }
            else
            {
                logMe += "EMPTY!!!!";
                logMe += "\r\n";
            }
            currentWarehouse = x;
            x.addCargoTransporter(player);
            Debug.Log(logMe);
            //x.unparkTransporter(player);
        };
        Action<IWarehouse<Component>> actionOnPlayerLeftWarehouse = x =>
        {            
            currentWarehouse = null;
            isChestOpen = false;
            isChestAvailableToOpen = false;
            x.removeCargoTransporter(player);
            Debug.Log("You left the warehouse");
        };
        //---- Special behaviour to player, when ever arrive to warehouse is added as transporter:
        //Following TransportGetsToWarehouseIsAddedAsAvailableTransporterTest        
        player.OnParkinSpaceAvailableFromWarehouse(actionOnParkingSpaceAvailable);
        player.OnTransportPartFromWarehouse(actionOnPlayerLeftWarehouse);
    }

    // Update is called once per frame
    void Update()
    {
        if (!areValuesInitiated)
        {
            constructionManager = GetComponentInParent<MapManangerBehaviour>();
            player = constructionManager.GetPlayer();
            realInstancePlayer = constructionManager.GetRealInstancePlayer();
            if (player != null)
            {
                InitCustomPlayerActionsAndProperties();
                areValuesInitiated = true;
            }            
        }
        if (animationTime > 0)
        {
            animationTime -= Time.deltaTime;
            if (animationTime <= 0.01)
            {
                player.SetVelocity(0.0f);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log(Screen.currentResolution);
            //Debug.Log("mouseDown = " + Input.mousePosition.x/ Screen.currentResolution.width + " " + Input.mousePosition.y/ Screen.currentResolution.height);
            Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
            Vector3 p0 = ray.GetPoint(0);
            Vector3 v = ray.direction;            
            float lambda = -p0.y / v.y;
            Vector3 rayHittingFloor = new Vector3(p0.x + lambda * v.x, p0.y + lambda * v.y, p0.z + lambda * v.z);
            Debug.Log("Ray origin " + p0 + " Direction: " + v + " Lambda = " +  lambda + " rayHittingFloor = " + rayHittingFloor);
            if (currentMarker != null)
            {
                Destroy(currentMarker.gameObject);                
            }
            currentMarker = Instantiate(marker, rayHittingFloor, Quaternion.identity);
            GoToRealUnityCoord(rayHittingFloor);
        }
    }

    public void GoToRealUnityCoord(Vector3 rayHittingFloor)
    {
        float[] ijCoord = MapFactoryBehaviour.turnRealIntoIJ(rayHittingFloor);
        //currentAnimationMove = new AnimationMove(player.GetCoordI(), player.GetCoordJ(), ijCoord[0], ijCoord[1],player);
        //this.getin
        float i0 = player.GetCoordI();
        float j0 = player.GetCoordJ();
        float i1 = ijCoord[0];
        float j1 = ijCoord[1];
        float vi;
        float vj;
        float distance = (float)Math.Sqrt((i0 - i1) * (i0 - i1) + (j0 - j1) * (j0 - j1));
        float moveTime = distance / PLAYER_VELOCITY;
        animationTime = moveTime;
        vi = (i1 - i0) / distance;
        vj = (j1 - j0) / distance;
        player.SetDirectionI(vi);
        player.SetDirectionJ(vj);
        player.SetVelocity(PLAYER_VELOCITY);
        float angle = 180.0f * Mathf.Atan2(vj, vi) / Mathf.PI;
        //angle += 180.0f;
        //realInstancePlayer.transform.Rotate(0, angle, 0);
    }

    
    void OnGUI()
    {
        if (realInstancePlayer == null)
        {
            return;
        }
        float newPlayerDirection = 0;
        GUI.skin.button.fontSize = 18;
        GUI.skin.label.fontSize = 18;
        if (inventoryOpen)
        {
            int i, j, cargoIndex = 0;
            GUI.Box(new Rect(450, 20, 360, 200), chestTexture);
            GUI.Label(new Rect(550, 20, 80, 50), "Inventory");
            player.showCargo().ForEach(x =>
            {
                i = cargoIndex % CHEST_ITEMS_INTERFACE;
                j = cargoIndex / CHEST_ITEMS_INTERFACE;
                GUI.Button(new Rect(480 + i * 160, 50 + j * 50, 150, 45), x.ToString());
                cargoIndex++;
            });
            List<ICombinableResource> allCombinables = new List<ICombinableResource>();
            player.showCargo().ForEach(c =>
            {
                if (c.ShowResource() is ICombinableResource)
                {
                    allCombinables.Add((ICombinableResource)c.ShowResource());
                }
            }
            );
            if (allCombinables.Count >= 2)
            {
                if (GUI.Button(new Rect(480 + 0, 50 + 4 * 50, 150, 45), "Combine"))
                {
                    ICombinableResource[] toCombine = null;
                    for (int ii = 0; ii < allCombinables.Count - 1; ii++)
                    {
                        ICombinableResource resource1 = allCombinables[ii];
                        for (int jj = ii+1; jj< allCombinables.Count; jj++)
                        {
                            ICombinableResource resource2 = allCombinables[jj];
                            if (resource1.CanCombineWith(resource2))
                            {
                                toCombine = new ICombinableResource[] { resource1,resource2};
                                break;
                            }
                        }
                        if (toCombine!=null)
                        {
                            break;
                        }
                    }
                    if (toCombine != null)
                    {
                        IActionable<Component> currentActionable = player.ShowAllActionables().Find(x=>x == toCombine[0]);
                        currentActionable.ShowAvailableActions()[0].execute(player, player, currentActionable, toCombine[1]);
                    }
                }
            }
        }
        if (isChestOpen)
        {
            int i, j, cargoIndex = 0;
            GUI.Box(new Rect(30, 20, 360, 200), chestTexture);
            GUI.Label(new Rect(200,20,80,50),"Chest");
            List<ICargo<Component>> toTransferToPlayer = new List<ICargo<Component>>();
            currentWarehouse.ShowAllCargo().ForEach(currentCargo =>
            {
                i = cargoIndex % CHEST_ITEMS_INTERFACE;
                j = cargoIndex / CHEST_ITEMS_INTERFACE;
                if (GUI.Button(new Rect(50 + i * (0 + 160), 50 + j * 50, 150, 45), currentCargo.ToString()))
                {
                    //currentWarehouse.UnloadCargoToCurrentTransporters(currentCargo); Do not modiy the colleciton inside the iteration!
                    toTransferToPlayer.Add(currentCargo);
                }
                cargoIndex++;
            });
            toTransferToPlayer.ForEach(cargoToSent=> currentWarehouse.UnloadCargoToCurrentTransporters(cargoToSent));
            i = 0;
            j = CHEST_ITEMS_INTERFACE;
            inventoryOpen = true; //If chest is open, show inventory too!
            if (GUI.Button(new Rect(200 + i * (0 + 10), 50 + j * 50, 150, 45), "Close"))
            {
                isChestOpen = false;
            }
            return; // <<<<<<<<<<<<<<<<<<<<<<<<< IF THE CHEST IS OPEN, JUST SHOW THAT!
        }        
        if (GUI.Button(new Rect(20, 30, 150, 45), "Up"))
        {
            newPlayerDirection = 90;
            player.SetDirectionI(0);
            player.SetDirectionJ(+1);
            player.SetVelocity(PLAYER_VELOCITY);
            animationTime = TOTAL_ANIMATION_TIME;
        }
        if (GUI.Button(new Rect(20, 30 + 50, 150, 45), "Down"))
        {
            newPlayerDirection = 270;
            player.SetDirectionI(0);
            player.SetDirectionJ(-1);
            player.SetVelocity(PLAYER_VELOCITY);
            animationTime = TOTAL_ANIMATION_TIME;
        }
        if (GUI.Button(new Rect(20, 30 + 2 * 50, 150, 45), "Right"))
        {
            newPlayerDirection = 360;
            player.SetDirectionI(1);
            player.SetDirectionJ(0);
            player.SetVelocity(PLAYER_VELOCITY);
            animationTime = TOTAL_ANIMATION_TIME;
        }
        if (GUI.Button(new Rect(20, 30 + 3 * 50, 150, 45), "Left"))
        {
            newPlayerDirection = 180;
            player.SetDirectionI(-1);
            player.SetDirectionJ(0);
            player.SetVelocity(PLAYER_VELOCITY);
            animationTime = TOTAL_ANIMATION_TIME;
        }
        if (isChestAvailableToOpen)
        {
            if (GUI.Button(new Rect(20, 30 + 4 * 50, 150, 45), "Open"))
            {
                isChestOpen = true;
            }
        }
        if (GUI.Button(new Rect(200, 30, 150, 45), "Inventory"))
        {
            inventoryOpen = !inventoryOpen;
        }
        if (player.ShowAvailableActions().Count > 0)
        {
            int j = 0;
            foreach (IAction<Component> currentAction in player.ShowAvailableActions())
            {
                if (GUI.Button(new Rect(200, 30 + j*50 + 50, 150, 45), currentAction.ToString()))
                {
                    Debug.Log("Action! Actionable source=" + currentAction.getSourceActionable());
                    currentAction.execute(player, null, null);
                }
                j++;
            }
        }
        if (newPlayerDirection!=0)
        {
            float rotation = currentPlayerDirection - newPlayerDirection;
            //Debug.Log("Rotation : " + rotation + " CurrentR = " + currentPlayerDirection + " NewPlayerDir = " + newPlayerDirection);
            realInstancePlayer.transform.Rotate(0, rotation, 0);
            currentPlayerDirection = newPlayerDirection;
        }        
    }

}
