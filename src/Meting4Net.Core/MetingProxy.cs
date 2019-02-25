using System;
using System.Collections.Generic;
using System.Text;

using System.Net;

namespace Meting4Net.Core
{
    public class MetingProxy
    {
        public WebProxy Proxy { get; set; }

        #region WebProxy new
        /// <summary>
        /// WebProxy new
        /// </summary>
        /// <param name="webProxy"></param>
        public MetingProxy(WebProxy webProxy)
        {
            this.Proxy = webProxy;
        } 
        #endregion

        #region 通过主机ID和端口号new
        /// <summary>
        /// 通过主机ID和端口号new
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public MetingProxy(string host, int port)
        {
            this.Proxy = new WebProxy(host, port);
        } 
        #endregion
    }
}
