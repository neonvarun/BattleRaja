using BattleRaja.Core.Application;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    public sealed class ProductionFlowMachineTests
    {
        [Test]
        public void OfflineFlowReachesGameplayWithSelectedFighter()
        {
            var flow = new ProductionFlowMachine();

            Assert.That(flow.FinishBootstrap().Accepted, Is.True);
            Assert.That(flow.OpenModeSelection().Accepted, Is.True);
            Assert.That(flow.SelectMode(ProductionGameMode.Offline).Accepted, Is.True);
            Assert.That(flow.SelectFighter(ProductionFighter.Maya).Accepted, Is.True);
            Assert.That(flow.BeginMatchLoading().Accepted, Is.True);
            Assert.That(flow.FinishMatchLoading().Accepted, Is.True);

            Assert.That(flow.State, Is.EqualTo(ProductionFlowState.Gameplay));
            Assert.That(flow.Mode, Is.EqualTo(ProductionGameMode.Offline));
            Assert.That(flow.Fighter, Is.EqualTo(ProductionFighter.Maya));
        }

        [Test]
        public void OnlineSelectionShowsExplicitUnavailableError()
        {
            var flow = new ProductionFlowMachine();
            flow.FinishBootstrap();
            flow.OpenModeSelection();

            var transition = flow.SelectMode(ProductionGameMode.Online);

            Assert.That(transition.Accepted, Is.True);
            Assert.That(flow.State, Is.EqualTo(ProductionFlowState.Error));
            Assert.That(flow.ErrorCode, Is.EqualTo("ONLINE_UNAVAILABLE"));
            Assert.That(flow.Retry().Accepted, Is.True);
            Assert.That(flow.State, Is.EqualTo(ProductionFlowState.ModeSelection));
        }

        [Test]
        public void SettingsResumeAndResultsHaveGuardedTransitions()
        {
            var flow = new ProductionFlowMachine();
            flow.FinishBootstrap();
            flow.OpenModeSelection();
            flow.SelectMode(ProductionGameMode.Offline);
            flow.SelectFighter(ProductionFighter.Bijli);
            flow.BeginMatchLoading();
            flow.FinishMatchLoading();

            Assert.That(flow.Pause().Accepted, Is.True);
            Assert.That(flow.OpenSettings().Accepted, Is.True);
            Assert.That(flow.Resume().Accepted, Is.True);
            Assert.That(flow.OpenSettings().Accepted, Is.True);
            Assert.That(flow.CloseSettings().Accepted, Is.True);
            Assert.That(flow.ShowResults().Accepted, Is.False);
            Assert.That(flow.OpenModeSelection().Accepted, Is.True);
        }

        [Test]
        public void ResultsSupportRematchAndReturnToMenu()
        {
            var flow = new ProductionFlowMachine();
            flow.FinishBootstrap();
            flow.OpenModeSelection();
            flow.SelectMode(ProductionGameMode.Offline);
            flow.SelectFighter(ProductionFighter.Pehel);
            flow.BeginMatchLoading();
            flow.FinishMatchLoading();
            flow.ShowResults();

            Assert.That(flow.Rematch().Accepted, Is.True);
            Assert.That(flow.FinishMatchLoading().Accepted, Is.True);
            Assert.That(flow.ReturnToMenu().Accepted, Is.True);
            Assert.That(flow.State, Is.EqualTo(ProductionFlowState.MainMenu));
        }

        [Test]
        public void InvalidTransitionsDoNotMutateState()
        {
            var flow = new ProductionFlowMachine();

            var transition = flow.SelectFighter(ProductionFighter.Pehel);

            Assert.That(transition.Accepted, Is.False);
            Assert.That(transition.ErrorCode, Is.EqualTo("INVALID_TRANSITION"));
            Assert.That(flow.State, Is.EqualTo(ProductionFlowState.Bootstrap));
        }

        [Test]
        public void TutorialRouteReturnsToMainMenu()
        {
            var flow = new ProductionFlowMachine();
            flow.FinishBootstrap();

            Assert.That(flow.OpenTutorial().Accepted, Is.True);
            Assert.That(flow.State, Is.EqualTo(ProductionFlowState.Tutorial));
            Assert.That(flow.CloseTutorial().Accepted, Is.True);
            Assert.That(flow.State, Is.EqualTo(ProductionFlowState.MainMenu));
        }
    }
}
