﻿using Nino.Core;
using UnityEngine;

namespace TaoTie
{
    [NinoType(false)]
    public partial class ConfigStorySceneGroupActor: ConfigStoryActor
    {
        [NinoMember(5)]
        public int SceneGroupActorId;


        public override async ETTask<GameObject> Get3dObj(StorySystem storySystem)
        {
            await ETTask.CompletedTask;
            if (storySystem.SceneGroup.TryGetActorEntity(SceneGroupActorId, out var unitId))
            {
                var unit = storySystem.Scene.GetManager<EntityManager>()?.Get(unitId);
                var model = unit?.GetComponent<UnitModelComponent>();
                if (model == null) return null;
                await model.WaitLoadGameObjectOver();
                return unit.GetComponent<UnitModelComponent>().EntityView.gameObject;
            }

            return null;
        }

        public override void Recycle3dObj(StorySystem storySystem, GameObject obj)
        {
            if (storySystem.SceneGroup.TryGetActorEntity(SceneGroupActorId, out var unitId))
            {
                var unit = storySystem.Scene.GetManager<EntityManager>()?.Get(unitId);
            }

            if (obj != null)
            {
                var root = storySystem.Scene?.GetManager<EntityManager>()?.GameObjectRoot;
                if (root != null) obj.transform.SetParent(root);
            }

            base.Recycle3dObj(storySystem, obj);
        }
    }
}