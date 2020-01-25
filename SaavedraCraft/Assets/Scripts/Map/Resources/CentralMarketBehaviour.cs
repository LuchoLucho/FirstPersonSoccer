using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralMarketBehaviour : MonoBehaviour
{
    private ICentralMarket<Component> centralMarket = new CentralMarket<Component>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<Transaction<Component>> transactions = GetCentralMarket().GetTransactions();
        if (transactions.Count > 0 )
        {
            Debug.Log("There are transactions! Cnt = " + transactions.Count);
            transactions.ForEach(t => t.DebitarAcreditar());
        }
    }

    public ICentralMarket<Component> GetCentralMarket()
    {
        return centralMarket;
    }
}
