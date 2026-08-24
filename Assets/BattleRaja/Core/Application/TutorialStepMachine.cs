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

    public enum TutorialAction
    {
        None = 0,
        Movement = 1,
        Aim = 2,
        BasicAttack = 3,
        Ability = 4,
        GadgetCollected = 5,
        GadgetUsed = 6,
        AandhiObserved = 7,
        Elimination = 8,
        Victory = 9
    }

    /// <summary>
    /// Deterministic, replayable tutorial progression. The tutorial overlay owns only
    /// presentation prompts; the actual movement/combat/match systems remain authoritative.
    /// </summary>
    public sealed class TutorialStepMachine
    {
        private TutorialStep _current;
        private bool _currentSatisfied;
        private bool _gadgetCollected;
        private bool _gadgetUsed;

        public TutorialStepMachine()
        {
            _current = TutorialStep.Movement;
        }

        public TutorialStep Current => _current;
        public bool IsComplete => _current == TutorialStep.Complete;
        public bool CurrentStepSatisfied => IsComplete || (_current == TutorialStep.Gadget
            ? _gadgetCollected && _gadgetUsed
            : _currentSatisfied);

        public TutorialAction RequiredAction
        {
            get
            {
                switch (_current)
                {
                    case TutorialStep.Movement: return TutorialAction.Movement;
                    case TutorialStep.Aim: return TutorialAction.Aim;
                    case TutorialStep.BasicAttack: return TutorialAction.BasicAttack;
                    case TutorialStep.Ability: return TutorialAction.Ability;
                    case TutorialStep.Gadget: return _gadgetCollected
                        ? TutorialAction.GadgetUsed
                        : TutorialAction.GadgetCollected;
                    case TutorialStep.Aandhi: return TutorialAction.AandhiObserved;
                    case TutorialStep.Elimination: return TutorialAction.Elimination;
                    case TutorialStep.Victory: return TutorialAction.Victory;
                    default: return TutorialAction.None;
                }
            }
        }

        public bool ObserveAction(TutorialAction action)
        {
            if (IsComplete || action == TutorialAction.None) return false;

            if (_current == TutorialStep.Gadget)
            {
                if (action == TutorialAction.GadgetCollected)
                {
                    _gadgetCollected = true;
                    return true;
                }

                if (action == TutorialAction.GadgetUsed && _gadgetCollected)
                {
                    _gadgetUsed = true;
                    return true;
                }

                return false;
            }

            if (action != RequiredAction) return false;
            _currentSatisfied = true;
            return true;
        }

        public bool TryAdvance()
        {
            if (IsComplete || !CurrentStepSatisfied) return false;
            _current = (TutorialStep)((int)_current + 1);
            _currentSatisfied = false;
            _gadgetCollected = false;
            _gadgetUsed = false;
            return true;
        }

        /// <summary>
        /// Explicit accessibility/test escape hatch. Normal progression must use
        /// ObserveAction followed by TryAdvance so a lesson cannot be completed by
        /// pressing the continue button alone.
        /// </summary>
        public void SkipToComplete()
        {
            _current = TutorialStep.Complete;
            _currentSatisfied = true;
            _gadgetCollected = true;
            _gadgetUsed = true;
        }

        public TutorialStep Advance()
        {
            TryAdvance();
            return _current;
        }

        public void Restart()
        {
            _current = TutorialStep.Movement;
            _currentSatisfied = false;
            _gadgetCollected = false;
            _gadgetUsed = false;
        }
    }
}
