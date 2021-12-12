using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using System.Linq;

namespace naumnek.FPS
{
    public class GameLogic : MonoBehaviour
    {
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
        }

        public Transform SpawnObject(GameObject prefab, float x, float y)
        {
            return Instantiate(prefab, new Vector2(x, y), Quaternion.identity).transform;
        }

        public void SpawnObject(ref List<Transform> list, GameObject prefab, float x, float y)
        {
            list.Add(Instantiate(prefab, new Vector2(x, y), Quaternion.identity).transform);
        }

        public void SpawnObject(ref List<Transform> list, GameObject prefab, Transform parent, float x, float y, float z)
        {
            GameObject obj = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity);
            obj.transform.SetParent(parent, false);
            list.Add(obj.transform);
        }

        public void SpawnObject(ref List<Transform> list, GameObject prefab, Transform parent, string name, float x, float y)
        {
            Transform parent2 = (new GameObject(name)).transform;
            parent2.position = new Vector2(x, y);
            Transform obj = Instantiate(prefab, new Vector2(0, 0), Quaternion.identity).transform;
            obj.SetParent(parent2, false);
            parent2.transform.SetParent(parent, false);
            list.Add(parent2);
        }

        public void RemoveObject(ref List<Transform> list, Transform target)
        {
            list.Remove(target);
            Destroy(target.gameObject);
        }
    }
}
