using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Gadgets;
using BattleRaja.Presentation.Movement;
using BattleRaja.Presentation.UI;
using UnityEngine;
using UnityEngine.UI;

namespace BattleRaja.Presentation.Match
{
    /// <summary>
    /// Production-facing player card for Bastion Crown. The legacy Solo HUD remains
    /// available to the MovementLab fixture, but the player route gets one compact card
    /// that groups fighter identity, health, ability and gadget state without exposing
    /// controller/debug vocabulary.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public sealed class BastionPlayerHud : MonoBehaviour
    {
        private OfflineMatchController _match;
        private PlayerFighterSelection _selection;
        private CombatHealth _health;
        private CombatAttackController _attack;
        private BijliFighterController _bijli;
        private PehelFighterController _pehel;
        private MayaFighterController _maya;
        private GadgetUser _gadget;

        private GameObject _card;
        private Text _identityText;
        private Text _healthText;
        private Text _abilityText;
        private Text _gadgetText;
        private Image _healthFill;
        private GameObject _squadStrip;
        private SquadStatusCard[] _squadCards;

        private bool _hasState;
        private int _lastHealth;
        private int _lastMaxHealth;
        private int _lastFighter;
        private int _lastAttackTenth;
        private int _lastAbilityTenth;
        private bool _lastAbilityActive;
        private int _lastGadgetTenth;
        private string _lastGadget = string.Empty;
        private string _lastFeedback = string.Empty;
        private bool _lastResultsShown;

        private void Start()
        {
            _match = FindAnyObjectByType<OfflineMatchController>();
            if (_match == null || !_match.IsBastionCrown) return;

            ResolvePlayerReferences();
            SuppressLegacySoloCards();
            BuildCard();
            Refresh(true);
        }

        private void Update()
        {
            if (_match == null || !_match.IsBastionCrown || _card == null) return;

            var resultsShown = _match.ResultsShown;
            if (resultsShown != _lastResultsShown)
            {
                _card.SetActive(!resultsShown);
                if (_squadStrip != null) _squadStrip.SetActive(!resultsShown);
                _lastResultsShown = resultsShown;
            }

            if (!resultsShown)
            {
                Refresh(false);
                RefreshSquad(false);
            }
        }

        private void ResolvePlayerReferences()
        {
            var agents = FindObjectsByType<MovementPlayerAgent>(FindObjectsInactive.Include);
            for (var i = 0; i < agents.Length; i++)
            {
                var agent = agents[i];
                if (agent == null || agent.ActorId != 1) continue;
                var player = agent.gameObject;
                _selection = player.GetComponent<PlayerFighterSelection>();
                _health = player.GetComponent<CombatHealth>();
                _attack = player.GetComponent<CombatAttackController>();
                _bijli = player.GetComponent<BijliFighterController>();
                _pehel = player.GetComponent<PehelFighterController>();
                _maya = player.GetComponent<MayaFighterController>();
                _gadget = player.GetComponent<GadgetUser>();
                return;
            }
        }

        private void SuppressLegacySoloCards()
        {
            // These cards are still useful in the isolated MovementLab regression
            // fixture. Bastion uses the single production card instead of stacking
            // two unrelated status blocks in the portrait safe area.
            var legacyFighter = FindAnyObjectByType<BijliHud>();
            if (legacyFighter != null) legacyFighter.gameObject.SetActive(false);
            var legacyGadget = FindAnyObjectByType<GadgetHud>();
            if (legacyGadget != null) legacyGadget.gameObject.SetActive(false);
        }

        private void BuildCard()
        {
            _card = CreatePanel(transform, "PlayerStatusCard", new Vector2(0.02f, 0.875f),
                new Vector2(0.39f, 0.995f), new Color(0.025f, 0.075f, 0.105f, 0.96f));

            var accent = new GameObject("PlayerAccent", typeof(RectTransform), typeof(Image));
            accent.transform.SetParent(_card.transform, false);
            var accentRect = (RectTransform)accent.transform;
            accentRect.anchorMin = new Vector2(0f, 0.08f);
            accentRect.anchorMax = new Vector2(0.018f, 0.92f);
            accentRect.offsetMin = Vector2.zero;
            accentRect.offsetMax = Vector2.zero;
            accent.GetComponent<Image>().color = BattleRajaUiTheme.Cyan;
            accent.GetComponent<Image>().raycastTarget = false;

            _identityText = CreateText(_card.transform, "PlayerIdentity", new Vector2(0.07f, 0.64f),
                new Vector2(0.96f, 0.94f), 19, TextAnchor.MiddleLeft, BattleRajaUiTheme.Text, true);
            _healthText = CreateText(_card.transform, "PlayerHealth", new Vector2(0.62f, 0.40f),
                new Vector2(0.96f, 0.62f), 14, TextAnchor.MiddleRight, BattleRajaUiTheme.Text, true);
            _abilityText = CreateText(_card.transform, "PlayerAbility", new Vector2(0.07f, 0.18f),
                new Vector2(0.96f, 0.39f), 14, TextAnchor.MiddleLeft, BattleRajaUiTheme.MutedText, false);
            _gadgetText = CreateText(_card.transform, "PlayerGadget", new Vector2(0.07f, 0.02f),
                new Vector2(0.96f, 0.17f), 14, TextAnchor.MiddleLeft, BattleRajaUiTheme.Gold, true);

            var trackObject = new GameObject("PlayerHealthTrack", typeof(RectTransform), typeof(Image));
            trackObject.transform.SetParent(_card.transform, false);
            var trackRect = (RectTransform)trackObject.transform;
            trackRect.anchorMin = new Vector2(0.07f, 0.42f);
            trackRect.anchorMax = new Vector2(0.58f, 0.53f);
            trackRect.offsetMin = Vector2.zero;
            trackRect.offsetMax = Vector2.zero;
            var track = trackObject.GetComponent<Image>();
            track.color = new Color(0.10f, 0.18f, 0.22f, 1f);
            track.raycastTarget = false;

            var fillObject = new GameObject("PlayerHealthFill", typeof(RectTransform), typeof(Image));
            fillObject.transform.SetParent(trackObject.transform, false);
            var fillRect = (RectTransform)fillObject.transform;
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;
            _healthFill = fillObject.GetComponent<Image>();
            _healthFill.type = Image.Type.Filled;
            _healthFill.fillMethod = Image.FillMethod.Horizontal;
            _healthFill.fillOrigin = 0;
            _healthFill.color = BattleRajaUiTheme.Mint;
            _healthFill.raycastTarget = false;

            // The player card explains the selected fighter; this adjacent strip keeps
            // the 4v4 state legible at a glance without becoming another input surface.
            _squadStrip = CreatePanel(transform, "FriendlySquadStrip", new Vector2(0.02f, 0.775f),
                new Vector2(0.39f, 0.87f), new Color(0.02f, 0.055f, 0.08f, 0.94f));
            _squadCards = new SquadStatusCard[3];
            for (var i = 0; i < _squadCards.Length; i++)
            {
                _squadCards[i] = CreateSquadCard(_squadStrip.transform, i + 2, i);
            }
        }

        private void Refresh(bool force)
        {
            if (_card == null) return;
            var snapshot = _health != null ? _health.Snapshot : default(HealthSnapshot);
            var fighter = _selection != null ? _selection.ActiveFighter : ProductionFighter.Bijli;
            var attackTenth = Mathf.RoundToInt((_attack != null ? _attack.CooldownRemaining : 0f) * 10f);
            var abilityRemaining = ResolveAbilityRemaining(fighter);
            var abilityTenth = Mathf.RoundToInt(abilityRemaining * 10f);
            var abilityActive = ResolveAbilityActive(fighter);
            var gadget = _gadget != null && _gadget.HasGadget ? FriendlyGadgetName(_gadget.HeldGadget.Value) : "EMPTY";
            var gadgetTenth = Mathf.RoundToInt((_gadget != null ? _gadget.CooldownRemaining : 0f) * 10f);
            var feedback = _gadget != null ? _gadget.Feedback ?? string.Empty : string.Empty;

            if (!force && _hasState && snapshot.CurrentHealth == _lastHealth && snapshot.MaxHealth == _lastMaxHealth &&
                (int)fighter == _lastFighter && attackTenth == _lastAttackTenth && abilityTenth == _lastAbilityTenth &&
                abilityActive == _lastAbilityActive && gadgetTenth == _lastGadgetTenth && gadget == _lastGadget &&
                feedback == _lastFeedback)
            {
                return;
            }

            var role = fighter == ProductionFighter.Pehel ? "ANCHOR" : fighter == ProductionFighter.Maya ? "RUNNER" : "SKIRMISHER";
            _identityText.text = string.Format("{0}  •  {1}", fighter.ToString().ToUpperInvariant(), role);
            _healthText.text = string.Format("{0} / {1}", snapshot.CurrentHealth, snapshot.MaxHealth);
            _healthFill.fillAmount = snapshot.MaxHealth > 0
                ? Mathf.Clamp01((float)snapshot.CurrentHealth / snapshot.MaxHealth)
                : 0f;
            _healthFill.color = snapshot.CurrentHealth <= snapshot.MaxHealth * 0.30f
                ? BattleRajaUiTheme.Danger
                : snapshot.CurrentHealth <= snapshot.MaxHealth * 0.60f
                    ? BattleRajaUiTheme.Saffron
                    : BattleRajaUiTheme.Mint;

            var basic = attackTenth > 0 ? string.Format("Bolt {0:0.0}s", attackTenth / 10f) : "Bolt ready";
            var specialName = fighter == ProductionFighter.Pehel ? "Charge" : fighter == ProductionFighter.Maya ? "Decoy" : "Dash";
            var special = abilityActive ? specialName + " active" : abilityTenth > 0
                ? string.Format("{0} {1:0.0}s", specialName, abilityTenth / 10f)
                : specialName + " ready";
            _abilityText.text = string.Format("{0}   •   {1}", basic, special);

            var gadgetState = gadgetTenth > 0 ? string.Format("{0}  {1:0.0}s", gadget, gadgetTenth / 10f) : gadget + "  ready";
            if (!string.IsNullOrEmpty(feedback)) gadgetState += "  •  " + FriendlyFeedback(feedback);
            _gadgetText.text = gadgetState;
            _hasState = true;
            _lastHealth = snapshot.CurrentHealth;
            _lastMaxHealth = snapshot.MaxHealth;
            _lastFighter = (int)fighter;
            _lastAttackTenth = attackTenth;
            _lastAbilityTenth = abilityTenth;
            _lastAbilityActive = abilityActive;
            _lastGadgetTenth = gadgetTenth;
            _lastGadget = gadget;
            _lastFeedback = feedback;
        }

        private void RefreshSquad(bool force)
        {
            if (_squadCards == null || _match == null) return;

            for (var i = 0; i < _squadCards.Length; i++)
            {
                var actorId = new CombatEntityId(i + 2);
                var card = _squadCards[i];
                if (!_match.TryGetBastionParticipant(actorId, out var snapshot))
                {
                    card.Root.SetActive(false);
                    continue;
                }

                card.Root.SetActive(true);
                var respawnTenth = Mathf.CeilToInt(snapshot.RespawnRemaining * 10f);
                var key = snapshot.CurrentHealth;
                key = key * 31 + snapshot.MaxHealth;
                key = key * 31 + (snapshot.Alive ? 1 : 0);
                key = key * 31 + (snapshot.RespawnPending ? 1 : 0);
                key = key * 31 + respawnTenth;
                if (!force && card.HasState && card.LastKey == key) continue;

                card.Name.text = string.Format("A{0}  {1}", actorId.Value,
                    FriendlyFighterName(snapshot.Member.FighterId));
                if (snapshot.Alive)
                {
                    card.Status.text = string.Format("{0}/{1}  {2}", snapshot.CurrentHealth,
                        snapshot.MaxHealth, FriendlyRoleName(snapshot.Member.Role));
                    card.Fill.fillAmount = snapshot.MaxHealth > 0
                        ? Mathf.Clamp01((float)snapshot.CurrentHealth / snapshot.MaxHealth)
                        : 0f;
                    card.Fill.color = snapshot.CurrentHealth <= snapshot.MaxHealth * 0.30f
                        ? BattleRajaUiTheme.Danger
                        : snapshot.CurrentHealth <= snapshot.MaxHealth * 0.60f
                            ? BattleRajaUiTheme.Saffron
                            : BattleRajaUiTheme.Mint;
                }
                else
                {
                    var status = snapshot.RespawnPending
                        ? (snapshot.RespawnRemaining > 0.01f
                            ? string.Format("RETURN {0:0}s", snapshot.RespawnRemaining)
                            : "RESPAWN READY")
                        : "OUT OF ACTION";
                    card.Status.text = status;
                    card.Fill.fillAmount = 0f;
                    card.Fill.color = snapshot.RespawnPending
                        ? BattleRajaUiTheme.Saffron
                        : BattleRajaUiTheme.Danger;
                }

                card.HasState = true;
                card.LastKey = key;
            }
        }

        private float ResolveAbilityRemaining(ProductionFighter fighter)
        {
            if (fighter == ProductionFighter.Pehel) return _pehel != null ? _pehel.AbilityCooldownRemaining : 0f;
            if (fighter == ProductionFighter.Maya) return _maya != null ? _maya.AbilityCooldownRemaining : 0f;
            return _bijli != null ? _bijli.DashCooldownRemaining : 0f;
        }

        private bool ResolveAbilityActive(ProductionFighter fighter)
        {
            if (fighter == ProductionFighter.Pehel) return _pehel != null && _pehel.ActionState != ChargeThrowState.Ready &&
                _pehel.ActionState != ChargeThrowState.Cooldown;
            return fighter == ProductionFighter.Maya && _maya != null && _maya.IsDecoyActive;
        }

        private static string FriendlyGadgetName(string id)
        {
            if (id.IndexOf("umbrella", System.StringComparison.OrdinalIgnoreCase) >= 0) return "UMBRELLA";
            if (id.IndexOf("dhol", System.StringComparison.OrdinalIgnoreCase) >= 0) return "DHOL";
            if (id.IndexOf("tiffin", System.StringComparison.OrdinalIgnoreCase) >= 0) return "TIFFIN";
            return "GADGET";
        }

        private static string FriendlyFeedback(string feedback)
        {
            if (feedback.StartsWith("Picked ", System.StringComparison.OrdinalIgnoreCase)) return "equipped";
            if (feedback.Equals("Cooldown", System.StringComparison.OrdinalIgnoreCase)) return "cooldown";
            if (feedback.Equals("No gadget held", System.StringComparison.OrdinalIgnoreCase)) return "empty";
            if (feedback.Equals("Gadget slot full", System.StringComparison.OrdinalIgnoreCase)) return "slot full";
            return feedback.ToLowerInvariant();
        }

        private static string FriendlyFighterName(ContentId fighterId)
        {
            var value = fighterId.Value ?? string.Empty;
            if (value.IndexOf("bijli", System.StringComparison.OrdinalIgnoreCase) >= 0) return "BIJLI";
            if (value.IndexOf("pehel", System.StringComparison.OrdinalIgnoreCase) >= 0) return "PEHEL";
            if (value.IndexOf("maya", System.StringComparison.OrdinalIgnoreCase) >= 0) return "MAYA";
            return value.ToUpperInvariant();
        }

        private static string FriendlyRoleName(BastionRole role)
        {
            switch (role)
            {
                case BastionRole.Anchor: return "ANCHOR";
                case BastionRole.Runner: return "RUNNER";
                case BastionRole.Flex: return "FLEX";
                default: return "SKIRMISHER";
            }
        }

        private static SquadStatusCard CreateSquadCard(Transform parent, int actorId, int index)
        {
            const float gap = 0.025f;
            var width = (1f - gap * 4f) / 3f;
            var minX = gap + index * (width + gap);
            var maxX = minX + width;
            var root = CreatePanel(parent, "AllyCard" + actorId, new Vector2(minX, 0.10f),
                new Vector2(maxX, 0.90f), new Color(0.035f, 0.11f, 0.14f, 0.98f));

            var accent = new GameObject("Accent", typeof(RectTransform), typeof(Image));
            accent.transform.SetParent(root.transform, false);
            var accentRect = (RectTransform)accent.transform;
            accentRect.anchorMin = new Vector2(0f, 0.08f);
            accentRect.anchorMax = new Vector2(0.035f, 0.92f);
            accentRect.offsetMin = Vector2.zero;
            accentRect.offsetMax = Vector2.zero;
            accent.GetComponent<Image>().color = BattleRajaUiTheme.Cyan;
            accent.GetComponent<Image>().raycastTarget = false;

            var name = CreateText(root.transform, "Name", new Vector2(0.10f, 0.54f), new Vector2(0.96f, 0.92f),
                11, TextAnchor.MiddleLeft, BattleRajaUiTheme.Text, true);
            var status = CreateText(root.transform, "Status", new Vector2(0.10f, 0.12f), new Vector2(0.96f, 0.52f),
                10, TextAnchor.MiddleLeft, BattleRajaUiTheme.MutedText, false);

            var trackObject = new GameObject("HealthTrack", typeof(RectTransform), typeof(Image));
            trackObject.transform.SetParent(root.transform, false);
            var trackRect = (RectTransform)trackObject.transform;
            trackRect.anchorMin = new Vector2(0.10f, 0.05f);
            trackRect.anchorMax = new Vector2(0.90f, 0.12f);
            trackRect.offsetMin = Vector2.zero;
            trackRect.offsetMax = Vector2.zero;
            trackObject.GetComponent<Image>().color = new Color(0.10f, 0.19f, 0.22f, 1f);
            trackObject.GetComponent<Image>().raycastTarget = false;

            var fillObject = new GameObject("HealthFill", typeof(RectTransform), typeof(Image));
            fillObject.transform.SetParent(trackObject.transform, false);
            var fillRect = (RectTransform)fillObject.transform;
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;
            var fill = fillObject.GetComponent<Image>();
            fill.type = Image.Type.Filled;
            fill.fillMethod = Image.FillMethod.Horizontal;
            fill.fillOrigin = 0;
            fill.color = BattleRajaUiTheme.Mint;
            fill.raycastTarget = false;

            return new SquadStatusCard(root, name, status, fill);
        }

        private static GameObject CreatePanel(Transform parent, string name, Vector2 min, Vector2 max, Color color)
        {
            var panel = new GameObject(name, typeof(RectTransform), typeof(Image));
            panel.transform.SetParent(parent, false);
            var rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = min;
            rect.anchorMax = max;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            BattleRajaUiTheme.StylePanel(panel, color);
            return panel;
        }

        private static Text CreateText(Transform parent, string name, Vector2 min, Vector2 max, int size,
            TextAnchor alignment, Color color, bool bold)
        {
            var textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);
            var rect = textObject.GetComponent<RectTransform>();
            rect.anchorMin = min;
            rect.anchorMax = max;
            rect.offsetMin = new Vector2(4f, 1f);
            rect.offsetMax = new Vector2(-4f, -1f);
            var text = textObject.GetComponent<Text>();
            BattleRajaUiTheme.StyleText(text, size, alignment, color, bold);
            return text;
        }

        private sealed class SquadStatusCard
        {
            public SquadStatusCard(GameObject root, Text name, Text status, Image fill)
            {
                Root = root;
                Name = name;
                Status = status;
                Fill = fill;
            }

            public GameObject Root { get; }
            public Text Name { get; }
            public Text Status { get; }
            public Image Fill { get; }
            public bool HasState { get; set; }
            public int LastKey { get; set; }
        }
    }
}
