using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeserkerAbilities : ClassAbilityGeneric
{

    private bool[] m_delays = new bool[4];

    private int m_enchantingWeaponDmg = 2;

    private int[] m_delayValues = { 4, 4, 4, 4 };
    
    public BeserkerAbilities(PlayerController playerController) : base(playerController){

    }

    public override void OnSkill1()
    {
        if (m_delays[0]) { return; }

        m_delays[0] = true;
        m_playerController.StartCoroutine(Delay(0));

        m_playerController.AddBuff((originalStats) =>
        {
            StatBlock newStats = (StatBlock) originalStats.Clone();
            newStats.attackSpeedMod *= 0.4f;
            newStats.attackDamageMod *= 1.1f;
            return newStats;
        }, 2);
    }

    public override void OnSkill2()
    {
        if (m_delays[1]) { return; }

        m_delays[1] = true;
        m_playerController.StartCoroutine(Delay(1));
    }

    public override void OnSkill3()
    {
        if (m_delays[2]) { return; }

        m_delays[2] = true;
        m_playerController.StartCoroutine(Delay(2));
    }

    public override void OnSkill4()
    {
        if (m_delays[3]) { return; }

        m_delays[3] = true;
        m_playerController.StartCoroutine(Delay(3));
    }

    private IEnumerator Delay(int arg)
    {
        yield return new WaitForSeconds(m_delayValues[arg]);
        m_delays[arg] = false;
    }
}
