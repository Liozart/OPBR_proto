using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerActionState
{
    ChooseAction, ChooseMove, None
}

public class Player : MonoBehaviour
{
    int playerPosInZone;
    /// <summary>
    /// Zone where the player is (2-12)
    /// </summary>
    public int PlayerPosInZone { get => playerPosInZone; set
        {
            if (value < 2 || value > 12)
                return;
            playerPosInZone = value;
            StartCoroutine(MovePlayerToZone(value));
        }
    }

    int maxHealth = 2;
    int health;
    public int Health { get => health; set
        {
            health = value;
            SetHeartHUD();
            if (value == 0)
            {
                Debug.LogError("DED");
            }
        }
    }

    public int equip1, equip2;
    public bool hasStealtKit, hasArmor, hasGazMask;
    public PlayerActionState Playerstate;

    List<GameObject> zoneInMoveRange;

    public TurnManager tManager;

    public Canvas dicerollCanvas;
    public Canvas actionCanvas;
    public Text dicerollText;
    public Text HUDPhaseText;
    public Text HUDEquip1;
    public Text HUDEquip2;
    public Image HUDHeart1;
    public Image HUDHeart2;

    public Material mat_gazGreen;
    public Material mat_InRangeOrange;
    public Material mat_InactiveGrey;

    // Start is called before the first frame update
    void Start()
    {
        tManager = GameObject.Find("GameManager").GetComponent<TurnManager>();
        ResetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (Playerstate == PlayerActionState.ChooseAction)
        {
            HUDPhaseText.text = "Choose action";
            Playerstate = PlayerActionState.None;
            actionCanvas.enabled = true;
        }
        if (Playerstate == PlayerActionState.ChooseMove)
        {
            HUDPhaseText.text = "Move/stay";
            RaycastHit[] hits = Physics.SphereCastAll(gameObject.transform.position, 10f, Vector3.forward, 0.0f);
            foreach (RaycastHit c in hits)
                if (c.collider.gameObject.name.Contains("Zone"))
                {
                    zoneInMoveRange.Add(c.collider.gameObject);
                    c.collider.gameObject.GetComponent<ParticleSystem>().Play();
                }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (Playerstate == PlayerActionState.ChooseMove)
            {
                Ray wp = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(wp, out hit))
                {
                    foreach (GameObject g in zoneInMoveRange)
                        if (g.Equals(hit.collider.gameObject))
                        {
                            Playerstate = PlayerActionState.None;
                            string r = g.name.Split('e')[1];
                            PlayerPosInZone = int.Parse(r);
                            foreach (GameObject gamo in zoneInMoveRange)
                            {
                                gamo.GetComponent<ParticleSystem>().Stop();
                            }
                            tManager.canAdvance = true;
                        }
                }
            }
        }
    }


    public void ClickSearchZone()
    {
        actionCanvas.enabled = false;
        dicerollCanvas.enabled = true;
        int res = DiceRoll(2, 6);
        ResolveItem(res);
        StartCoroutine(PlaySearchAnimation(res));
    }

    public void ClickAttack()
    {
        List<Player> victimList = new List<Player>();
        //Can attack on adjacent zones
        //if (equip1 == 10 || equip2 == 10)
        HUDPhaseText.text = "Gaz is moving..";
    }


    public void SetHeartHUD()
    {
        switch (Health)
        {
            case 2:
                HUDHeart2.enabled = true;
                break;
            case 1:
                HUDHeart2.enabled = false;
                HUDHeart1.enabled = true;
                break;
            case 0:
                HUDHeart2.enabled = false;
                HUDHeart1.enabled = false;
                break;
        }
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
            case 5: hasGazMask = true;
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

    IEnumerator PlaySearchAnimation(int diceres)
    {
        float animDuration = 0.5f;
        float cnt = 0;

        while (cnt < animDuration)
        {
            dicerollText.text = Random.Range(2, 13).ToString();
            cnt += Time.deltaTime;
            yield return null;
        }
        cnt = 0;
        dicerollText.text = diceres.ToString();
        while (cnt < animDuration)
        {
            cnt += Time.deltaTime;
            yield return null;
        }
        HUDEquip1.text = equip1.ToString();
        HUDEquip2.text = equip2.ToString();
        HUDPhaseText.text = "Gaz is moving..";
        dicerollCanvas.enabled = false;
        tManager.canAdvance = true;
    }

    /// <summary>
    /// Move the player gameobject to the selected zone
    /// </summary>
    /// <param name="zone">The zone number (2-12)</param>
    /// <returns></returns>
    IEnumerator MovePlayerToZone(int zone)
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
    /// Reset player states
    /// </summary>
    public void ResetPlayer()
    {
        health = maxHealth;
        equip1 = equip2 = 0;
        Playerstate = PlayerActionState.None;
        dicerollCanvas.enabled = false;
        actionCanvas.enabled = false;
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
