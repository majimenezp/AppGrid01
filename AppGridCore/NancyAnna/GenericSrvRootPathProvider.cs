using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;

namespace NancyAnna
{
    public class GenericSrvRootPathProvider : IRootPathProvider
    {
        public string GetRootPath()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }
    }
}
