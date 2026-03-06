using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AALUND13Cards.Standard.Handler {
    /// <summary>
    /// Basically the same as "Gun.ShootPojectileAction" but this will work for ALL guns the player have
    /// </summary>
    public class PlayerGunActions {
        public static Dictionary<Player, Action<Gun, GameObject>> OnGunShootAction = new Dictionary<Player, Action<Gun, GameObject>>();

        public static void AddShootAction(Player player, Action<Gun, GameObject> action) {
            if(!OnGunShootAction.ContainsKey(player)) OnGunShootAction.Add(player, action);
            else OnGunShootAction[player] += action;
        }

        public static void RemoveShootAction(Player player, Action<Gun, GameObject> action) {
            if(OnGunShootAction.ContainsKey(player)) OnGunShootAction[player] -= action;
        }

        internal static void InvokeShootAction(Player player, Gun gun, GameObject projectile) {
            if(!OnGunShootAction.ContainsKey(player)) OnGunShootAction.Add(player, (Action<Gun, GameObject>)null);
            
            OnGunShootAction[player]?.Invoke(gun, projectile);
        }
    }
}
