using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DicomApp.CommonDefinitions.DTO
{
    public class AppServiceDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }

        public int AppServiceClassID { get; set; }
        public string AppServiceClassName { get; set; }
        public int AppServiceGroupID { get; set; }
        public string AppServiceGroupName { get; set; }
        public bool AllowAnonymous { get; set; }
        public bool ShowToUser { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }
        public bool IsDeleted { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }

        public bool IsChecked { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }

        public List<int> SelectedAppServiceIDs { get; set; }
    }
}