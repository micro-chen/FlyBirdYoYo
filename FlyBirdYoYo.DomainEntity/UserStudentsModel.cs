
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

using FlyBirdYoYo.DbManage;
using FlyBirdYoYo.DbManage.Utilities;

namespace FlyBirdYoYo.DomainEntity
{
    //UserStudents
    [Table("UserStudents")]
    [PrimaryKey(Name = "Id")]
    [Serializable]
    public class UserStudentsModel : BaseEntity
    {
        #region 表字段属性

        private long _Id;
        /// <summary>
        /// Id
        /// </summary>	
        [DataMember]
        [Column("Id")]
        public long Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                NotifyPropertyChange("Id", value);
            }
        }
        private int? _Age;
        [DataMember]
        [Column("Age")]
        public int? Age
        {
            get { return _Age; }
            set
            {
                _Age = value;
                NotifyPropertyChange("Age", value);
            }
        }
        private string _Name;
        /// <summary>
        /// Name
        /// </summary>	
        [DataMember]
        [Column("Name")]
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                NotifyPropertyChange("Name", value);
            }
        }
        private bool? _Sex;
        [DataMember]
        [Column("Sex")]
        public bool? Sex
        {
            get { return _Sex; }
            set
            {
                _Sex = value;
                NotifyPropertyChange("Sex", value);
            }
        }
        private DateTime? _AddTime;
        [DataMember]
        [Column("AddTime")]
        public DateTime? AddTime
        {
            get { return _AddTime; }
            set
            {
                _AddTime = value;
                NotifyPropertyChange("AddTime", value);
            }
        }
        private decimal? _Score;
        [DataMember]
        [Column("Score")]
        public decimal? Score
        {
            get { return _Score; }
            set
            {
                _Score = value;
                NotifyPropertyChange("Score", value);
            }
        }
        private decimal? _Longitude;
        [DataMember]
        [Column("Longitude")]
        public decimal? Longitude
        {
            get { return _Longitude; }
            set
            {
                _Longitude = value;
                NotifyPropertyChange("Longitude", value);
            }
        }
        private decimal? _Latitude;
        [DataMember]
        [Column("Latitude")]
        public decimal? Latitude
        {
            get { return _Latitude; }
            set
            {
                _Latitude = value;
                NotifyPropertyChange("Latitude", value);
            }
        }
        private decimal? _HasPay;
        [DataMember]
        [Column("HasPay")]
        public decimal? HasPay
        {
            get { return _HasPay; }
            set
            {
                _HasPay = value;
                NotifyPropertyChange("HasPay", value);
            }
        }
        private int? _HomeNumber;
        [DataMember]
        [Column("HomeNumber")]
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