using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AALUND13Cards.Utils {
    public class PhotonPrefabPool : MonoBehaviour {
        public List<GameObject> Prefabs = new List<GameObject>();

        public void RegisterPrefabs() {
            foreach(var prefab in Prefabs) {
                if(prefab != null) {
                    PhotonNetwork.PrefabPool.RegisterPrefab(prefab.name, prefab);
                    LoggerUtils.LogInfo($"Registered prefab: {prefab.name}");
                }
            }
        }
    }
}
