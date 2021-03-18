using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJ : MonoBehaviour
{
    int pnjPosInZone;
    /// <summary>
    /// Zone where the player is (2-12)
    /// </summary>
    public int PNJPosInZone
    {
        get => pnjPosInZone; set
        {
            if (value < 2 || value > 12)
                return;
            pnjPosInZone = value;
            StartCoroutine(MovePNJToZone(value));
        }
    }

    int maxHealth = 2;
    int health;
    public int Health
    {
        get => health; set
        {
            health = value;
            if (value == 0)
            {
                //Notify death

                Destroy(gameObject);
            }
        }
    }

    public int equip1, equip2;
    public bool hasStealtKit, hasArmor, hasGazMask;
    public PlayerActionState PNJState;

    List<GameObject> zoneInMoveRange;

    public TurnManager tManager;

    // Start is called before the first frame update
    void Start()
    {
        tManager = GameObject.Find("GameManager").GetComponent<TurnManager>();
        ResetPNJ();
        PNJPosInZone = Random.Range(2, 13);
    }

    // Update is called once per frame
    void Update()
    {
        if (PNJState == PlayerActionState.ChooseAction)
        {
            BasicPnjActionAI();
        }
        if (PNJState == PlayerActionState.ChooseMove)
        {
            BasicPnjMovingAI();
        }
    }

    /// <summary>
    /// Basic AI for a PNJ action
    /// </summary>
    void BasicPnjActionAI()
    {
        int chanceSearch = (equip1 > 0) ? 3 : 2;
        chanceSearch += (equip2 > 0) ? 1 : 0;
        if (Random.Range(0, chanceSearch) == 0)
            PNJSearchZone();
        else
            PNJAttack();
    }

    /// <summary>
    /// Basic AI for a PNJ move phase
    /// </summary>
    void BasicPnjMovingAI()
    {
        RaycastHit[] hits = Physics.SphereCastAll(gameObject.transform.position, 10f, Vector3.forward, 0.0f);
        foreach (RaycastHit c in hits)
            if (c.collider.gameObject.name.Contains("Zone"))
                zoneInMoveRange.Add(c.collider.gameObject);
        //Choose a zone to go to
        PNJPosInZone = int.Parse(zoneInMoveRange[Random.Range(0, zoneInMoveRange.Count)].name.Split('e')[1]);
        PNJState = PlayerActionState.None;
    }

    public void PNJSearchZone()
    {
        int res = DiceRoll(2, 6);
        ResolveItem(res);
        PNJState = PlayerActionState.None;
    }

    public void PNJAttack()
    {
        List<Player> victimList = new List<Player>();
        //Can attack on adjacent zones
        //if (equip1 == 10 || equip2 == 10)
    }
    /// <summary>
    /// Move the player gameobject to the selected zone
    /// </summary>
    /// <param name="zone">The zone number (2-12)</param>
    /// <returns></returns>
    IEnumerator MovePNJToZone(int zone)
    {
        Vector3 zonePos = GameObject.Find("Zone" + zone).transform.position;
        float time = 0, duration = 1.0f;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, zonePos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = zonePos;
    }
    
    /// <summary>
     /// Resolve looted item
     /// </summary>
     /// <param name="item"></param>
    public void ResolveItem(int item)
    {
        switch (item)
        {
            case 2:
                hasStealtKit = true;
                break;
            case 3:
                hasArmor = true;
                break;
            case 4:
                if (equip1 == 0)
                    equip1 = 4;
                else if (equip2 == 0)
                    equip2 = 4;
                else
                    _ = (Random.Range(0, 2) == 0) ? equip1 = 4 : equip2 = 4;
                break;
            case 5:
                hasGazMask = true;
                break;
            case 6:
                if (equip1 == 0)
                    equip1 = 4;
                else if (equip2 == 0)
                    equip2 = 4;
                else
                    _ = (Random.Range(0, 2) == 0) ? equip1 = 4 : equip2 = 4;
                break;
            case 7:
                health++;
                if (health > maxHealth)
                    health = maxHealth;
                break;
            case 8:
                if (equip1 == 0)
                    equip1 = 8;
                else if (equip2 == 0)
                    equip2 = 8;
                else
                    _ = (Random.Range(0, 2) == 0) ? equip1 = 8 : equip2 = 8;
                break;
            case 9:
                if (equip1 == 0)
                    equip1 = 9;
                else if (equip2 == 0)
                    equip2 = 9;
                else
                    _ = (Random.Range(0, 2) == 0) ? equip1 = 9 : equip2 = 9;
                break;
            case 10:
                if (equip1 == 0)
                    equip1 = 10;
                else if (equip2 == 0)
                    equip2 = 10;
                else
                    _ = (Random.Range(0, 2) == 0) ? equip1 = 10 : equip2 = 10;
                break;
            case 11:
                if (equip1 == 0)
                    equip1 = 11;
                else if (equip2 == 0)
                    equip2 = 11;
                else
                    _ = (Random.Range(0, 2) == 0) ? equip1 = 11 : equip2 = 11;
                break;
            case 12:
                if (equip1 == 0)
                    equip1 = 12;
                else if (equip2 == 0)
                    equip2 = 12;
                else
                    _ = (Random.Range(0, 2) == 0) ? equip1 = 12 : equip2 = 12;
                break;
        }
    }

    /// <summary>
    /// Reset player states
    /// </summary>
    public void ResetPNJ()
    {
        health = maxHealth;
        equip1 = equip2 = 0;
        PNJState = PlayerActionState.None;
        zoneInMoveRange = new List<GameObject>();
    }

    /// <summary>
    /// Rolls some dices
    /// </summary>
    /// <param name="nb">The number of dices to roll</param>
    /// <param name="type">How many dots</param>
    /// <returns></returns>
    public int DiceRoll(int nb, int type)
    {
        int t = 0;
        for (int i = 0; i < nb; i++)
        {
            t += Random.Range(1, type + 1);
        }
        return t;
    }
}
