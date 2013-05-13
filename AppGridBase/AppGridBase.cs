using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppGrid
{
    [Serializable]
    public  class AppGridBase:MarshalByRefObject,IAppGridApp
    {
        protected Configuration configuration;

        protected  AppGridBase()
        {
            this.configuration = new Configuration();
        }
        public virtual void Start()
        {
        }
        public virtual void Stop()
        {
        }
        public virtual void Init(Configuration configuration)
        {
            this.configuration = configuration;
        }
    }
}
