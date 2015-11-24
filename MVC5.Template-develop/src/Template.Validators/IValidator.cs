using Template.Components.Alerts;
using System;
using System.Web.Mvc;

namespace Template.Validators
{
    public interface IValidator : IDisposable
    {
        ModelStateDictionary ModelState { get; set; }
        String CurrentAccountId { get; set; }
        AlertsContainer Alerts { get; set; }
    }
}
