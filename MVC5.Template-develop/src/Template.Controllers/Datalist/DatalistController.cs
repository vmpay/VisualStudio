using Datalist;
using Template.Components.Datalists;
using Template.Components.Mvc;
using Template.Components.Security;
using Template.Data.Core;
using Template.Objects;
using System;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Template.Controllers
{
    [AllowUnauthorized]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class DatalistController : BaseController
    {
        private IUnitOfWork UnitOfWork { get; set; }
        private Boolean Disposed { get; set; }

        public DatalistController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        [NonAction]
        public virtual JsonResult GetData(AbstractDatalist datalist, DatalistFilter filter)
        {
            datalist.CurrentFilter = filter;

            return Json(datalist.GetData(), JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        public JsonResult Role(DatalistFilter filter)
        {
            return GetData(new BaseDatalist<Role, RoleView>(UnitOfWork), filter);
        }

        protected override void Dispose(Boolean disposing)
        {
            if (Disposed) return;

            UnitOfWork.Dispose();
            Disposed = true;

            base.Dispose(disposing);
        }
    }
}
