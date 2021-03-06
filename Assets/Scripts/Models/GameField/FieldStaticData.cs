﻿using System;
using Assets.Scripts.Models.GameObjects;
using Newtonsoft.Json;

namespace Assets.Scripts.Models.GameField
{
    public class FieldStaticData
    {    
        public FieldStaticData(FieldCell[,] cells, Point start, Point finish)
        {
            Start = start;
            Finish = finish;
            Cells = cells;
        }
    
        [JsonProperty("p")] public Path[] Path { set; get; }
        [JsonProperty("m")] public FieldCell[,] Cells { private set; get; }
    
        public Point Start { get; private set; }
        public Point Finish { get; private set; }
    
        public int Width
        {
            get { return Cells.GetLength(1); }
        }

        public int Height
        {
            get { return Cells.GetLength(0); }
        }

        public DateTime EndTimeUtc { get; set; }
    }
}