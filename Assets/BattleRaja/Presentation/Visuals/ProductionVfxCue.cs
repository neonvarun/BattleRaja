using UnityEngine;

namespace BattleRaja.Presentation.Visuals
{
    /// <summary>
    /// Presentation-only particle cues owned by a saved production fighter prefab.
    /// Gameplay authority never depends on particle lifetime or particle callbacks.
    /// </summary>
    public sealed class ProductionVfxCue : MonoBehaviour
    {
        [SerializeField] private ParticleSystem attackBurst;
        [SerializeField] private ParticleSystem abilityBurst;
        [SerializeField] private ParticleSystem hitBurst;
        [SerializeField] private ParticleSystem eliminationBurst;
        [SerializeField] private ParticleSystem victoryBurst;
        [SerializeField] private ParticleSystem defeatBurst;

        public int AttackPlayCount { get; private set; }
        public int AbilityPlayCount { get; private set; }
        public int HitPlayCount { get; private set; }
        public int EliminationPlayCount { get; private set; }
        public int VictoryPlayCount { get; private set; }
        public int DefeatPlayCount { get; private set; }

        public bool HasAttackCue => attackBurst != null;
        public bool HasAbilityCue => abilityBurst != null;
        public bool HasHitCue => hitBurst != null;
        public bool HasEliminationCue => eliminationBurst != null;
        public bool HasVictoryCue => victoryBurst != null;
        public bool HasDefeatCue => defeatBurst != null;

        public void Configure(
            ParticleSystem attack,
            ParticleSystem ability,
            ParticleSystem hit,
            ParticleSystem elimination,
            ParticleSystem victory,
            ParticleSystem defeat)
        {
            attackBurst = attack;
            abilityBurst = ability;
            hitBurst = hit;
            eliminationBurst = elimination;
            victoryBurst = victory;
            defeatBurst = defeat;
        }

        public void PlayAttack()
        {
            AttackPlayCount++;
            Play(attackBurst);
        }

        public void PlayAbility()
        {
            AbilityPlayCount++;
            Play(abilityBurst);
        }

        public void PlayHit()
        {
            HitPlayCount++;
            Play(hitBurst);
        }

        public void PlayElimination()
        {
            EliminationPlayCount++;
            Play(eliminationBurst);
        }

        public void PlayVictory()
        {
            VictoryPlayCount++;
            Play(victoryBurst);
        }

        public void PlayDefeat()
        {
            DefeatPlayCount++;
            Play(defeatBurst);
        }

        private static void Play(ParticleSystem cue)
        {
            if (cue == null) return;
            cue.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            cue.Play(true);
        }
    }
}
