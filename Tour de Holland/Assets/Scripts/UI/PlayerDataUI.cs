using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataUI : MonoBehaviour
{
    [SerializeField] private Text moneyText;
    [SerializeField] private Text debtText;
    [SerializeField] private Text addMoneyText;
    [SerializeField] private Image playerIcon;
    [SerializeField] private GameObject escapedJailText;
    [SerializeField] private Transform[] purpleProperties;
    [SerializeField] private Transform[] blueProperties;
    [SerializeField] private Transform[] redProperties;
    [SerializeField] private Transform[] greenProperties;
    [SerializeField] private float timeForMoneyToComplete = 2;
    [SerializeField] private float minTimeToComplete = 0.25f;
    [SerializeField] private float maxTimeToComplete = 1.5f;
    [SerializeField] private float baseMoneyValueTime = 100;

    private float startingMoney = 0;
    private float targetMoney = 0;
    private float startMoneyTime;
    private float fracComplete;
    private float timeForMoneyToCompleteMultiplier = 1;
    private bool inDebt = false;
    private bool lerping = false;

    private void Start()
    {
        addMoneyText.gameObject.SetActive(false);
        debtText.gameObject.SetActive(false);
        escapedJailText.SetActive(false);
    }

    private IEnumerator CountMoneyToCurrentRealMoney()
    {
        lerping = true;

        yield return new WaitForSeconds(0.75f);
        startMoneyTime = Time.time;
        fracComplete = 0;
        float time = timeForMoneyToComplete * timeForMoneyToCompleteMultiplier;

        time = Mathf.Clamp(time, minTimeToComplete, maxTimeToComplete);

        while (fracComplete < 1)
        {
            fracComplete = (Time.time - startMoneyTime) / time;

            float showingMoney = Mathf.Lerp(startingMoney, targetMoney, fracComplete);

            string moneyString = Mathf.RoundToInt(showingMoney).ToString();

            if (!inDebt)
            {
                moneyText.text = moneyString;
            }
            else
            {
                debtText.text = "?" + moneyString;
            }            

            yield return null;
        }

        lerping = false;
    }

    public void UpdateIcon(Sprite sprite)
    {
        playerIcon.sprite = sprite;
    }

    public void ShowEscapeJail()
    {
        escapedJailText.SetActive(false);
        escapedJailText.SetActive(true);
    }

    public void UpdateMoneyText(int money, int oldMoney, bool debt)
    {
        int diff = money - oldMoney;

        string plusOrMin = diff > 0 ? "+" : "-";

        addMoneyText.text = plusOrMin + " ?" + Mathf.Abs(diff).ToString();
        addMoneyText.gameObject.SetActive(false);
        addMoneyText.gameObject.SetActive(true);

        if (lerping)
        {
            fracComplete = 1;
            if (!inDebt)
            {
                if (money < 0)
                {
                    targetMoney = 0;
                }

                moneyText.text = targetMoney.ToString();
            }
            else
            {
                debtText.text = targetMoney.ToString();
            }
            
            lerping = false;
        }

        debtText.gameObject.SetActive(debt && money < 0);
        inDebt = debt;

        startingMoney = oldMoney;
        targetMoney = money;

        timeForMoneyToCompleteMultiplier = (float)Mathf.Abs(diff) / baseMoneyValueTime;

        StartCoroutine(CountMoneyToCurrentRealMoney());
    }

    public void UpdateProperties(PropertyCard propertyCard, bool add)
    {
        switch (propertyCard.MyCardSet.PropertySetColor)
        {
            case PropertyCardSet.ColorOfSet.PURPLE:
                UpdateProperty(purpleProperties[propertyCard.PropertySetIndex], add);
                break;
            case PropertyCardSet.ColorOfSet.BLUE:
                UpdateProperty(blueProperties[propertyCard.PropertySetIndex], add);
                break;
            case PropertyCardSet.ColorOfSet.RED:
                UpdateProperty(redProperties[propertyCard.PropertySetIndex], add);
                break;
            case PropertyCardSet.ColorOfSet.GREEN:
                UpdateProperty(greenProperties[propertyCard.PropertySetIndex], add);
                break;
        }
    }

    private void UpdateProperty(Transform image, bool add)
    {
        image.transform.GetChild(add ? 0 : 1).gameObject.SetActive(true);
        image.transform.GetChild(add ? 1 : 0).gameObject.SetActive(false);
    }
}
