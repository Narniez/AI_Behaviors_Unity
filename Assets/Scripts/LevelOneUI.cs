using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelOneUI : MonoBehaviour
{
    public AI npc;

    public TextMeshProUGUI patrolText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI randomPatrolText;
    public TextMeshProUGUI canBeScaredText;

    public GameObject normalUI;
    public GameObject extraInfoUI;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (npc.CanPatrol())
        {
            patrolText.color = Color.green;
        }
        if (npc.CanAttack())
        {
            attackText.color = Color.green;
        }
        if (npc.CanRandomPatrol())
        {
            randomPatrolText.color = Color.green;
        }
        if (npc.CanBeScared())
        {
            canBeScaredText.color = Color.green;
        }
        if (!npc.CanBeScared())
        {
            canBeScaredText.color = Color.red;
        }
        if (!npc.CanPatrol())
        {
            patrolText.color = Color.red;
        }
        if (!npc.CanAttack())
        {
            attackText.color = Color.red;
        }
        if (!npc.CanRandomPatrol())
        {
            randomPatrolText.color = Color.red;
        }
        if (npc.CanShowExtraInfo())
        {
            normalUI.SetActive(false);
            extraInfoUI.SetActive(true);
        }
        if (!npc.CanShowExtraInfo())
        {
            normalUI.SetActive(true);
            extraInfoUI.SetActive(false);
        }

    }
}
