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
        private AudioClip _attackClip;
        private AudioClip _abilityClip;
        private AudioClip _hitClip;
        private AudioClip _eliminationClip;
        private AudioClip _pickupClip;
        private AudioClip _gadgetClip;
        private AudioClip _zoneWarningClip;
        private AudioClip _zoneClosingClip;
        private AudioClip _victoryClip;
        private AudioClip _defeatClip;
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
            _effectsSource.outputAudioMixerGroup = effectsGroup;
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.playOnAwake = false;
            _musicSource.loop = true;
            _musicSource.spatialBlend = 0f;
            _musicSource.outputAudioMixerGroup = musicGroup;
            _attackClip = CreateTone("AttackCue", 680f, 0.06f);
            _abilityClip = CreateTone("AbilityCue", 420f, 0.16f);
            _hitClip = CreateTone("HitCue", 120f, 0.08f);
            _eliminationClip = CreateTone("EliminationCue", 90f, 0.22f);
            _pickupClip = CreateTone("PickupCue", 860f, 0.10f);
            _gadgetClip = CreateTone("GadgetCue", 520f, 0.18f);
            _zoneWarningClip = CreateTone("ZoneWarningCue", 260f, 0.24f);
            _zoneClosingClip = CreateTone("ZoneClosingCue", 180f, 0.30f);
            _victoryClip = CreateTone("VictoryCue", 760f, 0.40f);
            _defeatClip = CreateTone("DefeatCue", 150f, 0.34f);
            _musicClip = CreateMusicLoop();
            _musicSource.clip = _musicClip;
            ApplyVolumes();
        }

        private void OnDestroy()
        {
            if (_attackClip != null) Destroy(_attackClip);
            if (_abilityClip != null) Destroy(_abilityClip);
            if (_hitClip != null) Destroy(_hitClip);
            if (_eliminationClip != null) Destroy(_eliminationClip);
            if (_pickupClip != null) Destroy(_pickupClip);
            if (_gadgetClip != null) Destroy(_gadgetClip);
            if (_zoneWarningClip != null) Destroy(_zoneWarningClip);
            if (_zoneClosingClip != null) Destroy(_zoneClosingClip);
            if (_victoryClip != null) Destroy(_victoryClip);
            if (_defeatClip != null) Destroy(_defeatClip);
            if (_musicClip != null) Destroy(_musicClip);
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
        public void PlayAbility() => Play(_abilityClip);
        public void PlayHit() => Play(_hitClip);
        public void PlayElimination() => Play(_eliminationClip);
        public void PlayPickup() => Play(_pickupClip);
        public void PlayGadget() => Play(_gadgetClip);
        public void PlayZoneWarning() => Play(_zoneWarningClip);
        public void PlayZoneClosing() => Play(_zoneClosingClip);
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
            ApplySourceVolumes();
            if (mixer == null) return;
            mixer.SetFloat("MusicVolume", ToDecibels(musicVolume));
            mixer.SetFloat("EffectsVolume", ToDecibels(effectsVolume));
        }

        private void ApplySourceVolumes()
        {
            if (_musicSource != null) _musicSource.volume = musicVolume;
            if (_effectsSource != null) _effectsSource.volume = effectsVolume;
        }

        private static float ToDecibels(float value) => value <= 0.0001f ? -80f : Mathf.Log10(value) * 20f;

        private static AudioClip CreateTone(string name, float frequency, float duration)
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
            return clip;
        }

        private static AudioClip CreateMusicLoop()
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
            return clip;
        }
    }
}
