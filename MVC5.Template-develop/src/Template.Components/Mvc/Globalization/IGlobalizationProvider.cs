using System;

namespace Template.Components.Mvc
{
    public interface IGlobalizationProvider
    {
        Language[] Languages { get; }
        Language DefaultLanguage { get; }
        Language CurrentLanguage { get; set; }

        Language this[String abbreviation] { get; }
    }
}
