// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

using BenchmarkDotNet.Running;

using Democrite.Toolbox.Benchmark.Sandbox;

BenchmarkRunner.Run<StringSplitOptiBenchmark>();

//var inst = new StringSplitOptiBenchmark();
//inst.ClassicSplit();
//inst.OptiSplit();
//inst.LessOptiSplit();
