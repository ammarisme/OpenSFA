

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFALibrary.Common
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DBFieldAttribute : Attribute
    {
        private string mFieldName;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="fieldName">name of the field that the property will be mapped to</param>
        public DBFieldAttribute(string fieldName)
        {
            mFieldName = fieldName;
        }

        public string FieldName
        {
            get { return mFieldName; }
        }
    }

    public class PrimaryKeyAttribute : Attribute
    {
        private bool isPrimary;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="fieldName">name of the field that the property will be mapped to</param>
        public PrimaryKeyAttribute()
        {
            isPrimary = true;
        }

        public bool IsPrimary
        {
            get { return isPrimary; }
        }
    }

    public class FileNameAttribute : Attribute
    {
        private string mFileName;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="fieldName">name of the field that the property will be mapped to</param>
        public FileNameAttribute(string fileName)
        {
            mFileName= fileName;
        }

        public string FileName
        {
            get { return mFileName; }
        }
    }
}
