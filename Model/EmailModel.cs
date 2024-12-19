using System;
using System.Net;
using System.Net.Mail;

namespace Model;

public class EmailModel
{
    public string From { get; set; }
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}