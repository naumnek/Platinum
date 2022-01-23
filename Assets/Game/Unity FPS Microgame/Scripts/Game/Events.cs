using UnityEngine;

namespace Unity.FPS.Game
{
    // The Game Events used across the Game.
    // Anytime there is a need for a new event, it should be added here.

    public static class Events
    {
        public static ObjectiveUpdateEvent ObjectiveUpdateEvent = new ObjectiveUpdateEvent();
        public static AllObjectivesCompletedEvent AllObjectivesCompletedEvent = new AllObjectivesCompletedEvent();
        public static GamePauseEvent GamePauseEvent = new GamePauseEvent();
        public static SwitchMusicEvent SwitchMusicEvent = new SwitchMusicEvent();
        public static StartGenerationEvent StartGenerationEvent = new StartGenerationEvent();
        public static EndGenerationEvent EndGenerationEvent = new EndGenerationEvent();
        public static ExitMenuEvent ExitMenuEvent = new ExitMenuEvent();
        public static GameOverEvent GameOverEvent = new GameOverEvent();
        public static PlayerDeathEvent PlayerDeathEvent = new PlayerDeathEvent();
        public static RoomMatchedEvent RoomMatchedEvent = new RoomMatchedEvent();
        public static EnemyKillEvent EnemyKillEvent = new EnemyKillEvent();
        public static PickupEvent PickupEvent = new PickupEvent();
        public static AmmoPickupEvent AmmoPickupEvent = new AmmoPickupEvent();
        public static DamageEvent DamageEvent = new DamageEvent();
        public static DisplayMessageEvent DisplayMessageEvent = new DisplayMessageEvent();
    }

    public class ObjectiveUpdateEvent : GameEvent
    {
        public Objective Objective;
        public string DescriptionText;
        public string CounterText;
        public bool IsComplete;
        public string NotificationText;
    }

    public class AllObjectivesCompletedEvent : GameEvent { }

    public class GameOverEvent : GameEvent
    {
        public bool Win;
    }

    public class SwitchMusicEvent : GameEvent
    {
        public AudioClip Music;
        public string SwitchMusic;
    }

    public class GamePauseEvent : GameEvent
    {
        public bool Pause;
    }

    public class ExitMenuEvent : GameEvent { }

    public class StartGenerationEvent : GameEvent
    {
        public int Seed;
    }

    public class EndGenerationEvent : GameEvent { }

    public class PlayerDeathEvent : GameEvent { }

    public class RoomMatchedEvent : GameEvent { }

    public class EnemyKillEvent : GameEvent
    {
        public GameObject Enemy;
        public int RemainingEnemyCount;
    }

    public class PickupEvent : GameEvent
    {
        public GameObject Pickup;
    }

    public class AmmoPickupEvent : GameEvent
    {
        public WeaponController Weapon;
    }

    public class DamageEvent : GameEvent
    {
        public GameObject Sender;
        public float DamageValue;
    }

    public class DisplayMessageEvent : GameEvent
    {
        public string Message;
        public float DelayBeforeDisplay;
    }
}
