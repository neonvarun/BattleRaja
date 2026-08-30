using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

namespace BattleRaja.Presentation.Visuals
{
    /// <summary>
    /// Original procedural cue layer for the vertical slice. It is silent until a user
    /// gesture starts it, which keeps Web autoplay policy safe. Optional mixer groups are
    /// exposed for the production audio asset pass.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public sealed class BattleRajaAudioDirector : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private AudioMixerGroup musicGroup;
        [SerializeField] private AudioMixerGroup effectsGroup;
        [SerializeField] [Range(0f, 1f)] private float musicVolume = 0.8f;
        [SerializeField] [Range(0f, 1f)] private float effectsVolume = 1f;
        [SerializeField] private bool reducedFlashMode;

        private AudioSource _effectsSource;
        private AudioSource _musicSource;
        private AudioClip _musicClip;
        private AudioClip _uiConfirmClip;
        private AudioClip _uiBackClip;
        private AudioClip _attackClip;
        private AudioClip _bijliAttackClip;
        private AudioClip _pehelAttackClip;
        private AudioClip _mayaAttackClip;
        private AudioClip _abilityClip;
        private AudioClip _bijliAbilityClip;
        private AudioClip _pehelAbilityClip;
        private AudioClip _mayaAbilityClip;
        private AudioClip _hitClip;
        private AudioClip _eliminationClip;
        private AudioClip _pickupClip;
        private AudioClip _gadgetClip;
        private AudioClip _umbrellaClip;
        private AudioClip _dholClip;
        private AudioClip _tiffinClip;
        private AudioClip _zoneWarningClip;
        private AudioClip _zoneClosingClip;
        private AudioClip _zoneFinalCircleClip;
        private AudioClip _victoryClip;
        private AudioClip _defeatClip;
        private readonly List<AudioClip> _generatedClips = new List<AudioClip>(16);
        private bool _started;

        public bool IsStarted => _started;
        public bool ReducedFlashMode { get => reducedFlashMode; set => reducedFlashMode = value; }

        private void Awake()
        {
            if (FindAnyObjectByType<AudioListener>() == null)
            {
                var listenerCamera = Camera.main != null ? Camera.main : FindAnyObjectByType<Camera>();
                if (listenerCamera != null) listenerCamera.gameObject.AddComponent<AudioListener>();
            }

            _effectsSource = gameObject.GetComponent<AudioSource>();
            if (_effectsSource == null)
            {
                enabled = false;
                return;
            }
            _effectsSource.playOnAwake = false;
            _effectsSource.loop = false;
            _effectsSource.spatialBlend = 0f;
            mixer = mixer != null ? mixer : Resources.Load<AudioMixer>("Audio/V1/BattleRajaV1");
            if (mixer != null)
            {
                if (musicGroup == null) musicGroup = FindMixerGroup("Music");
                if (effectsGroup == null) effectsGroup = FindMixerGroup("Combat");
            }
            _effectsSource.outputAudioMixerGroup = effectsGroup;
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.playOnAwake = false;
            _musicSource.loop = true;
            _musicSource.spatialBlend = 0f;
            _musicSource.outputAudioMixerGroup = musicGroup;
            _uiConfirmClip = LoadOrFallback("UiConfirm", 660f, 0.11f);
            _uiBackClip = LoadOrFallback("UiBack", 520f, 0.13f);
            _attackClip = LoadOrFallback("AttackGeneric", 680f, 0.06f);
            _bijliAttackClip = LoadOrFallback("AttackBijli", 740f, 0.08f);
            _pehelAttackClip = LoadOrFallback("AttackPehel", 150f, 0.12f);
            _mayaAttackClip = LoadOrFallback("AttackMaya", 880f, 0.10f);
            _abilityClip = LoadOrFallback("AbilityGeneric", 420f, 0.16f);
            _bijliAbilityClip = LoadOrFallback("AbilityBijli", 420f, 0.20f);
            _pehelAbilityClip = LoadOrFallback("AbilityPehel", 180f, 0.22f);
            _mayaAbilityClip = LoadOrFallback("AbilityMaya", 520f, 0.22f);
            _hitClip = LoadOrFallback("Hit", 120f, 0.08f);
            _eliminationClip = LoadOrFallback("Elimination", 90f, 0.22f);
            _pickupClip = LoadOrFallback("Pickup", 860f, 0.10f);
            _gadgetClip = LoadOrFallback("GadgetGeneric", 520f, 0.18f);
            _umbrellaClip = LoadOrFallback("GadgetUmbrella", 560f, 0.20f);
            _dholClip = LoadOrFallback("GadgetDhol", 140f, 0.24f);
            _tiffinClip = LoadOrFallback("GadgetTiffin", 320f, 0.20f);
            _zoneWarningClip = LoadOrFallback("ZoneWarning", 260f, 0.24f);
            _zoneClosingClip = LoadOrFallback("ZoneClosing", 180f, 0.30f);
            _zoneFinalCircleClip = LoadOrFallback("ZoneFinalCircle", 720f, 0.40f);
            _victoryClip = LoadOrFallback("Victory", 760f, 0.40f);
            _defeatClip = LoadOrFallback("Defeat", 150f, 0.34f);
            _musicClip = Resources.Load<AudioClip>("Audio/V1/MatchMusic") ?? CreateMusicLoop();
            _musicSource.clip = _musicClip;
            ApplyVolumes();
        }

