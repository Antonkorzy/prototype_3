//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace prototype_2
{
    using System;
    using System.Collections.Generic;
    
    public partial class acts
    {
        public System.Guid act_id { get; set; }
        public string number { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public string geo_location { get; set; }
        public byte[] photo { get; set; }
        public byte[] doc { get; set; }
        public string extra_info { get; set; }
        public System.Guid user_id { get; set; }
    
        public virtual users users { get; set; }
    }
}
