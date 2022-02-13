using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace droneproject.Domain
{
    public class Drone : BaseEntity
    {
        [Column(TypeName = "nvarchar(100)")]
        public string SerialNumber { get; set; }

        public ModelStatus Model { get; set; }

        public int Weight { get; set; }

        public int Battery { get; set; }

        public StateStatus State { get; set; }

        public string ReferenceKey { get; set; }
    }

    public enum ModelStatus
    {
        Lightweight,

        Middleweight,

        Cruiserweight,

        Heavyweight
    }

    public enum StateStatus
    {
        IDLE,

        LOADING,

        LOADED,

        DELIVERING,

        DELIVERED,

        RETURNING
    }
}
