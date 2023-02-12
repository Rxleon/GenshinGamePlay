﻿namespace TaoTie
{
    public class AttachToAfterAttack: AbilityMixin
    {
        public ConfigAttachToAfterAttack config => baseConfig as ConfigAttachToAfterAttack;

        private CombatComponent combatComponent;
        public override void Init(ActorAbility actorAbility, ActorModifier actorModifier, ConfigAbilityMixin config)
        {
            base.Init(actorAbility, actorModifier, config);
            combatComponent = actorAbility.Parent.GetParent<Entity>().GetComponent<CombatComponent>();
            if (combatComponent != null)
            {
                combatComponent.afterAttack += Execute;
            }

        }

        private void Execute(AttackResult result, CombatComponent other)
        {
            if (config.Actions != null)
            {
                for (int i = 0; i < config.Actions.Length; i++)
                {
                    config.Actions[i].DoExecute(actorAbility.Parent.GetParent<Entity>(), actorAbility, actorModifier, other.GetParent<Entity>());
                }
            }
        }

        public override void Dispose()
        {
            if (combatComponent != null)
            {
                combatComponent.afterAttack -= Execute;
                combatComponent = null;
            }
            base.Dispose();
        }
    }
}