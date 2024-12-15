using AALUND13Card.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Mods._AALUND13_Card.Scripts.MonoBehaviours {
    public class ClampHeaelthMono : MonoBehaviour {
        public CharacterData Data;
        public void Start() {
            Data = GetComponentInParent<Player>().data;
        }
        public void Update() {
            if(Data.maxHealth > Data.GetAdditionalData().MaxHealthCap) {
                Data.maxHealth = Data.GetAdditionalData().MaxHealthCap;
            }
        }
    }
}
