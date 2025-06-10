using GameData.src.Shared.Enums;
using GameData.src.Skill;

namespace GameLogic.Combat.Snapshots
{
    public struct DamageSnapshot
    {
        public SkillDefinition SkillDefinition;     //8
        public DamageType DamageType;               //4
        public AttackType AttackType;               //4
        public WeaponType WeaponType;               //4

        public float Damage;                        //4
        public float ScaleAdditive;                 //4
        public float ScaleIncreased;                //4
        public float ScaleEmpowered;                //4

        public float BaseCritChance;                //4
        public float CritAdditive;                  //4
        public float CritEmpowered;                 //4
        public float CritIncreased;                 //4
                                                    // Total: 40 bytes
    }
}