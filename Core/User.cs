using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core;


public class Users
{
    public int UserId { get; set; }

    public string UserName { get; set; } = String.Empty;

    public string Password { get; set; } = String.Empty;

    public string Role { get; set; } = "none";
}