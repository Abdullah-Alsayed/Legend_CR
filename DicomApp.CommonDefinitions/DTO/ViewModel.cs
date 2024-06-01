using System.Collections.Generic;

namespace DicomApp.CommonDefinitions.DTO
{
    public class ViewModel<T>
    {
        public List<T> ObjDTOs { get; set; }
        public T ObjDTO { get; set; }

        public LookupDTO Lookup { get; set; }
    }
}
