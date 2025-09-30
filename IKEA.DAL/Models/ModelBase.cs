using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Models
{
    public class ModelBase
    {
        public int Id { get; set; } // Unique identifier for the model
        public int CreatedBy { get; set; } // This could be a user ID or system identifierمين عدل مسح اد
        public DateTime CreatedOn { get; set; } // Timestamp for when the model was created
        public int LastModifiedBy { get; set; } // This could be a user ID or system identifier for the last modifier
        public DateTime LastModifiedOn { get; set; } // Timestamp for when the model was last modified
        public bool IsDeleted { get; set; } // Soft delete flag, indicating if the model is deleted
    }
}
