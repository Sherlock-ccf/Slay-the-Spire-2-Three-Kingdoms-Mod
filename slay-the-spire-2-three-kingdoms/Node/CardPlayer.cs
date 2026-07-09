using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace slay_the_spire_2_three_kingdoms.Node;

public static class CardPlayer
{
    public static void PlayCardSfx(string path)
    {
        if (NonInteractiveMode.IsActive) return;
        AudioStream stream;
        try
        {
            stream = PreloadManager.Cache.GetAsset<AudioStream>(path);
        }
        catch
        {
            GD.PrintErr($"[SFX] Could not load audio: {path}");
            return;
        }

        var audioPlayer = new AudioStreamPlayer();
        audioPlayer.Stream = stream;
        audioPlayer.Bus = "SFX";
        audioPlayer.Finished += () => audioPlayer.QueueFree();
        var combatRoom = NCombatRoom.Instance;
        if (combatRoom != null)
        {
            combatRoom.AddChild(audioPlayer);
            audioPlayer.Play();
        }
        else
        {
            audioPlayer.QueueFree();
        }
    }
}