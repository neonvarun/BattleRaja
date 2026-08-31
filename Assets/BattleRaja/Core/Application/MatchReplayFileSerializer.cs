using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using BattleRaja.Core.Domain;

namespace BattleRaja.Core.Application
{
    /// <summary>
    /// Versioned, Unity-independent persistence for deterministic match replays.
    /// The envelope carries a payload length and SHA-256 so interrupted or
    /// truncated diagnostics are rejected instead of being replayed silently.
    /// </summary>
    public static class MatchReplayFileSerializer
    {
        private const int LegacyFormatVersion = 1;
        private const int FormatVersion = 2;
        private const int MaximumCollectionCount = 1_000_000;
        private static readonly byte[] Magic = Encoding.ASCII.GetBytes("BRR1");

        public static void Write(MatchReplayFile replay, string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("A replay path is required.", nameof(path));
            var bytes = Serialize(replay);
            var fullPath = Path.GetFullPath(path);
            var directory = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);

            using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush(true);
            }
        }

        public static MatchReplayFile Read(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("A replay path is required.", nameof(path));
            return Deserialize(File.ReadAllBytes(path));
        }

        public static byte[] Serialize(MatchReplayFile replay)
        {
            ValidateReplay(replay);
            byte[] payload;
            using (var payloadStream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(payloadStream, Encoding.UTF8, true))
                {
                    WriteReplayPayload(writer, replay);
                }

                payload = payloadStream.ToArray();
            }

            byte[] digest;
            using (var sha = SHA256.Create()) digest = sha.ComputeHash(payload);

            using (var output = new MemoryStream(Magic.Length + sizeof(int) * 2 + payload.Length + digest.Length))
            {
                using (var writer = new BinaryWriter(output, Encoding.UTF8, true))
                {
                    writer.Write(Magic);
                    writer.Write(FormatVersion);
                    writer.Write(payload.Length);
                    writer.Write(payload);
                    writer.Write(digest);
                }

                return output.ToArray();
            }
        }

        public static MatchReplayFile Deserialize(byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            try
            {
                using (var input = new MemoryStream(bytes, false))
                using (var reader = new BinaryReader(input, Encoding.UTF8, true))
                {
                    var magic = reader.ReadBytes(Magic.Length);
                    if (!BytesEqual(Magic, magic)) throw new InvalidDataException("Replay magic is invalid.");
                    var formatVersion = reader.ReadInt32();
                    if (formatVersion != LegacyFormatVersion && formatVersion != FormatVersion)
                    {
                        throw new InvalidDataException("Replay format version is unsupported.");
                    }

                    var payloadLength = reader.ReadInt32();
                    if (payloadLength < 0 || payloadLength > input.Length - input.Position - 32)
                    {
                        throw new InvalidDataException("Replay payload length is invalid.");
                    }

                    var payload = reader.ReadBytes(payloadLength);
                    var expectedDigest = reader.ReadBytes(32);
                    if (payload.Length != payloadLength || expectedDigest.Length != 32 || input.Position != input.Length)
                    {
                        throw new InvalidDataException("Replay envelope is truncated or has trailing bytes.");
                    }

                    byte[] actualDigest;
                    using (var sha = SHA256.Create()) actualDigest = sha.ComputeHash(payload);
                    if (!BytesEqual(expectedDigest, actualDigest))
                    {
                        throw new InvalidDataException("Replay payload checksum does not match.");
                    }

                    using (var payloadStream = new MemoryStream(payload, false))
                    using (var payloadReader = new BinaryReader(payloadStream, Encoding.UTF8, true))
                    {
                        var replay = ReadReplayPayload(payloadReader, formatVersion >= FormatVersion);
                        if (payloadStream.Position != payloadStream.Length)
                        {
                            throw new InvalidDataException("Replay payload has trailing bytes.");
                        }

                        return replay;
                    }
                }
            }
            catch (EndOfStreamException exception)
            {
                throw new InvalidDataException("Replay data is truncated.", exception);
            }
            catch (ArgumentException exception)
            {
                throw new InvalidDataException("Replay data contains an invalid value.", exception);
            }
        }

        private static void ValidateReplay(MatchReplayFile replay)
        {
            if (replay == null) throw new ArgumentNullException(nameof(replay));
            if (replay.Frames == null || replay.TickStateHashes == null ||
                replay.TickStateSnapshots == null ||
                replay.Frames.Count != replay.TickStateHashes.Count ||
                replay.Frames.Count != replay.TickStateSnapshots.Count)
            {
                throw new InvalidDataException("Replay frames and tick hashes must have equal lengths.");
            }

            if (replay.Frames.Count > MaximumCollectionCount)
            {
                throw new InvalidDataException("Replay contains too many frames.");
            }
        }

        private static void WriteReplayPayload(BinaryWriter writer, MatchReplayFile replay)
        {
            var header = replay.Header;
            WriteString(writer, header.ArenaVersion);
            writer.Write(header.MatchSeed);
            writer.Write(header.FixedDeltaSeconds);
            writer.Write((int)header.Scenario);
            writer.Write(header.IncludesBastionState);

            WriteArrayCount(writer, header.Spawns.Length);
            for (var i = 0; i < header.Spawns.Length; i++) WriteSpawn(writer, header.Spawns[i]);

            WriteArrayCount(writer, header.Participants.Length);
            for (var i = 0; i < header.Participants.Length; i++) WriteParticipant(writer, header.Participants[i]);

            WriteArrayCount(writer, header.Pickups.Length);
            for (var i = 0; i < header.Pickups.Length; i++) WritePickup(writer, header.Pickups[i]);

            WriteArrayCount(writer, header.GadgetPickups.Length);
            for (var i = 0; i < header.GadgetPickups.Length; i++) WriteGadgetPickup(writer, header.GadgetPickups[i]);

            WriteArrayCount(writer, replay.Frames.Count);
            for (var i = 0; i < replay.Frames.Count; i++)
            {
                WriteFrame(writer, replay.Frames[i]);
                writer.Write(replay.TickStateHashes[i]);
                WriteSnapshots(writer, replay.TickStateSnapshots[i]);
            }
        }

        private static MatchReplayFile ReadReplayPayload(BinaryReader reader, bool hasBastionState)
        {
            var arenaVersion = ReadString(reader);
            var matchSeed = reader.ReadUInt32();
            var fixedDeltaSeconds = reader.ReadSingle();
            var scenario = (MatchReplayScenario)reader.ReadInt32();
            var includesBastionState = hasBastionState && reader.ReadBoolean();

            var spawns = new MatchSpawn[ReadArrayCount(reader)];
            for (var i = 0; i < spawns.Length; i++) spawns[i] = ReadSpawn(reader);

            var participants = new MatchReplayParticipant[ReadArrayCount(reader)];
            for (var i = 0; i < participants.Length; i++) participants[i] = ReadParticipant(reader);

            var pickups = new MatchPickupDefinition[ReadArrayCount(reader)];
            for (var i = 0; i < pickups.Length; i++) pickups[i] = ReadPickup(reader);

            var gadgetPickups = new GadgetPickupDefinition[ReadArrayCount(reader)];
            for (var i = 0; i < gadgetPickups.Length; i++) gadgetPickups[i] = ReadGadgetPickup(reader);

            var header = new MatchReplayHeader(
                arenaVersion,
                matchSeed,
                spawns,
                fixedDeltaSeconds,
                scenario,
                participants,
                pickups,
                gadgetPickups,
                includesBastionState);
            var replay = new MatchReplayFile(header);
            var frameCount = ReadArrayCount(reader);
            for (var i = 0; i < frameCount; i++)
            {
                var frame = ReadFrame(reader);
                var hash = reader.ReadUInt64();
                replay.AddFrame(frame, hash, ReadSnapshots(reader));
            }

            return replay;
        }

        private static void WriteSpawn(BinaryWriter writer, MatchSpawn spawn)
        {
            writer.Write(spawn.Id.Value);
            WriteFloat2(writer, spawn.Position);
            writer.Write(spawn.MaxHealth);
        }

        private static MatchSpawn ReadSpawn(BinaryReader reader) => new MatchSpawn(
            new CombatEntityId(reader.ReadInt32()),
            ReadFloat2(reader),
            reader.ReadInt32());

        private static void WriteParticipant(BinaryWriter writer, MatchReplayParticipant participant)
        {
            writer.Write(participant.ActorId.Value);
            writer.Write((int)participant.Faction);
            WriteWeapon(writer, participant.Weapon);
            WriteMovementTuning(writer, participant.Movement);
            WriteContentId(writer, participant.FighterId);
            writer.Write(participant.TickRate);
        }

        private static MatchReplayParticipant ReadParticipant(BinaryReader reader) => new MatchReplayParticipant(
            new CombatEntityId(reader.ReadInt32()),
            (CombatFaction)reader.ReadInt32(),
            ReadWeapon(reader),
            ReadMovementTuning(reader),
            ReadContentId(reader),
            reader.ReadInt32());

        private static void WriteWeapon(BinaryWriter writer, ProjectileWeaponDefinition weapon)
        {
            writer.Write(weapon.Damage);
            writer.Write(weapon.FireIntervalSeconds);
            writer.Write(weapon.ProjectileSpeed);
            writer.Write(weapon.MaxRange);
            writer.Write(weapon.LifetimeSeconds);
            writer.Write(weapon.Radius);
            writer.Write(weapon.CollisionLayerMask);
            writer.Write(weapon.AllowSelfHit);
            writer.Write(weapon.AllowFriendlyFire);
        }

        private static ProjectileWeaponDefinition ReadWeapon(BinaryReader reader) => new ProjectileWeaponDefinition(
            reader.ReadInt32(),
            reader.ReadSingle(),
            reader.ReadSingle(),
            reader.ReadSingle(),
            reader.ReadSingle(),
            reader.ReadSingle(),
            reader.ReadInt32(),
            reader.ReadBoolean(),
            reader.ReadBoolean());

        private static void WriteMovementTuning(BinaryWriter writer, MovementTuning tuning)
        {
            writer.Write(tuning.MaxSpeed);
            writer.Write(tuning.Acceleration);
            writer.Write(tuning.Deceleration);
            writer.Write(tuning.RotationSpeed);
            writer.Write(tuning.MovementDeadZone);
            writer.Write(tuning.AimDeadZone);
            writer.Write(tuning.InputSensitivity);
        }

        private static MovementTuning ReadMovementTuning(BinaryReader reader) => new MovementTuning(
            reader.ReadSingle(),
            reader.ReadSingle(),
            reader.ReadSingle(),
            reader.ReadSingle(),
            reader.ReadSingle(),
            reader.ReadSingle(),
            reader.ReadSingle());

        private static void WritePickup(BinaryWriter writer, MatchPickupDefinition pickup)
        {
            writer.Write(pickup.PickupId);
            writer.Write((int)pickup.Kind);
            writer.Write(pickup.Value);
            writer.Write(pickup.RespawnSeconds);
            WriteFloat2(writer, pickup.Position);
            writer.Write(pickup.CollectionRadius);
        }

        private static MatchPickupDefinition ReadPickup(BinaryReader reader) => new MatchPickupDefinition(
            reader.ReadInt32(),
            (MatchPickupKind)reader.ReadInt32(),
            reader.ReadInt32(),
            reader.ReadSingle(),
            ReadFloat2(reader),
            reader.ReadSingle());

        private static void WriteGadgetPickup(BinaryWriter writer, GadgetPickupDefinition pickup)
        {
            writer.Write(pickup.PickupId);
            WriteContentId(writer, pickup.GadgetId);
            WriteFloat2(writer, pickup.Position);
            writer.Write(pickup.CollectionRadius);
        }

        private static GadgetPickupDefinition ReadGadgetPickup(BinaryReader reader) => new GadgetPickupDefinition(
            reader.ReadInt32(),
            ReadContentId(reader),
            ReadFloat2(reader),
            reader.ReadSingle());

        private static void WriteFrame(BinaryWriter writer, MatchReplayFrame frame)
        {
            writer.Write(frame.SimulationTick);
            WriteArrayCount(writer, frame.MovementCommands.Length);
            for (var i = 0; i < frame.MovementCommands.Length; i++) WriteMovementCommand(writer, frame.MovementCommands[i]);

            WriteArrayCount(writer, frame.AttackCommands.Length);
            for (var i = 0; i < frame.AttackCommands.Length; i++) WriteAttackCommand(writer, frame.AttackCommands[i]);

            WriteArrayCount(writer, frame.AbilityCommands.Length);
            for (var i = 0; i < frame.AbilityCommands.Length; i++) WriteAbilityCommand(writer, frame.AbilityCommands[i]);

            WriteArrayCount(writer, frame.GadgetCommands.Length);
            for (var i = 0; i < frame.GadgetCommands.Length; i++) WriteGadgetCommand(writer, frame.GadgetCommands[i]);

            WriteArrayCount(writer, frame.CommandOrder.Length);
            for (var i = 0; i < frame.CommandOrder.Length; i++)
            {
                writer.Write((int)frame.CommandOrder[i].Kind);
                writer.Write(frame.CommandOrder[i].Index);
            }
        }

        private static MatchReplayFrame ReadFrame(BinaryReader reader)
        {
            var simulationTick = reader.ReadInt32();
            var movements = new MovementCommand[ReadArrayCount(reader)];
            for (var i = 0; i < movements.Length; i++) movements[i] = ReadMovementCommand(reader);

            var attacks = new AttackCommand[ReadArrayCount(reader)];
            for (var i = 0; i < attacks.Length; i++) attacks[i] = ReadAttackCommand(reader);

            var abilities = new MatchReplayAbilityCommand[ReadArrayCount(reader)];
            for (var i = 0; i < abilities.Length; i++) abilities[i] = ReadAbilityCommand(reader);

            var gadgets = new GadgetUseCommand[ReadArrayCount(reader)];
            for (var i = 0; i < gadgets.Length; i++) gadgets[i] = ReadGadgetCommand(reader);

            var commandOrder = new MatchReplayCommandOrder[ReadArrayCount(reader)];
            for (var i = 0; i < commandOrder.Length; i++)
            {
                commandOrder[i] = new MatchReplayCommandOrder(
                    (MatchReplayCommandKind)reader.ReadInt32(),
                    reader.ReadInt32());
            }

            return new MatchReplayFrame(simulationTick, movements, attacks, abilities, gadgets, commandOrder);
        }

        private static void WriteMovementCommand(BinaryWriter writer, MovementCommand command)
        {
            writer.Write(command.ActorId);
            writer.Write(command.SimulationTick);
            WriteFloat2(writer, command.Movement);
            WriteFloat2(writer, command.Aim);
        }

        private static MovementCommand ReadMovementCommand(BinaryReader reader) => new MovementCommand(
            reader.ReadInt32(),
            reader.ReadInt32(),
            ReadFloat2(reader),
            ReadFloat2(reader));

        private static void WriteAttackCommand(BinaryWriter writer, AttackCommand command)
        {
            writer.Write(command.InstigatorId.Value);
            writer.Write(command.SimulationTick);
            WriteFloat2(writer, command.Origin);
            WriteFloat2(writer, command.Direction);
            writer.Write(command.Pressed);
            writer.Write(command.InputSequence);
        }

        private static AttackCommand ReadAttackCommand(BinaryReader reader) => new AttackCommand(
            new CombatEntityId(reader.ReadInt32()),
            reader.ReadInt32(),
            ReadFloat2(reader),
            ReadFloat2(reader),
            reader.ReadBoolean(),
            reader.ReadInt32());

        private static void WriteAbilityCommand(BinaryWriter writer, MatchReplayAbilityCommand recorded)
        {
            var command = recorded.Command;
            writer.Write(command.InstigatorId.Value);
            writer.Write(command.SimulationTick);
            WriteContentId(writer, command.AbilityId);
            WriteFloat2(writer, command.RequestedDirection);
            writer.Write(command.Pressed);
            WriteFloat2(writer, recorded.Movement);
            WriteFloat2(writer, recorded.Facing);
            writer.Write(recorded.SpawnDecoy);
            WriteFloat2(writer, recorded.DecoyPosition);
        }

        private static MatchReplayAbilityCommand ReadAbilityCommand(BinaryReader reader)
        {
            var command = new AbilityCommand(
                new CombatEntityId(reader.ReadInt32()),
                reader.ReadInt32(),
                ReadContentId(reader),
                ReadFloat2(reader),
                reader.ReadBoolean());
            return new MatchReplayAbilityCommand(
                command,
                ReadFloat2(reader),
                ReadFloat2(reader),
                reader.ReadBoolean(),
                ReadFloat2(reader));
        }

        private static void WriteGadgetCommand(BinaryWriter writer, GadgetUseCommand command)
        {
            writer.Write(command.UserId.Value);
            WriteContentId(writer, command.GadgetId);
            WriteFloat2(writer, command.Origin);
            WriteFloat2(writer, command.Direction);
            writer.Write(command.Tick);
        }

        private static GadgetUseCommand ReadGadgetCommand(BinaryReader reader) => new GadgetUseCommand(
            new CombatEntityId(reader.ReadInt32()),
            ReadContentId(reader),
            ReadFloat2(reader),
            ReadFloat2(reader),
            reader.ReadInt32());

        private static void WriteSnapshots(BinaryWriter writer, MatchParticipantSnapshot[] snapshots)
        {
            snapshots = snapshots ?? Array.Empty<MatchParticipantSnapshot>();
            WriteArrayCount(writer, snapshots.Length);
            for (var i = 0; i < snapshots.Length; i++)
            {
                var snapshot = snapshots[i];
                writer.Write(snapshot.Id.Value);
                WriteFloat2(writer, snapshot.Position);
                writer.Write(snapshot.CurrentHealth);
                writer.Write(snapshot.MaxHealth);
                writer.Write(snapshot.Alive);
                writer.Write(snapshot.Placement);
                writer.Write(snapshot.Eliminations);
                writer.Write(snapshot.DamageDealt);
                writer.Write(snapshot.Assists);
                writer.Write(snapshot.SurvivalTimeSeconds);
            }
        }

        private static MatchParticipantSnapshot[] ReadSnapshots(BinaryReader reader)
        {
            var snapshots = new MatchParticipantSnapshot[ReadArrayCount(reader)];
            for (var i = 0; i < snapshots.Length; i++)
            {
                snapshots[i] = new MatchParticipantSnapshot(
                    new CombatEntityId(reader.ReadInt32()),
                    ReadFloat2(reader),
                    reader.ReadInt32(),
                    reader.ReadInt32(),
                    reader.ReadBoolean(),
                    reader.ReadInt32(),
                    reader.ReadInt32(),
                    reader.ReadInt32(),
                    reader.ReadInt32(),
                    reader.ReadSingle());
            }

            return snapshots;
        }

        private static void WriteContentId(BinaryWriter writer, ContentId value)
        {
            writer.Write((int)value.Kind);
            WriteString(writer, value.Value);
        }

        private static ContentId ReadContentId(BinaryReader reader) => new ContentId(
            (ContentIdKind)reader.ReadInt32(),
            ReadString(reader));

        private static void WriteFloat2(BinaryWriter writer, Float2 value)
        {
            writer.Write(value.X);
            writer.Write(value.Y);
        }

        private static Float2 ReadFloat2(BinaryReader reader) => new Float2(reader.ReadSingle(), reader.ReadSingle());

        private static void WriteString(BinaryWriter writer, string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value ?? string.Empty);
            writer.Write(bytes.Length);
            writer.Write(bytes);
        }

        private static string ReadString(BinaryReader reader)
        {
            var length = reader.ReadInt32();
            if (length < 0 || length > MaximumCollectionCount * 16)
            {
                throw new InvalidDataException("Replay string length is invalid.");
            }

            var bytes = reader.ReadBytes(length);
            if (bytes.Length != length) throw new InvalidDataException("Replay string is truncated.");
            return Encoding.UTF8.GetString(bytes);
        }

        private static void WriteArrayCount(BinaryWriter writer, int count)
        {
            if (count < 0 || count > MaximumCollectionCount) throw new InvalidDataException("Replay collection length is invalid.");
            writer.Write(count);
        }

        private static int ReadArrayCount(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            if (count < 0 || count > MaximumCollectionCount)
            {
                throw new InvalidDataException("Replay collection length is invalid.");
            }

            return count;
        }

        private static bool BytesEqual(byte[] left, byte[] right)
        {
            if (left == null || right == null || left.Length != right.Length) return false;
            var equal = true;
            for (var i = 0; i < left.Length; i++) equal &= left[i] == right[i];
            return equal;
        }
    }
}
