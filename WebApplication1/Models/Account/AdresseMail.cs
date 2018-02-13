using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Account
{
    public class AdresseMail
    {
        public string ID { get; set; }

        public AdresseMail(string mail)
        {
            ID = mail;
        }
    }
}