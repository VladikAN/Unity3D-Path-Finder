﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Finder
{
    public class FinderResult
    {
        public IEnumerable<Vector3> Path { get; set; }
        public uint?[,] Map { get; set; }
    }
}