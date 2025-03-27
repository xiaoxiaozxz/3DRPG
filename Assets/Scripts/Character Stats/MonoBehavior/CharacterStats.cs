using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO templateData;
    public CharacterData_SO characterData;
    public AttackData_SO attackData;

    [HideInInspector]
    public bool isCritical;

    void Awake() 
    {
        if(templateData != null)
        characterData = Instantiate(templateData);
    }


    
    #region Read from Data_SO
    public int MaxHealth
    {
        get { if(characterData != null) return characterData.maxHealth; else return 0;}
        set { characterData.maxHealth = value; }
    }
    public int CurrentHealth
    {
        get { if(characterData != null) return characterData.currentHealth; else return 0;}
        set { characterData.currentHealth = value; }
    }
    public int BaseDefence
    {
        get { if(characterData != null) return characterData.baseDefence; else return 0;}
        set { characterData.baseDefence = value; }
    }
    public int CurrentDefence
    {
        get { if(characterData != null) return characterData.currentDefence; else return 0;}
        set { characterData.currentDefence = value; }
    }

    #endregion

    #region Character Combat

    public void TakeDamage(CharacterStats attacker,CharacterStats defener)
    {
        int damage = Mathf.Max(attacker.CurrentDamage(attacker.attackData) - defener.CurrentDefence, 0);
        defener.CurrentHealth = Mathf.Max(defener.CurrentHealth - damage, 0);
        if(attacker.isCritical)
        {
            defener.GetComponent<Animator>().SetTrigger("Hit");
        }
        //TODO:Update UI
        //TODO:经验Update
    }

    private int CurrentDamage(AttackData_SO attacker)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.minDamage,attacker.maxDamage);
        if(isCritical)
        {
            coreDamage *= attacker.criticalMultiplier;
            Debug.Log("暴击！" + coreDamage);
        }
        return (int)coreDamage;
    }

    #endregion

}
