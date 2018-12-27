using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Transactions;
using System.AutoMapper;

using FlyBirdYoYo.DomainEntity.ViewModel;
using FlyBirdYoYo.BusinessServices;
using FlyBirdYoYo.DomainEntity;
using FlyBirdYoYo.DomainEntity.Login;

namespace FlyBirdYoYo.BusinessServices.Authentication
{
    public class OAuthLoginAuthProvider
    {



        public OAuthLoginAuthProvider()
        {

        }

        public LoginAuthedUserDTO Login(OAuthLoginViewModel model)
        {
            LoginAuthedUserDTO dtoModel = null;



            try
            {
                if (null == model)
                {
                    throw new Exception("授权模型为空！");
                }

                ////注意：这里后期等用户表创建好后，需要去用户表取信息，如果用户表的过期时间到了，那么还需要更新表数据
                ////1 查询用户是否存在
                ////2 不存在插入新的，并生成新的组，并插入access_token日志
                ////3 存在的，那么更新表
                //if (null == userModel)
                //{
                //    //插入模式

                //    //用户模型
                //    userModel = RegisterNewUser(model, out grpId);
                //}
                //else
                //{
                //    //更新模式

                //        grpId = UpdateUserInfo(model, pid, userModel);


                //}

                //model.UserId = userModel.Id;


                //dtoModel = new LoginAuthedUserDTO
                //{
                //    GroupId = 0,//用户所在分组
                //    Platform = model.Platform,
                //    ShopId = model.ShopId,
                //    ShopName = model.ShopName,
                //    UserId = model.UserId,
                //    UserName = model.UserName,
                //    Access_token = model.Access_token,
                //    TokenExpireTime = model.ExpireTime
                //};



            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dtoModel;
        }

        ///// <summary>
        ///// 更新登陆用户信息
        ///// </summary>
        ///// <param name="model"></param>
        ///// <param name="pid"></param>
        ///// <param name="userModel"></param>
        ///// <returns></returns>
        //private long UpdateUserInfo(OAuthLoginViewModel model, int pid, UserModel userModel)
        //{

        //    return grpId;
        //}

        ///// <summary>
        /////  注册新的用户信息
        ///// </summary>
        ///// <param name="model"></param>
        ///// <param name="userGoupId"></param>
        ///// <returns></returns>
        //private UserModel RegisterNewUser(OAuthLoginViewModel model, out long userGoupId)
        //{
        //    userGoupId = 1;



        //    using (var tran = new TransactionScope())
        //    {
        //        tran.Complete();//提交事务
        //    }

        //    return userModel;
        //}


    }
}
