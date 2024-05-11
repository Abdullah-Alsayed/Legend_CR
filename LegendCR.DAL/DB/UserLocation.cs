
using System;
using System.Collections.Generic;

namespace LegendCR.DAL.DB
{
    public partial class UserLocation
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string PlaceId { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int UserId { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime SubmitDateTime { get; set; }
        public int? BataryStatus { get; set; }
        public int? NetworkStatus { get; set; }
        public bool? IsOnline { get; set; }
        public double? DirectionAngle { get; set; }
        public double? AquirityDistance { get; set; }
        public long? ChannelId { get; set; }
        public long ApplicationId { get; set; }
        public string ImagePath { get; set; }

        public virtual CommonUser User { get; set; }
    }
}
