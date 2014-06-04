namespace SFA.Apprenticeships.Web.Common.Framework
{
    using System.Web.Mvc;
    using Models.Common;
    using SFA.Apprenticeships.Application.Interfaces.ReferenceData;

    public static class ViewDataDictionaryExtensions
    {
        public static void AddLookups(this ViewDataDictionary viewData, IReferenceDataProvider referenceDataService,
            params ReferenceDataTypes[] lookups)
        {
            foreach (var lookup in lookups)
            {
                var key = "List" + lookup;
                var values = referenceDataService.GetReferenceData(lookup.ToString());
                var list = new SelectList(values, "Id", "Description");

                viewData.Add(key, list);
            }
        }

        public static void AddLists(this ViewDataDictionary viewData, IReferenceDataProvider referenceDataService,
            params ReferenceDataTypes[] lists)
        {
            foreach (var list in lists)
            {
                var key = "List" + list;
                var values = referenceDataService.GetReferenceData(list.ToString());

                viewData.Add(key, values);
            }
        }
    }
}
