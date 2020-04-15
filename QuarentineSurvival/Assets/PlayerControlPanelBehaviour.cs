using Assets.Scripts.Players;
using SaavedraCraft.Model.Interfaces.Transportation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlPanelBehaviour : MonoBehaviour
{
    public const int TOTAL_ANIMATION_TIME = 40;

    private float currentPlayerDirection = 90.0f;//UP

    private MapManangerBehaviour constructionManager;
    private SinglePlayerComp player;
    private Component realInstancePlayer;

    private int animationTime = 0;

    private bool areValuesInitiated = false;
    private bool isChestAvailableToOpen = false;
    
    // Start is called before the first frame update
    void Start()
    {        
    }

    private void InitCustomPlayerActionsAndProperties()
    {
        Action<IWarehouse<Component>> actionOnParkingSpaceAvailable = x => 
        {
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
            Debug.Log(logMe);
        };
        player.OnParkinSpaceAvailableFromWarehouse(actionOnParkingSpaceAvailable);
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
            animationTime--;
            if (animationTime == 0)
            {
                player.SetVelocity(0.0f);
            }
        }
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
        if (GUI.Button(new Rect(20, 30, 150, 45), "Up"))
        {
            newPlayerDirection = 90;
            player.SetDirectionI(0);
            player.SetDirectionJ(+1);
            player.SetVelocity(0.3f);
            animationTime = TOTAL_ANIMATION_TIME;
        }
        if (GUI.Button(new Rect(20, 30 + 50, 150, 45), "Down"))
        {
            newPlayerDirection = 270;
            player.SetDirectionI(0);
            player.SetDirectionJ(-1);
            player.SetVelocity(0.3f);
            animationTime = TOTAL_ANIMATION_TIME;
        }
        if (GUI.Button(new Rect(20, 30 + 2 * 50, 150, 45), "Right"))
        {
            newPlayerDirection = 360;
            player.SetDirectionI(1);
            player.SetDirectionJ(0);
            player.SetVelocity(0.3f);
            animationTime = TOTAL_ANIMATION_TIME;
        }
        if (GUI.Button(new Rect(20, 30 + 3 * 50, 150, 45), "Left"))
        {
            newPlayerDirection = 180;
            player.SetDirectionI(-1);
            player.SetDirectionJ(0);
            player.SetVelocity(0.3f);
            animationTime = TOTAL_ANIMATION_TIME;
        }
        if (isChestAvailableToOpen)
        {
            if (GUI.Button(new Rect(20, 30 + 4 * 50, 150, 45), "Open"))
            {
                //TODO
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
