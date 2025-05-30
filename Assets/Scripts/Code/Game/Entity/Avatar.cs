﻿using System.Collections.Generic;
using UnityEngine;

namespace TaoTie
{
    /// <summary>
    /// 玩家
    /// </summary>
    public class Avatar:Actor,IEntity<int>
    {
        private long thirdCameraId;
        #region IEntity

        public override EntityType Type => EntityType.Avatar;

        public void Init(int configId)
        {
            CampId = CampConst.Player;
            var avatar = AddComponent<AvatarComponent,int>(configId);
            ConfigId = avatar.Config.UnitId;
            ConfigActor = GetActorConfig(Config.ActorConfig);
            if (ConfigActor.Common != null) LocalScale = Vector3.one * ConfigActor.Common.Scale;
            AddComponent<AttachComponent>();
            AddComponent<UnitModelComponent,ConfigModel>(ConfigActor.Model);
            AddComponent<NumericComponent,ConfigCombatProperty[]>(ConfigActor.Combat?.DefaultProperty);
            AddComponent<FsmComponent,ConfigFsmController>(GetFsmConfig(Config.FSM));
            AddComponent<CombatComponent,ConfigCombat>(ConfigActor.Combat);
            AddComponent<SkillComponent,ConfigSkillInfo[]>(ConfigActor.Skill?.Skills);
            AddComponent<LocalInputController>();
            CreateMoveComponent();
            AddComponent<ORCAAgentComponent>();
            using ListComponent<ConfigAbility> list = ConfigAbilityCategory.Instance.GetList(ConfigActor.Abilities);
            var ac = AddComponent<AbilityComponent,List<ConfigAbility>>(list);
            ac.AddAbilities(ConfigAbilityCategory.Instance.GetDefaultAvatarAbilities());
            AddComponent<EquipHoldComponent>();
            if (ConfigActor.Billboard != null)
                AddComponent<BillboardComponent, ConfigBillboard>(ConfigActor.Billboard);
            InitAsync().Coroutine();
        }

        private async ETTask InitAsync()
        {
            var model = GetComponent<UnitModelComponent>();
            await model.WaitLoadGameObjectOver();
            if(model.IsDispose) return;
            if (thirdCameraId == 0)
            {
                thirdCameraId = CameraManager.Instance.Create(GameConst.ThirdCameraConfigId);
            }

            await TimerManager.Instance.WaitAsync(1);
            var camera = CameraManager.Instance.Get<NormalCameraState>(thirdCameraId);
            var trans = model.EntityView;
            camera.SetFollow(trans);
            camera.SetTarget(trans);
            if (!PlatformUtil.IsMobile())
            {
                CameraManager.Instance.ChangeCursorLock(true, CursorStateType.UserInput);
                CameraManager.Instance.ChangeCursorVisible(true, CursorStateType.UserInput);
            }
        }

        public void Destroy()
        {
            if (thirdCameraId != 0)
            {
                CameraManager.Instance.Remove(ref thirdCameraId);
            }
            ConfigActor = null;
            ConfigId = default;
            CampId = 0;
        }

        #endregion
        
    }
}