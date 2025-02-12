﻿using Nino.Core;
using Sirenix.OdinInspector;

namespace TaoTie
{
    [NinoType(false)][LabelText("等待时间")]
    public partial class ConfigStoryWaitTimeClip: ConfigStoryClip
    {
        [NinoMember(10)][LabelText("时间间隔ms")]
        public int Interval;

        [NinoMember(11)]
        public bool IsGameTime = true;
        public override async ETTask Process(StorySystem storySystem)
        {
            if (IsGameTime)
            {
                await GameTimerManager.Instance.WaitAsync(Interval);
            }
            else
            {
                await TimerManager.Instance.WaitAsync(Interval);
            }
        }
    }
}