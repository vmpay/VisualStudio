﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public abstract class BaseModel
    {
        [Key]
        [Required]
        public String Id
        {
            get
            {
                return InternalId ?? (InternalId = Guid.NewGuid().ToString());
            }
            set
            {
                InternalId = value;
            }
        }
        private String InternalId
        {
            get;
            set;
        }

        public DateTime CreationDate
        {
            get
            {
                if (!IsCreationDateSet)
                    CreationDate = DateTime.Now;

                return InternalCreationDate;
            }
            protected set
            {
                IsCreationDateSet = true;
                InternalCreationDate = value;
            }
        }
        private Boolean IsCreationDateSet
        {
            get;
            set;
        }
        private DateTime InternalCreationDate
        {
            get;
            set;
        }
    }
}
