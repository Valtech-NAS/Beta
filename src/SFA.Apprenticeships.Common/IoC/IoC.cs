namespace SFA.Apprenticeships.Common.IoC
{
    using System.IO;
    using System.Reflection;
    using StructureMap;

    public static class IoC
    {
        public static IContainer Initialize()
        {
            var assemblyPath = (new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
            var fileInfo = new FileInfo(assemblyPath);
            var sfaDlls = Directory.GetFiles(fileInfo.Directory.FullName, "SFA.Apprenticeships.*.dll");
      
            ObjectFactory.Initialize(
                x => x.Scan(
                    scan =>
                    {
                        foreach (var sfaDll in sfaDlls)
                        {
                            var dll = new FileInfo(sfaDll);
                            scan.Assembly(dll.Name.Substring(0, dll.Name.Length - 4));
                        }

                        scan.LookForRegistries();
                    }));

            return ObjectFactory.Container;
        }
    }
}