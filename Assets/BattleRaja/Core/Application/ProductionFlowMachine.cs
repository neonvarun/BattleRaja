using System;

namespace BattleRaja.Core.Application
{
    /// <summary>
    /// Product-facing flow states. The state machine is deliberately independent of Unity,
    /// scenes and network SDKs so menu/error transitions can be tested without a player loop.
    /// </summary>
    public enum ProductionFlowState
    {
        Bootstrap,
        MainMenu,
        ModeSelection,
        FighterSelection,
        Tutorial,
        MatchLoading,
        Gameplay,
        Paused,
        Settings,
        Spectator,
        Results,
        Error
    }

    public enum ProductionGameMode
    {
        Offline,
        Online
    }

    public enum ProductionFighter
    {
        Bijli,
        Pehel,
        Maya
    }

    public enum ProductionFlowAction
    {
        FinishBootstrap,
        OpenModeSelection,
        SelectOffline,
        SelectOnline,
        OpenFighterSelection,
        SelectFighter,
        OpenTutorial,
        CloseTutorial,
        BeginMatchLoading,
        FinishMatchLoading,
        Pause,
        Resume,
        OpenSettings,
        CloseSettings,
        EnterSpectator,
        ShowResults,
        Rematch,
        ReturnToMenu,
        ShowError,
        Retry
    }

    public readonly struct ProductionFlowTransition
    {
        public ProductionFlowTransition(
            ProductionFlowState previous,
            ProductionFlowState current,
            ProductionFlowAction action,
            bool accepted,
            string errorCode)
        {
            Previous = previous;
            Current = current;
            Action = action;
            Accepted = accepted;
            ErrorCode = errorCode ?? string.Empty;
        }

        public ProductionFlowState Previous { get; }
        public ProductionFlowState Current { get; }
        public ProductionFlowAction Action { get; }
        public bool Accepted { get; }
        public string ErrorCode { get; }
    }

    /// <summary>
    /// Deterministic product-flow transition owner. It stores only local presentation intent;
    /// match authority remains in the simulation/application systems.
    /// </summary>
    public sealed class ProductionFlowMachine
    {
        private ProductionFlowState _state = ProductionFlowState.Bootstrap;
        private ProductionGameMode _mode = ProductionGameMode.Offline;
        private ProductionFighter _fighter = ProductionFighter.Bijli;
        private ProductionFlowState _errorReturnState = ProductionFlowState.MainMenu;
        private string _errorCode = string.Empty;

        public ProductionFlowState State => _state;
        public ProductionGameMode Mode => _mode;
        public ProductionFighter Fighter => _fighter;
        public string ErrorCode => _errorCode;

        public ProductionFlowTransition FinishBootstrap()
        {
            return Transition(ProductionFlowAction.FinishBootstrap, ProductionFlowState.MainMenu,
                ProductionFlowState.Bootstrap);
        }

        public ProductionFlowTransition OpenModeSelection()
        {
            return Transition(ProductionFlowAction.OpenModeSelection, ProductionFlowState.ModeSelection,
                ProductionFlowState.MainMenu, ProductionFlowState.FighterSelection);
        }

        public ProductionFlowTransition SelectMode(ProductionGameMode mode)
        {
            _mode = mode;
            if (mode == ProductionGameMode.Online)
            {
                _errorReturnState = ProductionFlowState.ModeSelection;
            }

            return Transition(mode == ProductionGameMode.Offline
                    ? ProductionFlowAction.SelectOffline
                    : ProductionFlowAction.SelectOnline,
                mode == ProductionGameMode.Offline
                    ? ProductionFlowState.FighterSelection
                    : ProductionFlowState.Error,
                ProductionFlowState.ModeSelection,
                mode == ProductionGameMode.Offline ? string.Empty : "ONLINE_UNAVAILABLE");
        }

        public ProductionFlowTransition OpenFighterSelection()
        {
            return Transition(ProductionFlowAction.OpenFighterSelection, ProductionFlowState.FighterSelection,
                ProductionFlowState.ModeSelection);
        }

        public ProductionFlowTransition SelectFighter(ProductionFighter fighter)
        {
            _fighter = fighter;
            return Transition(ProductionFlowAction.SelectFighter, ProductionFlowState.FighterSelection,
                ProductionFlowState.FighterSelection);
        }

        public ProductionFlowTransition OpenTutorial()
        {
            return Transition(ProductionFlowAction.OpenTutorial, ProductionFlowState.Tutorial,
                ProductionFlowState.MainMenu);
        }

        public ProductionFlowTransition CloseTutorial()
        {
            return Transition(ProductionFlowAction.CloseTutorial, ProductionFlowState.MainMenu,
                ProductionFlowState.Tutorial);
        }

        public ProductionFlowTransition BeginMatchLoading()
        {
            _errorReturnState = ProductionFlowState.MatchLoading;
            return Transition(ProductionFlowAction.BeginMatchLoading, ProductionFlowState.MatchLoading,
                ProductionFlowState.FighterSelection);
        }

        public ProductionFlowTransition FinishMatchLoading()
        {
            return Transition(ProductionFlowAction.FinishMatchLoading, ProductionFlowState.Gameplay,
                ProductionFlowState.MatchLoading);
        }

        public ProductionFlowTransition Pause()
        {
            return Transition(ProductionFlowAction.Pause, ProductionFlowState.Paused,
                ProductionFlowState.Gameplay);
        }

        public ProductionFlowTransition Resume()
        {
            return Transition(ProductionFlowAction.Resume, ProductionFlowState.Gameplay,
                ProductionFlowState.Paused, ProductionFlowState.Settings);
        }

