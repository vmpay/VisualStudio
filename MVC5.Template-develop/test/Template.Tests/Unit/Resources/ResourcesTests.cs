﻿using Template.Data.Core;
using Template.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using Xunit;

namespace Template.Tests.Unit.Resources
{
    public class ResourcesTests
    {
        [Fact]
        public void Resources_HasAllPrivilegeAreaTitles()
        {
            ResourceManager manager = Template.Resources.Privilege.Area.Titles.ResourceManager;
            using (Context context = new Context())
            {
                String[] areas = context
                    .Set<Privilege>()
                    .Select(priv => priv.Area)
                    .Distinct()
                    .ToArray();

                foreach (String area in areas)
                    Assert.True(!String.IsNullOrEmpty(manager.GetString(area)),
                        String.Format("Privilege area '{0}', does not have a title.", area));
            }
        }

        [Fact]
        public void Resources_HasAllPrivilegeControllerTitles()
        {
            ResourceManager manager = Template.Resources.Privilege.Controller.Titles.ResourceManager;
            using (Context context = new Context())
            {
                String[] controllers = context
                    .Set<Privilege>()
                    .Select(priv => priv.Area + priv.Controller)
                    .Distinct()
                    .ToArray();

                foreach (String controller in controllers)
                    Assert.True(!String.IsNullOrEmpty(manager.GetString(controller)),
                        String.Format("Privilege controller '{0}', does not have a title.", controller));
            }
        }

        [Fact]
        public void Resources_HasAllPrivilegeActionTitles()
        {
            ResourceManager manager = Template.Resources.Privilege.Action.Titles.ResourceManager;
            using (Context context = new Context())
            {
                String[] actions = context
                    .Set<Privilege>()
                    .Select(priv => priv.Area + priv.Controller + priv.Action)
                    .Distinct()
                    .ToArray();

                foreach (String action in actions)
                    Assert.True(!String.IsNullOrEmpty(manager.GetString(action)),
                        String.Format("Privilege action '{0}', does not have a title.", action));
            }
        }

        [Fact]
        public void Resources_HasEquivalents()
        {
            IEnumerable<CultureInfo> languages = new[] { new CultureInfo("en-GB"), new CultureInfo("lt-LT") };
            IEnumerable<Type> resourceTypes = Assembly
                .Load("Template.Resources")
                .GetTypes()
                .Where(type => type.Namespace.StartsWith("Template.Resources."));

            foreach (Type type in resourceTypes)
            {
                ResourceManager manager = new ResourceManager(type);
                IEnumerable<String> resourceKeys = new String[0];

                foreach (ResourceSet set in languages.Select(language => manager.GetResourceSet(language, true, true)))
                {
                    resourceKeys = resourceKeys.Union(set.Cast<DictionaryEntry>().Select(resource => resource.Key.ToString()));
                    resourceKeys = resourceKeys.Distinct();
                }

                foreach (CultureInfo language in languages)
                {
                    ResourceSet set = manager.GetResourceSet(language, true, true);
                    foreach (String key in resourceKeys)
                        Assert.True((set.GetObject(key) ?? "").ToString() != "",
                            String.Format("{0}, does not have translation for '{1}' in {2} language.",
                                type.FullName, key, language.EnglishName));
                }
            }
        }
    }
}
