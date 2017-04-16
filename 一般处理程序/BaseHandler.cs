using CommonHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.SessionState;

namespace BasePage.Handler
{
    public class BaseHandler:IHttpHandler,IRequiresSessionState
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            //LoginHelper.CheckIsLogin(context);//检查是否登录
            string action =context.Request["action"];
            Type type = this.GetType();
            MethodInfo methodinfo = type.GetMethod(action);
            if (methodinfo == null)
            {
                throw new Exception("action不存在");
            }
            object[] paAttrs = methodinfo.GetCustomAttributes(typeof(PowerActionAttribute), false);
            if (paAttrs.Length>0)
            {
                PowerActionAttribute poweraction = (PowerActionAttribute)paAttrs[0];
                AdminHelper.CheckPower(poweraction.Name);
            }
            methodinfo.Invoke(this, new object[] { context });
        }
    }
}