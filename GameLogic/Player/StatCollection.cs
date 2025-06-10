using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifiers.Operations;
using System;

namespace GameLogic.Player
{
    public class StatCollection
    {
        public const uint additiveIdx = 0;
        public const uint increasedIdx = 1;
        public const uint empoweredIdx = 2;

        private const uint opLen = 3;

        public float[,] DamageStats;
        public float[,] AttackStats;
        public float[,] GlobalStats;
        public float[,] WeaponStats;

        private readonly int dmgLen;
        private readonly int atkLen;
        private readonly int globLen;
        private readonly int wepLen;

        public StatCollection()
        {
            this.dmgLen = Enum.GetValues<DamageType>().Length;
            this.atkLen = Enum.GetValues<AttackType>().Length;
            this.globLen = Enum.GetValues<GlobalStat>().Length;
            this.wepLen = Enum.GetValues<WeaponType>().Length;

            this.DamageStats = new float[this.dmgLen, opLen];
            this.AttackStats = new float[this.atkLen, opLen];
            this.GlobalStats = new float[this.globLen, opLen];
            this.WeaponStats = new float[this.wepLen, opLen];
        }

        private static float GetStatGeneric<T>(T stat, ScalarOpType type, float[,] statArray, int length) where T : Enum
        {
            float value = 0f;
            int statValue = Convert.ToInt32(stat);

            for (int i = 0; i < length; i++)
            {
                int currentType = 1 << i;
                if ((statValue & currentType) != 0)
                {
                    value += statArray[i, (uint)type];
                }
            }
            return value;
        }

        private static float GetStatValueGeneric<T>(T stat, float[,] statArray, int length) where T : Enum
        {
            float value = 0f;
            int statValue = Convert.ToInt32(stat);

            for (int i = 0; i < length; i++)
            {
                int currentType = 1 << i;
                if ((statValue & currentType) != 0)
                {
                    var additive = statArray[currentType, additiveIdx];
                    var increased = statArray[currentType, increasedIdx];
                    var empowered = statArray[currentType, empoweredIdx];

                    value = additive * (1 + (increased / 100)) * (1 + (empowered / 100));
                }
            }

            return value;
        }

        public float GetStatValue(GlobalStat stat)
            => GetStatValueGeneric(stat, this.GlobalStats, this.globLen);
        public float GetStatValue(DamageType stat)
            => GetStatValueGeneric(stat, this.DamageStats, this.dmgLen);
        public float GetStatValue(WeaponType stat)
            => GetStatValueGeneric(stat, this.WeaponStats, this.wepLen);
        public float GetStatValue(AttackType stat)
            => GetStatValueGeneric(stat, this.AttackStats, this.atkLen);

        public float GetStat(DamageType stat, ScalarOpType type) =>
            GetStatGeneric(stat, type, this.DamageStats, this.dmgLen);
        public float GetStat(AttackType stat, ScalarOpType type) =>
            GetStatGeneric(stat, type, this.AttackStats, this.atkLen);
        public float GetStat(GlobalStat stat, ScalarOpType type) =>
            GetStatGeneric(stat, type, this.GlobalStats, this.globLen);
        public float GetStat(WeaponType stat, ScalarOpType type) =>
            GetStatGeneric(stat, type, this.WeaponStats, this.wepLen);
    }
}