using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class DefenceItem : MonoBehaviour
{
    // [SerializeField] public float range;
    [SerializeField] public int leftXOffset;
    [SerializeField] public int rightXOffset;
    [SerializeField] public int bottomYOffset;
    [SerializeField] public int topYOffset;


    [SerializeField] private DefenceItemType defenceItemType;
    [SerializeField] private float damage;
    [SerializeField] private float intervalInSeconds;

    
    [HideInInspector] public int xIndex;
    [HideInInspector] public int yIndex;
    [HideInInspector] public bool canAttack = true;
    
    // todo: how to deal with directions?

    private float _timeElapsedSinceLastAttackInSeconds;

    private void Start()
    {
        EventManager.DefenceItemReadyToAttack(this);
    }

    private void FixedUpdate()
    {
        if (canAttack)
        {
            return;
        }

        _timeElapsedSinceLastAttackInSeconds += Time.fixedDeltaTime;

        if (_timeElapsedSinceLastAttackInSeconds >= intervalInSeconds)
        {
            canAttack = true;
            EventManager.DefenceItemReadyToAttack(this);
        }
    }

    public void Attack(Enemy enemy)
    {
        enemy.TakeDamage(damage);
        
        _timeElapsedSinceLastAttackInSeconds = 0f;
        canAttack = false;
    }
}
