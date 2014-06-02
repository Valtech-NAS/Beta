namespace SFA.Apprenticeships.Web.Common.Framework
{
    using System.Web.Mvc;
    using Providers;
    using Models.Common;

    public static class ViewDataDictionaryExtensions
    {
        public static void AddLookups(this ViewDataDictionary viewData, IWebReferenceDataProvider referenceDataService,
            params WebReferenceDataTypes[] lookups)
        {
            foreach (var lookup in lookups)
            {
                var key = "List" + lookup;
                var values = referenceDataService.Get(lookup);
                var list = new SelectList(values, "Id", "Description");

                viewData.Add(key, list);
            }
        }

        public static void AddLists(this ViewDataDictionary viewData, IWebReferenceDataProvider referenceDataService,
            params WebReferenceDataTypes[] lists)
        {
            foreach (var list in lists)
            {
                var key = "List" + list;
                var values = referenceDataService.Get(list);

                viewData.Add(key, values);
            }
        }
    }
}
