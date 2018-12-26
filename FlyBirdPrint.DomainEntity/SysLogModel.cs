
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

using FlyBirdYoYo.DbManage;
using FlyBirdYoYo.DbManage.Utilities;

namespace FlyBirdYoYo.DomainEntity
{
    [Table ("sys_log")]
    [PrimaryKey (Name = "id")]
    [Serializable]
    public class SysLogModel: BaseEntity
    {
        #region 表字段属性

        private long _Id;
        /// <summary>
        /// 自增主键
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
        private long _SysUserId;
        /// <summary>
        /// 超管账号Id。系统日志则为0
        /// </summary>
        [DataMember]
        [Column ("sys_user_id")]
        public long SysUserId
        {
            get{ return _SysUserId; }
            set{
                _SysUserId = value;
                NotifyPropertyChange ("SysUserId", value);
            }
        }
        private int _LogType;
        [DataMember]
        [Column ("log_type")]
        public int  LogType
        {
            get{ return _LogType; }
            set{
                _LogType = value;
                NotifyPropertyChange ("LogType", value);
            }
        }
        private int _Level;
        [DataMember]
        [Column ("level")]
        public int Level
        {
            get{ return _Level; }
            set{
                _Level = value;
                NotifyPropertyChange ("Level", value);
            }
        }
        private string _LogContent;
        /// <summary>
        /// 日志内容
        /// </summary>
        [DataMember]
        [Column ("log_content")]
        public string LogContent
        {
            get{ return _LogContent; }
            set{
                _LogContent = value;
                NotifyPropertyChange ("LogContent", value);
            }
        }
        private string _IpAddress;
        /// <summary>
        /// 请求的Ip地址
        /// </summary>
        [DataMember]
        [Column ("ip_address")]
        public string IpAddress
        {
            get{ return _IpAddress; }
            set{
                _IpAddress = value;
                NotifyPropertyChange ("IpAddress", value);
            }
        }
        private DateTime _CreateTime;
        [DataMember]
        [Column ("create_time")]
        public DateTime CreateTime
        {
            get{ return _CreateTime; }
            set{
                _CreateTime = value;
                NotifyPropertyChange ("CreateTime", value);
            }
        }

        #endregion

    }
}