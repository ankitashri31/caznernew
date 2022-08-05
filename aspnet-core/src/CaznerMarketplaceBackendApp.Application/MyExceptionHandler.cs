using Abp.Dependency;
using Abp.Events.Bus.Exceptions;
using Abp.Events.Bus.Handlers;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace WhoMasterDataCollection
{
        public class MyExceptionHandler : IEventHandler<AbpHandledExceptionData>, ITransientDependency
        {
        public ILogger Logger { get; set; }

        public MyExceptionHandler()
        {
            Logger = NullLogger.Instance;
        }

        public void HandleEvent(AbpHandledExceptionData eventData)
        {
            Logger.Error("INFO: " + DateTime.UtcNow + " || Error: " + eventData.Exception.StackTrace);
            //TODO: Check eventData.Exception!
        }
   }
}
