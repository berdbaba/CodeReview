using UnityEngine;

[DisallowMultipleComponent]
public class EffectsSystem : MonoBehaviour
{
    [Header("Skill Level‐Up Effect")]
    [SerializeField]
    private ActionContainerInt _onSkillLeveledAction;
    [SerializeField]
    private ParticleSystem _skillLevelUpEffectPrefab;

    [Header("Generic Effect Trigger")]
    [SerializeField]
    private ActionContainerTransform _onPlayEffectAtAction;
    [SerializeField]
    private ParticleSystem _genericEffectPrefab;

    private void Awake()
    {
        if (_onSkillLeveledAction != null)
            _onSkillLeveledAction.action += HandleSkillLeveled;

        if (_onPlayEffectAtAction != null)
            _onPlayEffectAtAction.action += HandlePlayEffectAt;
    }

    private void HandleSkillLeveled(int skillIndex)
    {
        if (_skillLevelUpEffectPrefab == null) return;
        Instantiate(_skillLevelUpEffectPrefab, transform.position, Quaternion.identity).Play();
    }

    private void HandlePlayEffectAt(Transform target)
    {
        if (target == null || _genericEffectPrefab == null) return;
        Instantiate(_genericEffectPrefab, target.position, Quaternion.identity).Play();
    }

    private void OnDestroy()
    {
        if (_onSkillLeveledAction != null)
            _onSkillLeveledAction.action -= HandleSkillLeveled;

        if (_onPlayEffectAtAction != null)
            _onPlayEffectAtAction.action -= HandlePlayEffectAt;
    }
}