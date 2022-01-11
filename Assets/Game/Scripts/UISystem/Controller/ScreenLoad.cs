using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace naumnek.FPS
{
    public class ScreenLoad : MonoBehaviour
    {
        public FileManager fileManager;

        public void OnAnimationOver(string v)
        {
            if (fileManager == null) fileManager = FileManager.GetFileManager();
            switch (v)
            {
                case ("Unvisibily"):
                    fileManager.EndLoadScene();
                    break;
                case ("Visibily"):
                    fileManager.StartLoadScene();
                    break;
            }
        }
    }
}
