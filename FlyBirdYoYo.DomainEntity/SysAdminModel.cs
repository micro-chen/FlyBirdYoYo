
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

using FlyBirdYoYo.DbManage;
using FlyBirdYoYo.DbManage.Utilities;

namespace FlyBirdYoYo.DomainEntity
{
    [Table ("sys_admin")]
    [PrimaryKey (Name = "id")]
    [Serializable]
    public class SysAdminModel: BaseEntity
    {
        #region 表字段属性

        private long _Id;
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Column ("id")]
        public long Id
        {
            get{ return _Id; }
            set{
                _Id = value;
                NotifyPropertyChange ("Id", value);
            }
        }
        private string _Uname;
        /// <summary>
        /// 账号名
        /// </summary>
        [DataMember]
        [Column ("uname")]
        public string Uname
        {
            get{ return _Uname; }
            set{
                _Uname = value;
                NotifyPropertyChange ("Uname", value);
            }
        }
        private string _Upassword;
        /// <summary>
        /// 登录密码
        /// </summary>
        [DataMember]
        [Column ("upassword")]
        public string Upassword
        {
            get{ return _Upassword; }
            set{
                _Upassword = value;
                NotifyPropertyChange ("Upassword", value);
            }
        }
        private long ? _RoleId;
        [DataMember]
        [Column ("role_id")]
        public long ? RoleId
        {
            get{ return _RoleId; }
            set{
                _RoleId = value;
                NotifyPropertyChange ("RoleId", value);
            }
        }
        
        private string _PublicKey;
        /// <summary>
        /// 公钥
        /// </summary>
        [DataMember]
        [Column ("public_key")]
        public string PublicKey
        {
            get{ return _PublicKey; }
            set{
                _PublicKey = value;
                NotifyPropertyChange ("PublicKey", value);
            }
        }
        private bool _State;
        /// <summary>
        /// 账号状态：0 禁用  1正常
        /// </summary>
        [DataMember]
        [Column ("state")]
        public bool State
        {
            get{ return _State; }
            set{
                _State = value;
                NotifyPropertyChange ("State", value);
            }
        }

        #endregion

    }
}