        private void OnDestroy()
        {
            for (var i = 0; i < _generatedClips.Count; i++)
            {
                if (_generatedClips[i] != null) Destroy(_generatedClips[i]);
            }
        }

        private void Update()
        {
            var keyboardGesture = Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame;
            var mouseGesture = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
            var touchGesture = Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame;
            if (!_started && (keyboardGesture || mouseGesture || touchGesture))
            {
                StartFromUserGesture();
            }
        }

        public void StartFromUserGesture()
        {
            _started = true;
            ApplyVolumes();
            if (_musicSource != null && _musicClip != null && !_musicSource.isPlaying)
            {
                _musicSource.Play();
            }
        }

        public void SetMusicVolume(float value)
        {
            musicVolume = Mathf.Clamp01(value);
            ApplyVolumes();
        }

        public void SetEffectsVolume(float value)
        {
            effectsVolume = Mathf.Clamp01(value);
            ApplyVolumes();
        }

        public void PlayAttack() => Play(_attackClip);
        public void PlayAttack(string fighterId) => Play(SelectFighterClip(fighterId, _attackClip, _bijliAttackClip, _pehelAttackClip, _mayaAttackClip));
        public void PlayAbility() => Play(_abilityClip);
        public void PlayAbility(string fighterId) => Play(SelectFighterClip(fighterId, _abilityClip, _bijliAbilityClip, _pehelAbilityClip, _mayaAbilityClip));
        public void PlayHit() => Play(_hitClip);
        public void PlayElimination() => Play(_eliminationClip);
        public void PlayPickup() => Play(_pickupClip);
        public void PlayGadget() => Play(_gadgetClip);
        public void PlayGadget(string gadgetId)
        {
            if (string.IsNullOrEmpty(gadgetId)) { Play(_gadgetClip); return; }
            if (gadgetId.IndexOf("dhol", System.StringComparison.OrdinalIgnoreCase) >= 0) Play(_dholClip);
            else if (gadgetId.IndexOf("tiffin", System.StringComparison.OrdinalIgnoreCase) >= 0) Play(_tiffinClip);
            else if (gadgetId.IndexOf("umbrella", System.StringComparison.OrdinalIgnoreCase) >= 0) Play(_umbrellaClip);
            else Play(_gadgetClip);
        }
        public void PlayUiConfirm() => Play(_uiConfirmClip);
        public void PlayUiBack() => Play(_uiBackClip);
        public void PlayZoneWarning() => Play(_zoneWarningClip);
        public void PlayZoneClosing() => Play(_zoneClosingClip);
        public void PlayZoneFinalCircle() => Play(_zoneFinalCircleClip);
        public void PlayVictory() => Play(_victoryClip);
        public void PlayDefeat() => Play(_defeatClip);

