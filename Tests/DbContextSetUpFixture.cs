﻿using NUnit.Framework;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.UniqueConstraints;
using SomeBasicRavenApp.Core;
using System;
using System.IO;
using System.Linq;
using With;

[SetUpFixture]
class DbContextSetUpFixture
{
    public static IDocumentStore store;
    [SetUp]
    public void RunBeforeAnyTests()
    {
        //new RavenTestBase()
        var plugindirectory = Directory.EnumerateDirectories(Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "packages"))
            .FirstOrDefault(dir => Path.GetFileName(dir).StartsWith("RavenDB.Bundles.UniqueConstraints"));

        store = new EmbeddableDocumentStore()
        {
            Configuration = {
                    RunInUnreliableYetFastModeThatIsNotSuitableForProduction = true,
                    RunInMemory = true,
                    Storage = {
                        Voron= {
                            AllowOn32Bits = true
                        }
                    },
                    PluginsDirectory=Path.Combine(plugindirectory, "lib", "net45")
                }
        }.Tap(s =>
        {
            s.RegisterListener(new UniqueConstraintsStoreListener());
            s.RegisterListener(new CustomerVersion1ToVersion2Converter());
        });
        store.Initialize();
    }

    [TearDown]
    public void RunAfterAnyTests()
    {
        store.Dispose();
    }
}
