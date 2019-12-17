﻿using CentralDeErros.Domain.Models.Base;
using System;

namespace CentralDeErros.Domain.Models
{
    /// <summary>
    /// Classe de log
    /// </summary>
    public class Log : ModelBase
    { 
        public string Title { get; set; }
        public string Detail { get; set; }
        public int Event { get; set; }
        public string Level { get; set; }
        public string Environment { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Ip { get; set; }
        
        public Guid Token { get; set; }
        public virtual User User { get; set; }

        public Log()
        {
            Enabled = true;
            CreatedAt = DateTime.Now;
        }
    }
}
