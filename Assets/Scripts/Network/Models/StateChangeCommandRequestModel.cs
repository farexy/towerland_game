using System;
using System.Collections.Generic;
using Assets.Scripts.Models.State;
using Newtonsoft.Json;

namespace Assets.Scripts.Network.Models
{
    public class StateChangeCommandRequestModel
    {
        //public CommandId Id { set; get; }
        public Guid BattleId { set; get; }
        public IEnumerable<UnitCreationOption> UnitCreationOptions { set; get; }
        public IEnumerable<TowerCreationOption> TowerCreationOptions { set; get; }
        [JsonProperty("command")] public string CheatCommand { get; set; }
    
        public int CurrentTick { set; get; }
    }
}