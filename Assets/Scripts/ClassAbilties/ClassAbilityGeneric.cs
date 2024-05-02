using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassAbilityGeneric
{

    public PlayerController m_playerController;

    // Some abilities are gonna modify sword values

    public ClassAbilityGeneric(PlayerController playerController)
    {
        m_playerController = playerController;
    }
    public virtual void OnSkill1()
    {

    }

    public virtual void OnSkill2()
    {

    }

    public virtual void OnSkill3()
    {
      
    }

    public virtual void OnSkill4()
    {

    }

}
