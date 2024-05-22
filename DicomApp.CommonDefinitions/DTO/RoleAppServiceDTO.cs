using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DicomApp.CommonDefinitions.DTO
{
    public class RoleAppServiceDTO
    {
        public int Id { get; set; }

        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public string ClassName { get; set; }

        public int AppServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceTitle { get; set; }
        public bool ServiceAllowAnonymous { get; set; }
        public bool ServiceShowToUser { get; set; }
        public bool Enabled { get; set; }

        public List<int> SelectedRoleAppServiceIDs { get; set; }
    }
}
