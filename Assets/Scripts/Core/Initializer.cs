using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyPuzzle
{
    public class Initializer : MonoBehaviour
    {
        private static Initializer instance = null;
        public  static Initializer Instance { get { return instance; } }
        
        public bool InitDone { get; private set; }
        
        private List<Action>    initFuncs;
        private bool            initPaused;
        
        public void Awake()
        {
            instance = this;
        }
        
        public IEnumerator Start()
        {
            InitDone = false;
            
            // 注册需要初始化的模块
            registerInitModules();
            
            // 初始化各模块
            yield return StartCoroutine(init());

            // 切换场景
            //MissionManager.Instance.Init();
            //MissionStage.Instance.ChangeStage();
        }
        
        public void PauseInit()
        {
            initPaused = true;
        }
        
        public void ContinueInit()
        {
            initPaused = false;
        }
        
        // 注册需要初始化的模块
        private void registerInitModules()
        {
            initPaused = false;
            initFuncs = new List<Action>()
            {
                initConfigs,
                //GameDataManager.Instance.Init,
                //GameWorld.Instance.Init,
            };
        }
        
        // 进行各模块的初始化
        private IEnumerator init()
        {
            int initIndex = 0;
            while (initIndex < initFuncs.Count)
            {
                initFuncs[initIndex++]();
                while (initPaused)
                    yield return null;
            }
        }
        
        private void initConfigs()
        {
            var configs = Resources.LoadAll("ConfigFiles");
            foreach (TextAsset config in configs)
            {
                switch (config.name)
                {
                default:
                    //ConfigManager.Instance.CacheConfigContent(config.name, config.text);
                    break;
                }
            }
        }
        
        // End of class
    }
}