        public ProductionFlowTransition OpenSettings()
        {
            return Transition(ProductionFlowAction.OpenSettings, ProductionFlowState.Settings,
                ProductionFlowState.MainMenu, ProductionFlowState.Gameplay, ProductionFlowState.Paused);
        }

        public ProductionFlowTransition CloseSettings()
        {
            return Transition(ProductionFlowAction.CloseSettings, ProductionFlowState.MainMenu,
                ProductionFlowState.Settings);
        }

        public ProductionFlowTransition EnterSpectator()
        {
            return Transition(ProductionFlowAction.EnterSpectator, ProductionFlowState.Spectator,
                ProductionFlowState.Gameplay);
        }

        public ProductionFlowTransition ShowResults()
        {
            return Transition(ProductionFlowAction.ShowResults, ProductionFlowState.Results,
                ProductionFlowState.Gameplay, ProductionFlowState.Spectator);
        }

        public ProductionFlowTransition Rematch()
        {
            return Transition(ProductionFlowAction.Rematch, ProductionFlowState.MatchLoading,
                ProductionFlowState.Results);
        }

        public ProductionFlowTransition ReturnToMenu()
        {
            return Transition(ProductionFlowAction.ReturnToMenu, ProductionFlowState.MainMenu,
                new[]
                {
                    ProductionFlowState.ModeSelection, ProductionFlowState.FighterSelection,
                    ProductionFlowState.Tutorial, ProductionFlowState.MatchLoading,
                    ProductionFlowState.Gameplay, ProductionFlowState.Paused,
                    ProductionFlowState.Settings, ProductionFlowState.Spectator,
                    ProductionFlowState.Results, ProductionFlowState.Error
                }, string.Empty);
        }

        public ProductionFlowTransition ShowError(string errorCode, ProductionFlowState returnState)
        {
            _errorCode = string.IsNullOrEmpty(errorCode) ? "UNKNOWN_ERROR" : errorCode;
            _errorReturnState = returnState;
            return Transition(ProductionFlowAction.ShowError, ProductionFlowState.Error,
                new[]
                {
                    ProductionFlowState.Bootstrap, ProductionFlowState.MainMenu,
                    ProductionFlowState.ModeSelection, ProductionFlowState.FighterSelection,
                    ProductionFlowState.Tutorial, ProductionFlowState.MatchLoading,
                    ProductionFlowState.Gameplay, ProductionFlowState.Paused,
                    ProductionFlowState.Settings, ProductionFlowState.Spectator,
                    ProductionFlowState.Results
                }, errorCode);
        }

        public ProductionFlowTransition Retry()
        {
            var target = _errorReturnState == ProductionFlowState.MatchLoading
                ? ProductionFlowState.MatchLoading
                : _errorReturnState;
            var result = Transition(ProductionFlowAction.Retry, target, ProductionFlowState.Error);
            if (result.Accepted) _errorCode = string.Empty;
            return result;
        }

        private ProductionFlowTransition Transition(
            ProductionFlowAction action,
            ProductionFlowState next,
            params ProductionFlowState[] allowed)
        {
            return Transition(action, next, allowed, string.Empty);
        }

        private ProductionFlowTransition Transition(
            ProductionFlowAction action,
            ProductionFlowState next,
            ProductionFlowState requiredState,
            string errorCode)
        {
            return Transition(action, next, new[] { requiredState }, errorCode);
        }

        private ProductionFlowTransition Transition(
            ProductionFlowAction action,
            ProductionFlowState next,
            ProductionFlowState firstRequired,
            ProductionFlowState secondRequired)
        {
            return Transition(action, next, new[] { firstRequired, secondRequired }, string.Empty);
        }

        private ProductionFlowTransition Transition(
            ProductionFlowAction action,
            ProductionFlowState next,
            ProductionFlowState firstRequired,
            ProductionFlowState secondRequired,
            ProductionFlowState thirdRequired)
        {
            return Transition(action, next, new[] { firstRequired, secondRequired, thirdRequired }, string.Empty);
        }

        private ProductionFlowTransition Transition(
            ProductionFlowAction action,
            ProductionFlowState next,
            ProductionFlowState firstRequired,
            ProductionFlowState secondRequired,
            ProductionFlowState thirdRequired,
            ProductionFlowState fourthRequired,
            ProductionFlowState fifthRequired,
            ProductionFlowState sixthRequired,
            ProductionFlowState seventhRequired,
            ProductionFlowState eighthRequired,
            ProductionFlowState ninthRequired,
            ProductionFlowState tenthRequired)
        {
            return Transition(action, next, new[]
            {
                firstRequired, secondRequired, thirdRequired, fourthRequired, fifthRequired,
                sixthRequired, seventhRequired, eighthRequired, ninthRequired, tenthRequired
            }, string.Empty);
        }

        private ProductionFlowTransition Transition(
            ProductionFlowAction action,
            ProductionFlowState next,
            ProductionFlowState[] allowed,
            string errorCode)
        {
            var previous = _state;
            var accepted = false;
            for (var i = 0; i < allowed.Length; i++)
            {
                if (allowed[i] != _state) continue;
                accepted = true;
                break;
            }

            if (accepted)
            {
                _state = next;
                if (!string.IsNullOrEmpty(errorCode)) _errorCode = errorCode;
            }

            return new ProductionFlowTransition(previous, _state, action, accepted,
                accepted ? errorCode : "INVALID_TRANSITION");
        }
    }
}
