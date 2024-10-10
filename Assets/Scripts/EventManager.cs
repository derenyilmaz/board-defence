using System;

public static class EventManager
{
    public class DefenceItemReadyToAttackEventArgs : EventArgs
    {
        public DefenceItem DefenceItem;
    }
    
    public class DefenceItemPlacedEventArgs : EventArgs
    {
        public Constants.DefenceItemType DefenceItemType;
    }
    
    public class EnemyReadyToMoveEventArgs : EventArgs
    {
        public Enemy Enemy;
    }

    public class EnemyDiedEventArgs : EventArgs
    {
        public Constants.EnemyType EnemyType;
    }
    
    public class LevelStartedEventArgs : EventArgs
    {
        public LevelFormat LevelFormat;
    }
    
    public static event EventHandler<DefenceItemReadyToAttackEventArgs> OnDefenceItemReadyToAttack;
    public static event EventHandler<DefenceItemPlacedEventArgs> OnDefenceItemPlaced;
    public static event EventHandler<EnemyReadyToMoveEventArgs> OnEnemyReadyToMove;
    public static event EventHandler<EnemyDiedEventArgs> OnEnemyDied;
    public static event EventHandler<LevelStartedEventArgs> OnLevelStarted;

    public static event EventHandler OnLevelFailed;
    public static event EventHandler OnLevelWon;
    

    public static void DefenceItemReadyToAttack(DefenceItem defenceItem)
    {
        OnDefenceItemReadyToAttack?.Invoke(null, new DefenceItemReadyToAttackEventArgs { DefenceItem = defenceItem });
    }

    public static void DefenceItemPlaced(Constants.DefenceItemType defenceItemType)
    {
        OnDefenceItemPlaced?.Invoke(null, new DefenceItemPlacedEventArgs { DefenceItemType = defenceItemType });
    }
    
    public static void EnemyReadyToMove(Enemy enemy)
    {
        OnEnemyReadyToMove?.Invoke(null, new EnemyReadyToMoveEventArgs { Enemy = enemy });
    }
    
    public static void EnemyDied(Constants.EnemyType enemyType)
    {
        OnEnemyDied?.Invoke(null, new EnemyDiedEventArgs { EnemyType = enemyType });
    }
    
    public static void LevelStarted(LevelFormat levelFormat)
    {
        OnLevelStarted?.Invoke(null, new LevelStartedEventArgs {LevelFormat = levelFormat});
    }

    public static void LevelFailed()
    {
        OnLevelFailed?.Invoke(null, EventArgs.Empty);
    }
    
    public static void LevelWon()
    {
        OnLevelWon?.Invoke(null, EventArgs.Empty);
    }
}
