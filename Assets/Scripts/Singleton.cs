using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Singleton.Controller{
    /// \class
    /// \author Luke161 (Github)
    /// <summary>
    /// Singleton base class
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static T _instance;

        /// <summary>
        /// Singleton lock
        /// </summary>
        private static readonly object _instanceLock = new object();

        /// <summary>
        /// Application quit flag
        /// </summary>
        private static bool _quitting = false;
        
        /// <summary>
        /// Instance getter
        /// </summary>
        public static T instance {
            get {
                lock(_instanceLock){
                    if(_instance==null && !_quitting){

                        _instance = GameObject.FindObjectOfType<T>();
                        if(_instance==null){
                            GameObject go = new GameObject(typeof(T).ToString());
                            _instance = go.AddComponent<T>();

                            DontDestroyOnLoad(_instance.gameObject);
                        }
                    }

                    return _instance;
                }
            }
        }

        /// <summary>
        /// Unity Awake
        /// </summary>
        protected virtual void Awake()
        {
            if(_instance==null) _instance = gameObject.GetComponent<T>();
            else if(_instance.GetInstanceID()!=GetInstanceID()){
                Destroy(gameObject);
                throw new System.Exception(string.Format("Instance of {0} already exists, removing {1}",GetType().FullName,ToString()));
            }
        }

        /// <summary>
        /// On application quit
        /// </summary>
        protected virtual void OnApplicationQuit() 
        {
            _quitting = true;
        }
    }
}