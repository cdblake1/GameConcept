namespace TopDownGame.Data.impl
{
  internal interface ICombatant
  {
    Stats CurrentStats { get; }
    float MaxHealth { get; }
    float CurrentHealth { get; }
    float ApplyDamage(float damage);
  }
}
