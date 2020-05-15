using System;
using System.Collections;
using System.Collections.Generic;
using SaavedraCraft.Model.Interfaces;
using UnityEngine;

public class ComponentModelAware : MonoBehaviour
{
    private IObject<Component> quarentineModel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetQuarentineModel(IObject<Component> newModel)
    {
        quarentineModel = newModel;
    }

    public IObject<Component> GetQuarentineModel()
    {
        return quarentineModel;
    }
}
