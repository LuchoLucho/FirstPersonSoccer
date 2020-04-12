using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Component = UnityEngine.Component;

namespace Assets.Scripts
{
    public class MapFactoryBehaviour : MonoBehaviour,IConstructionManagerObserver<Component>
    {
        public const float MAX_TITLE_WIDTH = 4.0f;

        private MapManangerBehaviour constructionManager;
        public Component Baldio;
        private Dictionary<Vector2, Component> ijCoordEntity = new Dictionary<Vector2, Component>();
        private Dictionary<Vector2, Vector3> ijCoordToRealDic = new Dictionary<Vector2, Vector3>();
        private bool creationFlag = false;

        public void Start()
        {
            //StartTitleCreation();
        }

        public void Update()
        {
            if (!creationFlag)
            {
                creationFlag = true;
                StartTitleCreation();
            }
        }

        private void StartTitleCreation()
        {
            constructionManager = GetComponentInParent<MapManangerBehaviour>();
            constructionManager.AddConstructionManagerObserver(this);
            Action<float, float> toBeExecuted = (i, j) => AddNewEntityInCoord(i, j);
            TileByTileAndExectute(toBeExecuted);
            Action<float, float> toBeExecutedToSaveRealCoord = (i, j) =>
            {
                float[] realCoordArray = turnIJCoordIntoRealVector3(i, j);
                Vector3 realCoord = new Vector3(realCoordArray[0], realCoordArray[1], realCoordArray[2]);
                ijCoordToRealDic.Add(new Vector2(i, j), realCoord);
            };
            TileByTileAndExectute(toBeExecutedToSaveRealCoord);
        }

        public void NewBuildCreated(IObject<Component> constructionToBeRender)
        {
            //throw new NotImplementedException();
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

        private void AddNewEntityInCoord(float i, float j)
        {
            float[] realCoordArray = turnIJCoordIntoRealVector3(i, j);
            Vector3 realCoord = new Vector3(realCoordArray[0], realCoordArray[1], realCoordArray[2]);
            IObject<Component> newChildConstruction = constructionManager.getEntityFromTileCoor(i, j);
            Component newChild = null;
            if (newChildConstruction == null)
            {
                newChild = Instantiate(Baldio, realCoord, Quaternion.identity);
            }
            else
            {
                Component newChildComponentMolde = newChildConstruction.GetComponentMolde();
                newChild = Instantiate(newChildComponentMolde, realCoord, Quaternion.identity);
                newChildConstruction.SetComponentInstanciaReal(newChild);
                newChildConstruction.SetActive(true);
            }
            newChild.transform.parent = this.transform;
            /*ConstructionClickable consClickable = newChild.GetComponent<ConstructionClickable>();
            if (consClickable != null)
            {
                constructionManager.AddNewConstructionClickeable(consClickable);
            }*/
            ijCoordEntity.Add(new Vector2(i, j), newChild);
        }

        public static float[] turnIJCoordIntoRealVector3(float i, float j)
        {
            
            return new[] { 15*i, -1, 15*j };
        }
    }
}
