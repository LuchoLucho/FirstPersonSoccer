using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionFactory : MonoBehaviour, IConstructionManagerObserver<Component>
{

    public static float isoMetricAngleX = 34.3f*Mathf.PI/180.0f; //RAD
    public static float isoMetricAngleY = 25.7f * Mathf.PI / 180.0f; //RAD
    private const float moduleOfSpriteX = 1.50f;
    private const float moduleOfSpriteY = 1.62f;

    private static Vector2 vx = new Vector2(-moduleOfSpriteX * Mathf.Cos(isoMetricAngleX), -moduleOfSpriteX * Mathf.Sin(isoMetricAngleX));
    private static Vector2 vy = new Vector2(moduleOfSpriteY * Mathf.Cos(isoMetricAngleY), -moduleOfSpriteY * Mathf.Sin(isoMetricAngleY));

    public Component BaldioNS1;

    public const float MAX_TITLE_WIDTH = 8.0f;

    private ConstructionManager constructionManager;

    private static ConstructionFactory instance;

    private Dictionary<Vector2, Vector3> ijCoordToRealDic = new Dictionary<Vector2, Vector3>();
    private Dictionary<Vector2, Component> ijCoordSprite = new Dictionary<Vector2, Component>();
    private Dictionary<Vector2, Component> ijCoordMarker = new Dictionary<Vector2, Component>();

    public static ConstructionFactory GetInstance()
    {
        return instance;
    }

    public ConstructionFactory()
    {  
        instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        constructionManager = GetComponentInParent<ConstructionManager>();
        constructionManager.AddConstructionManagerObserver(this);
        Action<float, float> toBeExecuted = (i,j) => AddNewSpriteInCoord(i,j);
        TileByTileAndExectute(toBeExecuted);
        //----
        Action<float, float> toBeExecutedToSaveRealCoord = (i, j) => 
        {
            float[] realCoordArray = turnIJCoordIntoRealVector3(i, j);
            Vector3 realCoord = new Vector3(realCoordArray[0], realCoordArray[1], realCoordArray[2]);
            ijCoordToRealDic.Add(new Vector2(i, j), realCoord);
        };
        TileByTileAndExectute(toBeExecutedToSaveRealCoord);
    }

    private void TileByTileAndExectute(Action<float, float> toBeExecuted)
    {
        float i = -MAX_TITLE_WIDTH / 2, j = -MAX_TITLE_WIDTH / 2;
        while (j < MAX_TITLE_WIDTH)
        {
            i = -MAX_TITLE_WIDTH / 2;
            while (i < MAX_TITLE_WIDTH)
            {
                toBeExecuted(i, j);
                i += 1.0f;
            }
            j += 1.0f;
        }
    }

    public Vector2 GetAproximateIJFromRealCoord(Vector3 realPosition)
    {
        //I refuse to create an invert function!        
        float miniDistance = float.MaxValue;
        Vector2 minDistanceIJ = new Vector2();
        foreach (KeyValuePair <Vector2,Vector3> currentPair in ijCoordToRealDic)
        {
            float x = currentPair.Value.x - realPosition.x;
            float y = currentPair.Value.y - realPosition.y;
            float z = currentPair.Value.z - realPosition.z;
            float currentDistance = x * x + y * y + z * z;
            if (miniDistance > currentDistance)
            {
                miniDistance = currentDistance;
                minDistanceIJ = currentPair.Key;
            }
        }
        return minDistanceIJ;
    }

    public static float[] turnIJCoordIntoRealVector3(float i, float j)
    {
        Vector2 newCoord = new Vector2();
        newCoord += vx * i;
        newCoord += vy * j;
        float z = -j + (MAX_TITLE_WIDTH - i) / 10;
        return new[] { newCoord.x, newCoord.y, z };
    }

    public static float[] turnRealVector3CoordIntoIJ(float x, float y)
    {       
        float a = 1 - ((vx.y * vy.x) / (vx.x * vy.y));
        float b = (x / vx.x) - ( (y * vy.x) / (vx.x * vy.y) );
        float i = b / a; // I
        float j = (y / vy.y) - ( (vx.y * i) / vy.y );
        return new[] { i, j };
    }

    public Component GetComponentFromIJCoord(float i, float j)
    {
        return ijCoordSprite[new Vector2(i,j)];
    }

    private void AddNewSpriteInCoord(float i, float j)
    {
        float[] realCoordArray = turnIJCoordIntoRealVector3(i, j);
        Vector3 realCoord = new Vector3(realCoordArray[0], realCoordArray[1], realCoordArray[2]);
        IConstruction<Component> newChildConstruction = constructionManager.getConstructionFromTileCoor(i, j);
        Component newChild = null;
        if (newChildConstruction == null)
        {            
            newChild = Instantiate(BaldioNS1, realCoord, Quaternion.identity);
        }
        else
        {
            Component newChildComponentMolde = newChildConstruction.GetComponentMolde();
            newChild = Instantiate(newChildComponentMolde, realCoord, Quaternion.identity);
            newChildConstruction.SetComponentInstanciaReal(newChild);
            newChildConstruction.SetActive(true);
        }
        newChild.transform.parent = this.transform;
        ConstructionClickable consClickable = newChild.GetComponent<ConstructionClickable>();
        if (consClickable != null)
        {
            constructionManager.AddNewConstructionClickeable(consClickable);
        }
        ijCoordSprite.Add(new Vector2(i, j), newChild);
    }

    public bool AddNewMarkerToIJCoord(Component toBeAddedMolde, float i, float j)
    {
        float[] realCoordArray = turnIJCoordIntoRealVector3(i, j);
        Vector3 realCoord = new Vector3(realCoordArray[0], realCoordArray[1], -8+0*realCoordArray[2]);        
        Vector2 keyToBe = new Vector2(i, j);
        if (ijCoordMarker.ContainsKey(keyToBe))
        {
            return false;
        }
        Component newChild = Instantiate(toBeAddedMolde, realCoord, Quaternion.identity);
        ijCoordMarker.Add(keyToBe, newChild);
        return true;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void NewBuildCreated(IConstruction<Component> constructionToBeRender)
    {
        Vector2 keyToFindAlreadyBuild = new Vector2(constructionToBeRender.GetCoordI(), constructionToBeRender.GetCoordJ());
        if ( ijCoordSprite.ContainsKey(keyToFindAlreadyBuild))
        {
            //TODO: If sprint is a construction The construction manager should be the one detroying it first...?
            Component componentToBeDestroyed = ijCoordSprite[keyToFindAlreadyBuild];
            Destroy(componentToBeDestroyed.gameObject);
            ijCoordSprite.Remove(keyToFindAlreadyBuild);
        }
        AddNewSpriteInCoord(constructionToBeRender.GetCoordI(), constructionToBeRender.GetCoordJ());
    }
}
