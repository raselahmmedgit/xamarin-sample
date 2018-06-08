using System;
using System.Collections.Generic;
using System.Text;

namespace App1710.AppCore
{
    public class AppMessage
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsError { get; set; }

        public AppMessage SetSuccess(string message = "Success !")
        {
            AppMessage appMessage = new AppMessage() { IsSuccess = true, IsError = false, Message = message };
            return appMessage;
        }

        public AppMessage SetError(string message = "Error !")
        {
            AppMessage appMessage = new AppMessage() { IsSuccess = false, IsError = true, Message = message };
            return appMessage;
        }
    }
}
