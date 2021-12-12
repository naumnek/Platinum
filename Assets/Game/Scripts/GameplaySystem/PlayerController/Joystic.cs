using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace naumnek.FPS
{
    public class Joystic : MonoBehaviour
    {
        public bool mobile = false;
        public GameObject body_player;
        public GameObject controller;
        public GameObject marker; //интересующий нас обьект
        public GameObject marker_point; //точка проверки поворота
        private Transform targetB; //точка проверки поворота
        public int marker_radius = 100;
        public int point_marker_radius = 200;
        public float bodyRotate = 1f;
        //public VariableJoystick variableJoystick;

        void Start()
        {
            targetB = marker_point.transform;
            marker_radius = marker_radius * (int)controller.transform.localScale.x;
            point_marker_radius = point_marker_radius * (int)controller.transform.localScale.x;
        }

        void Update()
        {
            MarketMove();
            BodyRotate();
        }

        void MarketMove()
        {
            if (mobile == false)
            {
                if (Input.GetMouseButton(0)) //проверяем нажатие на экран
                {
                    Vector3 touch_pos = Input.mousePosition;
                    Vector3 marker_vector = touch_pos - marker.transform.position; //локальная переменная для записи координат касания
                    Vector3 point_marker_vector = touch_pos - marker_point.transform.position; //локальная переменная для записи координат касания
                    if (point_marker_vector.magnitude < point_marker_radius & marker_vector.magnitude < marker_radius) marker.transform.position = touch_pos;
                    MarketRotate();
                }
                else
                {
                    marker.transform.position = marker_point.transform.position;
                    marker_point.transform.rotation = marker.transform.rotation;
                }
            }
            else
            {
                if (Input.touchCount > 0) //проверяем нажатие на экран
                {
                    Vector3 touch_pos = Input.GetTouch(0).position;
                    Vector3 marker_vector = touch_pos - marker.transform.position; //локальная переменная для записи координат касания
                    Vector3 point_marker_vector = touch_pos - marker_point.transform.position; //локальная переменная для записи координат касания
                    if (point_marker_vector.magnitude < point_marker_radius & marker_vector.magnitude < marker_radius) marker.transform.position = touch_pos;
                    MarketRotate();
                }
                else
                {
                    marker.transform.position = marker_point.transform.position;
                    marker_point.transform.rotation = marker.transform.rotation;
                }
            }
        }

        void MarketRotate()
        {
            marker_point.transform.LookAt(marker.transform);
        }

        void BodyRotate()
        {
            body_player.transform.rotation = Quaternion.Slerp(body_player.transform.rotation, marker_point.transform.rotation, Time.deltaTime * bodyRotate); //поворачиваем персонажа плавно по кординатам поворотом movePoint                                                                                                                                                                                               //print(marker_point.transform.localEulerAngles.x.ToString());
        }
    }

}
