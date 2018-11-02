using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HoardSorter.Models
{
    public class Trades
    {
        [Key]

        public string Name { get; set; }
    }
}