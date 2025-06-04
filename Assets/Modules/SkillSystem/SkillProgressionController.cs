using System;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Strength,
    Agility,
    Intelligence
}

[Serializable]
public class SkillData
{
    public SkillType Type;
    public int Level = 1;
    public float CurrentXP;
    public float XPToNext => Level * 100f;
}

public class SkillSystem : MonoBehaviour
{
    [SerializeField]
    private List<SkillData> _skills = new();

    [SerializeField]
    private ActionContainerInt _upgradeSkillRequested;

    [SerializeField]
    private ActionContainerInt _onSkillLeveled;

    [SerializeField]
    private ActionContainerInt _onXpChanged;

    private void Awake()
    {
        if (_upgradeSkillRequested != null)
            _upgradeSkillRequested.action += HandleUpgradeRequest;
    }

    private void HandleUpgradeRequest(int skillIndex)
    {
        if (!IsValidIndex(skillIndex)) return;

        var skill = _skills[skillIndex];
        if (skill.CurrentXP >= skill.XPToNext)
        {
            skill.CurrentXP -= skill.XPToNext;
            skill.Level++;
            _onSkillLeveled?.FireAction(skillIndex);
        }
    }

    public void AddXp(int skillIndex, float amount)
    {
        if (!IsValidIndex(skillIndex)) return;

        var skill = _skills[skillIndex];
        skill.CurrentXP += amount;
        _onXpChanged?.FireAction(skillIndex);
    }

    public SkillData GetSkill(int index)
    {
        return IsValidIndex(index) ? _skills[index] : null;
    }

    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < _skills.Count;
    }
}