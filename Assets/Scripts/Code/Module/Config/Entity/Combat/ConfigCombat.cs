﻿using System.Collections.Generic;
using Nino.Core;
using Sirenix.OdinInspector;

namespace TaoTie
{
    [NinoType(false)]
    public partial class ConfigCombat
    {
        [NinoMember(1)][LabelText("基础属性")] [BoxGroup("属性")][TableList]
        public ConfigCombatProperty[] DefaultProperty;
        
        #if UNITY_EDITOR
        [Button("初始化")] [BoxGroup("属性")]
        private void Init()
        {
            var list = new List<int>();
            var fields = typeof(NumericType).GetFields();
            if (fields.Length > 0)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    if (!fields[i].IsStatic)
                    {
                        continue;
                    }
                    var val = (int) fields[i].GetValue(null);
                    if (val <= NumericType.Max || val%10!=1) continue;
                    list.Add(val);
                }
            }

            DefaultProperty = new ConfigCombatProperty[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                DefaultProperty[i] = new ConfigCombatProperty() {NumericType = list[i]};
            }
        }
        
        #endif
        [NinoMember(2)]
        public ConfigDie Die;

        [NinoMember(3)]
        public ConfigCombatLock CombatLock;

        [NinoMember(4)]
        public ConfigCombatBeHit BeHit;
    }
}