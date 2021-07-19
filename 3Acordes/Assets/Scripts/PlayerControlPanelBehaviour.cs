using Assets.Scripts.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class PlayerControlPanelBehaviour : MonoBehaviour
{
    private float movementSpeed = 0.5f;
    public const float PLAYER_VELOCITY = 0.85f;
    public const int TOTAL_ANIMATION_TIME = 1;

    private float currentPlayerDirection = 90.0f;//UP
    private MapManangerBehaviour constructionManager;
    private SinglePlayerComp player;
    private Component realInstancePlayer;
    private bool areValuesInitiated = false;
    private float animationTime = 0;
    private Animator animator;
    private bool isIdle = true;

    private void InitCustomPlayerActionsAndProperties()
    {
    }

    void Update()
    {
        /*
        //get the Input from Horizontal axis
        float horizontalInput = 1.5f*Input.GetAxis("Horizontal");
        //get the Input from Vertical axis
        float verticalInput = 0.75f*Input.GetAxis("Vertical");
        transform.position = transform.position + new Vector3(horizontalInput * movementSpeed * Time.deltaTime, verticalInput * movementSpeed * Time.deltaTime, 0);
        //output to log the position change
        Debug.Log(transform.position);
        */
        if (!areValuesInitiated)
        {
            constructionManager = GetComponentInParent<MapManangerBehaviour>();
            if (constructionManager == null)
            {
                Debug.LogError("constructionManager == null");
                return;
            }
            player = constructionManager.GetPlayer();
            realInstancePlayer = constructionManager.GetRealInstancePlayer();
            if (player != null)
            {
                InitCustomPlayerActionsAndProperties();
                areValuesInitiated = true;
                Debug.LogWarning("Player controller was initialized!");
            }
            animator = realInstancePlayer.GetComponentInChildren<Animator>();
            if (animator == null)
            {
                Debug.LogWarning("There is no Animator on PLAYER!");
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
        else if (!isIdle)
        {
            isIdle = true;
            //IDLE!
            Debug.Log("IDLE!");
            animator.SetBool("IsIdle", true);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsHitting", false);
        }
    }

    public void OnGUI()
    {
        if (realInstancePlayer == null)
        {
            return;
        }
        float newPlayerDirection = 0;
        if (GUI.Button(new Rect(20, 30 + 0, 100, 45), "Up"))
        {
            newPlayerDirection = 270;
            player.SetDirectionI(0);
            player.SetDirectionJ(+1);
            player.SetVelocity(PLAYER_VELOCITY);
            animationTime = TOTAL_ANIMATION_TIME;
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsHitting", false);
            isIdle = false;
        }
        if (GUI.Button(new Rect(20, 30 + 50, 100, 45), "Down"))
        {
            newPlayerDirection = 270;
            player.SetDirectionI(0);
            player.SetDirectionJ(-1);
            player.SetVelocity(PLAYER_VELOCITY);
            animationTime = TOTAL_ANIMATION_TIME;
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsHitting", false);
            isIdle = false;
        }
        if (GUI.Button(new Rect(20, 30 + 2 * 50, 100, 45), "Right"))
        {
            newPlayerDirection = 360;
            player.SetDirectionI(1);
            player.SetDirectionJ(0);
            player.SetVelocity(PLAYER_VELOCITY);
            animationTime = TOTAL_ANIMATION_TIME;
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsHitting", false);
            isIdle = false;
        }
        if (GUI.Button(new Rect(20, 30 + 3 * 50, 100, 45), "Left"))
        {
            newPlayerDirection = 180;
            player.SetDirectionI(-1);
            player.SetDirectionJ(0);
            player.SetVelocity(PLAYER_VELOCITY);
            animationTime = TOTAL_ANIMATION_TIME;
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsHitting", false);
            isIdle = false;
        }
        if (GUI.Button(new Rect(150, 30 + 0, 100, 45), "Pina"))
        {
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsHitting",true);
            animator.SetBool("IsWalking", false);
            player.SetDirectionI(0.0001f);
            player.SetDirectionJ(0.0001f);
            player.SetVelocity(0);
            animationTime = TOTAL_ANIMATION_TIME;
            isIdle = false;
        }
        if (GUI.Button(new Rect(150, 30 + 50, 100, 45), "No Pina"))
        {
            isIdle = true;
            animator.SetBool("IsIdle", true);
            animator.SetBool("IsHitting", false);
            animator.SetBool("IsWalking", false);
            player.SetVelocity(0);
        }
    }
}
