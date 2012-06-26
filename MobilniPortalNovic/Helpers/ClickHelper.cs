using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovic.Helpers
{
    public class ClickModelBinder : DefaultModelBinder
    {
        protected override void OnModelUpdated(ControllerContext controllerContext,
                                               ModelBindingContext bindingContext)
        {
            var recipe = bindingContext.Model as ClickCounter;
            recipe.SetDayOfWeekAndTimeOfDay();
        }
    }
}