        private void Play(AudioClip clip)
        {
            if (!_started || clip == null || _effectsSource == null) return;
            // The source and mixer already apply the effects preference. Passing the
            // preference a second time to PlayOneShot attenuated cues twice, making
            // combat feedback disappear on lower-volume settings.
            _effectsSource.PlayOneShot(clip);
        }

        private void ApplyVolumes()
        {
            // Keep the persisted controls on the owned sources. The generated mixer uses
            // named routing buses but intentionally has no fragile editor-only exposed
            // parameter metadata; probing absent names logs a Unity warning. A human-authored
            // mixer can still provide its own source/mixer automation without changing this
            // offline-safe fallback path.
            ApplySourceVolumes();
        }

        private void ApplySourceVolumes()
        {
            if (_musicSource != null) _musicSource.volume = musicVolume;
            if (_effectsSource != null) _effectsSource.volume = effectsVolume;
        }

        private AudioClip LoadOrFallback(string resourceName, float frequency, float duration)
        {
            return Resources.Load<AudioClip>("Audio/V1/" + resourceName) ?? CreateTone(resourceName + "Fallback", frequency, duration);
        }

        private AudioMixerGroup FindMixerGroup(string groupName)
        {
            if (mixer == null) return null;
            var groups = mixer.FindMatchingGroups(groupName);
            return groups != null && groups.Length > 0 ? groups[0] : null;
        }

        private static AudioClip SelectFighterClip(string fighterId, AudioClip fallback, AudioClip bijli, AudioClip pehel, AudioClip maya)
        {
            if (string.IsNullOrEmpty(fighterId)) return fallback;
            if (fighterId.IndexOf("pehel", System.StringComparison.OrdinalIgnoreCase) >= 0) return pehel;
            if (fighterId.IndexOf("maya", System.StringComparison.OrdinalIgnoreCase) >= 0) return maya;
            if (fighterId.IndexOf("bijli", System.StringComparison.OrdinalIgnoreCase) >= 0) return bijli;
            return fallback;
        }

        private AudioClip CreateTone(string name, float frequency, float duration)
        {
            const int sampleRate = 44100;
            var samples = Mathf.Max(1, Mathf.CeilToInt(sampleRate * duration));
            var clip = AudioClip.Create(name, samples, 1, sampleRate, false);
            var data = new float[samples];
            for (var i = 0; i < samples; i++)
            {
                var envelope = 1f - (float)i / samples;
                data[i] = Mathf.Sin(2f * Mathf.PI * frequency * i / sampleRate) * envelope * 0.18f;
            }

            clip.SetData(data, 0);
            _generatedClips.Add(clip);
            return clip;
        }

        private AudioClip CreateMusicLoop()
        {
            const int sampleRate = 22050;
            const float duration = 8f;
            var samples = Mathf.CeilToInt(sampleRate * duration);
            var clip = AudioClip.Create("BattleRajaLoop", samples, 1, sampleRate, false);
            var data = new float[samples];
            var notes = new[] { 220f, 277.18f, 329.63f, 440f, 329.63f, 277.18f, 246.94f, 329.63f };
            for (var i = 0; i < samples; i++)
            {
                var time = (float)i / sampleRate;
                var noteIndex = Mathf.FloorToInt(time * 2f) % notes.Length;
                var phase = time * notes[noteIndex] * Mathf.PI * 2f;
                var pulse = Mathf.Sin(time * Mathf.PI * 2f * 2f) * 0.08f;
                var envelope = 0.12f * (0.78f + 0.22f * Mathf.Sin(time * Mathf.PI * 2f / duration));
                data[i] = (Mathf.Sin(phase) * 0.65f + Mathf.Sin(phase * 0.5f) * 0.25f + pulse) * envelope;
            }

            clip.SetData(data, 0);
            _generatedClips.Add(clip);
            return clip;
        }
    }
}
