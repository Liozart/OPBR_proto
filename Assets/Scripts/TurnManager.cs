using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GlobalTurnState
{
    Reset, Action, Gaz, Move
}

public class TurnManager : MonoBehaviour
{
    public GameObject[] Zones;
    public List<GameObject> GazedZones;

    Player playerScript;
    List<PNJ> pnjListScritps;
    public GameObject pnjPrefab;

    int turn;
    public GlobalTurnState state;
    public bool canAdvance = false;

    public Material mat_inactiveGrey;
    public Material mat_selectedOrange;
    public Material mat_GazedGreen;

    //Inits and start the game
    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        state = GlobalTurnState.Reset;
        turn = 0;
        AdvanceInTurn();
    }

    //Execute the next phase of the turn
    void AdvanceInTurn()
    {
        canAdvance = false;
        switch (state)
        {
            //Reset game
            case GlobalTurnState.Reset:
                //Re-inits
                GazedZones = new List<GameObject>();
                playerScript.PlayerPosInZone = DiceRoll(2, 6);
                turn = 0;
                //Create some PNJs
                pnjListScritps = new List<PNJ>();
                CreateSomePNJs(Random.Range(3, 5));
                state = GlobalTurnState.Action;
                canAdvance = true;
                break;
            //Phase 1 : Action state
            case GlobalTurnState.Action:
                turn++;
                //Player choose his action
                playerScript.Playerstate = PlayerActionState.ChooseAction;
                //Execute action AI of all pnjs
                for (int i = 0; i < pnjListScritps.Count; i++)
                    pnjListScritps[i].PNJState = PlayerActionState.ChooseAction;
                state = GlobalTurnState.Gaz;
                break;
            //Phase 2 : Gaz state
            case GlobalTurnState.Gaz:
                //Set gaz on a random zone
                bool isSelected = false;
                int r = 0;
                while (!isSelected)
                {
                    r = Random.Range(0, 11);
                    if (!GazedZones.Contains(Zones[r]))
                    {
                        GazedZones.Add(Zones[r]);
                        Zones[r].GetComponent<Renderer>().material = mat_GazedGreen;
                        isSelected = true;
                    }
                }
                StartCoroutine(GazAnimation());
                break;
            //Phase 3 : Moving state
            case GlobalTurnState.Move:
                //Player can move
                playerScript.Playerstate = PlayerActionState.ChooseMove;
                //Execute move AI of all pnjs
                for (int i = 0; i < pnjListScritps.Count; i++)
                    pnjListScritps[i].PNJState = PlayerActionState.ChooseMove;
                state = GlobalTurnState.Action;
                break;
        }
    }

    IEnumerator GazAnimation()
    {
        yield return new WaitForSeconds(1.0f);
        state = GlobalTurnState.Move;
        canAdvance = true;
    }

    void CreateSomePNJs(int nb)
    {
        for (int i = 0; i < nb; i++)
        {
            GameObject g = Instantiate(pnjPrefab, playerScript.gameObject.transform.position, playerScript.gameObject.transform.rotation);
            pnjListScritps.Add(g.GetComponent<PNJ>());
            g.transform.SetParent(GameObject.Find("PNJs").transform);
        }
    }

    //Check if the manager can go forward to the next phase of the turn
    void Update()
    {
        if (canAdvance)
        {
            foreach (PNJ npc in pnjListScritps)
            {
                if (npc.PNJState != PlayerActionState.None)
                    return;
            }
            AdvanceInTurn();
        }
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
