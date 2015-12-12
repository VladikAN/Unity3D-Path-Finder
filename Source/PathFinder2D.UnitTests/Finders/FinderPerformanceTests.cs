﻿using NUnit.Framework;
using PathFinder2D.Core;
using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Extensions;
using PathFinder2D.Core.Finder;
using PathFinder2D.Core.Initializer;
using PathFinder2D.UnitTests.Extensions;
using PathFinder2D.UnitTests.Stubs;
using System;
using System.Diagnostics;

namespace PathFinder2D.UnitTests.Finders
{
    [TestFixture]
    public class FinderPerformanceTests
    {
        private MapDefinition _testMap;
        private readonly Random _random = new Random();

        [TestFixtureSetUp]
        public void Init()
        {
            var width = 128;
            var height = 128;
            var walls = 128;

            var field = new MapCell[width, height];
            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                    field[i, j] = new MapCell();

            for (var i = 0; i < walls; i++)
            {
                var w = _random.Next(10, width - 10);
                var h = _random.Next(10, height - 10);
                field[w, h].Blocked = true;
            }

            var terrain = new FakeTerrain(1, 0, 0, width, height, 1);
            _testMap = new MapDefinition(terrain, field, 1);
        }

        [TestCase(1, TestName = "Wave finder performance")]
        [TestCase(2, TestName = "Jump finder performance")]
        public void Wave_PerformanceTests(int finderNumber)
        {
            var finder = finderNumber == 1 ? (IFinder) new WaveFinder() : new JumpPointFinder();

            var pathFinderService = new PathFinderService(finder, new MapInitializer());
            pathFinderService.GetMaps().Add(1, _testMap);

            var start = _testMap.Terrain.ToVector3(new FakeFinderPoint { X = 0, Y = 0 });
            var end = _testMap.Terrain.ToVector3(new FakeFinderPoint { X = _testMap.FieldWidth, Y = _testMap.FieldHeight - 1 });

            var stopWatch = new Stopwatch();

            stopWatch.Start();
            var result = pathFinderService.FindPath(_testMap.Terrain.Id(), start, end);
            stopWatch.Stop();

            Console.WriteLine("Time taken: {0:D4} ms", stopWatch.ElapsedMilliseconds);
            AssertExtensions.IsValidPath(result, _testMap);
        }
    }
}