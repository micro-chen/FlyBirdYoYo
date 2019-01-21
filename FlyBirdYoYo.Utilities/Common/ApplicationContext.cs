using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Security.Principal;
using FlyBirdYoYo.Utilities.Ioc;
using FlyBirdYoYo.Utilities.Interface;
using FlyBirdYoYo.Utilities.SystemEnum;

namespace System
{
    /// <summary>
    /// 安全的程序应用上下文对象
    /// 说明：可以替代 HttpContext的安全上下文对象
    /// </summary>
    public class ApplicationContext
    {
        private static readonly string Key = "FlyBirdYoYo.Context.ApplicationContext";
        private IDictionary<string, object> _map;

        public bool HasValue
        {
            get
            {
                if (this._map != null)
                    return this._map.Count > 0;
                else
                    return false;
            }
        }

        private ILoginAuthedUserDTO _User;
        /// <summary>
        /// 当前登录认证用户
        /// </summary>
        public ILoginAuthedUserDTO User
        {
            get
            {
                return this._User;
            }
            set
            {
                this._User = value;
            }
        }




        /// <summary>
        /// 主店铺Id
        /// </summary>
        public long CreateUserId
        {
            get
            {

                if (null != this.User)
                {
                    return this.User.CreateUserId;
                }
                return -1;
            }

        }

        /// <summary>
        /// 店铺组Id
        /// </summary>
        public long GroupId
        {
            get
            {

                if (null != this.User)
                {
                    return this.User.GroupId;
                }
                return -1;
            }

        }




        /// <summary>
        /// 当前登录用户Id
        /// 注意：如果从新变更用户Id，会更新User
        /// </summary>
        public long UserId
        {
            get
            {
                long uid = -1;
                if (null != this.User)
                {
                    uid = this.User.UserId;
                }
                return uid;
            }


        }

        public string UserName
        {
            get
            {
                if (null != this.User)
                {
                    return this.User.UserName;
                }
                return null;
            }

        }



        /// <summary>
        /// 当前电商平台
        /// </summary>
        public PlatformEnum? Platform
        {
            get
            {
                if (null != this.User)
                {
                    return this.User.Platform;
                }
                return null;
            }
        }
        /// <summary>
        /// 授权码
        /// </summary>
        public string Access_token
        {
            get
            {
                if (null != this.User)
                {
                    return this.User.Access_token;
                }
                return null;
            }
        }

        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public bool IsSystemAdmin { get; set; }

        public static ApplicationContext Current
        {
            get
            {

                ApplicationContext applicationContext = (ApplicationContext)CallContext.GetData(ApplicationContext.Key);
                if (applicationContext == null)
                {
                    applicationContext = new ApplicationContext();
                    CallContext.SetData(ApplicationContext.Key, (object)applicationContext);
                }
                return applicationContext;

            }
            set
            {
                CallContext.SetData(ApplicationContext.Key, (object)value);
            }
        }

        static ApplicationContext()
        {
        }

        public ApplicationContext()
        {
            this._map = (IDictionary<string, object>)new ConcurrentDictionary<string, object>();
        }

        internal ApplicationContext(IDictionary<string, object> values)
        {
            this._map = values;
        }

        internal void Put(string key, int value)
        {
            this._map[key] = (object)value.ToString();
        }

        public void Put(string key, string value)
        {
            this._map[key] = value;
        }

        public void Put(string key, object value)
        {
            this._map[key] = value.ToJson();
        }

        public bool Contains(string key)
        {
            return this._map.ContainsKey(key);
        }

        public string Get(string key)
        {
            object obj;
            this._map.TryGetValue(key, out obj);
            return (string)obj;
        }

        public T Get<T>(string key)
        {
            object value;
            this._map.TryGetValue(key, out value);
            if (value != null)
                return value.ToString().FromJsonToObject<T>();
            else
                return default(T);
        }

        public bool Remove(string key)
        {
            return this._map.Remove(key);
        }





        public static void Clear()
        {
            CallContext.FreeNamedDataSlot(ApplicationContext.Key);
        }



        /// <summary>
        /// 当前应用的Http上下文
        /// </summary>
        public static class HttpContext
        {
            public static Microsoft.AspNetCore.Http.HttpContext Current
            {
                get
                {
                    Microsoft.AspNetCore.Http.HttpContext _context = null;
                    object factory = ServiceLocator.GetInstance<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
                    if (null != factory)
                    {
                        Microsoft.AspNetCore.Http.HttpContext context = ((Microsoft.AspNetCore.Http.HttpContextAccessor)factory).HttpContext;
                        _context = context;
                    }

                    return _context;

                }
            }
        }


    }
}
