using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppGrid
{

    public interface IAppGridApp
    {
        /// <summary>
        /// Start the application instance
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the application instance
        /// </summary>
        void Stop();

        /// <summary>
        /// Initialize the application with the saved configuration
        /// </summary>
        /// <param name="configuration">The configuration info that was saved from the admin console</param>
        void Init(Configuration configuration);
    }
}
