using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootTimeLine.Front.Models
{
    // Modèles retournés par les actions MeController.
    public class GetViewModel
    {
        public string Hometown { get; set; }
    }
}