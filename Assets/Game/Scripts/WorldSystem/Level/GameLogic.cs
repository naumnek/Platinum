using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using System.Linq;
using Unity.FPS.Game;

namespace naumnek.FPS
{
    public class GameLogic : MonoBehaviour
    {
        [Header("Options Room")]
        public string Title = "All enemies destroyed";
        public float DelayBeforeDisplay = 0.5f;

        [Header("Other")]
        public Timer timer;
        EnemySpawner m_EnemySpawner;
        bool load = true;
        private static GameLogic instance;

        public static GameLogic GetGameLogic()
        {
            return instance.GetComponent<GameLogic>();
        }

        void Start()
        {
            instance = this;
            EventManager.AddListener<RoomMatchedEvent>(OnRoomMatched);
        }

        public void OnRoomMatched(RoomMatchedEvent evt)
        {
            DisplayMessageEvent displayMessage = Events.DisplayMessageEvent;
            displayMessage.Message = Title;
            displayMessage.DelayBeforeDisplay = DelayBeforeDisplay;
            EventManager.Broadcast(displayMessage);
        }

        public static Transform SpawnObject(GameObject prefab, float x, float y)
        {
            return Instantiate(prefab, new Vector2(x, y), Quaternion.identity).transform;
        }

        public static void SpawnObject(ref List<Transform> list, GameObject prefab, float x, float y)
        {
            list.Add(Instantiate(prefab, new Vector2(x, y), Quaternion.identity).transform);
        }

        public static Transform SpawnObject(GameObject prefab, Transform parent, float x, float y, float z)
        {
            GameObject obj = Instantiate(prefab, parent);
            obj.transform.position = new Vector3(x, y, z);
            obj.transform.SetParent(parent);
            return obj.transform;
        }

        public static void SpawnObject(ref List<Transform> list, GameObject prefab, Transform parent, string name, float x, float y)
        {
            Transform parent2 = (new GameObject(name)).transform;
            parent2.position = new Vector2(x, y);
            Transform obj = Instantiate(prefab, new Vector2(0, 0), Quaternion.identity).transform;
            obj.SetParent(parent2, false);
            parent2.transform.SetParent(parent, false);
            list.Add(parent2);
        }

        public static void RemoveObject(ref List<Transform> list, Transform target)
        {
            list.Remove(target);
            Destroy(target.gameObject);
        }
    }
}
