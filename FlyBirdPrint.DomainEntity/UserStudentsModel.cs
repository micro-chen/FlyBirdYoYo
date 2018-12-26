
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

using FlyBirdYoYo.DbManage;
using FlyBirdYoYo.DbManage.Utilities;

namespace FlyBirdYoYo.DomainEntity
{
    [Table("user_students")]
    [PrimaryKey(Name = "id")]
    [Serializable]
    public class UserStudentsModel : BaseEntity
    {
        #region 表字段属性

        /// <summary>
        /// id
        /// </summary>	
        private long _Id;
        [DataMember]
        [Column("id")]
        public long Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                NotifyPropertyChange("Id", value);
            }
        }
        /// <summary>
        /// age
        /// </summary>	
        private int? _Age;
        [DataMember]
        [Column("age")]
        public int? Age
        {
            get { return _Age; }
            set
            {
                _Age = value;
                NotifyPropertyChange("Age", value);
            }
        }
        /// <summary>
        /// name
        /// </summary>	
        private string _Name;
        [DataMember]
        [Column("name")]
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                NotifyPropertyChange("Name", value);
            }
        }
        /// <summary>
        /// sex
        /// </summary>	
        private bool? _Sex;
        [DataMember]
        [Column("sex")]
        public bool? Sex
        {
            get { return _Sex; }
            set
            {
                _Sex = value;
                NotifyPropertyChange("Sex", value);
            }
        }
        /// <summary>
        /// add_time
        /// </summary>	
        private DateTime? _AddTime;
        [DataMember]
        [Column("add_time")]
        public DateTime? AddTime
        {
            get { return _AddTime; }
            set
            {
                _AddTime = value;
                NotifyPropertyChange("AddTime", value);
            }
        }
        /// <summary>
        /// score
        /// </summary>	
        private decimal? _Score;
        [DataMember]
        [Column("score")]
        public decimal? Score
        {
            get { return _Score; }
            set
            {
                _Score = value;
                NotifyPropertyChange("Score", value);
            }
        }
        /// <summary>
        /// longitude
        /// </summary>	
        private double? _Longitude;
        [DataMember]
        [Column("longitude")]
        public double? Longitude
        {
            get { return _Longitude; }
            set
            {
                _Longitude = value;
                NotifyPropertyChange("Longitude", value);
            }
        }
        /// <summary>
        /// latitude
        /// </summary>	
        private double? _Latitude;
        [DataMember]
        [Column("latitude")]
        public double? Latitude
        {
            get { return _Latitude; }
            set
            {
                _Latitude = value;
                NotifyPropertyChange("Latitude", value);
            }
        }
        /// <summary>
        /// has_pay
        /// </summary>	
        private decimal? _HasPay;
        [DataMember]
        [Column("has_pay")]
        public decimal? HasPay
        {
            get { return _HasPay; }
            set
            {
                _HasPay = value;
                NotifyPropertyChange("HasPay", value);
            }
        }
        /// <summary>
        /// home_number
        /// </summary>	
        private int? _HomeNumber;
        [DataMember]
        [Column("home_number")]
        public int? HomeNumber
        {
            get { return _HomeNumber; }
            set
            {
                _HomeNumber = value;
                NotifyPropertyChange("HomeNumber", value);
            }
        }

        #endregion
    }
}