namespace SFA.Apprenticeships.Domain.Entities.ReferenceData
{
    using System.Collections.Generic;

    public class Category
    {
        public string ShortName { get; set; }

        public string FullName { get; set; }

        public string CodeName { get; set; }

        public Category ParentCategory { get; set; }

        public IList<Category> SubCategories { get; set; }

        public long? Count { get; set; }
    }
}
