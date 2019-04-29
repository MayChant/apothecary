using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    public int immuneCost;
    public int speedCost;
    public int auraCost;
    public int worthCost;

    public Text immuneCostDisplay;
    public Text speedCostDisplay;
    public Text auraCostDisplay;
    public Text worthCostDisplay;
    public Text lifeDisplay;
    // Start is called before the first frame update
    void Start()
    {
        UpdatePrices();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePrices()
    {
        GetImmunePrice();
        GetSpeedPrice();
        GetAuraPrice();
        GetWorthPrice();
        lifeDisplay.text = GameManagerScript.life.ToString();
    }
    public void Continue()
    {
        SceneManager.LoadScene(1);
    }
    public void BuyImmune()
    {
        int after = CheckPrice(immuneCost, GameManagerScript.life);
        if (after < 0)
        {
            return;
        }
        GameManagerScript.life = after;
        GameManagerScript.immune++;
        UpdatePrices();
    }
    public void BuySpeed()
    {
        int after = CheckPrice(speedCost, GameManagerScript.life);
        if (after < 0)
        {
            return;
        }
        GameManagerScript.life = after;
        GameManagerScript.speed++;
        UpdatePrices();
    }
    public void BuyAura()
    {
        int after = CheckPrice(auraCost, GameManagerScript.life);
        if (after < 0)
        {
            return;
        }
        GameManagerScript.life = after;
        GameManagerScript.aura++;
        UpdatePrices();
    }

    public void BuyWorth()
    {
        int after = CheckPrice(worthCost, GameManagerScript.life);
        if (after < 0)
        {
            return;
        }
        GameManagerScript.life = after;
        GameManagerScript.worth++;
        UpdatePrices();
    }

    private int CheckPrice(int cost, int saving)
    {
        return saving - cost;
    }

    private void GetImmunePrice()
    {
        immuneCost = Mathf.CeilToInt(Mathf.Pow(GameManagerScript.immune, 1.7f) / 2f) + 5;
        immuneCostDisplay.text = immuneCost.ToString();
        if (CheckPrice(immuneCost, GameManagerScript.life) < 0)
        {
            immuneCostDisplay.color = Color.red;
        }
        else
        {
            immuneCostDisplay.color = Color.white;
        }
    }
    private void GetSpeedPrice()
    {
        speedCost = Mathf.CeilToInt(Mathf.Pow(GameManagerScript.speed, 1.6f) / 2f) + 3;
        speedCostDisplay.text = speedCost.ToString();
        if (CheckPrice(speedCost, GameManagerScript.life) < 0)
        {
            speedCostDisplay.color = Color.red;
        }
        else
        {
            speedCostDisplay.color = Color.white;
        }
    }
    private void GetAuraPrice()
    {
        auraCost = Mathf.FloorToInt(Mathf.Pow(GameManagerScript.aura, 2.0f) / 2f) + 3;
        auraCostDisplay.text = auraCost.ToString();
        if (CheckPrice(auraCost, GameManagerScript.life) < 0)
        {
            auraCostDisplay.color = Color.red;
        }
        else
        {
            auraCostDisplay.color = Color.white;
        }
    }
    private void GetWorthPrice()
    {
        worthCost = Mathf.FloorToInt(GameManagerScript.worth * 10 + Mathf.Pow(GameManagerScript.worth, 4f) / 2f);
        worthCostDisplay.text = worthCost.ToString();
        if (CheckPrice(worthCost, GameManagerScript.life) < 0)
        {
            worthCostDisplay.color = Color.red;
        }
        else
        {
            worthCostDisplay.color = Color.white;
        }
    }
}
