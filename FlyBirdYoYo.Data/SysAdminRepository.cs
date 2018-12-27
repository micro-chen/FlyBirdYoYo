using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

using FlyBirdYoYo.DbManage;
using FlyBirdYoYo.DomainEntity;
using FlyBirdYoYo.Utilities.Interface;

namespace FlyBirdYoYo.Data.Repository
{
    public  class SysAdminRepository: BaseRepository<SysAdminModel>, IDbContext<SysAdminModel>, IRepository
    {
        public SysAdminRepository()
        {}

    }
}