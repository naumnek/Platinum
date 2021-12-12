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
        public GameObject prefabPlayer;
        public List<GameObject> prefabEnemys = new List<GameObject>();
        public bool load = false;
        private Transform emptyEnemys;
        private Transform m_BodyPlayer;
        private GameLogic m_GameLogic;
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

        private void Start()
        {
            instance = this;
        }

        public void MainStart()
        {
            print("Point_2");
            switch (StartVariant)
            {
                case (1):
                    start1();
                    break;
                case (2):
                    start2();
                    break;
            }
        }

        private void start1()
        {
            m_GameLogic = GameLogic.GetGameLogic();
            Transform spawn = GameObject.FindGameObjectWithTag("Spawnpoint").transform;
            emptyEnemys = gameObject.transform;
            m_GameLogic.SpawnObject(ref listEnemys, prefabPlayer, emptyEnemys, spawn.position.x, spawn.position.y, spawn.position.z);
            m_BodyPlayer = listEnemys.First();
            spawnEnemy = true;

        }
        private void start2()
        {
            m_GameLogic = GameLogic.GetGameLogic();
            Transform spawn = GameObject.FindGameObjectWithTag("Spawnpoint").transform;
            emptyEnemys = gameObject.transform;
            listEnemys.Add(GameObject.FindGameObjectWithTag("Player").transform);
            m_BodyPlayer = listEnemys.First();
            m_BodyPlayer.position = spawn.position;
            m_BodyPlayer.GetComponent<CharacterController>().enabled = true;
            spawnEnemy = true;
        }

        void Update()
        {
            if (spawnEnemy == true)
            {
                CountEnemy();
            }
        }


        private void CountEnemy()
        {
            if (listEnemys.Count < min_enemy)
            {
                LogicEnemy(2, listEnemys.Last());
            }
            if (listEnemys.Count > max_enemy)
            {
                LogicEnemy(1, listEnemys[1]);

            }
        }

        private void LogicEnemy(int id, Transform target)
        {
            switch (id)
            {
                case (1):
                    m_GameLogic.RemoveObject(ref listEnemys, target);
                    break;
                case (2):
                    GameObject enemy = prefabEnemys[ran.Next(0, prefabEnemys.Count - 1)];
                    float x = target.position.x + ran.Next(minX_enemy, maxX_enemy);
                    float y = target.position.y + ran.Next(minY_enemy, maxY_enemy);
                    float z = target.position.z + ran.Next(minZ_enemy, maxZ_enemy);
                    m_GameLogic.SpawnObject(ref listEnemys, enemy, emptyEnemys, x, y, z);
                    break;
            }
        }
    }


}
