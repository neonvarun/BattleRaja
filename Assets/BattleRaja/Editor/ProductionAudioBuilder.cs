using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace BattleRaja.Editor
{
    /// <summary>
    /// Generates owned, reproducible PCM source audio for the V1 vertical slice. The
    /// waveforms are intentionally original synthesis recipes rather than recordings or
    /// references to another game. The generated WAV files are the editable provenance
    /// inputs; Unity imports them from Resources for the offline runtime.
    /// </summary>
    public static class ProductionAudioBuilder
    {
        public const string ResourceRoot = "Assets/BattleRaja/Resources/Audio/V1";
        public const string ResourceLoadRoot = "Audio/V1";
        public const string MixerPath = ResourceRoot + "/BattleRajaV1.mixer";

        private const int CombatSampleRate = 44100;
        private const int MusicSampleRate = 22050;

        [MenuItem("BattleRaja/Build V1 Production Audio")]
        public static void BuildAll()
        {
            Directory.CreateDirectory(ResourceRoot);
            foreach (var spec in ClipSpecs()) WriteWav(spec);
            EnsureMixer();
            ClearInvalidExposedVolumeParameters();
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            Debug.Log("BattleRaja production audio generated: owned WAV sources and mixer groups.");
        }

        private static IEnumerable<ClipSpec> ClipSpecs()
        {
            yield return new ClipSpec("UiConfirm", 0.11f, CombatSampleRate, 1, 1);
            yield return new ClipSpec("UiBack", 0.13f, CombatSampleRate, 1, 2);
            yield return new ClipSpec("AttackGeneric", 0.08f, CombatSampleRate, 1, 3);
            yield return new ClipSpec("AttackBijli", 0.09f, CombatSampleRate, 1, 4);
            yield return new ClipSpec("AttackPehel", 0.12f, CombatSampleRate, 1, 5);
            yield return new ClipSpec("AttackMaya", 0.10f, CombatSampleRate, 1, 6);
            yield return new ClipSpec("AbilityGeneric", 0.18f, CombatSampleRate, 1, 7);
            yield return new ClipSpec("AbilityBijli", 0.22f, CombatSampleRate, 1, 8);
            yield return new ClipSpec("AbilityPehel", 0.24f, CombatSampleRate, 1, 9);
            yield return new ClipSpec("AbilityMaya", 0.24f, CombatSampleRate, 1, 10);
            yield return new ClipSpec("Hit", 0.08f, CombatSampleRate, 1, 11);
            yield return new ClipSpec("Elimination", 0.24f, CombatSampleRate, 1, 12);
            yield return new ClipSpec("Pickup", 0.10f, CombatSampleRate, 1, 13);
            yield return new ClipSpec("GadgetGeneric", 0.18f, CombatSampleRate, 1, 14);
            yield return new ClipSpec("GadgetUmbrella", 0.20f, CombatSampleRate, 1, 15);
            yield return new ClipSpec("GadgetDhol", 0.26f, CombatSampleRate, 1, 16);
            yield return new ClipSpec("GadgetTiffin", 0.22f, CombatSampleRate, 1, 17);
            yield return new ClipSpec("ZoneWarning", 0.26f, CombatSampleRate, 1, 18);
            yield return new ClipSpec("ZoneClosing", 0.32f, CombatSampleRate, 1, 19);
            yield return new ClipSpec("ZoneFinalCircle", 0.42f, CombatSampleRate, 1, 24);
            yield return new ClipSpec("Victory", 0.48f, CombatSampleRate, 1, 20);
            yield return new ClipSpec("Defeat", 0.38f, CombatSampleRate, 1, 21);
            yield return new ClipSpec("BazaarAmbience", 12f, MusicSampleRate, 2, 22);
            yield return new ClipSpec("MatchMusic", 12f, MusicSampleRate, 2, 23);
        }

        private static void WriteWav(ClipSpec spec)
        {
            var frames = Mathf.Max(1, Mathf.CeilToInt(spec.Duration * spec.SampleRate));
            var bytesPerSample = 2;
            var dataLength = frames * spec.Channels * bytesPerSample;
            using (var stream = new MemoryStream(44 + dataLength))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));
                writer.Write(36 + dataLength);
                writer.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"));
                writer.Write(System.Text.Encoding.ASCII.GetBytes("fmt "));
                writer.Write(16);
                writer.Write((short)1);
                writer.Write((short)spec.Channels);
                writer.Write(spec.SampleRate);
                writer.Write(spec.SampleRate * spec.Channels * bytesPerSample);
                writer.Write((short)(spec.Channels * bytesPerSample));
                writer.Write((short)16);
                writer.Write(System.Text.Encoding.ASCII.GetBytes("data"));
                writer.Write(dataLength);

                for (var frame = 0; frame < frames; frame++)
                {
                    var time = (float)frame / spec.SampleRate;
                    var left = Sample(spec.Kind, time, spec.Duration, spec.SampleRate);
                    var right = spec.Channels == 2
                        ? Sample(spec.Kind, time + 0.017f, spec.Duration, spec.SampleRate)
                        : left;
                    writer.Write((short)(Mathf.Clamp(left, -1f, 1f) * 30000f));
                    if (spec.Channels == 2) writer.Write((short)(Mathf.Clamp(right, -1f, 1f) * 30000f));
                }

                File.WriteAllBytes(Path.Combine(ResourceRoot, spec.Name + ".wav"), stream.ToArray());
            }
        }

        private static float Sample(int kind, float time, float duration, int sampleRate)
        {
            var progress = Mathf.Clamp01(time / duration);
            var envelope = Mathf.Pow(1f - progress, kind >= 22 ? 0.18f : 0.72f);
            var value = 0f;
            switch (kind)
            {
                case 1: value = Note(time, 660f, 0.16f) + Note(time, 990f, 0.10f, 0.045f); break;
                case 2: value = Note(time, 520f - progress * 180f, 0.15f) + Note(time, 260f, 0.08f); break;
                case 3: value = Note(time, 690f, 0.20f) + Noise(time, 0.035f); break;
                case 4: value = Chirp(time, 700f, 1280f, 0.24f) + Note(time, 1760f, 0.08f, 0.018f); break;
                case 5: value = Note(time, 150f, 0.34f) + Note(time, 300f, 0.18f) + Noise(time, 0.02f); break;
                case 6: value = Note(time, 880f, 0.18f) - Note(time, 1320f, 0.12f, 0.03f) + Noise(time, 0.018f); break;
                case 7: value = Note(time, 420f, 0.24f) + Note(time, 630f, 0.14f, 0.06f); break;
                case 8: value = Chirp(time, 280f, 1320f, 0.28f) + Note(time, 1760f, 0.10f, 0.05f); break;
                case 9: value = Note(time, 115f, 0.38f) + Note(time, 230f, 0.20f) + Note(time, 345f, 0.12f, 0.08f); break;
                case 10: value = Note(time, 480f, 0.20f) + Note(time, 720f, 0.15f, 0.07f) + Note(time, 1080f, 0.10f, 0.14f); break;
                case 11: value = Note(time, 120f, 0.34f) + Noise(time, 0.12f); break;
                case 12: value = Note(time, 180f, 0.28f) + Note(time, 90f, 0.17f, 0.08f); break;
                case 13: value = Note(time, 860f, 0.17f) + Note(time, 1290f, 0.10f, 0.035f); break;
                case 14: value = Note(time, 520f, 0.22f) + Note(time, 780f, 0.10f, 0.08f); break;
                case 15: value = Note(time, 560f, 0.18f) + Note(time, 1120f, 0.13f, 0.06f); break;
                case 16: value = Drum(time, 0.28f, 110f) + Note(time, 220f, 0.22f, 0.03f) + Noise(time, 0.06f); break;
                case 17: value = Note(time, 320f, 0.28f) + Note(time, 640f, 0.17f, 0.08f); break;
                case 18: value = Note(time, 260f, 0.24f) + Note(time, 390f, 0.15f, 0.12f); break;
                case 19: value = Chirp(time, 210f, 120f, 0.18f) + Noise(time, 0.06f); break;
                case 24:
                    value = Chirp(time, 360f, 920f, 0.22f)
                        + Note(time, 1380f, 0.10f, 0.10f)
                        + Drum(time, 0.20f, 180f) * 0.30f;
                    break;
                case 20: value = Note(time, 523.25f, 0.18f) + Note(time, 659.25f, 0.15f, 0.11f) + Note(time, 783.99f, 0.12f, 0.22f); break;
                case 21: value = Note(time, 220f, 0.25f) + Note(time, 164.81f, 0.18f, 0.12f); break;
                case 22: value = 0.09f * (Noise(time, 1f) + Note(time, 146.83f, 0.12f) + Note(time, 220f, 0.08f, 2.6f)); break;
                case 23:
                    var notes = new[] { 220f, 277.18f, 329.63f, 440f, 329.63f, 277.18f, 246.94f, 329.63f };
                    var note = notes[Mathf.FloorToInt(time * 2f) % notes.Length];
                    value = Note(time, note, 0.16f) + Note(time, note * 2f, 0.05f, 0.03f) + Drum(time % 0.5f, 0.5f, 90f) * 0.08f;
                    break;
                default: value = Note(time, 440f, 0.12f); break;
            }

            var fadeIn = Mathf.Clamp01(time * 120f);
            return value * envelope * fadeIn * (kind >= 22 ? 0.72f : 0.55f);
        }

        private static float Note(float time, float frequency, float level, float delay = 0f)
        {
            if (time < delay) return 0f;
            var local = time - delay;
            return Mathf.Sin(local * frequency * Mathf.PI * 2f) * level;
        }

        private static float Chirp(float time, float start, float end, float level)
        {
            var frequency = Mathf.Lerp(start, end, Mathf.Clamp01(time / 0.3f));
            return Mathf.Sin(time * frequency * Mathf.PI * 2f) * level;
        }

        private static float Drum(float time, float window, float frequency)
        {
            if (time < 0f || time > window) return 0f;
            var envelope = Mathf.Pow(1f - time / window, 2.4f);
            return Mathf.Sin(time * frequency * Mathf.PI * 2f) * envelope * 0.7f;
        }

        private static float Noise(float time, float level)
        {
            var value = Mathf.Sin(time * 1731.17f) * Mathf.Sin(time * 271.91f + 0.7f);
            return value * level;
        }

        private static void EnsureMixer()
        {
            var mixer = AssetDatabase.LoadAssetAtPath<AudioMixer>(MixerPath);
            var targetNames = new[] { "Music", "Ambience", "UI", "Combat", "Abilities", "Gadgets", "Zone" };
            if (mixer != null)
            {
                var counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(MixerPath))
                {
                    if (asset == null || asset.GetType().FullName != "UnityEditor.Audio.AudioMixerGroupController") continue;
                    var name = asset.GetType().GetProperty("name", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(asset, null) as string;
                    if (Array.IndexOf(targetNames, name) < 0) continue;
                    counts[name] = counts.TryGetValue(name, out var count) ? count + 1 : 1;
                }

                if (counts.Values.Any(value => value > 1))
                {
                    // This is a generated V1 asset; rebuild only when an earlier
                    // headless pass left duplicate buses, never broad project data.
                    AssetDatabase.DeleteAsset(MixerPath);
                    AssetDatabase.Refresh();
                    mixer = null;
                }
            }

            if (mixer == null)
            {
                var type = Type.GetType("UnityEditor.Audio.AudioMixerController, UnityEditor");
                var method = type?.GetMethod("CreateMixerControllerAtPath", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                if (method == null) throw new InvalidOperationException("Unity AudioMixerController creation API is unavailable.");
                method.Invoke(null, new object[] { MixerPath });
                AssetDatabase.ImportAsset(MixerPath, ImportAssetOptions.ForceUpdate);
                mixer = AssetDatabase.LoadAssetAtPath<AudioMixer>(MixerPath);
            }

            if (mixer == null) throw new InvalidOperationException("Generated BattleRaja V1 mixer could not be loaded.");
            var controllerType = Type.GetType("UnityEditor.Audio.AudioMixerController, UnityEditor");
            var controller = AssetDatabase.LoadAllAssetsAtPath(MixerPath);
            UnityEngine.Object controllerAsset = null;
            for (var i = 0; i < controller.Length; i++)
            {
                if (controller[i] != null && controller[i].GetType().FullName == "UnityEditor.Audio.AudioMixerController")
                {
                    controllerAsset = controller[i];
                    break;
                }
            }

            if (controllerAsset != null && controllerType != null)
            {
                var groups = controllerType.GetMethod("GetAllAudioGroupsSlow", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.Invoke(controllerAsset, null) as System.Collections.IEnumerable;
                var existing = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                if (groups != null)
                {
                    foreach (var group in groups)
                    {
                        var name = group?.GetType().GetProperty("name", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(group, null) as string;
                        if (!string.IsNullOrEmpty(name)) existing.Add(name);
                    }
                }

                var createGroup = controllerType.GetMethod("CreateNewGroup", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var name in new[] { "Music", "Ambience", "UI", "Combat", "Abilities", "Gadgets", "Zone" })
                {
                    if (existing.Contains(name) || createGroup == null) continue;
                    createGroup.Invoke(controllerAsset, new object[] { name, true });
                }

                // CreateNewGroup also creates a valid group asset, but the editor
                // API does not always attach it to the master when invoked from a
                // headless generator. Wire every named bus under Master explicitly
                // so runtime FindMatchingGroups and routing are meaningful.
                var master = controllerType.GetProperty("masterGroup", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.GetValue(controllerAsset, null);
                if (master != null)
                {
                    var groupType = Type.GetType("UnityEditor.Audio.AudioMixerGroupController, UnityEditor");
                    var children = new List<object>();
                    foreach (var group in AssetDatabase.LoadAllAssetsAtPath(MixerPath))
                    {
                        if (group == null || group.GetType().FullName != "UnityEditor.Audio.AudioMixerGroupController" || ReferenceEquals(group, master)) continue;
                        var name = group.GetType().GetProperty("name", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(group, null) as string;
                        if (Array.IndexOf(new[] { "Music", "Ambience", "UI", "Combat", "Abilities", "Gadgets", "Zone" }, name) >= 0)
                        {
                            children.Add(group);
                        }
                    }

                    if (groupType != null)
                    {
                        var typedChildren = Array.CreateInstance(groupType, children.Count);
                        for (var i = 0; i < children.Count; i++) typedChildren.SetValue(children[i], i);
                        groupType.GetProperty("children", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                            ?.SetValue(master, typedChildren, null);
                    }
                }
                EditorUtility.SetDirty(controllerAsset);
            }

            AssetDatabase.SaveAssets();
        }

        private static void ClearInvalidExposedVolumeParameters()
        {
            var assets = AssetDatabase.LoadAllAssetsAtPath(MixerPath);
            UnityEngine.Object controllerAsset = null;
            for (var i = 0; i < assets.Length; i++)
            {
                if (assets[i] != null && assets[i].GetType().FullName == "UnityEditor.Audio.AudioMixerController")
                {
                    controllerAsset = assets[i];
                    break;
                }
            }

            if (controllerAsset == null) return;
            var controllerType = Type.GetType("UnityEditor.Audio.AudioMixerController, UnityEditor");
            var exposedProperty = controllerType?.GetProperty("exposedParameters", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (exposedProperty == null || !exposedProperty.PropertyType.IsArray) return;
            var elementType = exposedProperty.PropertyType.GetElementType();
            exposedProperty.SetValue(controllerAsset, Array.CreateInstance(elementType, 0), null);
            EditorUtility.SetDirty(controllerAsset);
            AssetDatabase.SaveAssets();
        }

        private readonly struct ClipSpec
        {
            public readonly string Name;
            public readonly float Duration;
            public readonly int SampleRate;
            public readonly int Channels;
            public readonly int Kind;

            public ClipSpec(string name, float duration, int sampleRate, int channels, int kind)
            {
                Name = name;
                Duration = duration;
                SampleRate = sampleRate;
                Channels = channels;
                Kind = kind;
            }
        }
    }
}
