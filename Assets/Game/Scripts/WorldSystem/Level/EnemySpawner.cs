using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using LowLevelGenerator.Scripts;

namespace naumnek.FPS
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("General")]
        public int StartVariant = 1;
        public Transform EnemyContainer;
        public GameObject prefabPlayer;
        public List<GameObject> prefabEnemys = new List<GameObject>();
        public Transform emptyPlayer;
        public Transform m_BodyPlayer;
        public bool load = false;
        [Header("Enemys Options")]
        public bool spawnEnemy = false;
        public int min_enemy = 1;
        public int max_enemy = 3;
        public int minX_enemy = 1;
        public int maxX_enemy = 8;
        public int minY_enemy = 1;
        public int maxY_enemy = 1;
        public int minZ_enemy = 1;
        public int maxZ_enemy = 8;
        [Header("Enemys List")]
        [SerializeField]
        List<Transform> listEnemys = new List<Transform>();
        private System.Random ran = new System.Random();
        private static EnemySpawner instance;

        public static EnemySpawner GetEnemySpawner()
        {
            return instance.GetComponent<EnemySpawner>();
        }
        public static void InitializePlayer(Transform point)
        {
            instance.SetSpawnerPlayer(point);
        }


        public static Transform ActivateSpawner(Transform spawner)
        {
            return instance.SpawnEnemys(spawner);
        }
        public void SetSpawnerPlayer(Transform point)
        {
            listEnemys.Add(m_BodyPlayer);
            m_BodyPlayer.position = point.position;
            m_BodyPlayer.SetParent(EnemyContainer);
            m_BodyPlayer.gameObject.SetActive(true);
            spawnEnemy = true;
        }

        private Transform SpawnEnemys(Transform spawner)
        {
            GameObject enemy = prefabEnemys[ran.Next(0, prefabEnemys.Count - 1)];
            float x = spawner.position.x + ran.Next(minX_enemy, maxX_enemy);
            float y = spawner.position.y + ran.Next(minY_enemy, maxY_enemy);
            float z = spawner.position.z + ran.Next(minZ_enemy, maxZ_enemy);
            //print("Spawn: " + enemy.name + " - " + target.name + " - " + x + " - " + y + " - " + z + " - )");
            Transform obj = GameLogic.SpawnObject(enemy, spawner, x, y, z);
            listEnemys.Add(obj);
            return obj;
        }

        void RemoveEnemy(Transform target)
        {
            GameLogic.RemoveObject(ref listEnemys, target);
        }

        void Awake()
        {
            instance = this;
        }

        public static void MainStart()
        {
            switch (instance.StartVariant)
            {
                case (1):
                    instance.start1();
                    break;
                case (2):
                    instance.start2();
                    break;
            }
        }

        private void start1()
        {
            listEnemys.Add(GameLogic.SpawnObject(prefabPlayer, EnemyContainer, emptyPlayer.position.x, emptyPlayer.position.y, emptyPlayer.position.z));
            m_BodyPlayer = listEnemys.First();
            spawnEnemy = true;

        }
        private void start2()
        {
            LoadManager.Load();
        }
    }


}
