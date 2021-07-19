using Assets.Scripts.SimpleBehaviours;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Interfaces.Transportation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class MapFactoryBehaviour : MonoBehaviour,IConstructionManagerObserver<Component>
    {
        public const float MAX_TITLE_WIDTH = 15.0f;
        public const float MAX_TITLE_HEIGH = 5.0f;
        public const float SCALE = 1.0f;

        public Component Baldio;

        private MapManangerBehaviour constructionManager;
        private bool creationFlag = false;
        private Dictionary<Vector2, Vector3> ijCoordToRealDic = new Dictionary<Vector2, Vector3>();
        private Dictionary<Vector2, Component> ijCoordEntity = new Dictionary<Vector2, Component>();

        public void NewBuildCreated(IObject<Component> constructionToBeRender)
        {
            //throw new NotImplementedException();
        }

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

        private void TileByTileAndExectute(Action<float, float> toBeExecuted)
        {
            float i = -MAX_TITLE_WIDTH , j = -MAX_TITLE_HEIGH;
            while (j < MAX_TITLE_HEIGH)
            {
                i = -MAX_TITLE_WIDTH;
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
                //return; //NO VALDIO!!!
                newChild = Instantiate(Baldio, realCoord, Quaternion.identity);
            }
            else
            {
                Component newChildComponentMolde = newChildConstruction.GetComponentMolde();
                newChild = Instantiate(newChildComponentMolde, realCoord, Quaternion.identity);
                newChildConstruction.SetComponentInstanciaReal(newChild);
                newChildConstruction.SetActive(true);
                if (newChild.GetComponents<ComponentModelAware>().Any())//The UI need to be aware of the model!
                {
                    newChild.GetComponents<ComponentModelAware>().ToList().ForEach(x => x.SetQuarentineModel(newChildConstruction));
                }
                if (newChild.GetComponentsInChildren<ComponentModelAware>().Any())//The UI need to be aware of the model!
                {
                    newChild.GetComponentsInChildren<ComponentModelAware>().ToList().ForEach(x => x.SetQuarentineModel(newChildConstruction));
                }
            }
            newChild.transform.parent = this.transform;
            /*ConstructionClickable consClickable = newChild.GetComponent<ConstructionClickable>();
            if (consClickable != null)
            {
                constructionManager.AddNewConstructionClickeable(consClickable);
            }*/
            ijCoordEntity.Add(new Vector2(i, j), newChild);
        }

        internal static Vector3 turnIJCoordIntoRealVector3(IObject<Component> obj)
        {
            float[] realCoord = turnIJCoordIntoRealVector3(obj.GetCoordI(), obj.GetCoordJ());
            return new Vector3(realCoord[0], realCoord[1],0);
        }

        public static float[] turnIJCoordIntoRealVector3(float i, float j)
        {
            return new[] { SCALE * i, -1, SCALE * j };
        }

        public static float[] turnRealIntoIJ(Vector3 realCoord)
        {
            return new[] { realCoord.x / SCALE, realCoord.z / SCALE };
        }
    }
}
