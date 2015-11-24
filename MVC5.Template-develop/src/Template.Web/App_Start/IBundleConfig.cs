using System.Web.Optimization;

namespace Template.Web
{
    public interface IBundleConfig
    {
        void RegisterBundles(BundleCollection bundles);
    }
}
