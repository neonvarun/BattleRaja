namespace BattleRaja.Core.Application
{
    public enum TutorialStep
    {
        Movement,
        Aim,
        BasicAttack,
        Ability,
        Gadget,
        Aandhi,
        Elimination,
        Victory,
        Complete
    }

    /// <summary>
    /// Deterministic, replayable tutorial progression. The tutorial overlay owns only
    /// presentation prompts; the actual movement/combat/match systems remain authoritative.
    /// </summary>
    public sealed class TutorialStepMachine
    {
        private TutorialStep _current;

        public TutorialStepMachine()
        {
            _current = TutorialStep.Movement;
        }

        public TutorialStep Current => _current;
        public bool IsComplete => _current == TutorialStep.Complete;

        public TutorialStep Advance()
        {
            if (IsComplete) return _current;
            _current = (TutorialStep)((int)_current + 1);
            return _current;
        }

        public void Restart()
        {
            _current = TutorialStep.Movement;
        }
    }
}
