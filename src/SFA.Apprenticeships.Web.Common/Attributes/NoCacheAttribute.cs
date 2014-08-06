namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System.Web.Mvc;

    public class NoCacheAttribute : OutputCacheAttribute
    {
        public NoCacheAttribute()
        {
            NoStore = true;
            Duration = 0;
            VaryByParam = "None";
        }
    }
}
