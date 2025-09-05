using AALUND13Cards.Extensions;
using AALUND13Cards.Handlers;
using JARL.Armor;
using RarityLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnboundLib;
using UnityEditor.PackageManager;
using UnityEngine;

namespace AALUND13Cards.Cards {
    public abstract class CustomStatModifers : MonoBehaviour {
        public abstract void Apply(Player player);
        public virtual void OnReassign(Player player) { }
    }
}
