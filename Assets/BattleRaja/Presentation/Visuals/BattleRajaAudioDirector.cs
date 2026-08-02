using UnityEngine;
using UnityEngine.Audio;

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
        private AudioClip _attackClip;
        private AudioClip _abilityClip;
        private AudioClip _hitClip;
        private AudioClip _eliminationClip;
        private bool _started;

        public bool IsStarted => _started;
        public bool ReducedFlashMode { get => reducedFlashMode; set => reducedFlashMode = value; }

        private void Awake()
        {
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
            _attackClip = CreateTone("AttackCue", 680f, 0.06f);
            _abilityClip = CreateTone("AbilityCue", 420f, 0.16f);
            _hitClip = CreateTone("HitCue", 120f, 0.08f);
            _eliminationClip = CreateTone("EliminationCue", 90f, 0.22f);
            ApplyVolumes();
        }

        private void Update()
        {
            if (!_started && (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.touchCount > 0))
            {
                StartFromUserGesture();
            }
        }

        public void StartFromUserGesture()
        {
            _started = true;
            ApplyVolumes();
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

        private void Play(AudioClip clip)
        {
            if (!_started || clip == null || _effectsSource == null) return;
            _effectsSource.PlayOneShot(clip, effectsVolume);
        }

        private void ApplyVolumes()
        {
            if (mixer == null) return;
            mixer.SetFloat("MusicVolume", ToDecibels(musicVolume));
            mixer.SetFloat("EffectsVolume", ToDecibels(effectsVolume));
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
    }
}
