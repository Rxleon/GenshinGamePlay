﻿using System;
using DaGenGraph;
using TaoTie.LitJson.Extensions;
using Nino.Core;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace TaoTie
{
    // Trigger
    [NinoType(false)]
    public abstract partial class ConfigSceneGroupTrigger
    {
        [PropertyOrder(int.MinValue)] 
        [NinoMember(1)][DrawIgnore]
        public int LocalId;
        [NinoMember(2)]
#if UNITY_EDITOR
        [OnCollectionChanged(nameof(Refresh))] [OnStateUpdate(nameof(Refresh))] 
#endif
        [SerializeReference] [PropertyOrder(int.MaxValue - 1)][DrawIgnore]
#if UNITY_EDITOR
        [TypeFilter("@"+nameof(OdinDropdownHelper)+"."+nameof(OdinDropdownHelper.GetFilteredActionTypeList)+"("+nameof(GetType)+"())")]
#endif
        public ConfigSceneGroupAction[] Actions;

#if UNITY_EDITOR
        [PropertyOrder(int.MinValue + 1)] [LabelText("策划备注")]
        public string Remarks;
        
        private void Refresh()
        {
            if (Actions == null) return;
            for (int i = 0; i < Actions.Length; i++)
            {
                if (Actions[i] != null)
                    Actions[i].HandleType = GetType();
            }

            Actions.Sort((a, b) => { return a.LocalId - b.LocalId; });
        }
#endif

        public abstract void OnTrigger(SceneGroup sceneGroup, IEventBase evt);
    }
    [NinoType(false)]
    public abstract class ConfigSceneGroupTrigger<T> : ConfigSceneGroupTrigger where T : IEventBase
    {
        [JsonIgnore]
        private Type EventType => TypeInfo<T>.Type;

        public sealed override void OnTrigger(SceneGroup sceneGroup, IEventBase evt)
        {
            if (evt.GetType() != EventType) return;
            if (!CheckCondition(sceneGroup, (T)evt)) return;
            Log.Info("OnTrigger: " + GetType().Name);
            if (Actions != null)
            {
                for (int i = 0; i < Actions.Length; i++)
                {
                    Actions[i].ExecuteAction(evt, sceneGroup, sceneGroup);
                }
            }
        }

        protected virtual bool CheckCondition(SceneGroup sceneGroup, T evt)
        {
            return true;
        }
    }
}