using UnityEngine;
using static Constants;

public class DefenceItem : MonoBehaviour
{
    [SerializeField] public float range;

    [SerializeField] private DefenceItemType defenceItemType;
    [SerializeField] private float damage;
    [SerializeField] private float intervalInSeconds;

    
    [HideInInspector] public int xIndex;
    [HideInInspector] public int yIndex;
    
    // todo: how to deal with directions?

    private float _timeElapsedSinceLastAttackInSeconds;
    private bool _canAttack = true;

    private void Start()
    {
        EventManager.DefenceItemReadyToAttack(this);
    }

    private void FixedUpdate()
    {
        if (_canAttack)
        {
            return;
        }

        _timeElapsedSinceLastAttackInSeconds += Time.fixedDeltaTime;

        if (_timeElapsedSinceLastAttackInSeconds >= intervalInSeconds)
        {
            _canAttack = true;
            EventManager.DefenceItemReadyToAttack(this);
        }
    }

    public void Attack(Enemy enemy)
    {
        enemy.TakeDamage(damage);
        
        _timeElapsedSinceLastAttackInSeconds = 0f;
        _canAttack = false;
    }
}
