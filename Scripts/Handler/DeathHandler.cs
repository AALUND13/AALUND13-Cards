using AALUND13Card.MonoBehaviours;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AALUND13Card.Handler {
    [Serializable]
    public class LastDamagingPlayer {
        public int playerID;
        public float timeSinceDamage;

        public LastDamagingPlayer(int playerID, float timeSinceDamage) {
            this.playerID = playerID;
            this.timeSinceDamage = timeSinceDamage;
        }

        // Method to convert LastDamagingPlayer to Vector2
        public Vector2 ToVector2() {
            return new Vector2(playerID, timeSinceDamage);
        }

        // Method to convert Vector2 to LastDamagingPlayer
        public static LastDamagingPlayer FromVector2(Vector2 vector) {
            return new LastDamagingPlayer((int)vector.x, vector.y);
        }
    }

    public static class Vector2Extensions {
        // Extension method to convert Vector2 to LastDamagingPlayer
        public static LastDamagingPlayer ToLastDamagingPlayer(this Vector2 vector) {
            return LastDamagingPlayer.FromVector2(vector);
        }

        // Extension method to convert LastDamagingPlayer to Vector2
        public static Vector2 ToVector2(this LastDamagingPlayer player) {
            return player.ToVector2();
        }
    }

    public class DeathHandler : MonoBehaviour {
        public List<LastDamagingPlayer> damagingPlayerList = new List<LastDamagingPlayer>();

        public void PlayerTakeDamage(Player damagingPlayer) {
            if(damagingPlayer == null)
                return;

            var existingPlayer = damagingPlayerList.FirstOrDefault(playerID => playerID.playerID == damagingPlayer.playerID);
            if(existingPlayer != null) {
                existingPlayer.timeSinceDamage = Time.time;
            } else {
                damagingPlayerList.Add(new LastDamagingPlayer(damagingPlayer.playerID, Time.time));
            }
        }

        [PunRPC]
        public void RPCA_PlayerDied(Vector2[] vectorNewDamagingPlayerList) {
            List<LastDamagingPlayer> newDamagingPlayerList = new List<LastDamagingPlayer>();
            foreach(Vector2 damagingPlayer in vectorNewDamagingPlayerList) {
                newDamagingPlayerList.Add(damagingPlayer.ToLastDamagingPlayer());
            }

            foreach(LastDamagingPlayer lastDamagingPlayer in newDamagingPlayerList) {
                Player damagingPlayer = PlayerManager.instance.players.Find(player => player.playerID == lastDamagingPlayer.playerID);
                if((lastDamagingPlayer.timeSinceDamage <= 5 && damagingPlayer.GetComponentInChildren<SoulstreakMono>() != null) && !damagingPlayer.data.dead) {
                    damagingPlayer.GetComponentInChildren<SoulstreakMono>().AddKill();


                    if(GetComponentInChildren<SoulstreakMono>() != null) {
                        damagingPlayer.GetComponentInChildren<SoulstreakMono>().AddKill((int)(GetComponentInChildren<SoulstreakMono>().KillsStreak * 0.5f));
                        GetComponentInChildren<SoulstreakMono>().ResetKill();
                    }
                }
            }

            if(GetComponentInChildren<SoulstreakMono>() != null) {
                GetComponentInChildren<SoulstreakMono>().ResetKill();
            }
        }

        public void PlayerDied() {
            if(PhotonNetwork.IsMasterClient || PhotonNetwork.OfflineMode) {
                List<Vector2> newDamagingPlayerList = new List<Vector2>();
                float currentTime = Time.time; // Capture current time once
                foreach(LastDamagingPlayer lastDamagingPlayer in damagingPlayerList) {
                    // Use the time since damage already calculated
                    newDamagingPlayerList.Add(new LastDamagingPlayer(lastDamagingPlayer.playerID, currentTime - lastDamagingPlayer.timeSinceDamage).ToVector2());
                }
                GetComponent<Player>().data.view.RPC("RPCA_PlayerDied", RpcTarget.All, newDamagingPlayerList.ToArray());
            }
        }
    }
}
