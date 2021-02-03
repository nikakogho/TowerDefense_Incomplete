using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    static int health, money;

    public static int Health { get { return health; } set { health = value; onHealthChanged(); } }
    public static int Money { get { return money; } set { money = value; onMoneyChanged(); } }

    public int startHealth;
    public int startMoney;

    public Text healthText;
    public Text moneyText;

    public delegate void OnStatChanged();

    public static OnStatChanged onMoneyChanged, onHealthChanged;

    public static PlayerStats instance;

    void Awake()
    {
        health = startHealth;
        money = startMoney;

        instance = this;

        onMoneyChanged += () =>
        {
            moneyText.text = money.ToString();
        };

        onHealthChanged += () =>
        {
            healthText.text = health.ToString();

            if (health <= 0)
            {
                //game over
            }
        };
    }

    void Start()
    {
        onMoneyChanged();
        onHealthChanged();
    }
}
